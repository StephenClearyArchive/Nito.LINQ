using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Cci;

namespace assimilate
{
    public sealed class FixedTypeNameFormatter : TypeNameFormatter
    {
        /// <summary>
        /// Returns a C#-like string that corresponds to the given referenced namespace definition and that conforms to the specified formatting options.
        /// Fixes the base method by honoring <see cref="NameFormattingOptions.DocumentationIdMemberKind"/> and <see cref="NameFormattingOptions.MemberKind"/>.
        /// </summary>
        /// <param name="unitNamespaceReference">The namespace reference whose name is returned.</param>
        /// <param name="formattingOptions">The options for formatting.</param>
        /// <returns>The namespace name.</returns>
        public override string GetNamespaceName(IUnitNamespaceReference unitNamespaceReference, NameFormattingOptions formattingOptions)
        {
            string ret = this.GetNamespaceNameWithoutKind(unitNamespaceReference, formattingOptions);
            if ((formattingOptions & NameFormattingOptions.DocumentationIdMemberKind) != 0)
            {
                ret = "N:" + ret;
            }
            else if ((formattingOptions & NameFormattingOptions.MemberKind) != 0)
            {
                ret = "namespace " + ret;
            }

            return ret;
        }

        /// <summary>
        /// Returns a C#-like string that corresponds to the given unit set namespace definition and that conforms to the specified formatting options.
        /// Fixes the base method by honoring <see cref="NameFormattingOptions.DocumentationIdMemberKind"/> and <see cref="NameFormattingOptions.MemberKind"/>.
        /// </summary>
        /// <param name="namespaceDefinition">The namespace definition whose name is returned.</param>
        /// <param name="formattingOptions">The options for formatting.</param>
        /// <returns>The namespace name.</returns>
        public override string GetNamespaceName(IUnitSetNamespace namespaceDefinition, NameFormattingOptions formattingOptions)
        {
            string ret = this.GetNamespaceNameWithoutKind(namespaceDefinition, formattingOptions);
            if ((formattingOptions & NameFormattingOptions.DocumentationIdMemberKind) != 0)
            {
                ret = "N:" + ret;
            }
            else if ((formattingOptions & NameFormattingOptions.MemberKind) != 0)
            {
                ret = "namespace " + ret;
            }

            return ret;
        }

        /// <summary>
        /// Returns a C#-like string that corresponds to the given type definition and that conforms to the specified formatting options.
        /// Fixes the base method by honoring <see cref="NameFormattingOptions.DocumentationIdMemberKind"/> and <see cref="NameFormattingOptions.MemberKind"/> correctly.
        /// </summary>
        /// <param name="nsType">The namespace type reference whose name is returned.</param>
        /// <param name="formattingOptions">The options for formatting; <see cref="NameFormattingOptions.DocumentationIdMemberKind"/> and <see cref="NameFormattingOptions.MemberKind"/> are ignored.</param>
        /// <returns>A possibly-qualified type name.</returns>
        protected override string GetNamespaceTypeName(INamespaceTypeReference nsType, NameFormattingOptions formattingOptions)
        {
            string ret = this.GetNamespaceTypeNameWithoutKind(nsType, formattingOptions);
            if ((formattingOptions & NameFormattingOptions.DocumentationIdMemberKind) != 0)
            {
                ret = "T:" + ret;
            }
            else if ((formattingOptions & NameFormattingOptions.MemberKind) != 0)
            {
                ret = this.GetTypeKind(nsType) + " " + ret;
            }

            return ret;
        }

        /// <summary>
        /// Gets a possibly-qualified namespace name. Identical to TypeNameFormatter.GetNamespaceName at the time of writing (duplicated to prevent their bug fixes from messing up our bug fixes).
        /// </summary>
        /// <param name="unitNamespaceReference">The namespace reference whose name is returned.</param>
        /// <param name="formattingOptions">The options for formatting; <see cref="NameFormattingOptions.DocumentationIdMemberKind"/> and <see cref="NameFormattingOptions.MemberKind"/> are ignored.</param>
        /// <returns>A possibly-qualified namespace name.</returns>
        private string GetNamespaceNameWithoutKind(IUnitNamespaceReference unitNamespaceReference, NameFormattingOptions formattingOptions)
        {
            INestedUnitNamespaceReference nestedUnitNamespace = unitNamespaceReference as INestedUnitNamespaceReference;
            if (nestedUnitNamespace == null)
            {
                // Root namespace
                return string.Empty;
            }

            if (nestedUnitNamespace.ContainingUnitNamespace is IRootUnitNamespaceReference || (formattingOptions & NameFormattingOptions.OmitContainingNamespace) != 0)
            {
                return nestedUnitNamespace.Name.Value;
            }
            else
            {
                return this.GetNamespaceNameWithoutKind(nestedUnitNamespace.ContainingUnitNamespace, formattingOptions) + "." + nestedUnitNamespace.Name.Value;
            }
        }

        /// <summary>
        /// Gets a possibly-qualified namespace name. Identical to TypeNameFormatter.GetNamespaceName at the time of writing (duplicated to prevent their bug fixes from messing up our bug fixes).
        /// </summary>
        /// <param name="namespaceDefinition">The namespace definition whose name is returned.</param>
        /// <param name="formattingOptions">The options for formatting; <see cref="NameFormattingOptions.DocumentationIdMemberKind"/> and <see cref="NameFormattingOptions.MemberKind"/> are ignored.</param>
        /// <returns>A possibly-qualified namespace name.</returns>
        private string GetNamespaceNameWithoutKind(IUnitSetNamespace namespaceDefinition, NameFormattingOptions formattingOptions)
        {
            INestedUnitSetNamespace nestedUnitSetNamespace = namespaceDefinition as INestedUnitSetNamespace;
            if (nestedUnitSetNamespace == null)
            {
                return namespaceDefinition.Name.Value;
            }

            if (nestedUnitSetNamespace.ContainingNamespace.Name.Value.Length == 0 || (formattingOptions & NameFormattingOptions.OmitContainingNamespace) != 0)
            {
                return nestedUnitSetNamespace.Name.Value;
            }
            else
            {
                return this.GetNamespaceNameWithoutKind(nestedUnitSetNamespace.ContainingUnitSetNamespace, formattingOptions) + "." + nestedUnitSetNamespace.Name.Value;
            }
        }

        /// <summary>
        /// Gets a possibly-qualified name for a namespace type reference. Similar to TypeNameFormatter.GetNamespaceTypeName (duplicated to prevent their bug fixes from messing up our bug fixes).
        /// </summary>
        /// <param name="nsType">The namespace type reference whose name is returned.</param>
        /// <param name="formattingOptions">The options for formatting; <see cref="NameFormattingOptions.DocumentationIdMemberKind"/> and <see cref="NameFormattingOptions.MemberKind"/> are ignored.</param>
        /// <returns>A possibly-qualified type name.</returns>
        private string GetNamespaceTypeNameWithoutKind(INamespaceTypeReference nsType, NameFormattingOptions formattingOptions)
        {
            string ret = this.AddGenericParametersIfNeeded(nsType, nsType.GenericParameterCount, formattingOptions, nsType.Name.Value);
            if ((formattingOptions & NameFormattingOptions.OmitContainingNamespace) == 0 && !(nsType.ContainingUnitNamespace is IRootUnitNamespaceReference))
            {
                ret = this.GetNamespaceNameWithoutKind(nsType.ContainingUnitNamespace, formattingOptions) + "." + ret;
            }

            return ret;
        }
    }
}
