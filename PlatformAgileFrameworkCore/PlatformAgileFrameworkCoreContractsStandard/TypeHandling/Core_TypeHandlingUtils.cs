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
using System.Reflection;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.TypeHandling.Exceptions;
using PlatformAgileFramework.TypeHandling.MemberExtensionMethods;
using PlatformAgileFramework.TypeHandling.MethodHelpers;
using PlatformAgileFramework.TypeHandling.ParameterHelpers;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;

namespace PlatformAgileFramework.TypeHandling
{
	/// <summary>
	/// Helper classes/methods for TypeHandling. This is the Core version. The
	/// other part of the partial class is the ECMA/CLR version. This part works for both.
	/// </summary>
	// ReSharper disable PartialTypeWithSinglePart
	public partial class TypeHandlingUtils : CoreContract_TypeHandlingUtils
	// ReSharper restore PartialTypeWithSinglePart
	{
		#region Delegates
		/// <summary>
		/// This is a delegate for filtering types according to arbitrary criteria.
		/// </summary>
		/// <typeparam name="T">
		/// Arbitrary type.
		/// </typeparam>
		/// <param name="typeInstance">
		/// The type instance to be checked.
		/// </param>
		/// <param name="auxData">
		/// Arbitrary auxiliary data to be used in the test.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the type passes the criteria.
		/// </returns>
		public delegate bool GenericMemberFilter<in T>(T typeInstance, object auxData)
			where T : MemberInfo;
		/// <summary>
		/// This is a delegate for filtering types according to arbitrary criteria.
		/// </summary>
		/// <typeparam name="T">
		/// Arbitrary type.
		/// </typeparam>
		/// <param name="typeInstance">
		/// The type instance to be checked.
		/// </param>
		/// <param name="obj">
		/// Arbitrary auxiliary data to be used in the test.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the type passes the criteria.
		/// </returns>
		public delegate bool GenericTypeFilter<in T>(T typeInstance, object obj);
		/// <summary>
		/// This is a delegate for filtering types according to arbitrary criteria.
		/// Types can be remote with a stringful representation.
		/// </summary>
		/// <param name="typeHolder">
		/// The type to be checked.
		/// </param>
		/// <param name="obj">
		/// Arbitrary auxiliary data to be used in the test.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the type passes the criteria.
		/// </returns>
		public delegate bool RemotableTypeFilter(PAFTypeHolder typeHolder, object obj);
		/// <typeparam name="T">Item type.</typeparam>
		/// <summary>
		/// This delegate makes a clone of the type <typeparamref name="T"/> for the
		/// observable update functions. This is usually a deep copy for avoiding
		/// concurrency problems when multiple threads operate on the same object,
		/// but may be a shallow cloner when needed.
		/// </summary>
		/// <param name="t">This is the item to make a copy of.</param>
		/// <returns>The copy.</returns>
		public delegate T TypeCloner<T>(T t);
		#endregion // Delegates
		#region Methods
		#region Partial Methods
		/// <summary>
		/// Makes a copy through "ICloneable" in ECMA/CLR.
		/// </summary>
		/// <param name="objectToClone">Incoming object.</param>
		/// <param name="clonedObject">
		/// Copy made through casting to "ICloneable".
		/// </param>
		// ReSharper disable UnusedMember.Local
		// ReSharper disable PartialMethodWithSinglePart
		static partial void MakeClone(object objectToClone, ref object clonedObject);
		// ReSharper restore PartialMethodWithSinglePart
		// ReSharper restore UnusedMember.Local
		#endregion // Partial Methods
		/// <summary>
		/// This method tries to make a deep copy of the type. It tests if the type
		/// has a copy constructor, then uses it if it does. This method uses
		/// reflection to determine if the type has a copy constructor and thus is not
		/// lightweight.
		/// </summary>
		/// <param name="obj">The object to copy.</param>
		/// <returns><see langword="null"/> if the operation was not successful.</returns>
		public static object ConstructorClone(object obj)
		{
			var objOut = TryDefaultConstructorCopy(obj);
			return objOut;
		}
		/// <summary>
		/// This method tries to make a deep copy of the type. It first tests for the type
		/// to be <see cref="IPAFGenericDeepCloneable{T}"/>, then if the type has a copy constructor,
		/// then gives up and passes the instance through to the output. This method
		/// uses reflection to determine if the type has a copy constructor and thus is
		/// not lightweight.
		/// </summary>
		/// <param name="t">The type instance to copy.</param>
		/// <typeparam name="T">
		/// The <see cref="Type"/> to copy.
		/// </typeparam>
		/// <returns><see langword="null"/> if copy not successful.</returns>
		public static T DeepCopy<T>(T t)
		{
			var cloneable = t as IPAFGenericDeepCloneable<T>;
			if (cloneable != null)
				return cloneable.DeepCloneItem();
			var obj = ConstructorClone(t);
			if (obj != null) return (T)obj;
			return t;
		}
		#region Type Filters
		/// <summary>
		/// This is a method for filtering <see cref="Type"/> objects according to
		/// the presence of certain <see cref="System.Attribute"/>s.
		/// This method tests whether attributes match based on a
		/// <c>typeof(Attribute)</c> check.
		/// </summary>
		/// <param name="type">
		/// This is a <see cref="Type"/> that may have attributes attached to it.
		/// It may be <see langword="null"/>, in which case the method returns <see langword="true"/>.
		/// </param>
		/// <param name="attributeTypes">
		/// This is an array of <see cref="Attribute"/> subtypes that must be present on
		/// the incoming type in order for it to "pass" the test criteria. If this argument
		/// is <see langword="null"/>, the method always returns <see langword="true"/>. If
		/// any of the attributes in this array are present on the type, it passes the test
		/// and <see langword="true"/> is returned.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the type passes the criteria.
		/// </returns>
		public static bool FilterTypeByAnyAttributePresence(Type type,
			IEnumerable<Type> attributeTypes)
		{
			// Get out if nothing to do.
			if ((attributeTypes == null) || (type == null))
			{
				return true;
			}

			attributeTypes = attributeTypes.IntoArray();

			foreach (var attributeType in attributeTypes)
			{
				if(type.GetTypeInfo().GetCustomAttribute(attributeType, true) != null) return true;
			}
			return false;
		}
		/// <summary>
		/// This is a method for filtering <see cref="Type"/> objects according to
		/// the presence or non presence of certain <see cref="System.Attribute"/>s.
		/// This method tests whether attributes match based on a<c>typeof(Attribute)</c> check.
		/// </summary>
		/// <param name="type">
		/// This is a <see cref="Type"/> that may have attributes attached to it.
		/// It may be <see langword="null"/>, in which case the method returns <see langword="true"/>.
		/// </param>
		/// <param name="attributeTypes">
		/// This is an array of <see cref="System.Attribute"/> subtypes that must NOT be present on
		/// the incoming type in order for it to "pass" the test criteria. If this argument
		/// is <see langword="null"/>, the method always returns <see langword="true"/>. If
		/// any of the attributes in this array is present on the type, it fails the test
		/// and <see langword="false"/> is returned.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the type passes the criteria.
		/// </returns>
		public static bool FilterTypeByNoAttributePresence(Type type, IEnumerable<Type> attributeTypes)
		{
			// Get out if nothing to do.
			if ((attributeTypes == null) || (type == null))
			{
				return true;
			}

			attributeTypes = attributeTypes.IntoArray();

			foreach (var attributeType in attributeTypes)
			{
				if (type.GetTypeInfo().GetCustomAttribute(attributeType, true) != null) return false;
			}
			// Doesn't have any of the proscribed attributes, so we pass.
			return true;
		}
		#endregion // Type Filters

		/// <summary>
		/// This method checks a given Type to see if it inherits from another
		/// given Type.
		/// </summary>
		/// <param name="inheritedType">
		/// This Type may be either an interface or a struct or class. If it is a
		/// struct, the method returns <see langword="false"/>. If it is class, and the
		/// <paramref name="typeToCheck"/> is an interface or struct, the method returns
		/// <see langword="false"/>.
		/// </param>
		/// <param name="typeToCheck">
		/// If this Type is a struct, the method returns <see langword="false"/> if the
		/// <paramref name="inheritedType"/> is not an interface. This parameter
		/// can represent a class, struct or interface.
		/// </param>
		/// <returns>
		/// Indicates whether <paramref name="typeToCheck"/> inherits from
		/// <paramref name="inheritedType">. If <paramref name="inheritedType"/>
		/// is an interface, the method only returns <see langword="true"/> if the inheritance
		/// is proper (i.e. type of <paramref name="typeToCheck"/> is not equal
		/// to type of <paramref name="inheritedType"/>). If <paramref name="inheritedType"/>
		/// is a class, <see langword="true"/> is returned if <paramref name="typeToCheck"/>
		/// is equal to type of inheritedType or a proper subclass.</paramref>
		/// </returns>
		/// <remarks>
		/// Noted, unfortunately, that this method is NOT lightweight. MS's GetInterfaces
		/// method generates a new Type[] array every time.
		/// </remarks>
		public static bool DoesSecondTypeInheritFromFirstType(Type inheritedType, Type typeToCheck)
		{
			// Interface collection is always flat. Just loop over them.
			// ReSharper disable LoopCanBeConvertedToQuery
			foreach (Type iface in typeToCheck.GatherImplementedInterfacesHierarchically())
			{
				// ReSharper restore LoopCanBeConvertedToQuery
				if (iface == inheritedType)
				{
					return true;
				}
			}
			// TODO - KRM - this is wrong, wrong, wrong! None of the wacky MS shit for windows store
			// TODO - should be used in our code, except in the extension class.
			// All done if we are an interface.
			if (typeToCheck.GetTypeInfo().IsInterface) return false;
			// Gotta' climb the inheritance tree.
			while (typeToCheck != typeof(object))
			{
				if (inheritedType == typeToCheck)
				{
					return true;
				}
				// ReSharper disable PossibleNullReferenceException
				typeToCheck = typeToCheck.GetTypeInfo().BaseType;
				// ReSharper restore PossibleNullReferenceException
			}
			return false;
		}

		/// <summary>
		/// This method checks a given Type to see if it inherits from another
		/// given Type.
		/// </summary>
		/// <param name="inheritedType">
		/// This Type may be either an interface or a struct or class. If it is a
		/// struct, the method returns <see langword="false"/>. If it is class, and the
		/// <paramref name="typeToCheck"/> is an interface or struct, the method returns
		/// <see langword="false"/>.
		/// </param>
		/// <param name="typeToCheck">
		/// If this Type is a struct, the method returns <see langword="false"/> if the
		/// <paramref name="inheritedType"/> is not an interface. This parameter
		/// can represent a class, struct or interface.
		/// </param>
		/// <param name="exactTypeMatch">
		/// Default of <see langword="false"/> allows an inherited type to return
		/// <see langword="true"/>. <see langword="true"/> means types must be an
		/// exact match.
		/// </param>
		/// <returns>
		/// Indicates whether <paramref name="typeToCheck"/> inherits from
		/// <paramref name="inheritedType">. If <paramref name="inheritedType"/>
		/// is an interface, the method returns <see langword="true"/> if the inheritance
		/// is proper (i.e. type of <paramref name="typeToCheck"/> is not equal
		/// to type of <paramref name="inheritedType"/>) or it is the exact
		/// same interfrace type. If <paramref name="inheritedType"/>
		/// is a class, <see langword="true"/> is returned if <paramref name="typeToCheck"/>
		/// is equal to type of inheritedType or a proper subclass.</paramref>
		/// </returns>
		/// <remarks>
		/// Noted, unfortunately, that this method is NOT lightweight. MS's GetInterfaces
		/// method generates a new Type[] array every time.
		/// </remarks>
		public static bool DoesSecondTypeInheritFromOrMatchFirstType(Type inheritedType, Type typeToCheck
			, bool exactTypeMatch = false)
		{
			if ((exactTypeMatch) && (inheritedType != typeToCheck)) return false;
			if (inheritedType == typeToCheck) return true;
			// Interface collection is always flat. Just loop over them.
			// ReSharper disable LoopCanBeConvertedToQuery
			foreach (Type iface in typeToCheck.GetTypeInfo().ImplementedInterfaces)
			{
				// ReSharper restore LoopCanBeConvertedToQuery
				if (iface == inheritedType)
				{
					return true;
				}
			}
			// All done if we are an interface.
			if (typeToCheck.GetTypeInfo().IsInterface) return false;
			// Gotta' climb the inheritance tree.
			while (typeToCheck != typeof(object))
			{
				if (inheritedType == typeToCheck)
				{
					return true;
				}
				// ReSharper disable PossibleNullReferenceException
				typeToCheck = typeToCheck.GetTypeInfo().BaseType;
				// ReSharper restore PossibleNullReferenceException
			}
			return false;
		}

		/// <summary>
		/// Determines whether an unqualified Generic is <see langword="null"/>.
		/// </summary>
		/// <param name="genericItem">Item to check.</param>
		/// <typeparam name="T">Generic type.</typeparam>
		/// <returns>
		/// <see langword="false"/> if the item is not a reference type or not
		/// <see langword="null"/>.
		/// </returns>
		public static bool GenericIsNull<T>(T genericItem)
		{
			// Get around everybody's complaint about value/reference comparisons.
			if ((!(genericItem is ValueType)) && (genericItem.Equals(default(T)))) return true;
			return false;
		}
		/// <summary>
		/// Uses reflection to retrieve a specific <see cref="MethodInfo"/> for a method
		/// on a Type. This method has the ability to filter <see cref="MethodInfo"/> elements
		/// according to general attribute settings.
		/// </summary>
		/// <param name="methodCharacteristics">
		/// These describe the parameters we are looking for on the method, along with its host
		/// object.
		/// </param>
		/// <param name="attributeTypes">
		/// This is an array of <see cref="Attribute"/> sub-types that methods are evaluated
		/// against within the <see paramref="attributeFilter"/>. If the method fails the evaluation,
		/// it is not returned.
		/// </param>
		/// <param name="attributeFilter">
		/// This is an <see cref="MemberExtensions.FilterAccessorByAttributTypes"/>
		/// delegate that will be used to evaluate the field against the supplied set of
		/// attributes. This filter must understand how to pull attributes from methods and
		/// examine them. If this filter is <see langword="null"/> and attributes is
		/// non-<see langword="null"/>, the standard CLR style attribute filtering is
		/// applied. With this standard filtering, <see cref="MethodInfo"/>s are returned
		/// only if they have attributes on the list of <see paramref="attributes"/> supplied.
		/// With this standard filtering, a single attribute match will qualify the method
		/// for return.
		/// </param>
		/// <param name="publicMethods">
		/// <see langword="true"/> to return public methods. <see langword="null"/> To return all methods.
		/// </param>
		/// <param name="instanceMethods">
		/// <see langword="true"/> to return instance methods. <see langword="null"/> to return all methods.
		/// </param>
		/// <param name="methodName">
		/// This name is the textual name of the method as known by the host. <see langword="null"/>
		/// may cause multiple methods to be returned.
		/// </param>
		/// <returns>
		/// A MethodInfo object set, if any one is found that passes the filtering criteria
		/// or an empty collection if not.
		/// </returns>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">"methodCharacteristics"</exception>
		/// </exceptions>
		/// <remarks>
		/// This updated method involves a breaking change, since binding flags are no longer
		/// supported. Every piece of client code that we know of uses flattened hierarchy and
		/// public and instance binding flags. This method flattens hierarchy (travels up the
		/// inheritance tree) and finds instance and public methods by default. If you want
		/// something else, assemble a mechanism of of the pieces in the extension classes.
		/// </remarks>
		public static IEnumerable<MethodInfo> GetMethodInfos(
			MethodCharacteristics methodCharacteristics,
			IEnumerable<Type> attributeTypes = null,
			MemberExtensions.FilterAccessorByAttributTypes attributeFilter = null,
			bool? publicMethods = true, bool? instanceMethods = true,
			string methodName = null)
		{
			// Avoid double enumeration.
			attributeTypes = attributeTypes.BuildCollection();

			// The one thng we gotta' have..
			if (methodCharacteristics == null) throw new ArgumentNullException("methodCharacteristics");

			var methodInfoReturnedColl = new Collection<MethodInfo>();

			// Pull every single method off the type.
			var methodInfos
				= methodCharacteristics.HostType.GatherImplementedMethodsHierarchically().FilterMethodInfos();

			if(methodInfos == null) return null;

			foreach (var methodInfo in methodInfos)
			{
				// If we have no custom filter, just do the standard CLR attribute filter if we
				// have attributes - otherwise use our custom filter.
				if (attributeTypes != null)
				{
					// Plug in our standard filter if we need it.
					if (attributeFilter == null)
						attributeFilter = MemberExtensions.FilterMembersByAnyAttributeTypePresence;
					// Apply the filter.
					if (attributeFilter(methodInfo, attributeTypes)) continue;
				}

				// Filter by parameters.
				MethodCharacteristics foundCharacteristics
					= new MethodCharacteristics(methodInfo.GetParameters(), methodCharacteristics.HostType); 
				if(foundCharacteristics.CanCallWith(methodCharacteristics))
					methodInfoReturnedColl.Add(methodInfo);
			}
			// No filtering at all - just return unconditionally.
			return methodInfoReturnedColl;
		}
		/// <summary>
		/// Just returns an array that has type of each passed object.
		/// </summary>
		/// <param name="objects">
		/// Array of objects. <see langword="null"/> causes <see langword="null"/> to be returned.
		/// </param>
		/// <returns>Type array or <see langword="null"/>.
		/// </returns>
		public static Type[] GetObjectTypes(object[] objects)
		{
			if (objects == null)
				return null;
			var types = new Type[objects.Length];
			var count = 0;
			foreach (var o in objects)
			{
				types[count++] = o.GetType();
			}
			return types;
		}


		/// <summary>
		/// This method makes a clone of the object through the "ICloneable"
		/// interface. Since the interface is internal in the SL model, we
		/// use a partial method to enable it in the ECMA/CLR extension.
		/// </summary>
		/// <param name="objectToClone">
		/// <see langword="null"/> returns <see langword="null"/>.
		/// </param>
		/// <returns>
		/// <see langword="null"/> in SL environment or if <paramref name="objectToClone"/>
		/// does not implement "ICloneable".
		/// </returns>
		/// <exceptions>
		/// Exceptions are neither caught or thrown.
		/// </exceptions>
		// ReSharper disable once InconsistentNaming
		public static object ICloneableCopy(object objectToClone)
		{
			if (objectToClone == null) return null;
			if (!objectToClone.GetType().IsTypeICloneable()) return null;
			object clonedObject = null;
			// ReSharper disable once InvocationIsSkipped
			MakeClone(objectToClone, ref clonedObject);
			// ReSharper disable once ExpressionIsAlwaysNull
			return clonedObject;
		}

		/// <summary>
		/// This method tests to see if a <see cref="Type"/> is copyable. A type can
		/// be copyable if either of two conditions exist:
		/// <list type="bullet">
		/// <item>
		///	<term>Copy constructor</term>
		///	<description>The type has a PUBLIC copy constructor.</description>.
		///	</item>
		/// <item>
		///	<term>ICloneable</term>
		///	<description>The type implements "ICloneable".</description>
		///	</item>
		/// </list>
		/// The conditions are checked in the order listed. A copy constructor is
		/// preferred over "ICloneable", since copy constructors are usually deep
		/// copies.
		/// </summary>
		/// <param name="t">The type to check.</param>
		/// <returns><see langword="true"/> if the type is copyable.</returns>
		/// <remarks>
		/// "ICloneable" is internal in SL, so it doesn't qualify
		/// as copyable. Extension is made through partial method in ECMA/CLR.
		/// </remarks>
		public static bool IsTypeCopyable(Type t)
		{
			if (t.GetInstanceConstructor() != null)
				return true;
			if (t.IsTypeICloneable())
				return true;
			return false;
		}
		/// <summary>
		/// Searches for several different ways to copy a generic item - checked
		/// in the following order.
		/// <list type="number">
		/// <item>
		/// <term>Item wears <see cref="IPAFGenericDeepCloneable{T}"/></term>
		/// <description>
		/// Copies based on "deep" cloning.
		/// </description>
		/// </item>
		/// <item>
		/// <term>Item has a copy constructor with one argument.</term>
		/// <description>
		/// Copies with the copy constructor.
		/// </description>
		/// </item>
		/// <item>
		/// <term>Neither</term>
		/// <description>
		/// Just passes out the original item.
		/// </description>
		/// </item>
		/// </list> 
		/// </summary>
		/// <typeparam name="T">Type of the item.</typeparam>
		/// <param name="itemToReplicate">The item.</param>
		/// <returns><see langword="null"/> gets <see langword="null"/>.</returns>
		/// <exceptions>
		/// Exceptions are neither thrown or caught.
		/// </exceptions>
		public static T LayeredSafeReplication<T>(T itemToReplicate)
		{
			if (GenericIsNull(itemToReplicate)) return default(T);

			var deepCloneableItem = itemToReplicate as IPAFGenericDeepCloneable<T>;
			if (deepCloneableItem != null) return deepCloneableItem.DeepCloneItem();

			var clonedObject = ConstructorClone(itemToReplicate);
			if (clonedObject != null) return (T)clonedObject;

			clonedObject = ICloneableCopy(itemToReplicate);
			if (clonedObject != null) return (T)clonedObject;

			// No deep cloning possible - just return the item.
			return itemToReplicate;
		}

		/// <summary>
		/// This method makes a shallow copy of the type. It just returns
		/// the input.
		/// </summary>
		/// <typeparam name="T">
		/// </typeparam>
		public static T ShallowCopy<T>(T t)
		{
			return t;
		}

		/// <summary>
		/// This method tries to make a copy of the object from its copy constructor if
		/// it has one. This method uses reflection to determine
		/// if the type has a copy constructor and thus is not lightweight.
		/// </summary>
		/// <param name="o">The object to copy.</param>
		/// <returns><see langword="null"/> if no copy constructor.</returns>
		/// <exceptions>
		/// None thrown and none caught.
		/// </exceptions>
		public static object TryDefaultConstructorCopy(object o)
		{
			return TryConstructorCopy(o, null);
		}

		/// <summary>
		/// This method tries to make a copy of the object from its copy
		/// constructor if it has one. This method uses reflection to
		/// determine if the type has a copy constructor and thus is not
		/// lightweight.
		/// </summary>
		/// <param name="o">
		/// The object to copy. <see langword="null"/> gets <see langword="null"/>.
		/// </param>
		/// <param name="constructorParams">
		/// Parameters to pass to the constructor. These are appended to the first parameter,
		/// which is the the object to copy. <see langword="null"/> for no additional params is OK.
		/// </param>
		/// <returns>
		/// The copy or <see langword="null"/> if no copy constructor matching the
		/// parameter set is found.
		/// </returns>
		/// <exceptions>
		/// None thrown and none caught.
		/// </exceptions>
		public static object TryConstructorCopy(object o, IList<object> constructorParams)
		{
			if (o == null) return null;
			if (constructorParams == null) constructorParams = new Collection<object>();
			constructorParams.Insert(0, o);
			var constructorParamTypes = GetObjectTypes(constructorParams.IntoArray());
			// First we need to check if we can copy.
			// TODO - KRM change for dynamic lookup of privilege level.
			var info
				= constructorParamTypes[0].GetInstanceConstructor(new MethodParameters(constructorParamTypes));
			return info != null ? info.Invoke(constructorParams.IntoArray()) : null;
		}
		#region Exception Generators
		/// <summary>
		/// Generates an exception to throw when two types don't match.
		/// a type.
		/// </summary>
		/// <param name="typeThatShouldMatch">
		/// Type that should match the other type.
		/// </param>
		/// <param name="otherTypeThatShouldMatch">Other type that must match exactly.</param>
		/// <returns>
		/// <see cref="PAFStandardException{IPAFTypeMismatchExceptionData}"/> with
		/// <see cref="PAFTypeMismatchExceptionMessageTags.TYPES_NOT_AN_EXACT_MATCH"/>
		/// message. If either argument is <see langword="null"/>, method returns <see langword="null"/>.
		/// </returns>
		public static PAFStandardException<IPAFTypeMismatchExceptionData>
			TypeMismatchException(Type typeThatShouldMatch, Type otherTypeThatShouldMatch)
		{
			if (typeThatShouldMatch == null) return null;
			if (otherTypeThatShouldMatch == null) return null;
			if (typeThatShouldMatch == otherTypeThatShouldMatch) return null;
			var data = new PAFTypeMismatchExceptionData(PAFTypeHolder.IHolder(typeThatShouldMatch),
				PAFTypeHolder.IHolder(otherTypeThatShouldMatch));
			return new PAFStandardException<IPAFTypeMismatchExceptionData>(data,
				PAFTypeMismatchExceptionMessageTags.TYPES_NOT_AN_EXACT_MATCH);
		}
		/// <summary>
		/// Generates an exception to throw when an object is not castable to
		/// a type.
		/// </summary>
		/// <param name="objThatShouldInherit">Object to check.</param>
		/// <param name="typeToInheritFrom">Type that the object must be castable to.</param>
		/// <returns>
		/// <see cref="PAFStandardException{IPAFTypeMismatchExceptionData}"/> with
		/// <see cref="PAFTypeMismatchExceptionMessageTags.FIRST_TYPE_NOT_CASTABLE_TO_SECOND_TYPE"/>
		/// message. If either argument is <see langword="null"/>, method returns <see langword="null"/>.
		/// </returns>
		public static PAFStandardException<IPAFTypeMismatchExceptionData>
			ObjectNotInheritedException(object objThatShouldInherit, Type typeToInheritFrom)
		{
			if (objThatShouldInherit == null) return null;
			if (typeToInheritFrom == null) return null;

			if (objThatShouldInherit.GetType() == typeToInheritFrom)
				return null;

			if (objThatShouldInherit.GetType().IsTypeASubtypeOf(typeToInheritFrom))
				return null;

			var typeThatShouldInherit = objThatShouldInherit.GetType();
			return TypeNotInheritedException(typeThatShouldInherit, typeToInheritFrom);
		}
		/// <summary>
		/// Generates an exception to throw when a type is not castable to
		/// a another.
		/// </summary>
		/// <param name="typeThatShouldInherit">
		/// Type that should inherit from the other type.
		/// </param>
		/// <param name="typeToInheritFrom">Type that the object must be castable to.</param>
		/// <returns>
		/// <see cref="PAFStandardException{IPAFTypeMismatchExceptionData}"/> with
		/// <see cref="PAFTypeMismatchExceptionMessageTags.FIRST_TYPE_NOT_CASTABLE_TO_SECOND_TYPE"/>
		/// message. If either argument is <see langword="null"/>, method returns <see langword="null"/>.
		/// </returns>
		public static PAFStandardException<IPAFTypeMismatchExceptionData>
			TypeNotInheritedException(Type typeThatShouldInherit, Type typeToInheritFrom)
		{
			if (typeThatShouldInherit == null) return null;
			if (typeToInheritFrom == null) return null;
			if (DoesSecondTypeInheritFromOrMatchFirstType(
				typeToInheritFrom, typeThatShouldInherit)) return null;
			var data = new PAFTypeMismatchExceptionData(PAFTypeHolder.IHolder(typeThatShouldInherit),
				PAFTypeHolder.IHolder(typeToInheritFrom));
			return new PAFStandardException<IPAFTypeMismatchExceptionData>(data,
				PAFTypeMismatchExceptionMessageTags.FIRST_TYPE_NOT_CASTABLE_TO_SECOND_TYPE);
		}
		#endregion // Exception Generators
		#endregion // Methods
	}
}