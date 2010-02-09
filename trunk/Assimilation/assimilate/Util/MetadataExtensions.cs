using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Cci;

namespace assimilate
{
    public static class MetadataExtensions
    {
        public static IEnumerable<ITypeDefinition> ContainingTypesAndSelf(this ITypeDefinition type)
        {
            while (true)
            {
                yield return type;

                INestedTypeDefinition nestedType = type as INestedTypeDefinition;
                if (nestedType != null)
                {
                    type = nestedType.ContainingType.ResolvedType;
                }
                else
                {
                    yield break;
                }
            }
        }

        public static bool MayBeExposed(this TypeMemberVisibility visibility)
        {
            return visibility != TypeMemberVisibility.Private && visibility != TypeMemberVisibility.Other &&
                visibility != TypeMemberVisibility.FamilyAndAssembly && visibility != TypeMemberVisibility.Assembly;
        }

        public static bool IsExposed(this ITypeDefinition type)
        {
            if (type.ContainingTypesAndSelf().Any(x => !x.TypeVisibilityAsTypeMemberVisibility().MayBeExposed()))
            {
                return false;
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

        /// <summary>
        /// Returns a C#-like string that corresponds to the given namespace definition and that conforms to the specified formatting options.
        /// </summary>
        public static string GetNamespaceName(this IUnitSetNamespace namespaceDefinition, NameFormattingOptions formattingOptions)
        {
            return new FixedTypeNameFormatter().GetNamespaceName(namespaceDefinition, formattingOptions);
        }

        /// <summary>
        /// Returns a C#-like string that corresponds to the given namespace definition and that conforms to the specified formatting options.
        /// </summary>
        public static string GetNamespaceName(this IUnitNamespaceReference namespaceReference, NameFormattingOptions formattingOptions)
        {
            return new FixedTypeNameFormatter().GetNamespaceName(namespaceReference, formattingOptions);
        }

        /// <summary>
        /// Returns a C#-like string that corresponds to a source expression that would bind to the given type definition when appearing in an appropriate context.
        /// </summary>
        public static string GetTypeName(ITypeReference type)
        {
            return new FixedTypeNameFormatter().GetTypeName(type, NameFormattingOptions.None);
        }

        /// <summary>
        /// Returns a C#-like string that corresponds to the given type definition and that conforms to the specified formatting options.
        /// </summary>
        public static string GetTypeName(ITypeReference type, NameFormattingOptions formattingOptions)
        {
            return new FixedTypeNameFormatter().GetTypeName(type, formattingOptions);
        }

        /// <summary>
        /// Returns a C#-like string that corresponds to the given type member definition and that conforms to the specified formatting options.
        /// </summary>
        public static string GetMemberSignature(ITypeMemberReference member, NameFormattingOptions formattingOptions)
        {
            return new FixedSignatureFormatter(new FixedTypeNameFormatter()).GetMemberSignature(member, formattingOptions);
        }

        /// <summary>
        /// Returns a C#-like string that corresponds to the given method definition and that conforms to the specified formatting options.
        /// </summary>
        public static string GetMethodSignature(IMethodReference method, NameFormattingOptions formattingOptions)
        {
            return new FixedSignatureFormatter(new FixedTypeNameFormatter()).GetMethodSignature(method, formattingOptions);
        }

        /// <summary>
        /// Returns the Base class. If there is no base type it returns null.
        /// </summary>
        /// <param name="typeDef">The type whose base class is to be returned.</param>
        public static ITypeDefinition BaseClass(this ITypeDefinition typeDef)
        {
            return TypeHelper.BaseClass(typeDef);
        }

        /// <summary>
        /// Returns the most derived common base class that all types that satisfy the constraints of the given
        /// generic parameter must derive from.
        /// </summary>
        public static ITypeDefinition EffectiveBaseClass(this IGenericParameter genericParameter)
        {
            return TypeHelper.EffectiveBaseClass(genericParameter);
        }

        /// <summary>
        /// Returns true a value of this type can be treated as a compile time constant.
        /// Such values need not be stored in memory in order to be representable. For example, they can appear as part of a CLR instruction.
        /// </summary>
        public static bool IsCompileTimeConstantType(this ITypeReference type)
        {
            return TypeHelper.IsCompileTimeConstantType(type);
        }

        /// <summary>
        /// Returns true if the CLR allows integer operators to be applied to values of the given type.
        /// </summary>
        public static bool IsPrimitiveInteger(this ITypeReference type)
        {
            return TypeHelper.IsPrimitiveInteger(type);
        }

        /// <summary>
        /// Returns true if the CLR allows signed integer operators to be applied to values of the given type.
        /// </summary>
        public static bool IsSignedPrimitiveInteger(this ITypeReference type)
        {
            return TypeHelper.IsSignedPrimitiveInteger(type);
        }

        /// <summary>
        /// Returns true if the CLR allows unsigned integer operators to be applied to values of the given type.
        /// </summary>
        public static bool IsUnsignedPrimitiveInteger(this ITypeReference type)
        {
            return TypeHelper.IsUnsignedPrimitiveInteger(type);
        }

        /// <summary>
        /// Decides if the given type definition is visible to assemblies other than the assembly it is defined in (and other than its friends).
        /// </summary>
        public static bool IsVisibleOutsideAssembly(this ITypeDefinition typeDefinition)
        {
            return TypeHelper.IsVisibleOutsideAssembly(typeDefinition);
        }

        /// <summary>
        /// Returns the most accessible visibility that is not greater than the given visibility and the visibilities of each of the given typeArguments.
        /// For the purpose of computing the intersection, namespace types are treated as being TypeMemberVisibility.Public or TypeMemberVisibility.Assembly.
        /// Generic type instances are treated as having a visibility that is the intersection of the generic type's visibility and all of the type arguments' visibilities.
        /// </summary>
        public static TypeMemberVisibility GenericInstanceVisibilityAsTypeMemberVisibility(this TypeMemberVisibility templateVisibility, IEnumerable<ITypeReference> typeArguments)
        {
            return TypeHelper.GenericInstanceVisibilityAsTypeMemberVisibility(templateVisibility, typeArguments);
        }

        /// <summary>
        /// Returns a TypeMemberVisibility value that corresponds to the visibility of the given type definition.
        /// Namespace types are treated as being TypeMemberVisibility.Public or TypeMemberVisibility.Assembly.
        /// Generic type instances are treated as having a visibility that is the intersection of the generic type's visibility and all of the type arguments' visibilities.
        /// </summary>
        public static TypeMemberVisibility TypeVisibilityAsTypeMemberVisibility(this ITypeDefinition type)
        {
            return TypeHelper.TypeVisibilityAsTypeMemberVisibility(type);
        }

        /// <summary>
        /// Returns the unit that defines the given type. If the type is a structural type, such as a pointer the result is 
        /// the defining unit of the element type, or in the case of a generic type instance, the definining type of the generic template type.
        /// </summary>
        public static IUnit GetDefiningUnit(this ITypeDefinition typeDefinition)
        {
            return TypeHelper.GetDefiningUnit(typeDefinition);
        }

        /// <summary>
        /// Returns a reference to the unit that defines the given referenced type. If the referenced type is a structural type, such as a pointer or a generic type instance,
        /// then the result is null.
        /// </summary>
        public static IUnitReference GetDefiningUnitReference(this ITypeReference typeReference)
        {
            return TypeHelper.GetDefiningUnitReference(typeReference);
        }

        /// <summary>
        /// Returns a field of the given declaring type that has the given name.
        /// If no such field can be found, Dummy.Field is returned.
        /// </summary>
        /// <param name="declaringType">The type thats declares the field.</param>
        /// <param name="fieldName">The name of the field.</param>
        public static IFieldDefinition GetField(this ITypeDefinition declaringType, IName fieldName)
        {
            return TypeHelper.GetField(declaringType, fieldName);
        }

        /// <summary>
        /// Returns a field of the given declaring type that has the same name and signature as the given field reference.
        /// If no such field can be found, Dummy.Field is returned.
        /// </summary>
        /// <param name="declaringType">The type thats declares the field.</param>
        /// <param name="fieldReference">A reference to the field.</param>
        public static IFieldDefinition GetField(this ITypeDefinition declaringType, IFieldReference fieldReference)
        {
            return TypeHelper.GetField(declaringType, fieldReference);
        }

        /// <summary>
        /// Returns a method of the given declaring type that has the given name and that matches the given parameter types.
        /// If no such method can be found, Dummy.Method is returned.
        /// </summary>
        /// <param name="declaringType">The type that declares the method to be returned.</param>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="parameterTypes">A list of types that should correspond to the parameter types of the returned method.</param>
        public static IMethodDefinition GetMethod(this ITypeDefinition declaringType, IName methodName, params ITypeReference[] parameterTypes)
        {
            return TypeHelper.GetMethod(declaringType, methodName, parameterTypes);
        }

        /// <summary>
        /// Returns the first method, if any, of the given list of type members that has the given name and that matches the given parameter types.
        /// If no such method can be found, Dummy.Method is returned.
        /// </summary>
        /// <param name="members">A list of type members.</param>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="parameterTypes">A list of types that should correspond to the parameter types of the returned method.</param>
        public static IMethodDefinition GetMethod(this IEnumerable<ITypeDefinitionMember> members, IName methodName, params ITypeReference[] parameterTypes)
        {
            return TypeHelper.GetMethod(members, methodName, parameterTypes);
        }

        /// <summary>
        /// Returns a method of the given declaring type that matches the given method reference.
        /// If no such method can be found, Dummy.Method is returned.
        /// </summary>
        /// <param name="declaringType">The type that declares the method to be returned.</param>
        /// <param name="methodReference">A method reference whose name and signature matches that of the desired result.</param>
        /// <returns></returns>
        public static IMethodDefinition GetMethod(this ITypeDefinition declaringType, IMethodReference methodReference)
        {
            return TypeHelper.GetMethod(declaringType, methodReference);
        }

        /// <summary>
        /// Gets the Invoke method from the delegate. Returns Dummy.Method if the delegate type is malformed.
        /// </summary>
        /// <param name="delegateType">A delegate type.</param>
        /// <param name="host">The host application that provided the nametable used by delegateType.</param>
        public static IMethodDefinition GetInvokeMethod(this ITypeDefinition delegateType, IMetadataHost host)
        {
            return TypeHelper.GetInvokeMethod(delegateType, host);
        }

        /// <summary>
        /// Returns the first method, if any, of the given list of type members that matches the signature of the given method.
        /// If no such method can be found, Dummy.Method is returned.
        /// </summary>
        /// <param name="members">A list of type members.</param>
        /// <param name="methodSignature">A method whose signature matches that of the desired result.</param>
        /// <returns></returns>
        public static IMethodDefinition GetMethod(this IEnumerable<ITypeDefinitionMember> members, IMethodReference methodSignature)
        {
            return TypeHelper.GetMethod(members, methodSignature);
        }

        /// <summary>
        /// Returns the nested type, if any, of the given declaring type with the given name and given generic parameter count.
        /// If no such type is found, Dummy.NestedType is returned.
        /// </summary>
        /// <param name="declaringType">The type to search for a nested type with the given name and number of generic parameters.</param>
        /// <param name="typeName">The name of the nested type to return.</param>
        /// <param name="genericParameterCount">The number of generic parameters. Zero if the type is not generic, larger than zero otherwise.</param>
        /// <returns></returns>
        public static INestedTypeDefinition GetNestedType(this ITypeDefinition declaringType, IName typeName, int genericParameterCount)
        {
            return TypeHelper.GetNestedType(declaringType, typeName, genericParameterCount);
        }

        /// <summary>
        /// Try to compute the self instance of a type, that is, a fully instantiated and specialized type reference. 
        /// For example, use T and T1 to instantiate A&lt;T&gt;.B.C&lt;T1&gt;. If successful, result is set to a 
        /// IGenericTypeInstance if type definition is generic, or a specialized nested type reference if one of
        /// the parent of typeDefinition is generic, or typeDefinition if none of the above. Failure happens when 
        /// one of its parent's members is not properly initialized. 
        /// </summary>
        /// <param name="typeDefinition">A type definition whose self instance is to be computed.</param>
        /// <param name="result">The self instantiated reference to typeDefinition. Valid only when returning true. </param>
        /// <returns>True if the instantiation succeeded. False if typeDefinition is a nested type and we cannot find such a nested type definition 
        /// in its parent's self instance.</returns>
        public static bool TryGetFullyInstantiatedSpecializedTypeReference(this ITypeDefinition typeDefinition, out ITypeReference result)
        {
            return TypeHelper.TryGetFullyInstantiatedSpecializedTypeReference(typeDefinition, out result);
        }

        /// <summary>
        /// Returns the computed size (number of bytes) of a type. May call the SizeOf property of the type.
        /// Use SizeOfType(ITypeReference, bool) to suppress the use of the SizeOf property.
        /// </summary>
        /// <param name="type">The type whose size is wanted. If not a reference to a primitive type, this type must be resolvable.</param>
        public static uint SizeOfType(this ITypeReference type)
        {
            return TypeHelper.SizeOfType(type);
        }

        /// <summary>
        /// Returns the computed size (number of bytes) of a type. 
        /// </summary>
        /// <param name="type">The type whose size is wanted. If not a reference to a primitive type, this type must be resolvable.</param>
        /// <param name="mayUseSizeOfProperty">If true the SizeOf property of the given type may be evaluated and used
        /// as the result of this routine if not 0. Remember to specify false for this parameter when using this routine in the implementation
        /// of the ITypeDefinition.SizeOf property.</param>
        public static uint SizeOfType(this ITypeReference type, bool mayUseSizeOfProperty)
        {
            return TypeHelper.SizeOfType(type, mayUseSizeOfProperty);
        }

        /// <summary>
        /// Returns the byte alignment that values of the given type ought to have. The result is a power of two and greater than zero.
        /// May call the Alignment property of the type.
        /// Use TypeAlignment(ITypeDefinition, bool) to suppress the use of the Alignment property.    
        /// </summary>
        /// <param name="type">The type whose size is wanted. If not a reference to a primitive type, this type must be resolvable.</param>
        public static ushort TypeAlignment(this ITypeReference type)
        {
            return TypeHelper.TypeAlignment(type);
        }

        /// <summary>
        /// Returns the byte alignment that values of the given type ought to have. The result is a power of two and greater than zero.
        /// </summary>
        /// <param name="type">The type whose size is wanted. If not a reference to a primitive type, this type must be resolvable.</param>
        /// <param name="mayUseAlignmentProperty">If true the Alignment property of the given type may be inspected and used
        /// as the result of this routine if not 0. Rembmer to specify false for this parameter when using this routine in the implementation
        /// of the ITypeDefinition.Alignment property.</param>
        public static ushort TypeAlignment(this ITypeReference type, bool mayUseAlignmentProperty)
        {
            return TypeHelper.TypeAlignment(type, mayUseAlignmentProperty);
        }

        /// <summary>
        /// Returns true if the given type extends System.Attribute.
        /// </summary>
        public static bool IsAttributeType(this ITypeDefinition type)
        {
            return TypeHelper.IsAttributeType(type);
        }

        /// <summary>
        /// Returns the number of bytes that separate the start of an instance of the items's declaring type from the start of the field itself.
        /// </summary>
        /// <param name="item">The item (field or nested type) of interests, which must not be static. </param>
        /// <param name="containingTypeDefinition">The type containing the item.</param>
        /// <returns></returns>
        public static uint ComputeFieldOffset(this ITypeDefinitionMember item, ITypeDefinition containingTypeDefinition)
        {
            return MemberHelper.ComputeFieldOffset(item, containingTypeDefinition);
        }

        /// <summary>
        /// Returns zero or more base class and interface methods that are explicitly overridden by the given method.
        /// </summary>
        /// <remarks>
        /// IMethodReferences are returned (as opposed to IMethodDefinitions) because the references are directly available:
        /// no resolving is needed to find them.
        /// </remarks>
        public static IEnumerable<IMethodReference> GetExplicitlyOverriddenMethods(this IMethodDefinition overridingMethod)
        {
            return MemberHelper.GetExplicitlyOverriddenMethods(overridingMethod);
        }

        /// <summary>
        /// Returns the number of least significant bits in the representation of field.Type that should be ignored when reading or writing the field value at MemberHelper.GetFieldOffset(field).
        /// </summary>
        /// <param name="field">The bit field whose bit offset is to returned.</param>
        public static uint GetFieldBitOffset(this IFieldDefinition field)
        {
            return MemberHelper.GetFieldBitOffset(field);
        }

        /// <summary>
        /// Get the field offset of a particular field, whose containing type may have its own policy
        /// of assigning offset. For example, a struct and a union in C may be different. 
        /// </summary>
        /// <param name="field">The field whose offset is to returned. The field must not be static.</param>
        public static uint GetFieldOffset(this IFieldDefinition field)
        {
            return MemberHelper.GetFieldOffset(field);
        }

        /// <summary>
        /// Returns zero or more interface methods that are implemented by the given method. Only methods from interfaces that
        /// are directly implemented by the containing type of the given method are returned. Interfaces declared on base classes
        /// are always fully implemented by the base class, albeit sometimes by an abstract method that is itself implemented by a derived class method.
        /// </summary>
        /// <remarks>
        /// IMethodDefinitions are returned (as opposed to IMethodReferences) because it isn't possible to find the interface methods
        /// without resolving the interface references to their definitions.
        /// </remarks>
        public static IEnumerable<IMethodDefinition> GetImplicitlyImplementedInterfaceMethods(this IMethodDefinition implementingMethod)
        {
            return MemberHelper.GetImplicitlyImplementedInterfaceMethods(implementingMethod);
        }

        /// <summary>
        /// Returns the method from the closest base class that is overridden by the given method.
        /// If no such method exists, Dummy.Method is returned.
        /// </summary>
        public static IMethodDefinition GetImplicitlyOverriddenBaseClassMethod(this IMethodDefinition derivedClassMethod)
        {
            return MemberHelper.GetImplicitlyOverriddenBaseClassMethod(derivedClassMethod);
        }

        /// <summary>
        /// Returns the method from the derived class that overrides the given method.
        /// If no such method exists, Dummy.Method is returned.
        /// </summary>
        public static IMethodDefinition GetImplicitlyOverridingDerivedClassMethod(this IMethodDefinition baseClassMethod, ITypeDefinition derivedClass)
        {
            return MemberHelper.GetImplicitlyOverridingDerivedClassMethod(baseClassMethod, derivedClass);
        }

        /// <summary>
        /// Decides if the given type definition member is visible outside of the assembly it
        /// is defined in.
        /// It does not take into account friend assemblies: the meaning of this method
        /// is that it returns true for those members that are visible outside of their
        /// defining assembly to *all* assemblies.
        /// </summary>
        public static bool IsVisibleOutsideAssembly(this ITypeDefinitionMember typeDefinitionMember)
        {
            return MemberHelper.IsVisibleOutsideAssembly(typeDefinitionMember);
        }

        /// <summary>
        /// Returns true if the field signature has the System.Runtime.CompilerServices.IsVolatile modifier.
        /// Such fields should only be accessed with volatile reads and writes.
        /// </summary>
        /// <param name="field">The field to inspect for the System.Runtime.CompilerServices.IsVolatile modifier.</param>
        public static bool IsVolatile(this IFieldDefinition field)
        {
            return MemberHelper.IsVolatile(field);
        }
    }
}
