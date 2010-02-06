using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Cci;

namespace assimilate
{
    public static class MetadataExtensions
    {
        public static bool MayBeExposed(this TypeMemberVisibility visibility)
        {
            return visibility != TypeMemberVisibility.Private && visibility != TypeMemberVisibility.Other &&
                visibility != TypeMemberVisibility.FamilyAndAssembly && visibility != TypeMemberVisibility.Assembly;
        }

        public static bool IsExposed(this ITypeDefinition type)
        {
            while (type != null)
            {
                INestedTypeDefinition nestedType = type as INestedTypeDefinition;
                if (nestedType == null)
                {
                    return type.TypeVisibilityAsTypeMemberVisibility().MayBeExposed();
                }

                if (!nestedType.TypeVisibilityAsTypeMemberVisibility().MayBeExposed())
                {
                    return false;
                }

                type = nestedType.ContainingType.ResolvedType;
            }

            return true;
        }

        public static bool IsExposed(this ITypeDefinitionMember member)
        {
            if (!member.Visibility.MayBeExposed())
            {
                return false;
            }

            return member.ContainingType.ResolvedType.IsExposed();
        }

        public static bool IsExposed(this INestedTypeDefinition type)
        {
            return ((ITypeDefinition)type).IsExposed();
        }

        public static TypeMemberVisibility TypeVisibilityAsTypeMemberVisibility(this ITypeDefinition type)
        {
            return TypeHelper.TypeVisibilityAsTypeMemberVisibility(type);
        }
    }
}
