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
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

#region Using Directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;

#endregion

namespace PlatformAgileFramework.TypeHandling.MemberExtensionMethods
{
	/// <summary>
	/// This class implements extensions for <see cref="MemberInfo"/>s.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> JAW(P) </author>
	/// <date> 11jul2015 </date>
	/// <description>
	/// Collected <see cref="MemberInfo"/> helpers from around different classes that
	/// looked like they were pre-3.5 and made them extension methods in here.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 31jul2012 </date>
	/// <description>
	/// New.
	/// Needed a bit more infrastructure for reflection on members. Cleaned
	/// up and integrated stuff from prior frameworks. Member filtering is
	/// the main thing for now - more later.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Safe.
	/// </threadsafety>
// ReSharper disable PartialTypeWithSinglePart
// For Silverlight.
	public static partial class MemberExtensions
// ReSharper restore PartialTypeWithSinglePart
	{	
		#region Delegates
		/// <summary>
		/// This is a delegate for filtering <see cref="MemberInfo"/>'s with an arbitrary
		/// auxiliary object.
		/// </summary>
		/// <param name="member">
		/// This is a <see cref="MemberInfo"/> which may be <see langword="null"/>, in which
		/// case the method returns <see langword="true"/>.
		/// </param>
		/// <param name="obj">
		/// This is an arbitrary auxiliary object that can provide data to aid
		/// in the filtering. It may be <see langword="null"/>, depending on the design of the filter.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the member passes the criteria.
		/// </returns>
		public delegate bool FilterMember(MemberInfo member, object obj);
		/// <summary>
		/// This is a delegate for filtering <see cref="MemberInfo"/>'s with an arbitrary
		/// auxiliary object.
		/// </summary>
		/// <typeparam name="T">
		/// This is a <see cref="Type"/> derived from <see cref="MemberInfo"/>. The parameter
		/// is not defined as contravariant, since we need an exact type.
		/// </typeparam>
		/// <param name="info">
		/// This is a type derived from <see cref="MemberInfo"/> which may be <see langword="null"/>, in which
		/// case the method returns <see langword="true"/>.
		/// </param>
		/// <param name="obj">
		/// This is an arbitrary auxiliary object that can provide data to aid
		/// in the filtering. It may be <see langword="null"/>, depending on the design of the filter.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the member passes the criteria.
		/// </returns>
// ReSharper disable TypeParameterCanBeVariant
		public delegate bool FilterMember<T>(T info, object obj) where T : MemberInfo;
// ReSharper restore TypeParameterCanBeVariant
		/// <summary>
		/// This is a delegate for filtering <see cref="MemberInfo"/>'s with a type-safe
		/// auxiliary object.
		/// </summary>
		/// <typeparam name="U">
		/// This is a <see cref="Type"/> with no particular constraints.
		/// </typeparam>
		/// <param name="info">
		/// This is a <see cref="MemberInfo"/> which may be <see langword="null"/>, in which
		/// case the method returns <see langword="true"/>.
		/// </param>
		/// <param name="auxObj">
		/// This is an auxiliary object of <typeparamref name="U"/> that can provide data to aid
		/// in the filtering. It may be <see langword="null"/>, depending on the design of the filter.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the member passes the criteria.
		/// </returns>
		// ReSharper disable TypeParameterCanBeVariant
		public delegate bool FilterMemberGeneric<in U>(MemberInfo info, U auxObj);
		/// <summary>
		/// This is a delegate for filtering accessor objects (PropertyDescriptors, FieldInfos,
		/// etc.) according to specific conditions on their System.Attributes.
		/// The delegate is obviously very general, but is named according to it's original
		/// purpose. Methods like ICustomTypeDescriptor.GetProperties(Attributes[])
		/// offer filtering only based on the presence of certain attribute Types. This delegate
		/// is designed to allow clients to perform more general filtering based on attributes.
		/// </summary>
		/// <param name="accessor">
		/// This is a constituent accessor that may have attributes attached to it. It may be
		/// <see langword="null"/>, in which case the method returns <see langword="true"/>.
		/// </param>
		/// <param name="attributes">
		/// This is an array of <see cref="Attribute"/> subtypes that are to be evaluated
		/// as to whether a given accessor "passes" the criteria for inclusion. It may
		/// be <see langword="null"/>, in which case the method returns <see langword="true"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the accessor passes the criteria.
		/// </returns>
		/// <remarks>
		/// A simple example of a useful criterion that is not available in the
		/// CLR is to check to see whether a given accessor does NOT have a certain
		/// attribute. Much more complex scenarios are possible as in the
		/// "LayeredSerializer".
		/// </remarks>
		public delegate bool FilterAccessorByAttributTypes(MemberInfo accessor,
			IEnumerable<Type> attributes);
		#endregion // Delegates
		#region Extension Methods
		/// <summary>
		/// This method is used to determine whether a member is decorated with a given
		/// attribute described with a string. This method is useful when Types are being handled
		/// remotely and the actual attribute <see cref="Type"/> is not loaded into the caller's
		/// "AppDomain". In these cases, the attributes on the members must be
		/// evaluated through their names.
		/// </summary>
		/// <param name="memberInfo">
		/// <see cref="MemberInfo"/>'s. Can be <see langword="null"/>.
		/// </param>
		/// <param name="attributeName">
		/// Name of the <see cref="Attribute"/>. If the caller forgets that CLI attributes end
		/// with "Attribute", this suffix is appended. Blank or <see langword="null"/> name
		/// returns <see langword="false"/>. This name is the simple name of an attribute
		/// type only - no namespace, no assembly, etc.
		/// </param>
		/// <returns>
		/// <see langword="false"/> for <see langword="null"/> input or name. Otherwise <see langword="true"/> if the attribute
		/// is present.
		/// </returns>
        /// <remarks>
        /// Sadly, in MS's new implementation of the reflection library, <see cref="TypeInfo"/> derives
        /// from from <see cref="MemberInfo"/>, so this method will handle type infos properly, too. This
        /// method finds inherited attributes on <see cref="TypeInfo"/>s, climbing the type hierarchy.
        /// Use <see cref="PAFTypeReflectionExtensions.PAFGetCustomAttributeData"/> if this is not what
        /// is needed.
        /// </remarks>
		public static bool DoesMemberHavePublicNamedAttributeInfo(
			this MemberInfo memberInfo, string attributeName)
		{
			if (memberInfo == null) return false;
			if (string.IsNullOrEmpty(attributeName)) return false;
			if (!attributeName.Contains("Attribute")) attributeName += "Attribute";
			var attributes = memberInfo.PAFGetCustomAttributeData(attributeName, true);
			if (attributes == null) return false;
			return true;
		}

		/// <summary>
		/// Filters members by name.
		/// </summary>
		/// <param name="incomingMembersToFilter">One of us.</param>
		/// <param name="memberName">
		/// If <see langword="null"/>, doesn't filter, but passes all to output.
		/// </param>
		/// <returns>
		/// Empty enumeration if no members have the name.
		/// </returns>
		// ReSharper disable once InconsistentNaming
		public static IEnumerable<MemberInfo> FilterMembersOnName(
			this IEnumerable<MemberInfo> incomingMembersToFilter,
			string memberName = null)
		{
			var memberInfos = new Collection<MemberInfo>();

			foreach (var member in incomingMembersToFilter)
			{
				if((memberName == null) || (member.Name == memberName))
					memberInfos.Add(member);
			}
			return memberInfos;
		}
		/// <summary>
		/// This is a method for filtering <see cref="MemberInfo"/> objects according to
		/// their name NOT being in a prescribed set of names.
		/// </summary>
		/// <param name="memberInfo">
		/// This is a <see cref="MemberInfo"/>. It may be <see langword="null"/>, in which
		/// case the method returns <see langword="true"/>.
		/// </param>
		/// <param name="auxData">
		/// This is a set of names that the member must NOT have in order for it to "pass"
		/// the test criteria. If this argument is <see langword="null"/>, the method
		/// always returns <see langword="true"/>. Comparison is ordinal case-sensitive.
		/// Any blank or <see langword="null"/> are ignored. This must be an
		/// <see cref="IEnumerable{T}"/> or the method returns <see langword="true"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the member passes the criteria.
		/// </returns>
		public static bool FilterMembersByNoNamesPresent(this MemberInfo memberInfo,
			object auxData)
		{
			var proscribedNames = auxData as IEnumerable<string>;
			// Get out if nothing to do.
			if ((proscribedNames == null) || (memberInfo == null))
			{
				return true;
			}

			// We can't have any of the names in the list.
			foreach (var proscribedName in proscribedNames)
			{
				if (string.IsNullOrEmpty(proscribedName)) continue;
				if (string.CompareOrdinal(proscribedName, memberInfo.Name) == 0) return false;
			}
			return true;
		}
		/// <summary>
		/// This is a method for filtering <see cref="MemberInfo"/> objects according to
		/// the presence or non presence of certain <see cref="System.Attribute"/>s. The
		/// whole reason that we built this is that the CLI does not have a built-in
		/// filtering option such as GetFields(BindingFlags, Attribute[]). This method
		/// tests whether attributes match based on a<c>typeof(Attribute)</c> check.
		/// </summary>
		/// <param name="memberInfo">
		/// This is a <see cref="MemberInfo"/> that may have attributes attached to it.
		/// It may be <see langword="null"/>, in which case the method returns <see langword="true"/>.
		/// </param>
		/// <param name="attributeTypes">
		/// This is an array of attribute <see cref="Type"/>s that must be present on
		/// the incoming member in order for it to "pass" the test criteria. If this argument
		/// is <see langword="null"/>, the method always returns <see langword="true"/>. If
		/// any of the attributes in this array are present on the member, it passes the test
		/// and <see langword="true"/> is returned.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the member passes the criteria.
		/// </returns>
		public static bool FilterMembersByAnyAttributeTypePresence(this MemberInfo memberInfo,
			IEnumerable<Type> attributeTypes)
		{
			// Get out if nothing to do.
			if ((attributeTypes == null) || (memberInfo == null))
			{
				return true;
			}
			attributeTypes = attributeTypes.IntoArray();

			// Double loop - if we collide with anything, we pass.
			// TODO - KRM fix this s**t.
			// ReSharper disable LoopCanBeConvertedToQuery
			foreach (Attribute memberAttribute in memberInfo.GetCustomAttributes(typeof(Attribute), true))
			{
				// ReSharper restore LoopCanBeConvertedToQuery
				// ReSharper disable LoopCanBeConvertedToQuery
				foreach (var incomingAttributeType in attributeTypes.IntoArray())
				{
					// ReSharper restore LoopCanBeConvertedToQuery
					if (incomingAttributeType == memberAttribute.GetType())
					{
						return true;
					}
				}
			}
			return false;
		}
		/// <summary>
		/// This is a method for filtering <see cref="MemberInfo"/> objects according to
		/// the presence or non presence of certain <see cref="System.Attribute"/>s. The
		/// whole reason that we built this is that the CLR does not have a built-in
		/// filtering option such as GetFields(BindingFlags, Attribute[]). This method
		/// tests whether attributes match based on a<c>typeof(Attribute)</c> check.
		/// </summary>
		/// <param name="memberInfo">
		/// This is a <see cref="MemberInfo"/> that may have attributes attached to it.
		/// It may be <see langword="null"/>, in which case the method returns <see langword="true"/>.
		/// </param>
		/// <param name="attributeTypes">
		/// This is an array of specific attribute <see cref="Type"/>s that must not be present on
		/// the incoming member in order for it to "pass" the test criteria. If this argument
		/// is <see langword="null"/>, the method always returns <see langword="true"/>. If
		/// any of the attributes in this array is present on the member, it fails the test
		/// and <see langword="false"/> is returned.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the member passes the criteria.
		/// </returns>
		public static bool FilterMembersByNoAttributeTypesPresence(this MemberInfo memberInfo,
			IEnumerable<Type> attributeTypes)
		{
			// Get out if nothing to do.
			if ((attributeTypes == null) || (memberInfo == null))
			{
				return true;
			}

			// No double enum, please.
			attributeTypes = attributeTypes.IntoArray();

			// Double loop - if we collide with anything, we fail.
			// ReSharper disable LoopCanBeConvertedToQuery
			foreach (Attribute memberAttribute
				in memberInfo.GetCustomAttributes(typeof(Attribute), true))
			{
				// ReSharper restore LoopCanBeConvertedToQuery
				// ReSharper disable LoopCanBeConvertedToQuery
				foreach (var incomingAttributetype in attributeTypes)
				{
					// ReSharper restore LoopCanBeConvertedToQuery
					if (incomingAttributetype == memberAttribute.GetType())
					{
						return false;
					}
				}
			}
			// Doesn't have any of the proscribed attributes, so we pass.
			return true;
		}
		/// <summary>
		/// This is a method for filtering <see cref="MemberInfo"/> objects according to
		/// the presence or non presence of certain <see cref="System.Attribute"/>s. The
		/// whole reason that we built this is that the CLR does not have a built-in
		/// filtering option such as GetFields(BindingFlags, Attribute[]). This method
		/// tests whether attributes match based on a<c>typeof(Attribute)</c> check.
		/// </summary>
		/// <param name="memberObject">
		/// This is a <see cref="MemberInfo"/> that may have attributes attached to it.
		/// It may be <see langword="null"/>, in which case the method returns <see langword="true"/>.
		/// </param>
		/// <param name="attributes">
		/// This is an array of <see cref="System.Attribute"/>s that must not be present on
		/// the incoming member in order for it to "pass" the test criteria. If this argument
		/// is <see langword="null"/>, the method always returns <see langword="true"/>. If
		/// any of the attributes in this array is present on the member, it fails the test
		/// and <see langword="false"/> is returned.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the member passes the criteria.
		/// </returns>
		/// <exception> <see cref="Exception"/> is thrown:
		/// <c>"Problem casting between Types."</c> if the
		/// <paramref name="memberObject"/> is not a <see cref="MemberInfo"/> Type.
		/// </exception>
		public static bool FilterMembersByNoAttributePresence(object memberObject,
			IEnumerable<Attribute> attributes)
		{
			MemberInfo memberInfo;

			// Get out if nothing to do.
			if ((attributes == null) || (memberObject == null))
			{
				return true;
			}

			// TODO KRM - so how come we are not passing a memberinfo to start with?
			// Can't survive a problem with the wrong Type.
			if ((memberInfo = (memberObject as MemberInfo)) == null)
			{
				var message = string.Format("Problem casting between Types:\nFrom Type: {0}\nTo Type: {1}\n", memberObject.GetType().FullName, typeof(MemberInfo).FullName);
				//// SL upgrade - phase 2 - put EandE in.
				////throw new PAFTypeException(message, true);
				// TODO - KRM put in correct exception.
				throw new Exception(message);
			}

			// No double enum, please.
			attributes = attributes.IntoArray();

			// Double loop - if we collide with anything, we fail.
			// ReSharper disable LoopCanBeConvertedToQuery
			foreach (Attribute memberAttribute
				in memberInfo.GetCustomAttributes(typeof(Attribute), true))
			{
				// ReSharper restore LoopCanBeConvertedToQuery
				// ReSharper disable LoopCanBeConvertedToQuery
				foreach (var incomingAttribute in attributes)
				{
					// ReSharper restore LoopCanBeConvertedToQuery
					if (incomingAttribute.GetType() == memberAttribute.GetType())
					{
						return false;
					}
				}
			}
			// Doesn't have any of the proscribed attributes, so we pass.
			return true;
		}

		/// <summary>
		/// This method is used to return a list of members which are NOT decorated with a given
		/// attribute described with a string. This method is useful when Types are being handled
		/// remotely and the actual attribute <see cref="Type"/> is not loaded into the caller's
		/// "AppDomain". In these cases, the attributes on the members must be
		/// evaluated through their names.
		/// </summary>
		/// <param name="memberInfos">
		/// Collection of <see cref="MemberInfo"/>'s. Can be <see langword="null"/>.
		/// </param>
		/// <param name="attributeName">
		/// Name of the <see cref="Attribute"/>. If the caller forgets that CLI attributes end
		/// with "Attribute", this suffix is appended. Blank or name returns an
		/// empty list. <see langword="null"/> name returns <see langword="null"/>.
		/// empty list.
		/// </param>
		/// <returns>
		/// <see langword="null"/>for <see langword="null"/> incoming collection. Otherwise the list of <see cref="MemberInfo"/>'s
		/// WITHOUT the given attributes.
		/// </returns>
		/// <exceptions>
		/// <exception> <see cref="ArgumentNullException"/>"memberInfos"</exception>
		/// </exceptions>
		/// <exceptions>
		/// <exception> <see cref="ArgumentNullException"/>"attributeName"</exception>
		/// </exceptions>
		/// <remarks>
		/// This one has to be kept for backwards compatibility.
		/// Goshaloma uses it everywhere.
		/// </remarks>
		public static IEnumerable<MemberInfo> GetMembersWithoutPublicNamedAttributeInfo
			(this IEnumerable<MemberInfo> memberInfos, string attributeName)
		{
			if (memberInfos == null) throw new ArgumentNullException("memberInfos");
			if (attributeName == null) throw new ArgumentNullException("attributeName");
			var foundMemberInfoList = new Collection<MemberInfo>();
			if (attributeName == "") return foundMemberInfoList;
			foreach (var memberInfo in memberInfos)
			{
				if (memberInfo.PAFGetCustomAttributeData(attributeName, true) == null)
				{
					foundMemberInfoList.Add(memberInfo);
				}
			}
			return foundMemberInfoList;
		}
		/// <summary>
		/// This method is used to return a list of members which are decorated with a given
		/// attribute described with a string. This method is useful when Types are being handled
		/// remotely and the actual attribute <see cref="Type"/> is not loaded into the caller's
		/// "AppDomain". In these cases, the attributes on the members must be
		/// evaluated through their names.
		/// </summary>
		/// <param name="memberInfos">
		/// Collection of <see cref="MemberInfo"/>'s. Can be <see langword="null"/>.
		/// </param>
		/// <param name="attributeName">
		/// Name of the <see cref="Attribute"/>. If the caller forgets that CLI attributes end
		/// with "Attribute", this suffix is appended. Blank or name returns an
		/// empty list. <see langword="null"/> name returns <see langword="null"/>.
		/// empty list.
		/// </param>
		/// <returns>
		/// The list of <see cref="MemberInfo"/>'s - never <see langword="null"/>.
		/// with the given attributes.
		/// </returns>
		/// <exceptions>
		/// <exception> <see cref="ArgumentNullException"/>"memberInfos"</exception>
		/// </exceptions>
		/// <exceptions>
		/// <exception> <see cref="ArgumentNullException"/>"attributeName"</exception>
		/// </exceptions>
		public static IList<MemberInfo> GetMembersWithPublicNamedAttributeInfo(
			this IEnumerable<MemberInfo> memberInfos, string attributeName)
		{
			if (memberInfos == null) throw new ArgumentNullException(nameof(memberInfos));
			if (attributeName == null) throw new ArgumentNullException(nameof(attributeName));
			var foundMemberInfoList = new Collection<MemberInfo>();
			if (attributeName == "") return foundMemberInfoList;
			foreach (var memberInfo in memberInfos)
			{
				if (memberInfo.PAFGetCustomAttributeData(attributeName, true) != null)
				{
					foundMemberInfoList.Add(memberInfo);
				}
			}
			return foundMemberInfoList;
		}
		#endregion // Extension Methods
	}
}
