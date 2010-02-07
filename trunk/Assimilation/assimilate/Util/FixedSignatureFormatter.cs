using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Cci;

namespace assimilate
{
    public sealed class FixedSignatureFormatter : SignatureFormatter
    {
        private TypeNameFormatter typeNameFormatter;

        public FixedSignatureFormatter(TypeNameFormatter typeNameFormatter)
            : base(typeNameFormatter)
        {
            this.typeNameFormatter = typeNameFormatter;
        }

        // Fixed:
        //  Property translates .ctor, Finalize, add_*, remove_*
        protected override void AppendMethodName(IMethodReference method, NameFormattingOptions formattingOptions, StringBuilder sb)
        {
            if ((formattingOptions & NameFormattingOptions.DocumentationIdMemberKind) != 0)
            {
                sb.Append("M:");
            }
            if ((formattingOptions & NameFormattingOptions.OmitContainingType) == 0)
            {
                sb.Append(this.typeNameFormatter.GetTypeName(method.ContainingType, ParentFullyQualifiedNameFormattingOptions(formattingOptions)));
                sb.Append('.');
            }
            // Special name translation
            string methodName = method.Name.Value;
            if ((formattingOptions & NameFormattingOptions.FormattingForDocumentationId) != 0) methodName = this.MapToDocumentationIdName(methodName);
            if ((formattingOptions & NameFormattingOptions.PreserveSpecialNames) == 0)
            {
                if (method.ResolvedMethod.IsSpecialName)
                {
                    if (methodName.StartsWith("get_"))
                    {
                        //^ assume methodName.Length >= 4;
                        sb.Append(methodName.Substring(4));
                        sb.Append(".get");
                    }
                    else if (methodName.StartsWith("set_"))
                    {
                        //^ assume methodName.Length >= 4;
                        sb.Append(methodName.Substring(4));
                        sb.Append(".set");
                    }
                    else if (methodName.StartsWith("add_"))
                    {
                        sb.Append(methodName.Substring(4));
                        sb.Append(".add");
                    }
                    else if (methodName.StartsWith("remove_"))
                    {
                        sb.Append(methodName.Substring(7));
                        sb.Append(".remove");
                    }
                    else if (methodName == ".ctor")
                    {
                        sb.Append(this.typeNameFormatter.GetTypeName(method.ContainingType, ParentClassNameFormattingOptions(formattingOptions)));
                    }
                    else
                    {
                        sb.Append(methodName);
                    }
                }
                else if (methodName == "Finalize")
                {
                    sb.Append("~" + this.typeNameFormatter.GetTypeName(method.ContainingType, ParentClassNameFormattingOptions(formattingOptions)));
                }
                else
                {
                    sb.Append(methodName);
                }
            }
            else
            {
                sb.Append(methodName);
            }
        }

        // Fixed: honors NameFormattingOptions.MemberKind
        public override string GetEventSignature(IEventDefinition eventDef, NameFormattingOptions formattingOptions)
        {
            if (ShouldPrependKind(formattingOptions))
            {
                return "event " + base.GetEventSignature(eventDef, formattingOptions & ~NameFormattingOptions.MemberKind);
            }
            else
            {
                return base.GetEventSignature(eventDef, formattingOptions);
            }
        }

        // Fixed: honors NameFormattingOptions.MemberKind
        public override string GetFieldSignature(IFieldReference field, NameFormattingOptions formattingOptions)
        {
            if (ShouldPrependKind(formattingOptions))
            {
                return "field " + base.GetFieldSignature(field, formattingOptions & ~NameFormattingOptions.MemberKind);
            }
            else
            {
                return base.GetFieldSignature(field, formattingOptions);
            }
        }

        // Fixed: honors NameFormattingOptions.MemberKind
        public override string GetMethodSignature(IMethodReference method, NameFormattingOptions formattingOptions)
        {
            if (ShouldPrependKind(formattingOptions))
            {
                return "method " + base.GetMethodSignature(method, formattingOptions & ~NameFormattingOptions.MemberKind);
            }
            else
            {
                return base.GetMethodSignature(method, formattingOptions);
            }
        }

        // Fixed: honors NameFormattingOptions.MemberKind
        public override string GetPropertySignature(IPropertyDefinition property, NameFormattingOptions formattingOptions)
        {
            if (ShouldPrependKind(formattingOptions))
            {
                return "property " + base.GetPropertySignature(property, formattingOptions & ~NameFormattingOptions.MemberKind);
            }
            else
            {
                return base.GetPropertySignature(property, formattingOptions);
            }
        }

        private static NameFormattingOptions ParentClassNameFormattingOptions(NameFormattingOptions signatureFormattingOptions)
        {
            NameFormattingOptions ret = signatureFormattingOptions;
            
            // Strip MemberKind's
            ret &= ~NameFormattingOptions.DocumentationIdMemberKind;
            ret &= ~NameFormattingOptions.MemberKind;

            // Strip everything but the class name
            ret |= NameFormattingOptions.OmitContainingNamespace;
            ret |= NameFormattingOptions.OmitContainingType;

            return ret;
        }

        private static NameFormattingOptions ParentFullyQualifiedNameFormattingOptions(NameFormattingOptions signatureFormattingOptions)
        {
            NameFormattingOptions ret = signatureFormattingOptions;

            // Strip MemberKind's
            ret &= ~NameFormattingOptions.DocumentationIdMemberKind;
            ret &= ~NameFormattingOptions.MemberKind;

            return ret;
        }

        private static bool ShouldPrependKind(NameFormattingOptions formattingOptions)
        {
            if ((formattingOptions & NameFormattingOptions.DocumentationIdMemberKind) != 0)
            {
                return false;
            }

            return (formattingOptions & NameFormattingOptions.MemberKind) != 0;
        }

        private delegate void FixedDelegate();

        private struct FixedValueType { }

        private class FixedReferenceType { };

        private class FixedGenericType<T> { };

        private event FixedDelegate FixedEvent;

        private int FixedProperty { get; set; }

        private int FixedField;

        ~FixedSignatureFormatter() { }
    }
}
