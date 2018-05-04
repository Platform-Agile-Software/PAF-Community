//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2016 Icucom Corporation
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

#region Using Directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using PlatformAgileFramework.TypeHandling.MethodHelpers;

#endregion

namespace PlatformAgileFramework.TypeHandling.PropertyExtensionMethods
{
	/// <summary>
	/// This class implements extensions for <see cref="PropertyInfo"/>s.
	/// </summary>
	/// <history>
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
	/// <date> 07aug2012 </date>
	/// <description>
	/// New.
	/// Reflection on props
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Safe.
	/// </threadsafety>
// ReSharper disable PartialTypeWithSinglePart
// For Silverlight.
	public static partial class PropertyExtensions
// ReSharper restore PartialTypeWithSinglePart
	{
		#region PropertyInfo Filters

		/// <summary>
		/// Filters public/private instance/static props. "this" is an enumeration of <see cref="PropertyInfo"/>'s
		/// that we wish to filter. We look for props with constraints on "setters".
		/// </summary>
		/// <param name="incomingPropsToFilter">One of us.</param>
		/// <param name="publicMethods">
		/// <see langword="true"/> to return props with public getters. <see langword="null"/>
		/// to return both ublic and non-public.
		/// </param>
		/// <param name="instanceMethods">
		/// <see langword="true"/> to return instance props. <see langword="null"/>
		/// to return both instance and static.
		/// </param>
		/// <returns>
		/// <see langword="null"/> if no get methods of proper characteristics on the props.
		/// </returns>
		// ReSharper disable once InconsistentNaming
		public static IEnumerable<PropertyInfo> FilterSetMethodsOnPI(
			this IEnumerable<PropertyInfo> incomingPropsToFilter,
			bool? publicMethods = true, bool? instanceMethods = true)
		{

			Collection<PropertyInfo> propInfos = new Collection<PropertyInfo>();

			if (incomingPropsToFilter == null) return propInfos;

			foreach (var prop in incomingPropsToFilter)
			{
				// First check set method.
				MethodInfo setMethod = null;
				if (prop.CanRead) setMethod = prop.SetMethod;

				// Safety valve for crazy compiler errors.........
				if (setMethod == null) continue;

				if (setMethod.FilterMethodBaseOnPI(publicMethods, instanceMethods) == false)
					continue;

				propInfos.Add(prop);
			}
			return propInfos;
		}

		/// <summary>
		/// Filters public/private instance/static props. "this" is an enumeration of <see cref="PropertyInfo"/>'s
		/// that we wish to filter.We look for props with constraints on "getters".
		/// </summary>
		/// <param name="incomingPropsToFilter">One of us.</param>
		/// <param name="publicMethods">
		/// <see langword="true"/> to return props with public getters. <see langword="null"/>
		/// to return both public and non-public.
		/// </param>
		/// <param name="instanceMethods">
		/// <see langword="true"/> to return instance props. <see langword="null"/>
		/// to return both instance and static.
		/// </param>
		/// <returns>
		/// <see langword="null"/> if no get methods of proper characteristics on the props.
		/// </returns>
		// ReSharper disable once InconsistentNaming
		public static IEnumerable<PropertyInfo> FilterGetMethodsOnPI(
			this IEnumerable<PropertyInfo> incomingPropsToFilter,
			bool? publicMethods = true, bool? instanceMethods = true)
		{
			Collection<PropertyInfo> propInfos = new Collection<PropertyInfo>();
			if (incomingPropsToFilter == null) return propInfos;

			foreach (var prop in incomingPropsToFilter)
			{
				// First check get method.
				MethodInfo getMethod = null;
				if (prop.CanRead) getMethod = prop.GetMethod;

				// Safety valve for crazy compiler errors.........
				if (getMethod == null) continue;

				if (getMethod.FilterMethodBaseOnPI(publicMethods, instanceMethods) == false)
					continue;

				propInfos.Add(prop);
			}
			return propInfos;
		}
		/// <summary>
		/// This is a method for filtering <see cref="PropertyInfo"/> objects according to
		/// the presence or non presence of a getter method.
		/// </summary>
		/// <param name="pInfo">
		/// This is a <see cref="PropertyInfo"/> that may have a getter method
		/// attached to it. It may be <see langword="null"/>, in which case the method
		///  returns <see langword="true"/>.
		/// </param>
		/// <param name="obj">
		/// Unused.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the type passes the criteria.
		/// </returns>
		public static bool FilterPropertyByGetterPresence(this PropertyInfo pInfo,
			object obj)
		{
			// Get out if nothing to do.
			if (pInfo == null) {
				return true;
			}

			if (pInfo.CanRead) return true;

			return false;
		}
		/// <summary>
		/// This is a method for filtering <see cref="PropertyInfo"/> objects according to
		/// the presence or non presence of a getter and setter method.
		/// </summary>
		/// <param name="pInfo">
		/// This is a <see cref="PropertyInfo"/> that may have a getter and setter method
		/// attached to it. It may be <see langword="null"/>, in which case the method
		/// returns <see langword="true"/>.
		/// </param>
		/// <param name="obj">
		/// Unused.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the type passes the criteria.
		/// </returns>
		public static bool FilterPropertyBySetterAndGetterPresence(this PropertyInfo pInfo,
			object obj)
		{
			if (
				(FilterPropertyByGetterPresence(pInfo, obj))
				&&
				(FilterPropertyBySetterPresence(pInfo, obj))
			)
				return true;
			return false;
		}
		/// <summary>
		/// This is a method for filtering <see cref="PropertyInfo"/> objects according to
		/// the presence or non presence of a setter method.
		/// </summary>
		/// <param name="pInfo">
		/// This is a <see cref="PropertyInfo"/> that may have a setter method
		/// attached to it. It may be <see langword="null"/>, in which case the method
		///  returns <see langword="true"/>.
		/// </param>
		/// <param name="obj">
		/// Unused.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the type passes the criteria.
		/// </returns>
		public static bool FilterPropertyBySetterPresence(this PropertyInfo pInfo,
			object obj)
		{
			// Get out if nothing to do.
			if (pInfo == null)
			{
				return true;
			}

			if (pInfo.CanWrite) return true;

			return false;
		}
		#endregion // PropertyInfo Filters
	}
}
