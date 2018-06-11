//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2017 Icucom Corporation
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.TypeHandling.MemberExtensionMethods;
using PlatformAgileFramework.TypeHandling.MethodHelpers;
using PlatformAgileFramework.TypeHandling.ParameterHelpers;


namespace PlatformAgileFramework.TypeHandling.TypeExtensionMethods
{
    /// <summary>
    /// This is a new helper class for the rewrite of the reflection system in 4.5. It
    /// provides helpers in one place so code is not duplicated elsewhere.
    /// </summary>
    /// <history>
    /// <contribution>
    /// <author> KRM </author>
    /// <date> 03jul2017 </date>
    /// <desription>
    /// Added a couple methods for the reintroduction of xUnit emulator. 
    /// </desription>
    /// </contribution>
    /// <contribution>
    /// <author> JWM(S) </author>
    /// <date> 03jul2015 </date>
    /// <desription>
    /// Changed strategy after consulting with KRM to return empty collections, not
    /// <see langword="null"/>'s.
    /// </desription>
    /// </contribution>
	/// <contribution>
    /// <author> KRM </author>
    /// <date> 17jun2015 </date>
    /// <desription>
    /// New. Built to wrap new reflection library rewrite stuff. This exists so we
    /// can support clients who can't or won't upgrade to 4.5. There will be a separate
    /// file for pre-4.5 cutomers.
    /// </desription>
    /// </contribution>
    /// </history>
    // ReSharper disable once PartialTypeWithSinglePart
    public static partial class PAFTypeReflectionExtensions
	// ReSharper restore PartialTypeWithSinglePart
	{
        /// <summary>
        /// Simple method just changes <see cref="TypeInfo"/>'s back to their
        /// associated types.
        /// </summary>
        /// <param name="typeInfos">One of us.</param>
        /// <returns>
        /// Empty collection if incoming enum is empty or null.
        /// </returns>
        public static IEnumerable<Type> BackToTypes(this IEnumerable<TypeInfo> typeInfos)
	    {
	        var typeCollection = new Collection<Type>();
	        if (typeInfos == null) return typeCollection;

	        foreach (var typeInfo in typeInfos)
	        {
	            typeCollection.Add(typeInfo.AsType());
	        }
	        return typeCollection;
	    }

	    /// <summary>
	    /// Simple method just grabs the simple name of the assembly,
	    /// which is normally the file name.
	    /// </summary>
	    /// <param name="type">The type to find the assy for.</param>
	    /// <returns>
	    /// <see langword="null"/> if input is <see langword="null"/> .
	    /// </returns>
	    public static string PAFGetAssemblySimpleName(this Type type)
	    {
	        var assemblySimpleName = type?.GetTypeInfo().Assembly.GetName().Name;
	        return assemblySimpleName;
	    }
	    /// <summary>
	    /// Simple method just grabs the assembly associated with a <see cref="Type"/>.
	    /// </summary>
	    /// <param name="type">The type to find the assy for.</param>
	    /// <returns>
	    /// <see langword="null"/> if input is <see langword="null"/> .
	    /// </returns>
	    public static Assembly PAFGetAssemblyForType(this Type type)
	    {
	        var assembly = type?.GetTypeInfo().Assembly;
	        return assembly;
	    }

		/// <summary>
        /// Grabs the custom attribute data types for all attributes 
        /// on a member or in an inheitance hierarchy.
        /// </summary>
        /// <param name="memberInfo">One of us.</param>
        /// <param name="findInheritedAttributes">
        /// Default = <see langword="false"/> to be in consonance with the new "lightweightedness"
        /// theme. Note that this parameter is ignored if we are not a <see cref="TypeInfo"/>.
        /// </param>
        /// <returns>
        /// Empty collection if no attributes with the correct characteristics are found.
        /// </returns>
        public static IEnumerable<Type> PAFGetCustomAttributeTypes(
			this MemberInfo memberInfo,	bool findInheritedAttributes = false)
		{
			var col = new Collection<Type>();
			var attDatas = PAFGetCustomAttributeData(memberInfo, null, findInheritedAttributes);
			if (attDatas == null) return col;
			foreach (var attData in attDatas)
			{
				col.Add(attData.GetType());				
			}
			return col;
		}

        /// <summary>
        /// Grabs the custom attribute data for all attributes of a given type declared
        /// on a member or in an inheritance hierarchy. Note that this method was necessary
        /// because we do want to reflect on assemblies without loading them, so we needed
        /// to use a stringful representation of the type. This method is needed to support
        /// legacy apps. This is usually only
        /// needed when we have the ability to load assemblies in lieu of staticly
        /// linking them. We don't know what capabilities are eventually going to wind
        /// up on mobile devices, but we need this on 4.5 .Net going forward, we suspect.
        /// <see cref="Type"/> is lightweight now, but we need this method for back/forward
        /// compatibility.
        /// </summary>
        /// <param name="memberInfo">One of us.</param>
        /// <param name="typeStringOfAttribute">
        /// "Simple" name of the attribute type. <see langword="null"/> to return all attributes.
        /// </param>
        /// <param name="findInheritedAttributes">
        /// Default = <see langword="false"/> to be in consonance with the new "lightweightedness"
        /// theme. Note that this parameter is ignored if we are not a <see cref="TypeInfo"/>.
        /// </param>
        /// <returns>
        /// <see langword="null"/> if no attributes with the correct characteristics are found.
        /// This behavior CANNOT be changed, because use cases require it.
        /// </returns>
        public static IEnumerable<CustomAttributeData> PAFGetCustomAttributeData(
            this MemberInfo memberInfo, string typeStringOfAttribute = null,
            bool findInheritedAttributes = false)
        {
            // tack on suffix if caller forgot.
	        if (!string.IsNullOrEmpty(typeStringOfAttribute)
	            &&
	            // ReSharper disable once PossibleNullReferenceException
				// Rare ReSharper bug
	            !typeStringOfAttribute.Contains("Attribute"))
	        {
		        typeStringOfAttribute += "Attribute";
	        }

	        var col = new Collection<CustomAttributeData>();
            do
            {
                var aDatas = memberInfo.CustomAttributes;
                if (aDatas == null) return null;
	            // ReSharper disable once LoopCanBePartlyConvertedToQuery
                foreach (var aData in aDatas)
                {
                    var aDataType = aData.AttributeType;
                    var aSimpleName = aDataType.Name;
                    if ((typeStringOfAttribute != null) &&
                        (string.Equals(aSimpleName, typeStringOfAttribute, StringComparison.Ordinal)))
                    {
                        col.Add(aData);
                    }
                }

	            TypeInfo typeInfo;
	            if (((typeInfo = (memberInfo as TypeInfo)) == null) || (!findInheritedAttributes)) break;

                // Bail out if we're at the root. Writing it this way is a bit more clear.
                Type type = typeInfo.AsType();
                if ((type == typeof(object)) || (type.IsInterface)) break;

                // If we get here we are working with TypeInfo and we want TypeInfo's
                // Inherited attributes.
                typeInfo = typeInfo.BaseType.PAFGetTypeInfo();

                // Easiest way to update memberInfo for the loop.
                memberInfo = typeInfo;

            } while (true);
            if (col.Count == 0) return null;
            return col;
        }

        /// <summary>
        /// Returns all of a type's "declared" methods.
        /// </summary>
        /// <param name="type">One of us.</param>
        /// <returns>
        ///  Never <see langword="null"/>.
        /// </returns>
        public static IEnumerable<MethodInfo> PAFGetMethods(this Type type)
        {
            var typeInfo = type.GetTypeInfo();
            var methods = typeInfo.DeclaredMethods;
            return methods;
        }

        /// <summary>
        /// A method to climb a type hierarchy and pull data from various supertypes
        /// and aggregate it. This is needed to replace the functionality in binding flags.
        /// </summary>
        /// <param name="typeInfo">One of us.</param>
        /// <param name="dataGetter">
        /// Delegate to pull required data off a <see cref="TypeInfo"/>.
        /// </param>
        /// <param name="excludeDuplicates">
        /// <see langword="true"/> to ignore duplicate data found in the hierarchy.
        /// Default = <see langword="false"/>.
        /// </param>
        /// <param name="numLevelsUp">
        /// Number of levels in the type inheritance hierarchy to climb. The default is -1,
        /// which goes to the top. 0 just gets the current level
        /// </param>
        /// <param name="ignoreObject">
        /// If <see langword="true"/> we don't grab stuff off the base class <see cref="Object"/>.
        /// </param>
        /// <returns>
        /// Empty collection if no attributes with the correct characteristics are found.
        /// </returns>
        /// <exceptions>
        /// <exception cref="ArgumentNullException">"typeInfo"</exception>
        /// <exception cref="ArgumentNullException">"dataGetter"</exception>
        /// </exceptions>
        /// <remarks>
        /// This method was needed to replace type constituent fetches with
        /// "flattened" binding flags. There were a bunch of one-offs of more
        /// sophisticated search specs that have also been absorbed into this
        /// method and its <see paramref="datGetter"/>s.
        /// </remarks>
        public static IList<T> GetHierarchicalEnumerableTypeData<T>(
			this TypeInfo typeInfo, Func<TypeInfo, IEnumerable<T>> dataGetter,
			bool excludeDuplicates = false, int numLevelsUp = -1, bool ignoreObject = true)
		{
			if (typeInfo == null) throw new ArgumentNullException("typeInfo");
			if (dataGetter == null) throw new ArgumentNullException("dataGetter");

			// ReSharper disable once CollectionNeverUpdated.Local
			// KRM - resharper error.
			IList<T> accumulatedData = new Collection<T>();


			do
			{
				// Get out early if we are on object and we are ignoring it.
				if ((typeInfo.BaseType == null) && (ignoreObject))
					return accumulatedData;

				var fetchedData = dataGetter(typeInfo);

				accumulatedData = accumulatedData.Combine(fetchedData, excludeDuplicates);

				// KRM Compiler bug requires this.
				Type baseType = typeInfo.BaseType;

				if (baseType == null) return accumulatedData;

				// KRM so that this works - WTF?
				typeInfo = baseType.PAFGetTypeInfo();

				if (numLevelsUp > 0)
					numLevelsUp--;

			} while (numLevelsUp != 0);

			return accumulatedData;
		}

		/// <summary>
		/// Just gets an instance constructor for a type with varying arguments.
		/// </summary>
		/// <param name="type">One of us.</param>
		/// <param name="parametersToPass">
		/// <see langword="null"/> to search for a default (parameterless) constructor. Must
		/// have a <see langword="null"/> retval property.
		/// </param>
		/// <param name="publicOnly">
		/// <see langword="true"/> to return only public constructors. Since we are not
		/// allowing any type variation, there can only be one constructor with a given
		/// set of parameters (or none).
		/// </param>
		public static ConstructorInfo GetInstanceConstructor(this Type type,
			MethodParameters parametersToPass = null, bool? publicOnly = true)
		{
			var typeInfo = type.PAFGetTypeInfo();

		    var constructors = typeInfo.DeclaredConstructors.BuildCollection();

            // Filter out static, incompatible scope methods.
            var cons = constructors?.FilterMethodBasesOnPI(publicOnly);
			if (cons == null) return null;

			foreach (var con in cons)
			{
				var paramsDefined = con.GetParameters();
				var requiredParams = new MethodParameters(paramsDefined);

				if (requiredParams.CanCallWith(parametersToPass))
					return (ConstructorInfo)con;
			}
			// Didn't get one.
			return null;
		}
		/// <summary>
		/// Gets an instance constructor for a type with type as the single
		/// argument.
		/// </summary>
		/// <param name="type">One of us.</param>
		/// <param name="publicOnly">
		/// <see langword="true"/> to return only public constructors. Since we are not
		/// allowing any type variation, there can only be one constructor with a given
		/// set of parameters (or none).
		/// </param>
		public static ConstructorInfo GetCopyConstructor(this Type type,
			bool? publicOnly = true)
		{
			var typeInfo = type.PAFGetTypeInfo();

			var declaredConstructors = typeInfo.DeclaredConstructors;

			if (declaredConstructors == null) return null;

			var parameterCharacteristics = new Collection<ParameterCharacteristics> { new ParameterCharacteristics(type) };
			var methodParameters = new MethodParameters(parameterCharacteristics);

			return type.GetInstanceConstructor(methodParameters, publicOnly);
		}

		/// <summary>
		/// Just returns the type's TypeInfo, which is also its MemberInfo. This
		/// is a wrapper in 4.5 and trivial in 4.0.
		/// </summary>
		/// <param name="type">One of us.</param>
		/// <returns>
		/// The member info.
		/// </returns>">
		/// <exceptions>
		/// <exception cref="ArgumentNullException">"type"</exception>
		/// </exceptions>
		public static MemberInfo GetATypesMemberInfo(this Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

			return type.PAFGetTypeInfo();
		}

		/// <summary>
		/// A little shim needed for 4.5+. Just returns the type's assembly.
		/// which is a wrapper in 4.5 and trivial in 4.0.
		/// </summary>
		/// <param name="type">One of us.</param>
		/// <returns>
		/// The assembly.
		/// </returns>">
		/// <exceptions>
		/// <exception cref="ArgumentNullException">"type"</exception>
		/// </exceptions>
		public static Assembly GetDefiningAssembly(this Type type)
		{
			if (type == null) throw new ArgumentNullException("type");
			return type.PAFGetTypeInfo().Assembly;
		}
		/// <summary>
		/// Just returns the type's TypeInfo, which is also its MemberInfo. This
		/// is a wrapper in 4.5 and trivial in 4.0. Deals with collections of types.
		/// </summary>
		/// <param name="types">One of us.</param>
		/// <returns>
		/// The member infos.
		/// </returns>">
		/// <exceptions>
		/// <exception cref="ArgumentNullException">"types"</exception>
		/// </exceptions>
		public static IEnumerable<MemberInfo> GetMemberInfosFromTypes(this IEnumerable<Type> types)
		{
			if (types == null) throw new ArgumentNullException("types");
			var memberInfos = new Collection<MemberInfo>();
			foreach (var type in types)
			{
				memberInfos.Add(type.PAFGetTypeInfo());
			}

			return memberInfos;
		}

		/// <summary>
		/// This method is used to return a collection of types which are NOT decorated with a given
		/// attribute described with a string. This method is useful when Types are being handled
		/// remotely and the actual attribute <see cref="Type"/> is not loaded into the caller's
		/// "AppDomain". In these cases, the attributes on the members must be
		/// evaluated through their names.
		/// </summary>
		/// <param name="types">
		/// Collection of <see cref="Type"/>'s. Can be <see langword="null"/>.
		/// </param>
		/// <param name="attributeName">
		/// Name of the <see cref="Attribute"/>. If the caller forgets that CLI attributes end
		/// with "Attribute", this suffix is appended. Blank name returns an
		/// empty list.
		/// </param>
		/// <returns>
		/// <see langword="null"/>for <see langword="null"/> incoming collection. Otherwise
		/// the list of <see cref="Type"/>'s WITHOUT the given attribute.
		/// </returns>
		/// <exceptions>
		/// <exception> <see cref="ArgumentNullException"/>"types"</exception>
		/// </exceptions>
		/// <exceptions>
		/// <exception> <see cref="ArgumentNullException"/>"attributeName"</exception>
		/// </exceptions>
		public static IEnumerable<Type> GetTypesWithoutPublicNamedAttributeInfo(
			this IEnumerable<Type> types, string attributeName)
		{
			if (types == null) throw new ArgumentNullException("types");
			if (attributeName == null) throw new ArgumentNullException("attributeName");
			var memberInfos = types.GetMemberInfosFromTypes();
			if (memberInfos == null) return null;

			if (attributeName == "") return new Collection<Type>();

			var filteredMemberInfos = memberInfos.GetMembersWithoutPublicNamedAttributeInfo(attributeName);

		    var filteredTypeInfos = filteredMemberInfos?.ConvertableEnumElementsIntoList<MemberInfo, TypeInfo>();

			return filteredTypeInfos?.BackToTypes();
		}
		/// <summary>
		/// This method is used to return a collection of types which are decorated with a given
		/// attribute described with a string. This method is useful when Types are being handled
		/// remotely and the actual attribute <see cref="Type"/> is not loaded into the caller's
		/// "AppDomain". In these cases, the attributes on the members must be
		/// evaluated through their names.
		/// </summary>
		/// <param name="types">
		/// Collection of <see cref="Type"/>'s. Can be <see langword="null"/>.
		/// </param>
		/// <param name="attributeName">
		/// Name of the <see cref="Attribute"/>. If the caller forgets that CLI attributes end
		/// with "Attribute", this suffix is appended. Blank name returns an
		/// empty list.
		/// </param>
		/// <returns>
		/// <see langword="null"/>for <see langword="null"/> incoming collection. Otherwise
		/// the list of <see cref="Type"/>'s with the given attribute.
		/// </returns>
		/// <exceptions>
		/// <exception> <see cref="ArgumentNullException"/>"types"</exception>
		/// </exceptions>
		/// <exceptions>
		/// <exception> <see cref="ArgumentNullException"/>"attributeName"</exception>
		/// </exceptions>
		public static IEnumerable<Type> GetTypesWithPublicNamedAttributeInfo(
			this IEnumerable<Type> types, string attributeName)
		{
			if (types == null) throw new ArgumentNullException("types");
			if (attributeName == null) throw new ArgumentNullException("attributeName");
			var memberInfos = types.GetMemberInfosFromTypes();
			if (memberInfos == null) return null;

			if (attributeName == "") return new Collection<Type>();

			var filteredMemberInfos = memberInfos.GetMembersWithPublicNamedAttributeInfo(attributeName);

		    var filteredTypeInfos = filteredMemberInfos?.ConvertableEnumElementsIntoList<MemberInfo, TypeInfo>();

			return filteredTypeInfos?.BackToTypes();
		}

		/// <summary>
		/// Determines if one type is assignable from another.
		/// </summary>
		/// <param name="type">One of us.</param>
		/// <param name="typeToAssign">
		/// <see cref="Type"/> to check.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if assignable.
		/// </returns>
		public static bool IsTypeAssignableFrom(this Type type, Type typeToAssign)
		{
			if (type.PAFGetTypeInfo().IsAssignableFrom(typeToAssign.PAFGetTypeInfo()))
				return true;
			return false;
		}

		/// <summary>
		/// Determines if one type is a subtype of another.
		/// </summary>
		/// <param name="type">One of us.</param>
		/// <param name="superType">
		/// <see cref="Type"/> to check.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if <paramref name="superType"/> is truly a supertype.
		/// </returns>
		public static bool IsTypeASubtypeOf(this Type type, Type superType)
		{
			if (type.PAFGetTypeInfo().IsSubclassOf(superType))
				return true;
			return false;
		}

		/// <summary>
		/// Determines if the type is an "enum".
		/// </summary>
		/// <param name="type">One of us.</param>
		/// <returns>
		/// <see langword="true"/> if an enum type.
		/// </returns>
		public static bool IsTypeAnEnum(this Type type)
		{
			if (type.PAFGetTypeInfo().IsEnum)
				return true;
			return false;
		}

		/// <summary>
		/// Determines if a type is an interface type.
		/// </summary>
		/// <param name="type">One of us.</param>
		/// <returns>
		/// <see langword="true"/> if an interface.
		/// </returns>
		public static bool IsTypeAnInterfaceType(this Type type)
		{
			if (type.PAFGetTypeInfo().IsInterface)
				return true;
			return false;
		}


		/// <summary>
		/// Determines if a type is a value type.
		/// </summary>
		/// <param name="type">One of us.</param>
		/// <returns>
		/// <see langword="true"/> if a <see cref="ValueType"/>.
		/// </returns>
		public static bool IsTypeAValueType(this Type type)
		{
			if (type.PAFGetTypeInfo().IsValueType)
				return true;
			return false;
		}

		/// <summary>
		/// This method attempts to load all types defined in an assembly, also
		/// loading dependant assemblies in order to fetch typeinfos. Some types
		/// will not be loadable due to security and trust issues in different
		/// environments. The way the MS reflection library is SUPPOSED to work
		/// is that internal (versus public) types from the calling assembly and
		/// from assemblies whose internals are exposed to the calling assembly
		/// will also be returned.
		/// </summary>
		/// <param name="assembly">One of us.</param>
		/// <returns>
		/// List of accessible types under current security or trust restrictions.
		/// </returns>
		public static IEnumerable<Type> PAFGetAccessibleTypes(this Assembly assembly)
		{
			IEnumerable<Type> accessibleTypes;
			try
			{
				accessibleTypes = assembly.DefinedTypes.BackToTypes();
				return accessibleTypes;
			}
			catch (ReflectionTypeLoadException ex)
			{
				// The exception is thrown if some types cannot be loaded in partial trust.
				// For our purposes we just want to get the types that are loaded, which are
				// provided in the Types property of the exception.
				accessibleTypes = ex.Types.RemoveNullElements();
				return accessibleTypes;
			}
		}

		/// <summary>
		/// This method attempts to locate a type within an assembly. Needed as a shim
		/// since MS broke this in 4.5+ PCLs. All of the types returned by this method
		/// will have been loaded and thus verified as "accessible" by the calling assembly
		/// in the current trust environment.
		/// </summary>
		/// <param name="assembly">One of us.</param>
		/// <param name="namespaceQualifiedTypeName">
		/// Name of the type. No embedded blanks or any other sloppiness - we don't
		/// normalize the string.
		/// </param>
		/// <returns>
		/// The type, if found. <see langword="null"/> if not found.
		/// </returns>
		public static Type PAFGetAccessibleType(this Assembly assembly, string namespaceQualifiedTypeName)
		{
			Type foundType = null;
			var accessibleTypes = assembly.PAFGetAccessibleTypes();

			if (accessibleTypes != null)
			{
				foreach (var type in accessibleTypes)
				{
					// ReSharper disable once InconsistentNaming
					var foundNSQTN = type.FullName;
					if (string.CompareOrdinal(foundNSQTN, namespaceQualifiedTypeName) == 0)
					{
						foundType = type;
						break;
					}
				}
			}

			return foundType;
		}


		/// <summary>
		/// This method wraps the "GetTypeInfo", since it's that which actually loads
		/// the type and we may want to handle errors in one place.
		/// </summary>
		/// <param name="type">One of us.</param>
		/// <returns>
		/// The type's typeinfo.
		/// </returns>
		/// <remarks>
		/// This implementation is just a passthru to "GetTypeInfo()".
		/// </remarks>
		public static TypeInfo PAFGetTypeInfo(this Type type)
		{
            if(type == null)
                return null;
            return type.GetTypeInfo();
		}

		#region DataGetters
		/// <summary>
		/// Gets interfaces implemented on the current type, but not its superclasses.
		/// </summary>
		/// <param name="typeInfo">info for the current type.</param>
		/// <returns>Empty collection for no interfaces.</returns>
		public static IEnumerable<Type> GetImplementedInterfacesAtLevel(
			this TypeInfo typeInfo)
		{
			return typeInfo.ImplementedInterfaces;
		}
		/// <summary>
		/// Gets interfaces implemented up a type hierarchy.
		/// </summary>
		/// <param name="type">Current type.</param>
		/// <returns>Empty collection for no interfaces.</returns>
		public static IEnumerable<Type> GatherImplementedInterfacesHierarchically(this Type type)
		{
			return type.PAFGetTypeInfo().GetHierarchicalEnumerableTypeData(
				GetImplementedInterfacesAtLevel, true);
		}
		/// <summary>
		/// Gets methods declared on the current type, but not its superclasses.
		/// </summary>
		/// <param name="typeInfo">info for the current type.</param>
		/// <returns>Empty collection for no methods.</returns>
		public static IEnumerable<MethodInfo> GetDeclareddMethodsOnType(
			this TypeInfo typeInfo)
		{
			return typeInfo.DeclaredMethods;
		}

		/// <summary>
		/// Gets methods with given name implemented up a type hierarchy.
		/// </summary>
		/// <param name="type">Current type.</param>
		/// <param name="methodName"></param>
		/// <returns>Empty collection for no methods.</returns>
		public static IList<MethodInfo> GatherImplementedNamedMethodsHierarchically
			(this Type type, string methodName)
		{
			var returnList = new List<MethodInfo>();

			 var methodInfos = type.PAFGetTypeInfo().GetHierarchicalEnumerableTypeData(
				GetDeclareddMethodsOnType, true);

			if (methodInfos.Count == 0)
				return returnList;

			return returnList.FilterMethodInfosOnNameMatch(methodName);

		}
		/// <summary>
		/// Gets <see cref="MethodInfo"/>s implemented up a type hierarchy.
		/// </summary>
		/// <param name="type">Current type.</param>
		/// <returns>Empty collection for no methods.</returns>
		public static IList<MethodInfo> GatherImplementedMethodsHierarchically(this Type type)
		{
			return type.PAFGetTypeInfo().GetHierarchicalEnumerableTypeData(
				GetDeclareddMethodsOnType, true);
		}
		/// <summary>
		/// Gets properties implemented on the current type, but not its superclasses.
		/// </summary>
		/// <param name="typeInfo">info for the current type.</param>
		/// <returns>Empty collection for no props.</returns>
		public static IList<PropertyInfo> GetImplementedPropertiesAtLevel(
			this TypeInfo typeInfo)
		{
			return typeInfo.DeclaredProperties.ToList();
		}
		/// <summary>
		/// Gets properties implemented up a type hierarchy.
		/// </summary>
		/// <param name="type">Current type.</param>
		/// <returns>Empty collection for no properties.</returns>
		public static IEnumerable<PropertyInfo> GatherImplementedPropertiesHierarchically(this Type type)
		{
			return type.PAFGetTypeInfo().GetHierarchicalEnumerableTypeData(
				GetImplementedPropertiesAtLevel, true);
		}
		#endregion // DataGetters
	}
}
