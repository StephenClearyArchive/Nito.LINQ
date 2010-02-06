using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Cci;
using Microsoft.Cci.MutableCodeModel;

namespace assimilate
{
    public sealed class StripToMetadata
    {
        private readonly IMetadataHost host;
        private readonly List<IMethodDefinition> requiredMethods;
        private IMethodDefinition entryPoint;

        public StripToMetadata(IMetadataHost host)
        {
            this.host = host;
            this.requiredMethods = new List<IMethodDefinition>();
        }

        public static Assembly Run(IMetadataHost host, IAssembly assembly)
        {
            Assembly mutableModule = assembly as Assembly;
            if (mutableModule == null)
            {
                return Run(host, new MetadataMutator(host).Visit(assembly));
            }
            else
            {
                return Run(host, mutableModule);
            }
        }

        public static Assembly Run(IMetadataHost host, Assembly assembly)
        {
            var parent = new StripToMetadata(host);
            assembly = new Pass1(parent).Visit(assembly);
            assembly = new Pass2(parent).Visit(assembly);
            return assembly;
        }

        private static string DumpStack()
        {
            var trace = new System.Diagnostics.StackTrace();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            foreach (var frame in trace.GetFrames())
            {
                var method = frame.GetMethod();
                if (method.Name.Equals("DumpStack")) continue;
                sb.AppendLine(method.ToString());
            }

            return sb.ToString();
        }

        // Pass 1:
        //   Strip non-exposed properties and events
        //   Mark remaining accessors as "important"
        //   Remove method bodies and implementations
        //   Strip non-exposed fields
        private sealed class Pass1 : MetadataMutator
        {
            private readonly StripToMetadata parent;

            public Pass1(StripToMetadata parent)
                : base(parent.host, true)
            {
                this.parent = parent;
            }

            public override Module Visit(Module module)
            {
                this.parent.entryPoint = module.EntryPoint.ResolvedMethod;
                return base.Visit(module);
            }

            public override List<IResourceReference> Visit(List<IResourceReference> resourceReferences)
            {
                // Remove all references to resources
                return base.Visit(new List<IResourceReference>(0));
            }

            public override List<INamespaceMember> Visit(List<INamespaceMember> namespaceMembers)
            {
                // Strip private/internal non-nested types
                return base.Visit(namespaceMembers.Where(x => 
                    {
                        INamespaceTypeDefinition type = x as INamespaceTypeDefinition;
                        return type == null || type.IsExposed() || TypeContainsEntryPoint(type, this.parent.entryPoint);
                    }).ToList());
            }

            public override List<INestedTypeDefinition> Visit(List<INestedTypeDefinition> nestedTypeDefinitions)
            {
                // Strip private/internal nested types
                return base.Visit(nestedTypeDefinitions.Where(x => x.IsExposed() || TypeContainsEntryPoint(x, this.parent.entryPoint)).ToList());
            }

            public override List<ITypeReference> Visit(List<ITypeReference> typeReferences)
            {
                // Remove references to private/internal types from type references
                return base.Visit(typeReferences.Where(x => x.ResolvedType.IsExposed()).ToList());
            }

            public override List<ICustomAttribute> Visit(List<ICustomAttribute> customAttributes)
            {
                // Remove references to private/internal types from custom attributes
                return base.Visit(customAttributes.Where(x => x.Type.ResolvedType.IsExposed()).ToList());
            }

            public override List<ILocalDefinition> Visit(List<ILocalDefinition> locals)
            {
                // Strip all local variables
                return base.Visit(new List<ILocalDefinition>(0));
            }

            public override List<IOperation> Visit(List<IOperation> operations)
            {
                // Strip all actual code
                return base.Visit(new List<IOperation>(0));
            }

            public override List<IMethodImplementation> Visit(List<IMethodImplementation> methodImplementations)
            {
                // Strip all defined method implementations
                return base.Visit(new List<IMethodImplementation>(0));
            }

            public override List<IFieldDefinition> Visit(List<IFieldDefinition> fieldDefinitions)
            {
                // Strip private/internal fields
                return base.Visit(fieldDefinitions.Where(x => x.IsExposed()).ToList());
            }

            public override List<IPropertyDefinition> Visit(List<IPropertyDefinition> propertyDefinitions)
            {
                // Strip private/internal properties
                return base.Visit(propertyDefinitions.Where(x => x.IsExposed()).ToList());
            }

            public override PropertyDefinition Visit(PropertyDefinition propertyDefinition)
            {
                // Mark property accessors as required
                this.parent.requiredMethods.AddRange(propertyDefinition.Accessors.Select(x => x.ResolvedMethod));
                return base.Visit(propertyDefinition);
            }

            public override List<IEventDefinition> Visit(List<IEventDefinition> eventDefinitions)
            {
                // Strip private/internal events
                return base.Visit(eventDefinitions.Where(x => x.IsExposed()).ToList());
            }

            public override EventDefinition Visit(EventDefinition eventDefinition)
            {
                // Mark event accessors as required
                this.parent.requiredMethods.AddRange(eventDefinition.Accessors.Select(x => x.ResolvedMethod));
                return base.Visit(eventDefinition);
            }

            private static bool TypeContainsEntryPoint(ITypeDefinition type, IMethodDefinition entryPoint)
            {
                return type.ContainingTypesAndSelf().Contains(entryPoint.ContainingTypeDefinition);
            }
        }

        // Pass 2: remove non-exposed methods, except for the important ones
        private sealed class Pass2 : MetadataMutator
        {
            private readonly StripToMetadata parent;

            public Pass2(StripToMetadata parent)
                : base(parent.host, true)
            {
                this.parent = parent;
            }

            public override List<IMethodDefinition> Visit(List<IMethodDefinition> methodDefinitions)
            {
                if (this.stopTraversal)
                {
                    return methodDefinitions;
                }

                // Strip private or internal methods
                return base.Visit(methodDefinitions.Where(x =>
                    x.IsExposed() ||
                    // Allow the module entry point to be private
                    x == this.parent.entryPoint ||
                    // Allow important methods to remain
                    this.parent.requiredMethods.Contains(x)
                    ).ToList());
            }
        }
    }
}
