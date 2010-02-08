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

        public MergeAssemblies(IMetadataHost host)
        {
            this.host = host;
            this.typeNameFormatter = new FixedTypeNameFormatter();
            this.signatureFormatter = new FixedSignatureFormatter(this.typeNameFormatter);
            this.baseAssemblyDefinitions = new Dictionary<string, object>();
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
            new ReadDefinitions(parent).Visit(baseAssembly);
            //            new FindDifferences(parent, baseAssembly).Visit(addedAssembly);
            //            baseAssembly = new MergeDefinitions(parent).Visit(baseAssembly);
            return baseAssembly;
        }

        // Reads all namespace, type, and member definitions into a dictionary
        private sealed class ReadDefinitions : BaseMetadataTraverser
        {
            private readonly MergeAssemblies parent;

            public ReadDefinitions(MergeAssemblies parent)
            {
                this.parent = parent;
            }

            public override void Visit(IRootUnitNamespace rootUnitNamespace)
            {
                string name = this.parent.typeNameFormatter.GetNamespaceName(rootUnitNamespace, NameFormattingOptions.DocumentationId);
                if (!this.parent.baseAssemblyDefinitions.ContainsKey(name))
                {
                    this.parent.baseAssemblyDefinitions.Add(name, rootUnitNamespace);
                }

                base.Visit(rootUnitNamespace);
            }

            public override void Visit(INestedUnitNamespace nestedUnitNamespace)
            {
                string name = this.parent.typeNameFormatter.GetNamespaceName(nestedUnitNamespace, NameFormattingOptions.DocumentationId);
                if (!this.parent.baseAssemblyDefinitions.ContainsKey(name))
                {
                    this.parent.baseAssemblyDefinitions.Add(name, nestedUnitNamespace);
                }

                base.Visit(nestedUnitNamespace);
            }

            public override void Visit(ITypeDefinition typeDefinition)
            {
                string name = this.parent.typeNameFormatter.GetTypeName(typeDefinition, NameFormattingOptions.DocumentationId);
                if (!this.parent.baseAssemblyDefinitions.ContainsKey(name))
                {
                    this.parent.baseAssemblyDefinitions.Add(name, typeDefinition);
                }

                base.Visit(typeDefinition);
            }

            public override void Visit(ITypeDefinitionMember typeDefinitionMember)
            {
                string name = this.parent.signatureFormatter.GetMemberSignature(typeDefinitionMember, NameFormattingOptions.DocumentationId);
                if (!this.parent.baseAssemblyDefinitions.ContainsKey(name))
                {
                    this.parent.baseAssemblyDefinitions.Add(name, typeDefinitionMember);
                }

                base.Visit(typeDefinitionMember);
            }
        }
    }
}
