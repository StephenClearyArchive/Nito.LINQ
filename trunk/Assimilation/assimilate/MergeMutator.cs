using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Cci;
using Microsoft.Cci.MutableCodeModel;

namespace assimilate
{
    public sealed class MergeAssemblies
    {
        private readonly IMetadataHost host;
        private readonly FixedTypeNameFormatter typeNameFormatter;
        private readonly FixedSignatureFormatter signatureFormatter;
        private readonly Dictionary<string, object> baseAssemblyDefinitions;
        private readonly Dictionary<string, MergeData> mergeInstructions;

        private sealed class MergeData
        {
            /// <summary>
            /// Gets or sets the place in the base assembly metadata where this definition should be inserted. This is always a RootUnitNamespace, NestedUnitNamespace, TypeDefinition, and/or TypeDefinitionMember.
            /// </summary>
            public object BaseAssemblyInjectionPoint { get; set; }

            /// <summary>
            /// Gets or sets the definition from the added assembly metadata. This is always an INestedUnitNamespace, ITypeDefinition, or ITypeDefinitionMember.
            /// </summary>
            public object AddedAssemblyDefinition { get; set; }
        }

        public MergeAssemblies(IMetadataHost host)
        {
            this.host = host;
            this.typeNameFormatter = new FixedTypeNameFormatter();
            this.signatureFormatter = new FixedSignatureFormatter(this.typeNameFormatter);
            this.baseAssemblyDefinitions = new Dictionary<string, object>();
            this.mergeInstructions = new Dictionary<string, MergeData>();
        }

        public static Assembly Run(IMetadataHost host, IAssembly baseAssembly, IAssembly addedAssembly)
        {
            Assembly mutableModule = baseAssembly as Assembly;
            if (mutableModule == null)
            {
                return Run(host, new MetadataMutator(host).Visit(baseAssembly), addedAssembly);
            }
            else
            {
                return Run(host, mutableModule, addedAssembly);
            }
        }

        public static Assembly Run(IMetadataHost host, Assembly baseAssembly, IAssembly addedAssembly)
        {
            var parent = new MergeAssemblies(host);
            new ReadDefinitions(parent.baseAssemblyDefinitions, parent.typeNameFormatter, parent.signatureFormatter, NameFormattingOptions.DocumentationId).Visit(baseAssembly);
            new FindDifferences(parent).Visit(addedAssembly);
            parent.Merge(baseAssembly, addedAssembly, parent);
            baseAssembly = new MetadataMutator(parent.host).Visit(baseAssembly);

#if DEBUG
            // Verify that the merge picked up all definitions
            Dictionary<string, object> addedAssemblyDefinitions = new Dictionary<string, object>();
            Dictionary<string, object> mergedDefinitions = new Dictionary<string, object>();

            new ReadDefinitions(addedAssemblyDefinitions, parent.typeNameFormatter, parent.signatureFormatter, NameFormattingOptions.DocumentationId).Visit(addedAssembly);
            new ReadDefinitions(mergedDefinitions, parent.typeNameFormatter, parent.signatureFormatter, NameFormattingOptions.DocumentationId).Visit(baseAssembly);

            foreach (var kvp in parent.baseAssemblyDefinitions)
            {
                if (!mergedDefinitions.ContainsKey(kvp.Key))
                {
                    Console.WriteLine("Verification error: base assembly definition " + kvp.Key + " was not merged.");
                }
            }

            foreach (var kvp in addedAssemblyDefinitions)
            {
                if (!mergedDefinitions.ContainsKey(kvp.Key))
                {
                    Console.WriteLine("Verification error: added assembly definition " + kvp.Key + " was not merged.");
                }
            }
#endif

            return baseAssembly;
        }

        private void Merge(Assembly baseAssembly, IAssembly addedAssembly, MergeAssemblies state)
        {
            foreach (var kvp in state.mergeInstructions)
            {
                UnitNamespace unitNamespaceInjectionPoint = kvp.Value.BaseAssemblyInjectionPoint as UnitNamespace;
                if (unitNamespaceInjectionPoint != null)
                {
                    unitNamespaceInjectionPoint.Members.Add((INamespaceMember)this.Duplicate(kvp.Value.AddedAssemblyDefinition, unitNamespaceInjectionPoint));
                }
                else
                {
                    TypeDefinition typeDefinitionInjectionPoint = kvp.Value.BaseAssemblyInjectionPoint as TypeDefinition;
                    if (typeDefinitionInjectionPoint == null)
                    {
                        throw new Exception("Base assembly injection point is of unacceptable type " + kvp.Value.BaseAssemblyInjectionPoint.GetType().Name + ".");
                    }

                    if (kvp.Value.AddedAssemblyDefinition is IEventDefinition)
                    {
                        typeDefinitionInjectionPoint.Events.Add((IEventDefinition)this.Duplicate(kvp.Value.AddedAssemblyDefinition, typeDefinitionInjectionPoint));
                    }
                    else if (kvp.Value.AddedAssemblyDefinition is IFieldDefinition)
                    {
                        typeDefinitionInjectionPoint.Fields.Add((IFieldDefinition)this.Duplicate(kvp.Value.AddedAssemblyDefinition, typeDefinitionInjectionPoint));
                    }
                    else if (kvp.Value.AddedAssemblyDefinition is IMethodDefinition)
                    {
                        typeDefinitionInjectionPoint.Methods.Add((IMethodDefinition)this.Duplicate(kvp.Value.AddedAssemblyDefinition, typeDefinitionInjectionPoint));
                    }
                    else if (kvp.Value.AddedAssemblyDefinition is INestedTypeDefinition)
                    {
                        typeDefinitionInjectionPoint.NestedTypes.Add((INestedTypeDefinition)this.Duplicate(kvp.Value.AddedAssemblyDefinition, typeDefinitionInjectionPoint));
                    }
                    else if (kvp.Value.AddedAssemblyDefinition is IPropertyDefinition)
                    {
                        typeDefinitionInjectionPoint.Properties.Add((IPropertyDefinition)this.Duplicate(kvp.Value.AddedAssemblyDefinition, typeDefinitionInjectionPoint));
                    }
                    else
                    {
                        throw new Exception("Added assembly definition type " + kvp.Value.AddedAssemblyDefinition.GetType().Name + " cannot be added to a base assembly type definition.");
                    }
                }
            }
        }

        /// <summary>
        /// Duplicates a definition from the added assembly into the base assembly. Note that this performs a "shallow" duplication.
        /// </summary>
        /// <param name="source">The original definition from the added assembly. This must be an INestedUnitNamespace, ITypeDefinition, or ITypeDefinitionMember.</param>
        /// <param name="newContainer">The definition's new container in the base assembly. This must be a UnitNamespace or TypeDefinition.</param>
        /// <returns>The new definition, ready to be added to the base assembly. This is an INamespaceMember if <paramref name="newContainer"/> was a UnitNamespace, and this is an ITypeDefinitionMember if <paramref name="newContainer"/> was a TypeDefinition.</returns>
        private object Duplicate(object source, object newContainer)
        {
            INestedUnitNamespace nestedUnitNamespace = source as INestedUnitNamespace;
            if (nestedUnitNamespace != null)
            {
                UnitNamespace containingNamespace = newContainer as UnitNamespace;
                if (containingNamespace == null)
                {
                    throw new Exception("Cannot duplicate a nested unit namespace unless the new container is a unit namespace.");
                }

                NestedUnitNamespace ret = new NestedUnitNamespace();
                ret.Copy(nestedUnitNamespace, this.host.InternFactory);
                ret.ContainingUnitNamespace = containingNamespace;
                ret.Locations = new List<ILocation>();
                return ret;
            }
            else
            {
                ITypeDefinition typeDefinition = source as ITypeDefinition;
                if (typeDefinition != null)
                {
                    INamespaceTypeDefinition namespaceTypeDefinition = source as INamespaceTypeDefinition;
                    if (namespaceTypeDefinition != null)
                    {
                        UnitNamespace containingNamespace = newContainer as UnitNamespace;
                        if (containingNamespace == null)
                        {
                            throw new Exception("Cannot duplicate a namespace type definition unless the new container is a unit namespace.");
                        }

                        NamespaceTypeDefinition ret = new NamespaceTypeDefinition();
                        ret.Copy(namespaceTypeDefinition, this.host.InternFactory);
                        ret.ContainingUnitNamespace = containingNamespace;
                        ret.Locations = new List<ILocation>();
                        return ret;
                    }
                    else
                    {
                        INestedTypeDefinition nestedTypeDefinition = source as INestedTypeDefinition;
                        if (nestedTypeDefinition == null)
                        {
                            throw new Exception("Cannot duplicate unknown type definition of type " + typeDefinition.GetType().Name);
                        }

                        TypeDefinition containingType = newContainer as TypeDefinition;
                        if (containingType == null)
                        {
                            throw new Exception("Cannot duplicate a nested type definition unless the new container is a type definition.");
                        }

                        NestedTypeDefinition ret = new NestedTypeDefinition();
                        ret.Copy(nestedTypeDefinition, this.host.InternFactory);
                        ret.ContainingTypeDefinition = containingType;
                        ret.Locations = new List<ILocation>();
                        return ret;
                    }
                }
                else
                {
                    ITypeDefinitionMember typeMemberDefinition = source as ITypeDefinitionMember;
                    if (typeMemberDefinition == null)
                    {
                        throw new Exception("Cannot duplicate object of unknown type: " + source.GetType().Name + ".");
                    }

                    TypeDefinition containingType = newContainer as TypeDefinition;
                    if (containingType == null)
                    {
                        throw new Exception("Cannot duplicate a type member definition unless the new container is a type definition.");
                    }

                    IEventDefinition eventDefinition = source as IEventDefinition;
                    if (eventDefinition != null)
                    {
                        EventDefinition ret = new EventDefinition();
                        ret.Copy(eventDefinition, this.host.InternFactory);
                        ret.ContainingTypeDefinition = containingType;
                        ret.Locations = new List<ILocation>();
                        return ret;
                    }
                    else
                    {
                        IFieldDefinition fieldDefinition = source as IFieldDefinition;
                        if (fieldDefinition != null)
                        {
                            FieldDefinition ret = new FieldDefinition();
                            ret.Copy(fieldDefinition, this.host.InternFactory);
                            ret.ContainingTypeDefinition = containingType;
                            ret.Locations = new List<ILocation>();
                            return ret;
                        }
                        else
                        {
                            IMethodDefinition methodDefinition = source as IMethodDefinition;
                            if (methodDefinition != null)
                            {
                                MethodDefinition ret = new MethodDefinition();
                                ret.Copy(methodDefinition, this.host.InternFactory);
                                ret.ContainingTypeDefinition = containingType;
                                ret.Locations = new List<ILocation>();
                                return ret;
                            }
                            else
                            {
                                INestedTypeDefinition nestedTypeDefinition = source as NestedTypeDefinition;
                                if (nestedTypeDefinition != null)
                                {
                                    NestedTypeDefinition ret = new NestedTypeDefinition();
                                    ret.Copy(nestedTypeDefinition, this.host.InternFactory);
                                    ret.ContainingTypeDefinition = containingType;
                                    ret.Locations = new List<ILocation>();
                                    return ret;
                                }
                                else
                                {
                                    IPropertyDefinition propertyDefinition = source as IPropertyDefinition;
                                    if (propertyDefinition == null)
                                    {
                                        throw new Exception("Cannot duplicate type definition member of unknown type: " + source.GetType().Name + ".");
                                    }

                                    PropertyDefinition ret = new PropertyDefinition();
                                    ret.Copy(propertyDefinition, this.host.InternFactory);
                                    ret.ContainingTypeDefinition = containingType;
                                    ret.Locations = new List<ILocation>();
                                    return ret;
                                }
                            }
                        }
                    }
                }
            }
        }

        // Reads all namespace, type, and member definitions into a dictionary
        private sealed class ReadDefinitions : BaseMetadataTraverser
        {
            private readonly Dictionary<string, object> definitions;
            private readonly TypeNameFormatter typeNameFormatter;
            private readonly SignatureFormatter signatureFormatter;
            private readonly NameFormattingOptions formattingOptions;

            public ReadDefinitions(Dictionary<string, object> definitions, TypeNameFormatter typeNameFormatter, SignatureFormatter signatureFormatter, NameFormattingOptions formattingOptions)
            {
                this.definitions = definitions;
                this.typeNameFormatter = typeNameFormatter;
                this.signatureFormatter = signatureFormatter;
                this.formattingOptions = formattingOptions;
            }

            public override void Visit(IRootUnitNamespace rootUnitNamespace)
            {
                string name = this.typeNameFormatter.GetNamespaceName(rootUnitNamespace, NameFormattingOptions.DocumentationId);
                if (!this.definitions.ContainsKey(name))
                {
                    this.definitions.Add(name, rootUnitNamespace);
                }

                base.Visit(rootUnitNamespace);
            }

            public override void Visit(INestedUnitNamespace nestedUnitNamespace)
            {
                string name = this.typeNameFormatter.GetNamespaceName(nestedUnitNamespace, NameFormattingOptions.DocumentationId);
                if (!this.definitions.ContainsKey(name))
                {
                    this.definitions.Add(name, nestedUnitNamespace);
                }

                base.Visit(nestedUnitNamespace);
            }

            public override void Visit(ITypeDefinition typeDefinition)
            {
                string name = this.typeNameFormatter.GetTypeName(typeDefinition, NameFormattingOptions.DocumentationId);
                if (!this.definitions.ContainsKey(name))
                {
                    this.definitions.Add(name, typeDefinition);
                }

                base.Visit(typeDefinition);
            }

            public override void Visit(ITypeDefinitionMember typeDefinitionMember)
            {
                string name = this.signatureFormatter.GetMemberSignature(typeDefinitionMember, NameFormattingOptions.DocumentationId);
                if (!this.definitions.ContainsKey(name))
                {
                    this.definitions.Add(name, typeDefinitionMember);
                }

                base.Visit(typeDefinitionMember);
            }
        }

        private sealed class FindDifferences : BaseMetadataTraverser
        {
            private readonly MergeAssemblies parent;

            public FindDifferences(MergeAssemblies parent)
            {
                this.parent = parent;
            }

            public override void Visit(INamespaceDefinition namespaceDefinition)
            {
                INestedUnitNamespace nestedUnitNamespace = namespaceDefinition as INestedUnitNamespace;
                if (nestedUnitNamespace == null)
                {
                    base.Visit(namespaceDefinition);
                    return;
                }

                string name = this.parent.typeNameFormatter.GetNamespaceName(nestedUnitNamespace, NameFormattingOptions.DocumentationId);
                if (!this.parent.baseAssemblyDefinitions.ContainsKey(name))
                {
                    if (this.parent.mergeInstructions.ContainsKey(name))
                    {
                        return;
                    }

                    string containingNamespaceName = this.parent.typeNameFormatter.GetNamespaceName(nestedUnitNamespace.ContainingUnitNamespace, NameFormattingOptions.DocumentationId);
                    if (!this.parent.baseAssemblyDefinitions.ContainsKey(containingNamespaceName))
                    {
                        throw new Exception("Base assembly does not have namespace definition for " + containingNamespaceName + " which contains added assembly namespace " + name + ".");
                    }

                    this.parent.mergeInstructions.Add(name, new MergeData
                    {
                        AddedAssemblyDefinition = nestedUnitNamespace,
                        BaseAssemblyInjectionPoint = this.parent.baseAssemblyDefinitions[containingNamespaceName],
                    });
                }
                else
                {
                    base.Visit(namespaceDefinition);
                }
            }

            public override void Visit(ITypeDefinition typeDefinition)
            {
                string name = this.parent.typeNameFormatter.GetTypeName(typeDefinition, NameFormattingOptions.DocumentationId);
                if (!this.parent.baseAssemblyDefinitions.ContainsKey(name))
                {
                    if (this.parent.mergeInstructions.ContainsKey(name))
                    {
                        return;
                    }

                    object injectionPoint = null;
                    INamespaceMember namespaceMember = typeDefinition as INamespaceMember;
                    if (namespaceMember != null)
                    {
                        IUnitNamespaceReference containingNamespace = namespaceMember.ContainingNamespace as IUnitNamespaceReference;
                        if (containingNamespace != null)
                        {
                            string containingNamespaceName = this.parent.typeNameFormatter.GetNamespaceName(containingNamespace, NameFormattingOptions.DocumentationId);
                            if (!this.parent.baseAssemblyDefinitions.ContainsKey(containingNamespaceName))
                            {
                                throw new Exception("Base assembly does not have namespace definition for " + containingNamespaceName + " which contains added assembly type " + name + ".");
                            }

                            injectionPoint = this.parent.baseAssemblyDefinitions[containingNamespaceName];
                        }
                    }

                    if (injectionPoint == null)
                    {
                        ITypeDefinitionMember typeMember = typeDefinition as ITypeDefinitionMember;
                        if (typeMember == null)
                        {
                            throw new Exception("Added assembly type " + name + " is not contained by a namespace or a type.");
                        }

                        string containingTypeName = this.parent.typeNameFormatter.GetTypeName(typeMember.ContainingType, NameFormattingOptions.DocumentationId);
                        if (!this.parent.baseAssemblyDefinitions.ContainsKey(containingTypeName))
                        {
                            throw new Exception("Base assembly does not have type definition for " + containingTypeName + " which contains added assembly type " + name + ".");
                        }

                        injectionPoint = this.parent.baseAssemblyDefinitions[containingTypeName];
                    }

                    this.parent.mergeInstructions.Add(name, new MergeData
                    {
                        AddedAssemblyDefinition = typeDefinition,
                        BaseAssemblyInjectionPoint = injectionPoint,
                    });
                }
                else
                {
                    base.Visit(typeDefinition);
                }
            }

            public override void Visit(ITypeDefinitionMember typeDefinitionMember)
            {
                string name = this.parent.signatureFormatter.GetMemberSignature(typeDefinitionMember, NameFormattingOptions.DocumentationId);
                if (!this.parent.baseAssemblyDefinitions.ContainsKey(name))
                {
                    if (this.parent.mergeInstructions.ContainsKey(name))
                    {
                        return;
                    }

                    string containingTypeName = this.parent.typeNameFormatter.GetTypeName(typeDefinitionMember.ContainingType, NameFormattingOptions.DocumentationId);
                    if (!this.parent.baseAssemblyDefinitions.ContainsKey(containingTypeName))
                    {
                        throw new Exception("Base assembly does not have type definition for " + containingTypeName + " which contains added assembly type " + name + ".");
                    }

                    this.parent.mergeInstructions.Add(name, new MergeData
                    {
                        AddedAssemblyDefinition = typeDefinitionMember,
                        BaseAssemblyInjectionPoint = this.parent.baseAssemblyDefinitions[containingTypeName],
                    });
                }
                else
                {
                    base.Visit(typeDefinitionMember);
                }
            }
        }
    }
}
