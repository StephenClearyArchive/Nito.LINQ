using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Cci.MutableCodeModel;
using System.Xml;
using Microsoft.Cci;

namespace assimilate
{
    public sealed class DumpMutator : MetadataMutator
    {
        private XmlWriter output;

        public DumpMutator(IMetadataHost host, XmlWriter output)
            : base(host, true)
        {
            this.output = output;
        }

        public override Assembly Visit(Assembly assembly)
        {
            this.output.WriteStartElement("Assembly");
            this.output.WriteAttributeString("UnifiedAssemblyIdentity", assembly.UnifiedAssemblyIdentity.ToString());
            var ret = base.Visit(assembly);
            this.output.WriteEndElement();
            return ret;
        }

        public override CustomAttribute Visit(CustomAttribute customAttribute)
        {
            this.output.WriteStartElement("CustomAttribute");
            this.output.WriteAttributeString("Type", customAttribute.Type.ToString());
            var ret = base.Visit(customAttribute);
            this.output.WriteEndElement();
            return ret;
        }

        public override EventDefinition Visit(EventDefinition eventDefinition)
        {
            this.output.WriteStartElement("EventDefinition");
            this.output.WriteAttributeString("Name", eventDefinition.Name.ToString());
            this.output.WriteAttributeString("Type", eventDefinition.Type.ToString());
            var ret = base.Visit(eventDefinition);
            this.output.WriteEndElement();
            return ret;
        }

        public override FieldDefinition Visit(FieldDefinition fieldDefinition)
        {
            this.output.WriteStartElement("FieldDefinition");
            this.output.WriteAttributeString("Name", fieldDefinition.Name.ToString());
            this.output.WriteAttributeString("Type", fieldDefinition.Type.ToString());
            var ret = base.Visit(fieldDefinition);
            this.output.WriteEndElement();
            return ret;
        }

        public override FieldReference Visit(FieldReference fieldReference)
        {
            this.output.WriteStartElement("FieldReference");
            this.output.WriteAttributeString("Name", fieldReference.Name.ToString());
            this.output.WriteAttributeString("Type", fieldReference.Type.ToString());
            var ret = base.Visit(fieldReference);
            this.output.WriteEndElement();
            return ret;
        }

        public override FileReference Visit(FileReference fileReference)
        {
            this.output.WriteStartElement("FileReference");
            this.output.WriteAttributeString("FileName", fileReference.FileName.ToString());
            var ret = base.Visit(fileReference);
            this.output.WriteEndElement();
            return ret;
        }

        public override FunctionPointerTypeReference Visit(FunctionPointerTypeReference functionPointerTypeReference)
        {
            this.output.WriteStartElement("FunctionPointerTypeReference");
            this.output.WriteAttributeString("ResolvedType", functionPointerTypeReference.ResolvedType.ToString());
            var ret = base.Visit(functionPointerTypeReference);
            this.output.WriteEndElement();
            return ret;
        }

        public override GenericMethodParameter Visit(GenericMethodParameter genericMethodParameter)
        {
            this.output.WriteStartElement("GenericMethodParameter");
            this.output.WriteAttributeString("Name", genericMethodParameter.Name.ToString());
            var ret = base.Visit(genericMethodParameter);
            this.output.WriteEndElement();
            return ret;
        }

        public override GenericParameter Visit(GenericParameter genericParameter)
        {
            this.output.WriteStartElement("GenericParameter");
            this.output.WriteAttributeString("Name", genericParameter.Name.ToString());
            var ret = base.Visit(genericParameter);
            this.output.WriteEndElement();
            return ret;
        }

        public override GenericTypeInstanceReference Visit(GenericTypeInstanceReference genericTypeInstanceReference)
        {
            this.output.WriteStartElement("GenericTypeInstanceReference");
            this.output.WriteAttributeString("ResolvedType", genericTypeInstanceReference.ResolvedType.ToString());
            var ret = base.Visit(genericTypeInstanceReference);
            this.output.WriteEndElement();
            return ret;
        }

        public override GenericTypeParameter Visit(GenericTypeParameter genericTypeParameter)
        {
            this.output.WriteStartElement("GenericTypeParameter");
            this.output.WriteAttributeString("Name", genericTypeParameter.Name.ToString());
            var ret = base.Visit(genericTypeParameter);
            this.output.WriteEndElement();
            return ret;
        }

        public override GenericTypeParameterReference Visit(GenericTypeParameterReference genericTypeParameterReference)
        {
            this.output.WriteStartElement("GenericTypeParameterReference");
            this.output.WriteAttributeString("Name", genericTypeParameterReference.Name.ToString());
            var ret = base.Visit(genericTypeParameterReference);
            this.output.WriteEndElement();
            return ret;
        }

        public override GlobalFieldDefinition Visit(GlobalFieldDefinition globalFieldDefinition)
        {
            this.output.WriteStartElement("GlobalFieldDefinition");
            this.output.WriteAttributeString("Name", globalFieldDefinition.Name.ToString());
            var ret = base.Visit(globalFieldDefinition);
            this.output.WriteEndElement();
            return ret;
        }

        public override GlobalMethodDefinition Visit(GlobalMethodDefinition globalMethodDefinition)
        {
            this.output.WriteStartElement("GlobalMethodDefinition");
            this.output.WriteAttributeString("Name", globalMethodDefinition.Name.ToString());
            var ret = base.Visit(globalMethodDefinition);
            this.output.WriteEndElement();
            return ret;
        }

        public override List<IAliasForType> Visit(List<IAliasForType> aliasesForTypes)
        {
            if (aliasesForTypes.Count != 0) this.output.WriteStartElement("AliasesForTypes");
            var ret = base.Visit(aliasesForTypes);
            if (aliasesForTypes.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<IAssemblyReference> Visit(List<IAssemblyReference> assemblyReferences)
        {
            if (assemblyReferences.Count != 0) this.output.WriteStartElement("AssemblyReferences");
            var ret = base.Visit(assemblyReferences);
            if (assemblyReferences.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<ICustomAttribute> Visit(List<ICustomAttribute> customAttributes)
        {
            if (customAttributes.Count != 0) this.output.WriteStartElement("CustomAttributes");
            var ret = base.Visit(customAttributes);
            if (customAttributes.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<ICustomModifier> Visit(List<ICustomModifier> customModifiers)
        {
            if (customModifiers.Count != 0) this.output.WriteStartElement("CustomModifiers");
            var ret = base.Visit(customModifiers);
            if (customModifiers.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<IEventDefinition> Visit(List<IEventDefinition> eventDefinitions)
        {
            if (eventDefinitions.Count != 0) this.output.WriteStartElement("EventDefinitions");
            var ret = base.Visit(eventDefinitions);
            if (eventDefinitions.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<IFieldDefinition> Visit(List<IFieldDefinition> fieldDefinitions)
        {
            if (fieldDefinitions.Count != 0) this.output.WriteStartElement("FieldDefinitions");
            var ret = base.Visit(fieldDefinitions);
            if (fieldDefinitions.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<IFileReference> Visit(List<IFileReference> fileReferences)
        {
            if (fileReferences.Count != 0) this.output.WriteStartElement("FileReferences");
            var ret = base.Visit(fileReferences);
            if (fileReferences.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<IGenericMethodParameter> Visit(List<IGenericMethodParameter> genericMethodParameters, IMethodDefinition declaringMethod)
        {
            if (genericMethodParameters.Count != 0) this.output.WriteStartElement("GenericMethodParameters");
            var ret = base.Visit(genericMethodParameters, declaringMethod);
            if (genericMethodParameters.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<IGenericTypeParameter> Visit(List<IGenericTypeParameter> genericTypeParameters)
        {
            if (genericTypeParameters.Count != 0) this.output.WriteStartElement("GenericTypeParameters");
            var ret = base.Visit(genericTypeParameters);
            if (genericTypeParameters.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<ILocalDefinition> Visit(List<ILocalDefinition> locals)
        {
            if (locals.Count != 0) this.output.WriteStartElement("LocalDefinitions");
            var ret = base.Visit(locals);
            if (locals.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<ILocation> Visit(List<ILocation> locations)
        {
            if (locations.Count != 0) this.output.WriteStartElement("Locations");
            var ret = base.Visit(locations);
            if (locations.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<IMetadataExpression> Visit(List<IMetadataExpression> metadataExpressions)
        {
            if (metadataExpressions.Count != 0) this.output.WriteStartElement("MetadataExpressions");
            var ret = base.Visit(metadataExpressions);
            if (metadataExpressions.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<IMetadataNamedArgument> Visit(List<IMetadataNamedArgument> namedArguments)
        {
            if (namedArguments.Count != 0) this.output.WriteStartElement("MetadataNamedArguments");
            var ret = base.Visit(namedArguments);
            if (namedArguments.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<IMethodDefinition> Visit(List<IMethodDefinition> methodDefinitions)
        {
            if (methodDefinitions.Count != 0) this.output.WriteStartElement("MethodDefinitions");
            var ret = base.Visit(methodDefinitions);
            if (methodDefinitions.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<IMethodImplementation> Visit(List<IMethodImplementation> methodImplementations)
        {
            if (methodImplementations.Count != 0) this.output.WriteStartElement("MethodImplementations");
            var ret = base.Visit(methodImplementations);
            if (methodImplementations.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<IMethodReference> Visit(List<IMethodReference> methodReferences)
        {
            if (methodReferences.Count != 0) this.output.WriteStartElement("MethodReferences");
            var ret = base.Visit(methodReferences);
            if (methodReferences.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<IModule> Visit(List<IModule> modules)
        {
            if (modules.Count != 0) this.output.WriteStartElement("Modules");
            var ret = base.Visit(modules);
            if (modules.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<IModuleReference> Visit(List<IModuleReference> moduleReferences)
        {
            if (moduleReferences.Count != 0) this.output.WriteStartElement("ModuleReferences");
            var ret = base.Visit(moduleReferences);
            if (moduleReferences.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<INamespaceMember> Visit(List<INamespaceMember> namespaceMembers)
        {
            if (namespaceMembers.Count != 0) this.output.WriteStartElement("NamespaceMembers");
            var ret = base.Visit(namespaceMembers);
            if (namespaceMembers.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<INestedTypeDefinition> Visit(List<INestedTypeDefinition> nestedTypeDefinitions)
        {
            if (nestedTypeDefinitions.Count != 0) this.output.WriteStartElement("NestedTypeDefinitions");
            var ret = base.Visit(nestedTypeDefinitions);
            if (nestedTypeDefinitions.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<IOperation> Visit(List<IOperation> operations)
        {
            if (operations.Count != 0) this.output.WriteStartElement("Operations");
            var ret = base.Visit(operations);
            if (operations.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<IOperationExceptionInformation> Visit(List<IOperationExceptionInformation> exceptionInformations)
        {
            if (exceptionInformations.Count != 0) this.output.WriteStartElement("OperationExceptionInformations");
            var ret = base.Visit(exceptionInformations);
            if (exceptionInformations.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<IParameterDefinition> Visit(List<IParameterDefinition> parameterDefinitions)
        {
            if (parameterDefinitions.Count != 0) this.output.WriteStartElement("ParameterDefinitions");
            var ret = base.Visit(parameterDefinitions);
            if (parameterDefinitions.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<IParameterTypeInformation> Visit(List<IParameterTypeInformation> parameterTypeInformationList)
        {
            if (parameterTypeInformationList.Count != 0) this.output.WriteStartElement("ParameterTypeInformations");
            var ret = base.Visit(parameterTypeInformationList);
            if (parameterTypeInformationList.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<IPropertyDefinition> Visit(List<IPropertyDefinition> propertyDefinitions)
        {
            if (propertyDefinitions.Count != 0) this.output.WriteStartElement("PropertyDefinitions");
            var ret = base.Visit(propertyDefinitions);
            if (propertyDefinitions.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<IResourceReference> Visit(List<IResourceReference> resourceReferences)
        {
            if (resourceReferences.Count != 0) this.output.WriteStartElement("ResourceReferences");
            var ret = base.Visit(resourceReferences);
            if (resourceReferences.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<ISecurityAttribute> Visit(List<ISecurityAttribute> securityAttributes)
        {
            if (securityAttributes.Count != 0) this.output.WriteStartElement("SecurityAttributes");
            var ret = base.Visit(securityAttributes);
            if (securityAttributes.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<ITypeDefinitionMember> Visit(List<ITypeDefinitionMember> typeDefinitionMembers)
        {
            if (typeDefinitionMembers.Count != 0) this.output.WriteStartElement("TypeDefinitionMembers");
            var ret = base.Visit(typeDefinitionMembers);
            if (typeDefinitionMembers.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<ITypeReference> Visit(List<ITypeReference> typeReferences)
        {
            if (typeReferences.Count != 0) this.output.WriteStartElement("TypeReferences");
            var ret = base.Visit(typeReferences);
            if (typeReferences.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<IWin32Resource> Visit(List<IWin32Resource> win32Resources)
        {
            if (win32Resources.Count != 0) this.output.WriteStartElement("Win32Resources");
            var ret = base.Visit(win32Resources);
            if (win32Resources.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override LocalDefinition Visit(LocalDefinition localDefinition)
        {
            this.output.WriteStartElement("LocalDefinition");
            this.output.WriteAttributeString("Name", localDefinition.Name.ToString());
            var ret = base.Visit(localDefinition);
            this.output.WriteEndElement();
            return ret;
        }

        public override ManagedPointerTypeReference Visit(ManagedPointerTypeReference managedPointerTypeReference)
        {
            this.output.WriteStartElement("ManagedPointerTypeReference");
            this.output.WriteAttributeString("ResolvedType", managedPointerTypeReference.ResolvedType.ToString());
            var ret = base.Visit(managedPointerTypeReference);
            this.output.WriteEndElement();
            return ret;
        }

        public override MarshallingInformation Visit(MarshallingInformation MarshallingInformation)
        {
            this.output.WriteStartElement("MarshallingInformation");
            this.output.WriteAttributeString("UnmanagedType", MarshallingInformation.UnmanagedType.ToString());
            var ret = base.Visit(MarshallingInformation);
            this.output.WriteEndElement();
            return ret;
        }

        public override MatrixTypeReference Visit(MatrixTypeReference matrixTypeReference)
        {
            this.output.WriteStartElement("MatrixTypeReference");
            this.output.WriteAttributeString("ResolvedType", matrixTypeReference.ResolvedType.ToString());
            var ret = base.Visit(matrixTypeReference);
            this.output.WriteEndElement();
            return ret;
        }

        public override MetadataConstant Visit(MetadataConstant constant)
        {
            this.output.WriteStartElement("MetadataConstant");
            this.output.WriteAttributeString("Type", constant.Type.ToString());
            var ret = base.Visit(constant);
            this.output.WriteEndElement();
            return ret;
        }

        public override MetadataCreateArray Visit(MetadataCreateArray createArray)
        {
            this.output.WriteStartElement("MetadataCreateArray");
            this.output.WriteAttributeString("Type", createArray.Type.ToString());
            var ret = base.Visit(createArray);
            this.output.WriteEndElement();
            return ret;
        }

        public override MetadataNamedArgument Visit(MetadataNamedArgument namedArgument)
        {
            this.output.WriteStartElement("MetadataNamedArgument");
            this.output.WriteAttributeString("ArgumentName", namedArgument.ArgumentName.ToString());
            var ret = base.Visit(namedArgument);
            this.output.WriteEndElement();
            return ret;
        }

        public override MetadataTypeOf Visit(MetadataTypeOf typeOf)
        {
            this.output.WriteStartElement("MetadataTypeOf");
            this.output.WriteAttributeString("TypeToGet", typeOf.TypeToGet.ToString());
            var ret = base.Visit(typeOf);
            this.output.WriteEndElement();
            return ret;
        }

        public override MethodBody Visit(MethodBody methodBody)
        {
            this.output.WriteStartElement("MethodBody");
            var ret = base.Visit(methodBody);
            this.output.WriteEndElement();
            return ret;
        }

        public override MethodDefinition Visit(MethodDefinition methodDefinition)
        {
            this.output.WriteStartElement("MethodDefinition");
            this.output.WriteAttributeString("Name", methodDefinition.Name.ToString());
            var ret = base.Visit(methodDefinition);
            this.output.WriteEndElement();
            return ret;
        }

        public override MethodImplementation Visit(MethodImplementation methodImplementation)
        {
            this.output.WriteStartElement("MethodImplementation");
            this.output.WriteAttributeString("ImplementedMethod", methodImplementation.ImplementedMethod.ToString());
            this.output.WriteAttributeString("ImplementingMethod", methodImplementation.ImplementingMethod.ToString());
            var ret = base.Visit(methodImplementation);
            this.output.WriteEndElement();
            return ret;
        }

        public override Microsoft.Cci.MutableCodeModel.AssemblyReference Visit(Microsoft.Cci.MutableCodeModel.AssemblyReference assemblyReference)
        {
            this.output.WriteStartElement("AssemblyReference");
            this.output.WriteAttributeString("AssemblyIdentity", assemblyReference.AssemblyIdentity.ToString());
            var ret = base.Visit(assemblyReference);
            this.output.WriteEndElement();
            return ret;
        }

        public override Microsoft.Cci.MutableCodeModel.CustomModifier Visit(Microsoft.Cci.MutableCodeModel.CustomModifier customModifier)
        {
            this.output.WriteStartElement("CustomModifier");
            this.output.WriteAttributeString("Modifier", customModifier.Modifier.ToString());
            var ret = base.Visit(customModifier);
            this.output.WriteEndElement();
            return ret;
        }

        public override Microsoft.Cci.MutableCodeModel.GenericMethodInstanceReference Visit(Microsoft.Cci.MutableCodeModel.GenericMethodInstanceReference genericMethodInstanceReference)
        {
            this.output.WriteStartElement("GenericMethodInstanceReference");
            this.output.WriteAttributeString("Name", genericMethodInstanceReference.Name.ToString());
            var ret = base.Visit(genericMethodInstanceReference);
            this.output.WriteEndElement();
            return ret;
        }

        public override Microsoft.Cci.MutableCodeModel.GenericMethodParameterReference Visit(Microsoft.Cci.MutableCodeModel.GenericMethodParameterReference genericMethodParameterReference)
        {
            this.output.WriteStartElement("GenericMethodParameterReference");
            this.output.WriteAttributeString("Name", genericMethodParameterReference.Name.ToString());
            var ret = base.Visit(genericMethodParameterReference);
            this.output.WriteEndElement();
            return ret;
        }

        public override Microsoft.Cci.MutableCodeModel.MethodReference Visit(Microsoft.Cci.MutableCodeModel.MethodReference methodReference)
        {
            this.output.WriteStartElement("MethodReference");
            this.output.WriteAttributeString("Name", methodReference.Name.ToString());
            var ret = base.Visit(methodReference);
            this.output.WriteEndElement();
            return ret;
        }

        public override Microsoft.Cci.MutableCodeModel.ModifiedTypeReference Visit(Microsoft.Cci.MutableCodeModel.ModifiedTypeReference modifiedTypeReference)
        {
            this.output.WriteStartElement("ModifiedTypeReference");
            this.output.WriteAttributeString("ResolvedType", modifiedTypeReference.ResolvedType.ToString());
            var ret = base.Visit(modifiedTypeReference);
            this.output.WriteEndElement();
            return ret;
        }

        public override Microsoft.Cci.MutableCodeModel.ModuleReference Visit(Microsoft.Cci.MutableCodeModel.ModuleReference moduleReference)
        {
            this.output.WriteStartElement("ModuleReference");
            this.output.WriteAttributeString("Name", moduleReference.Name.ToString());
            var ret = base.Visit(moduleReference);
            this.output.WriteEndElement();
            return ret;
        }

        public override Microsoft.Cci.MutableCodeModel.NamespaceTypeReference Visit(Microsoft.Cci.MutableCodeModel.NamespaceTypeReference namespaceTypeReference)
        {
            this.output.WriteStartElement("NamespaceTypeReference");
            this.output.WriteAttributeString("Name", namespaceTypeReference.Name.ToString());
            var ret = base.Visit(namespaceTypeReference);
            this.output.WriteEndElement();
            return ret;
        }

        public override Microsoft.Cci.MutableCodeModel.NestedUnitNamespaceReference Visit(Microsoft.Cci.MutableCodeModel.NestedUnitNamespaceReference nestedUnitNamespaceReference)
        {
            this.output.WriteStartElement("NestedUnitNamespaceReference");
            this.output.WriteAttributeString("Name", nestedUnitNamespaceReference.Name.ToString());
            var ret = base.Visit(nestedUnitNamespaceReference);
            this.output.WriteEndElement();
            return ret;
        }

        public override Microsoft.Cci.MutableCodeModel.RootUnitNamespaceReference Visit(Microsoft.Cci.MutableCodeModel.RootUnitNamespaceReference rootUnitNamespaceReference)
        {
            this.output.WriteStartElement("RootUnitNamespaceReference");
            this.output.WriteAttributeString("ResolvedUnitNamespace", rootUnitNamespaceReference.ResolvedUnitNamespace.ToString());
            var ret = base.Visit(rootUnitNamespaceReference);
            this.output.WriteEndElement();
            return ret;
        }

        public override Module Visit(Module module)
        {
            this.output.WriteStartElement("Module");
            this.output.WriteAttributeString("Name", module.Name.ToString());
            var ret = base.Visit(module);
            this.output.WriteEndElement();
            return ret;
        }

        public override NamespaceAliasForType Visit(NamespaceAliasForType namespaceAliasForType)
        {
            this.output.WriteStartElement("NamespaceAliasForType");
            this.output.WriteAttributeString("Name", namespaceAliasForType.Name.ToString());
            this.output.WriteAttributeString("AliasedType", namespaceAliasForType.AliasedType.ToString());
            var ret = base.Visit(namespaceAliasForType);
            this.output.WriteEndElement();
            return ret;
        }

        public override NamespaceTypeDefinition Visit(NamespaceTypeDefinition namespaceTypeDefinition)
        {
            this.output.WriteStartElement("NamespaceTypeDefinition");
            this.output.WriteAttributeString("Name", namespaceTypeDefinition.Name.ToString());
            var ret = base.Visit(namespaceTypeDefinition);
            this.output.WriteEndElement();
            return ret;
        }

        public override NestedAliasForType Visit(NestedAliasForType nestedAliasForType)
        {
            this.output.WriteStartElement("NestedAliasForType");
            this.output.WriteAttributeString("Name", nestedAliasForType.Name.ToString());
            this.output.WriteAttributeString("AliasedType", nestedAliasForType.AliasedType.ToString());
            var ret = base.Visit(nestedAliasForType);
            this.output.WriteEndElement();
            return ret;
        }

        public override NestedTypeDefinition Visit(NestedTypeDefinition nestedTypeDefinition)
        {
            this.output.WriteStartElement("NestedTypeDefinition");
            this.output.WriteAttributeString("Name", nestedTypeDefinition.Name.ToString());
            var ret = base.Visit(nestedTypeDefinition);
            this.output.WriteEndElement();
            return ret;
        }

        public override NestedTypeReference Visit(NestedTypeReference nestedTypeReference)
        {
            this.output.WriteStartElement("NestedTypeReference");
            this.output.WriteAttributeString("Name", nestedTypeReference.Name.ToString());
            var ret = base.Visit(nestedTypeReference);
            this.output.WriteEndElement();
            return ret;
        }

        public override NestedUnitNamespace Visit(NestedUnitNamespace nestedUnitNamespace)
        {
            this.output.WriteStartElement("NestedUnitNamespace");
            this.output.WriteAttributeString("Name", nestedUnitNamespace.Name.ToString());
            var ret = base.Visit(nestedUnitNamespace);
            this.output.WriteEndElement();
            return ret;
        }

        public override OperationExceptionInformation Visit(OperationExceptionInformation operationExceptionInformation)
        {
            this.output.WriteStartElement("OperationExceptionInformation");
            this.output.WriteAttributeString("HandlerKind", operationExceptionInformation.HandlerKind.ToString());
            if (operationExceptionInformation.HandlerKind == HandlerKind.Catch)
                this.output.WriteAttributeString("ExceptionType", operationExceptionInformation.ExceptionType.ToString());
            var ret = base.Visit(operationExceptionInformation);
            this.output.WriteEndElement();
            return ret;
        }

        public override ParameterDefinition Visit(ParameterDefinition parameterDefinition)
        {
            this.output.WriteStartElement("ParameterDefinition");
            this.output.WriteAttributeString("Name", parameterDefinition.Name.ToString());
            var ret = base.Visit(parameterDefinition);
            this.output.WriteEndElement();
            return ret;
        }

        public override ParameterTypeInformation Visit(ParameterTypeInformation parameterTypeInformation)
        {
            this.output.WriteStartElement("ParameterTypeInformation");
            this.output.WriteAttributeString("Type", parameterTypeInformation.Type.ToString());
            var ret = base.Visit(parameterTypeInformation);
            this.output.WriteEndElement();
            return ret;
        }

        public override PlatformInvokeInformation Visit(PlatformInvokeInformation platformInvokeInformation)
        {
            this.output.WriteStartElement("PlatformInvokeInformation");
            this.output.WriteAttributeString("ImportModule", platformInvokeInformation.ImportModule.ToString());
            this.output.WriteAttributeString("ImportName", platformInvokeInformation.ImportName.ToString());
            var ret = base.Visit(platformInvokeInformation);
            this.output.WriteEndElement();
            return ret;
        }

        public override PointerTypeReference Visit(PointerTypeReference pointerTypeReference)
        {
            this.output.WriteStartElement("PointerTypeReference");
            this.output.WriteAttributeString("TargetType", pointerTypeReference.TargetType.ToString());
            var ret = base.Visit(pointerTypeReference);
            this.output.WriteEndElement();
            return ret;
        }

        public override PropertyDefinition Visit(PropertyDefinition propertyDefinition)
        {
            this.output.WriteStartElement("PropertyDefinition");
            this.output.WriteAttributeString("Name", propertyDefinition.Name.ToString());
            var ret = base.Visit(propertyDefinition);
            this.output.WriteEndElement();
            return ret;
        }

        public override IResourceReference Visit(ResourceReference resourceReference)
        {
            this.output.WriteStartElement("ResourceReference");
            this.output.WriteAttributeString("Name", resourceReference.Name.ToString());
            var ret = base.Visit(resourceReference);
            this.output.WriteEndElement();
            return ret;
        }

        public override RootUnitNamespace Visit(RootUnitNamespace rootUnitNamespace)
        {
            this.output.WriteStartElement("RootUnitNamespace");
            this.output.WriteAttributeString("Name", rootUnitNamespace.Name.ToString());
            var ret = base.Visit(rootUnitNamespace);
            this.output.WriteEndElement();
            return ret;
        }

        public override SectionBlock Visit(SectionBlock sectionBlock)
        {
            this.output.WriteStartElement("SectionBlock");
            this.output.WriteAttributeString("PESectionKind", sectionBlock.PESectionKind.ToString());
            var ret = base.Visit(sectionBlock);
            this.output.WriteEndElement();
            return ret;
        }

        public override SecurityAttribute Visit(SecurityAttribute securityAttribute)
        {
            this.output.WriteStartElement("SecurityAttribute");
            this.output.WriteAttributeString("Action", securityAttribute.Action.ToString());
            var ret = base.Visit(securityAttribute);
            this.output.WriteEndElement();
            return ret;
        }

        public override SpecializedFieldReference Visit(SpecializedFieldReference specializedFieldReference)
        {
            this.output.WriteStartElement("SpecializedFieldReference");
            this.output.WriteAttributeString("Name", specializedFieldReference.Name.ToString());
            var ret = base.Visit(specializedFieldReference);
            this.output.WriteEndElement();
            return ret;
        }

        public override SpecializedMethodReference Visit(SpecializedMethodReference specializedMethodReference)
        {
            this.output.WriteStartElement("SpecializedMethodReference");
            this.output.WriteAttributeString("Name", specializedMethodReference.Name.ToString());
            var ret = base.Visit(specializedMethodReference);
            this.output.WriteEndElement();
            return ret;
        }

        public override SpecializedNestedTypeReference Visit(SpecializedNestedTypeReference specializedNestedTypeReference)
        {
            this.output.WriteStartElement("SpecializedNestedTypeReference");
            this.output.WriteAttributeString("Name", specializedNestedTypeReference.Name.ToString());
            var ret = base.Visit(specializedNestedTypeReference);
            this.output.WriteEndElement();
            return ret;
        }

        public override ITypeDefinitionMember Visit(TypeDefinitionMember typeDefinitionMember)
        {
            this.output.WriteStartElement("TypeDefinitionMember");
            this.output.WriteAttributeString("Name", typeDefinitionMember.Name.ToString());
            var ret = base.Visit(typeDefinitionMember);
            this.output.WriteEndElement();
            return ret;
        }

        public override TypeReference Visit(TypeReference typeReference)
        {
            this.output.WriteStartElement("TypeReference");
            this.output.WriteAttributeString("ResolvedType", typeReference.ResolvedType.ToString());
            var ret = base.Visit(typeReference);
            this.output.WriteEndElement();
            return ret;
        }

        public override Unit Visit(Unit unit)
        {
            this.output.WriteStartElement("Unit");
            this.output.WriteAttributeString("Name", unit.Name.ToString());
            var ret = base.Visit(unit);
            this.output.WriteEndElement();
            return ret;
        }

        public override UnitNamespace Visit(UnitNamespace unitNamespace)
        {
            this.output.WriteStartElement("UnitNamespace");
            this.output.WriteAttributeString("Name", unitNamespace.Name.ToString());
            var ret = base.Visit(unitNamespace);
            this.output.WriteEndElement();
            return ret;
        }

        public override UnitNamespaceReference Visit(UnitNamespaceReference unitNamespaceReference)
        {
            this.output.WriteStartElement("UnitNamespaceReference");
            this.output.WriteAttributeString("ResolvedUnitNamespace", unitNamespaceReference.ResolvedUnitNamespace.ToString());
            var ret = base.Visit(unitNamespaceReference);
            this.output.WriteEndElement();
            return ret;
        }

        public override VectorTypeReference Visit(VectorTypeReference vectorTypeReference)
        {
            this.output.WriteStartElement("VectorTypeReference");
            this.output.WriteAttributeString("ResolvedType", vectorTypeReference.ResolvedType.ToString());
            var ret = base.Visit(vectorTypeReference);
            this.output.WriteEndElement();
            return ret;
        }

        public override List<ICustomAttribute> VisitMethodReturnValueAttributes(List<ICustomAttribute> customAttributes)
        {
            if (customAttributes.Count != 0) this.output.WriteStartElement("MethodReturnValueAttributes");
            var ret = base.VisitMethodReturnValueAttributes(customAttributes);
            if (customAttributes.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override List<ICustomModifier> VisitMethodReturnValueCustomModifiers(List<ICustomModifier> customModifers)
        {
            if (customModifers.Count != 0) this.output.WriteStartElement("MethodReturnValueCustomModifiers");
            var ret = base.VisitMethodReturnValueCustomModifiers(customModifers);
            if (customModifers.Count != 0) this.output.WriteEndElement();
            return ret;
        }

        public override IMarshallingInformation VisitMethodReturnValueMarshallingInformation(MarshallingInformation marshallingInformation)
        {
            this.output.WriteStartElement("MethodReturnValueMarshallingInformation");
            this.output.WriteAttributeString("UnmanagedType", marshallingInformation.UnmanagedType.ToString());
            var ret = base.VisitMethodReturnValueMarshallingInformation(marshallingInformation);
            this.output.WriteEndElement();
            return ret;
        }

        public override void VisitPrivateHelperMembers(List<INamedTypeDefinition> typeDefinitions)
        {
            if (typeDefinitions.Count != 0) this.output.WriteStartElement("PrivateHelperMembers");
            base.VisitPrivateHelperMembers(typeDefinitions);
            if (typeDefinitions.Count != 0) this.output.WriteEndElement();
        }

        public override List<ICustomAttribute> VisitPropertyReturnValueAttributes(List<ICustomAttribute> customAttributes)
        {
            if (customAttributes.Count != 0) this.output.WriteStartElement("PropertyReturnValueAttributes");
            var ret = base.VisitPropertyReturnValueAttributes(customAttributes);
            if (customAttributes.Count != 0) this.output.WriteEndElement();
            return ret;
        }
    }
}
