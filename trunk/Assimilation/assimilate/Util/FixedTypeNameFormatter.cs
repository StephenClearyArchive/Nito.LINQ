using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Cci;

namespace assimilate
{
    public sealed class FixedTypeNameFormatter : TypeNameFormatter
    {
        // Fixed: honors NameFormattingOptions.MemberKind
        public override string GetNamespaceName(IUnitNamespaceReference unitNamespace, NameFormattingOptions formattingOptions)
        {
            if ((formattingOptions & NameFormattingOptions.DocumentationIdMemberKind) != 0)
            {
                return "N:" + base.GetNamespaceName(unitNamespace, formattingOptions & ~NameFormattingOptions.DocumentationIdMemberKind & ~NameFormattingOptions.MemberKind);
            }
            else if ((formattingOptions & NameFormattingOptions.MemberKind) != 0)
            {
                return "namespace " + base.GetNamespaceName(unitNamespace, formattingOptions & ~NameFormattingOptions.MemberKind);
            }
            else
            {
                return base.GetNamespaceName(unitNamespace, formattingOptions);
            }
        }

        // Fixed: honors NameFormattingOptions.MemberKind
        public override string GetNamespaceName(IUnitSetNamespace namespaceDefinition, NameFormattingOptions formattingOptions)
        {
            if ((formattingOptions & NameFormattingOptions.DocumentationIdMemberKind) != 0)
            {
                return "N:" + base.GetNamespaceName(namespaceDefinition, formattingOptions & ~NameFormattingOptions.DocumentationIdMemberKind & ~NameFormattingOptions.MemberKind);
            }
            else if ((formattingOptions & NameFormattingOptions.MemberKind) != 0)
            {
                return "namespace " + base.GetNamespaceName(namespaceDefinition, formattingOptions & ~NameFormattingOptions.MemberKind);
            }
            else
            {
                return base.GetNamespaceName(namespaceDefinition, formattingOptions);
            }
        }

        // Fixed: honors NameFormattingOptions.DocumentationIdMemberKind and NameFormattingOptions.MemberKind
        protected override string GetNamespaceTypeName(INamespaceTypeReference nsType, NameFormattingOptions formattingOptions)
        {
            if ((formattingOptions & NameFormattingOptions.DocumentationIdMemberKind) != 0)
            {
                return "T:" + base.GetNamespaceTypeName(nsType, formattingOptions & ~NameFormattingOptions.DocumentationIdMemberKind & ~NameFormattingOptions.MemberKind);
            }
            else if ((formattingOptions & NameFormattingOptions.MemberKind) != 0)
            {
                return this.GetTypeKind(nsType) + " " + base.GetNamespaceTypeName(nsType, formattingOptions & ~NameFormattingOptions.MemberKind);
            }
            else
            {
                return base.GetNamespaceTypeName(nsType, formattingOptions);
            }
        }
    }
}
