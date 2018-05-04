//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2014 Icucom Corporation
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
using PlatformAgileFramework.Platform;
using PlatformAgileFramework.Serializing;
using PlatformAgileFramework.StringParsing;
using PlatformAgileFramework.TypeHandling.Exceptions;
using PlatformAgileFramework.TypeHandling.MemberExtensionMethods;
using PlatformAgileFramework.TypeHandling.MemberExtensionMethods.Helpers;
using PlatformAgileFramework.TypeHandling.ParameterHelpers;
using PlatformAgileFramework.TypeHandling.PropertyExtensionMethods;

namespace PlatformAgileFramework.TypeHandling.TypeExtensionMethods
{
	/// <summary>
	/// <para>
	/// This class has a set of extension methods that can be used for CLR
	/// or Silverlight scenarios. Some of these methods were built specifically
	/// to enable missing CLR functionality on SilverLight. The cross-platform
	/// developer should use the ones here if it is desired to port applications
	/// to environments that do not support full ECMA/CLR features.
	/// </para>
	/// </summary>
	/// <history>
	/// <author> KRM </author>
	/// <date> 04jun2015 </date>
	/// <contribution>
	/// <description>
	/// Added history and updated for MS 4.5 reflection lib rewrite.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Conditionally thread-safe. See member descriptions.
	/// </threadsafety>
	// ReSharper disable PartialTypeWithSinglePart
	public static partial class TypeExtensions
	// ReSharper restore PartialTypeWithSinglePart
	{
		/// <summary>
		/// Public version of SL's TypeFilter.
		/// </summary>
		/// <param name="type">The type to examine.</param>
		/// <param name="filterCriteria">Criteria object to filter against.
		/// An example would be a name.</param>
		/// <returns>Whether the type passes the test.</returns>
		/// <threadsafety>
		/// The implementation of this delegate must be thread safe if the methods
		/// in which it is used are to be thread-safe.
		/// </threadsafety>
		public delegate bool TypeFilterDelegate(Type type, object filterCriteria);
		#region Methods
		#region Partial Methods
		/// <summary>
		/// Examines the type for "ICloneable" in ECMA/CLR.
		/// </summary>
		/// <param name="isCloneable">
		/// Return value indicating whether the type wears the interface.
		/// </param>
		/// <param name="type">The type to examine.</param>
		// ReSharper disable UnusedMember.Local
		// ReSharper disable PartialMethodWithSinglePart
		static partial void IsTheTypeCloneable(Type type, ref bool isCloneable);
		/// <summary>
		/// Method will determine whether a type is serializable.
		/// </summary>
		/// <param name="type">
		/// The Type to be checked for serializability.
		/// </param>
		/// <param name="serializable">
		/// ref variable to be set to <see langword="true"/> if serializable. If
		/// <paramref name="serializable"/> is already <see langword="true"/>, the
		/// method should simply return, never set it false.
		/// </param>
		/// <threadsafety>
		/// The implementation of this method must be thread safe if the methods
		/// in which it is used are to be thread-safe.
		/// </threadsafety>
		// ReSharper disable UnusedMember.Local
		// ReSharper disable PartialMethodWithSinglePart
		static partial void IsTheTypeSerializable(Type type, ref bool serializable);
		// ReSharper restore PartialMethodWithSinglePart
		// ReSharper restore UnusedMember.Local
		#endregion // Partial Methods
		/// <summary>
		/// Just checks to see if a type implements an interface.
		/// </summary>
		/// <param name="type">"this" reference from Type class.</param>
		/// <param name="interfaceTypeObject">
		/// Type of the interface. <see langword="null"/> returns <see langword="false"/>.
		/// Object must be a <see cref="Type"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if type implements interface.
		/// </returns>
		public static bool DoesTypeImplementInterface(this Type type,
			object interfaceTypeObject)
		{
			if (interfaceTypeObject == null) return false;

			var interfaceType = (Type) interfaceTypeObject;

			var typeInfo = type.GetTypeInfo();

			var interfaceTypes = typeInfo.ImplementedInterfaces;

			// Get out if no interfaces.
			if (interfaceTypes == null) return false;
			// Now apply the criteria, if there is one.
			foreach (var t in interfaceTypes) {
				if (interfaceType == t) {
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Just checks to see if a type implements an interface.
		/// </summary>
		/// <param name="type">"this" reference from Type class.</param>
		/// <param name="interfaceTypeObject">
		/// Type of the interface. <see langword="null"/> returns <see langword="false"/>.
		/// Object must be a <see cref="Type"/>.
		/// </param>
		/// <returns>
		/// <see langword="false"/> if type implements interface.
		/// </returns>
		public static bool DoesTypeNotImplementInterface(this Type type,
			object interfaceTypeObject)
		{
			return !DoesTypeImplementInterface(type, interfaceTypeObject);
		}

		/// <summary>
		/// A replacement for the CLR "FindInterfaces" method.
		/// </summary>
		/// <param name="type">"this" reference from Type class.</param>
		/// <param name="filter">The filtering delegate. May be <see langword="null"/>.
		/// </param>
		/// <param name="filterCriteria">
		/// The filtering criteria object. May be <see langword="null"/>.
		/// </param>
		/// <returns>
		/// List of the interface types that a type implements that conform
		/// to the criteria. Return value can be <see langword="null"/>.
		/// </returns>
		/// <remarks><see langword="null"/> <paramref name="filter"/> or
		/// <paramref name="filterCriteria"/> returns an empty list.</remarks>
		public static IEnumerable<Type> FindConformingInterfaces(this Type type, TypeHandlingUtils.GenericTypeFilter<Type> filter, object filterCriteria)
		{
			ICollection<Type> interfaces = null;
			var types = type.GetTypeInfo().ImplementedInterfaces;
			// Get out if no interfaces.
			if (types == null) return null;
			// Now apply the criteria, if there is one.
			foreach (var t in types) {
				if ((filter != null) && (filterCriteria != null) && (filter(t, filterCriteria))) {
					if(interfaces == null) 	interfaces = new Collection<Type>();
					interfaces.Add(t);
				}
			}
			return interfaces;
		}
		/// <summary>
		/// Determines if the type implements the "ICloneable" interface.
		/// Since this is not a public interface in SL, we employ a partial
		/// method to enable it in ECMA/CLR.
		/// </summary>
		/// <param name="type">this</param>
		/// <returns>
		/// <see langword="false"/> in the SL model or if the type does not
		/// implement the interface.
		/// </returns>
		public static bool IsTypeICloneable(this Type type)
		{
			bool isTheTypeCloneable = false;
			// ReSharper disable InvocationIsSkipped
			IsTheTypeCloneable(type, ref isTheTypeCloneable);
			// ReSharper restore InvocationIsSkipped
			// ReSharper disable ConditionIsAlwaysTrueOrFalse
			// ReSharper disable HeuristicUnreachableCode
			return isTheTypeCloneable;
			// ReSharper restore HeuristicUnreachableCode
			// ReSharper restore ConditionIsAlwaysTrueOrFalse

		}
		/// <summary>
		/// This method determines if a <see cref="Type"/> is constructable.
		/// A check is made for a public constructor. If the application is operating
		/// in elevated trust mode, the search includes non-public constructors.
		/// </summary>
		/// <param name="type">"this" reference from Type class.</param>
		/// <param name="obj">
		/// Additional object argument to allow the method to be compliant with
		/// <see cref=" MemberExtensions.FilterMember{Type}"/>. In this case, the
		/// object is an <see cref="IEnumerable{Type}"/> containing the set of
		/// types that a public constructor on the type must accept. If this
		/// parameter is <see langword="null"/> or empty, a default (parameterless)
		/// constructor is searched for. Constructor parameters are all assumed
		/// to be "in" parameters.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the Type is constructable with the specified types.
		/// </returns>
		/// <exception cref="PAFStandardException{T}">
		/// The exception is thrown with <see cref="PAFTypeMismatchExceptionMessageTags.IS_WRONG_TYPE_FOR_PARAMETER_TYPE"/>
		/// if the <paramref name="obj"/> is not an <see cref="IEnumerable{Type}"/>.
		/// </exception>
		public static bool IsTypePublicConstructable(this Type type, object obj = null)
		{
			if (!type.IsTypeInstantiable()) return false;

			Type[] types;
			if (obj == null)
				types = new Type[0];
			else {
				if (!(obj is IEnumerable<Type>)) {
					var errorString = obj.GetType() + " "
						+ PAFTypeMismatchExceptionMessageTags.IS_WRONG_TYPE_FOR_PARAMETER_TYPE + ": " + typeof(IEnumerable<Type>);
					var data = new PAFTypeMismatchExceptionData(new PAFTypeHolder(obj.GetType()));
					throw new PAFStandardException<PAFTypeMismatchExceptionData>(data, errorString);
				}
				types = (obj as IEnumerable<Type>).IntoArray();
			}

			var methodParams = new MethodParameters(types);

			var info
				= type.GetInstanceConstructor(methodParams);
			return info != null;
		}
		/// <summary>
		/// This method determines if a <see cref="Type"/> is constructable.
		/// A check is made for a public constructor that takes no arguments.
		/// If the application is operating in elevated trust mode, the search
		/// includes non-public constructors.
		/// </summary>
		/// <param name="type">"this" reference from Type class.</param>
		/// <param name="obj">
		/// Additional object argument to allow the method to be compliant with
		/// <see cref="MemberExtensions.FilterMember{Type}"/>. Not used in this method.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the Type is constructable with a default constructor.
		/// </returns>
		public static bool IsTypeDefaultConstructable(this Type type, object obj = null)
		{
			return type.IsTypePublicConstructable();
		}
		/// <summary>
		/// This method determines if a <see cref="Type"/> is neither
		/// abstract or a pure interface.
		/// </summary>
		/// <param name="type">"this" reference from Type class.</param>
		/// <param name="obj">
		/// Additional object argument to allow the method to be compliant with
		/// <see cref="MemberExtensions.FilterMember{Type}"/>. Not used in this
		/// method.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the Type is not abstract or a pure interface type..
		/// </returns>
		public static bool IsTypeInstantiable(this Type type, object obj = null)
		{
			return ((!type.GetTypeInfo().IsAbstract) && (!type.GetTypeInfo().IsInterface));
		}
		/// <summary>
		/// This method determines if a <see cref="Type"/> is decorated with any
		/// attribute that would make it serializable or is a primitive type.
		/// </summary>
		/// <param name="type">"this" reference from Type class.</param>
		/// <returns>
		/// <see langword="true"/> if the Type is serializable.
		/// </returns>
		public static bool IsTypeSerializable(this Type type)
		{
			// TODO search for interfaces.
			bool isTheTypeSerializable = false;
			// ReSharper disable InvocationIsSkipped
			IsTheTypeSerializable(type, ref isTheTypeSerializable);
			// ReSharper restore InvocationIsSkipped
			// ReSharper disable ConditionIsAlwaysTrueOrFalse
			// ReSharper disable HeuristicUnreachableCode
			if (isTheTypeSerializable) return true;
			// ReSharper restore HeuristicUnreachableCode
			// ReSharper restore ConditionIsAlwaysTrueOrFalse
			if (type.GetATypesMemberInfo().FilterMembersByAnyAttributeTypePresence(PAFSerializationUtils.GetSerializationEnablingAttributeTypes()))
				return true;
			return false;
		}

		/// <summary>
		/// This method determines if a <see cref="Type"/> is a subclss of
		/// another type.
		/// </summary>
		/// <param name="type">"this" reference from Type class.</param>
		/// <param name="potentialSuperclassType">
		/// Class that might be a supertype of "this".
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the <paramref name="potentialSuperclassType"/>
		///  is indeed a supertype.
		/// </returns>
		public static bool IsTypeSubclassOf(this Type type,
			Type potentialSuperclassType)
		{
			return type.GetTypeInfo().IsSubclassOf(potentialSuperclassType);
		}

        /// <summary>
        /// Method provides local formatting for Core. Prints a heading followed by a
        /// string representation of all public gettable instance properties not in
        /// <paramref name="excludedProperties"/>. The format is a column of
        /// name/value pairs. Travels up the inheritance chain up to but not including
        /// <see cref="Object"/>.
        /// </summary>
        /// <param name="objectToFormat">
        /// An object whose properties are to be output of <see langword="null"/>.
        /// <see langword="null"/> returns <see langword="null"/>.
        /// </param>
        /// <param name="heading">
        /// String that provides input for the formatting process. If the string is <see cref="String.Empty"/>,
        /// no heading is printed. If the string is <see langword="null"/>, the <see cref="Type.FullName"/>
        /// of <paramref name="objectToFormat"/> is used as a heading.
        /// </param>
        /// <param name="excludedProperties">
        /// Names of all properties that are to be excluded in the output.
        /// </param>
        /// <returns>
        /// A string representation of the properties or <see langword="null"/> if
        /// <paramref name="objectToFormat"/> is <see langword="null"/>.
        /// </returns>
        public static string PublicPropsToString(object objectToFormat, string heading = null,
            IEnumerable<string> excludedProperties = null, bool fullName = false)
		{
			if (objectToFormat == null) return null;
			var typeToFormat = objectToFormat.GetType();

			// null gives our default heading.
			if (heading == null) heading = typeToFormat.FullName;
			var outputString = string.Empty;

			// Want a heading at all?
			if (heading != string.Empty) {
				outputString = heading + PlatformUtils.LTRMN + "-----" + PlatformUtils.LTRMN;
			}

			// Pull all the public instance props off the class.
			var props = typeToFormat.GatherImplementedPropertiesHierarchically();
			// ReSharper disable once PossibleMultipleEnumeration
			props.FilterGetMethodsOnPI();
			var aggregator = new PAFMemberFilterAggregator<PropertyInfo>("Not named gettable props");
			var notNamedFilter
				= new PAFMemberFilter<MemberInfo>(
					MemberExtensions.FilterMembersByNoNamesPresent, null,
					excludedProperties);
			var gettableFilter = new PAFMemberFilter<PropertyInfo>(
				PropertyExtensions.FilterPropertyByGetterPresence);
			aggregator.AddFilter(notNamedFilter);
			aggregator.AddFilter(gettableFilter);
			// ReSharper disable once PossibleMultipleEnumeration
			var culledProps = aggregator.ApplyFilter(props);

			if ((culledProps == null) || (culledProps.Count == 0))
				return outputString;

			// Add all the found props to the output in a sensible default format.
			foreach (var prop in culledProps)
			{
				// Get its getter method, and get the prop value.
				var propValue = prop.GetMethod.Invoke(objectToFormat, null);
				outputString
					+= StringParsingUtils.FormatNameValue(prop.Name, propValue);
			}
			return outputString;
		}

		/// <summary>
		/// This method just wraps a type in a <see cref="PAFTypeHolder"/>
		/// and returns the interface.
		/// </summary>
		/// <param name="type">"this" reference from Type class.</param>
		/// <returns>
		/// The interface.
		/// </returns>
		public static IPAFTypeHolder ToTypeholder(this Type type)
		{
			return new PAFTypeHolder(type);
		}
		/// <summary>
		/// This method determines if a <see cref="Type"/> has a certain "unqualified"
		/// name. This is the type name without the namespace. A check is made for a 
		/// namespace string, followed by a dot which is then stripped off before
		/// the comparison is made with an incoming <see cref="String"/>.
		/// </summary>
		/// <param name="type">"this" reference from Type class.</param>
		/// <param name="obj">
		/// Additional object argument to allow the method to be compliant with
		/// <see cref="MemberExtensions.FilterMember{Type}"/>. In this case, the
		/// object is an <see cref="String"/> containing the unqualified name. If this
		/// parameter is <see langword="null"/> or empty, <see langword="false"/> is returned.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the unqualified names match.
		/// </returns>
		/// <exception cref="PAFStandardException{PAFTMED}">
		/// The exception is thrown with <see cref="PAFTypeMismatchExceptionMessageTags.IS_WRONG_TYPE_FOR_PARAMETER_TYPE"/>
		/// if the <paramref name="obj"/> is not an <see cref="String"/>.
		/// </exception>
		public static bool UnqualifiedNamesMatch(this Type type, object obj = null)
		{
			if (obj == null)
				return false;
			if (!(obj is string))
			{
				var errorString = obj.GetType() + " "
					+ PAFTypeMismatchExceptionMessageTags.IS_WRONG_TYPE_FOR_PARAMETER_TYPE + ": " + typeof(string);
				var data = new PAFTypeMismatchExceptionData(new PAFTypeHolder(obj.GetType()));
				throw new PAFStandardException<PAFTypeMismatchExceptionData>(data, errorString);
			}

			var typeName = obj as string;
			if (typeName == string.Empty) return false;

			// ReSharper disable PossibleNullReferenceException
			var index = type.FullName.LastIndexOf(".", StringComparison.Ordinal);
			// ReSharper restore PossibleNullReferenceException
			// It's gotta' be here and not at beginning or end.
			if ((index <= 0) || (index == type.FullName.Length - 1))
				return false;
			// If we got this far, we can pull it apart.
			var unqualifiedTypeName = type.FullName.Substring(index + 1);
			return unqualifiedTypeName == typeName;
		}
		#endregion // Methods
	}
}
