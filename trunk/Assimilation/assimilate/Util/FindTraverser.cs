using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Cci.MutableCodeModel;
using System.Xml;
using Microsoft.Cci;
using System.Text.RegularExpressions;

namespace assimilate
{
    public sealed class FindTraverser : BaseMetadataTraverser
    {
        private readonly SearchLocation searchLocations;

        private readonly Regex regex;

        private readonly TypeNameFormatter typeFormatter;

        private readonly FixedSignatureFormatter memberFormatter;

        private readonly NameFormattingOptions outputFormattingOptions;

        private readonly NameFormattingOptions searchFormattingOptions;

        private readonly Dictionary<string, object> alreadyPrinted;

        [Flags]
        public enum SearchLocation
        {
            NamespaceReference = 0x1,
            NamespaceDefinition = 0x2,
            TypeReference = 0x4,
            TypeDefinition = 0x8,
            MemberReference = 0x10,
            MemberDefinition = 0x20,
            Namespace = NamespaceReference | NamespaceDefinition,
            Type = TypeReference | TypeDefinition,
            Member = MemberReference | MemberDefinition,
            Reference = NamespaceReference | TypeReference | MemberReference,
            Definition = NamespaceDefinition | TypeDefinition | MemberDefinition,
            All = Reference | Definition,
        }

        public FindTraverser(Regex regex, SearchLocation searchLocations, NameFormattingOptions searchFormattingOptions, NameFormattingOptions outputFormattingOptions)
        {
            this.searchLocations = searchLocations;
            this.regex = regex;
            this.typeFormatter = new FixedTypeNameFormatter();
            this.memberFormatter = new FixedSignatureFormatter(this.typeFormatter);
            this.searchFormattingOptions = searchFormattingOptions;
            this.outputFormattingOptions = outputFormattingOptions;
            this.alreadyPrinted = new Dictionary<string, object>();
        }

        public override void Visit(IRootUnitNamespaceReference rootUnitNamespaceReference)
        {
            if (this.Searching(SearchLocation.NamespaceReference) && this.IsMatch(rootUnitNamespaceReference))
            {
                this.DisplayResult(this.GetLocation() + " has reference to namespace " + this.Format(rootUnitNamespaceReference));
            }

            base.Visit(rootUnitNamespaceReference);
        }

        public override void Visit(INestedUnitNamespaceReference nestedUnitNamespaceReference)
        {
            if (this.Searching(SearchLocation.NamespaceReference) && this.IsMatch(nestedUnitNamespaceReference))
            {
                this.DisplayResult(this.GetLocation() + " has reference to namespace " + this.Format(nestedUnitNamespaceReference));
            }

            base.Visit(nestedUnitNamespaceReference);
        }

        public override void Visit(IRootUnitNamespace rootUnitNamespace)
        {
            if (this.Searching(SearchLocation.NamespaceDefinition) && this.IsMatch(rootUnitNamespace))
            {
                this.DisplayResult(this.Format(rootUnitNamespace));
            }

            base.Visit(rootUnitNamespace);
        }

        public override void Visit(INestedUnitNamespace nestedUnitNamespace)
        {
            if (this.Searching(SearchLocation.NamespaceDefinition) && this.IsMatch(nestedUnitNamespace))
            {
                this.DisplayResult(this.Format(nestedUnitNamespace));
            }

            base.Visit(nestedUnitNamespace);
        }

        public override void Visit(ITypeReference typeReference)
        {
            if (this.Searching(SearchLocation.TypeReference) && this.IsMatch(typeReference))
            {
                this.DisplayResult(this.GetLocation() + " has reference to type " + this.Format(typeReference));
            }

            base.Visit(typeReference);
        }

        public override void Visit(ITypeDefinition typeDefinition)
        {
            if (this.Searching(SearchLocation.TypeDefinition) && this.IsMatch(typeDefinition))
            {
                this.DisplayResult(this.Format(typeDefinition));
            }

            base.Visit(typeDefinition);
        }

        public override void Visit(ITypeMemberReference typeMemberReference)
        {
            if (this.Searching(SearchLocation.MemberReference) && this.IsMatch(typeMemberReference))
            {
                this.DisplayResult(this.GetLocation() + " has reference to member " + this.Format(typeMemberReference));
            }

            base.Visit(typeMemberReference);
        }

        public override void Visit(ITypeDefinitionMember typeMember)
        {
            if (this.Searching(SearchLocation.MemberDefinition) && this.IsMatch(typeMember))
            {
                this.DisplayResult(this.Format(typeMember));
            }

            base.Visit(typeMember);
        }

        private void DisplayResult(string location)
        {
            if (!this.alreadyPrinted.ContainsKey(location))
            {
                Console.WriteLine(location);
                this.alreadyPrinted.Add(location, null);
            }
        }

        private string GetLocation()
        {
            foreach (var step in this.path)
            {
                var member = step as ITypeDefinitionMember;
                if (member != null)
                {
                    return this.Format(member);
                }

                var type = step as ITypeDefinition;
                if (type != null)
                {
                    return this.Format(type);
                }

                var ns = step as IUnitSetNamespace;
                if (ns != null)
                {
                    return this.Format(ns);
                }
            }

            return "Assembly";
        }

        private bool Searching(SearchLocation location)
        {
            return (this.searchLocations & location) == location;
        }

        private bool IsMatch(ITypeReference typeReference)
        {
            return this.regex.IsMatch(this.typeFormatter.GetTypeName(typeReference, this.searchFormattingOptions));
        }

        private bool IsMatch(ITypeMemberReference typeMemberReference)
        {
            return this.regex.IsMatch(this.memberFormatter.GetMemberSignature(typeMemberReference, this.searchFormattingOptions));
        }

        private bool IsMatch(IUnitNamespaceReference unitNamespaceReference)
        {
            return this.regex.IsMatch(this.typeFormatter.GetNamespaceName(unitNamespaceReference, this.searchFormattingOptions));
        }

        private bool IsMatch(IUnitSetNamespace unitSetNamespace)
        {
            return this.regex.IsMatch(this.typeFormatter.GetNamespaceName(unitSetNamespace, this.searchFormattingOptions));
        }

        private string Format(ITypeReference type)
        {
            return this.typeFormatter.GetTypeName(type, this.outputFormattingOptions);
        }

        private string Format(ITypeMemberReference member)
        {
            return this.memberFormatter.GetMemberSignature(member, this.outputFormattingOptions);
        }

        private string Format(IUnitSetNamespace unitSetNamespace)
        {
            return this.typeFormatter.GetNamespaceName(unitSetNamespace, this.outputFormattingOptions);
        }

        private string Format(IUnitNamespaceReference unitNamespaceReference)
        {
            return this.typeFormatter.GetNamespaceName(unitNamespaceReference, this.outputFormattingOptions);
        }
    }
}
