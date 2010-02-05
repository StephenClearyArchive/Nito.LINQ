using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Cci;

namespace assimilate
{
    public static class MetadataExtensions
    {
        public static bool IsExposed(this ITypeDefinitionMember member)
        {
            return member.Visibility != TypeMemberVisibility.Private && member.Visibility != TypeMemberVisibility.Other &&
                member.Visibility != TypeMemberVisibility.FamilyAndAssembly && member.Visibility != TypeMemberVisibility.Assembly;
        }
    }
}
