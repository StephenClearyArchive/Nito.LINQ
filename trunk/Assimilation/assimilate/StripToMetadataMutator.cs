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

        public StripToMetadata(IMetadataHost host)
        {
            this.host = host;
            this.requiredMethods = new List<IMethodDefinition>();
        }

        public static Module Run(IMetadataHost host, IModule module)
        {
            Module mutableModule = module as Module;
            if (mutableModule == null)
            {
                return Run(host, new MetadataMutator(host).Visit(module));
            }
            else
            {
                return Run(host, mutableModule);
            }
        }

        public static Module Run(IMetadataHost host, Module module)
        {
            var parent = new StripToMetadata(host);
            module = new Pass1(parent).Visit(module);
            module = new Pass2(parent).Visit(module);
            return module;
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

            public override List<ILocalDefinition> Visit(List<ILocalDefinition> locals)
            {
                // Strip local variables
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
                // Strip private or internal fields
                return base.Visit(fieldDefinitions.Where(x => x.IsExposed()).ToList());
            }

            public override List<IPropertyDefinition> Visit(List<IPropertyDefinition> propertyDefinitions)
            {
                // Strip private or internal properties
                return base.Visit(propertyDefinitions.Where(x => x.IsExposed()).ToList());
            }

            public override PropertyDefinition Visit(PropertyDefinition propertyDefinition)
            {
                this.parent.requiredMethods.AddRange(propertyDefinition.Accessors.Select(x => x.ResolvedMethod));
                return base.Visit(propertyDefinition);
            }

            public override List<IEventDefinition> Visit(List<IEventDefinition> eventDefinitions)
            {
                // Strip private or internal events
                return base.Visit(eventDefinitions.Where(x => x.IsExposed()).ToList());
            }

            public override EventDefinition Visit(EventDefinition eventDefinition)
            {
                this.parent.requiredMethods.AddRange(eventDefinition.Accessors.Select(x => x.ResolvedMethod));
                return base.Visit(eventDefinition);
            }
        }

        // Pass 2: remove non-exposed methods, except for the important ones
        private sealed class Pass2 : MetadataMutator
        {
            private readonly StripToMetadata parent;

            private IMethodDefinition entryPoint;

            public Pass2(StripToMetadata parent)
                : base(parent.host, true)
            {
                this.parent = parent;
            }

            public override Module Visit(IModule module)
            {
                this.entryPoint = module.EntryPoint.ResolvedMethod;
                return base.Visit(module);
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
                    x == this.entryPoint ||
                    // Allow important methods to remain
                    this.parent.requiredMethods.Contains(x)
                    ).ToList());
            }
        }
    }
}
