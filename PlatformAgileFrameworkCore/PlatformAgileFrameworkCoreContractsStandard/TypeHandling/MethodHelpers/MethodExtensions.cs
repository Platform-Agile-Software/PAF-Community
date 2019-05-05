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
using System.Linq;
using System.Reflection;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.TypeHandling.ParameterHelpers;

#endregion

namespace PlatformAgileFramework.TypeHandling.MethodHelpers
{
	/// <summary>
	/// This class implements extensions for <see cref="MethodBase"/>s and
	/// <see cref="MethodInfo"/>s.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> BMC </author>
	/// <date> 29sep2015 </date>
	/// <description>
	/// Helping Kurt - added the "methodinfo" stuff. Also did the filtering
	/// on names for BasicxUnit support as extension methods.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 30jun2015 </date>
	/// <description>
	/// New.
	/// Had to build "new-style" method reflection stuff during
	/// the upgrade to Windows Store reflection rewrite.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Safe.
	/// </threadsafety>
// ReSharper disable PartialTypeWithSinglePart
	public static partial class MethodExtensions
// ReSharper restore PartialTypeWithSinglePart
	{
		/// <summary>
		/// Filters public/private instance/static method. "this" is a <see cref="MethodBase"/>
		/// that we wish to filter.
		/// </summary>
		/// <param name="incomingMethodToFilter">One of us.</param>
		/// <param name="publicMethods">
		/// <see langword="true"/> to pass public methods. <see langword="null"/> To pass all methods.
		/// </param>
		/// <param name="instanceMethods">
		/// <see langword="true"/> to pass instance methods. <see langword="null"/> to pass all methods.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the method passes both criteria, if they are specified.
		/// </returns>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">"IncomingMethodToFilter"</exception>
		/// </exceptions>
		// ReSharper disable once InconsistentNaming
		public static bool FilterMethodBaseOnPI(
			this MethodBase incomingMethodToFilter,
			bool? publicMethods = true, bool? instanceMethods = true)
		{
			if (incomingMethodToFilter == null)
				throw new ArgumentNullException("incomingMethodToFilter");

			// Filter only if public has value.
			if (publicMethods.HasValue)
			{
				if (!incomingMethodToFilter.IsPublic && publicMethods.Value)
					return false;
                // Do not remove - there is a case for this.
				if (incomingMethodToFilter.IsPublic && !publicMethods.Value)
					return false;
			}

			// Filter only if instance has value.
			if (instanceMethods.HasValue)
			{
				if (incomingMethodToFilter.IsStatic && instanceMethods.Value)
					return false;
				if (!incomingMethodToFilter.IsStatic && !instanceMethods.Value)
					return false;
			}
			return true;
		}

		/// <summary>
		/// Filters public/private instance/static method bases. "this" is
		/// an enumeration of <see cref="MethodBase"/>'s
		/// that we wish to filter.
		/// </summary>
		/// <param name="incomingMethodsToFilter">One of us.</param>
		/// <param name="publicMethods">
		/// <see langword="true"/> to return public methods. <see langword="null"/> To return all methods.
		/// </param>
		/// <param name="instanceMethods">
		/// <see langword="true"/> to return instance methods. <see langword="null"/> to return all methods.
		/// </param>
		/// <returns>
		/// Empty collection if no methods of proper characteristics are found.
		/// </returns>
		// ReSharper disable once InconsistentNaming
		public static IList<MethodBase> FilterMethodBasesOnPI(
			this IEnumerable<MethodBase> incomingMethodsToFilter,
			bool? publicMethods = true, bool? instanceMethods = true)
		{
			Collection<MethodBase> methodBases = new Collection<MethodBase>();

			if (incomingMethodsToFilter == null) return methodBases;

			foreach (var method in incomingMethodsToFilter)
			{
				if (method.FilterMethodBaseOnPI(publicMethods, instanceMethods) == false)
					continue;

				methodBases.Add(method);
			}
			return methodBases;
		}
		/// <summary>
		/// Filters public/private instance/static method. "this" is
		/// an enumeration of <see cref="MethodInfo"/>'s
		/// that we wish to filter.
		/// </summary>
		/// <param name="incomingMethodsToFilter">One of us.</param>
		/// <param name="publicMethods">
		/// <see langword="true"/> to return public methods. <see langword="null"/> To return all methods.
		/// </param>
		/// <param name="instanceMethods">
		/// <see langword="true"/> to return instance methods. <see langword="null"/> to return all methods.
		/// </param>
		/// <returns>
		/// Empty collection if no methods of proper characteristics are found.
		/// </returns>
		// ReSharper disable once InconsistentNaming
		public static IList<MethodInfo> FilterMethodsOnPI(
			this IEnumerable<MethodInfo> incomingMethodsToFilter,
			bool? publicMethods = true, bool? instanceMethods = true)
		{
			Collection<MethodInfo> methodInfos = new Collection<MethodInfo>();

			if (incomingMethodsToFilter == null) return methodInfos;

			foreach (var method in incomingMethodsToFilter)
			{
				if (method.FilterMethodBaseOnPI(publicMethods, instanceMethods) == false)
					continue;

				methodInfos.Add(method);
			}
			return methodInfos;
		}
		/// <summary>
		/// This one's pretty simple - just gets <see cref="MethodInfo"/> elements and ignores
		/// constructors which are a separate subclass of methodbase.
		/// </summary>
		/// <param name="incomingMethodsToFilter">One of us.</param>
		/// <returns>
		/// Empty collection if no methods of proper characteristics are found or if input
		/// is <see langword="null"/>.
		/// </returns>
		public static IList<MethodInfo> FilterMethodInfos(
			this IEnumerable<MethodBase> incomingMethodsToFilter)
		{
			Collection<MethodInfo> methodInfos = new Collection<MethodInfo>();

			if (incomingMethodsToFilter == null) return methodInfos;

			foreach (var method in incomingMethodsToFilter)
			{
				MethodInfo methodInfo;
				if ((methodInfo = method as MethodInfo) != null)

					methodInfos.Add(methodInfo);
			}
			return methodInfos;
		}

		/// <summary>
		/// This one filters on <see cref="MethodBase"/>s exactly matching.
		/// </summary>
		/// <param name="incomingMethodBasesToFilter">One of us.</param>
		/// <param name="parameterInfos">
		/// List of parameters which must match the methd's EXACTLY,
		/// in
		/// </param>
		/// <returns>
		/// Empty collection if no methods of proper characteristics are found or if input
		/// is <see langword="null"/>.
		/// </returns>
		public static IList<MethodBase> FilterMethodBasesOnParameters(
			this IEnumerable<MethodBase> incomingMethodBasesToFilter,
			IEnumerable<ParameterInfo> parameterInfos)
		{
			var methodBases = new Collection<MethodBase>();

			if (incomingMethodBasesToFilter == null)
			{
				return methodBases;
			}

			foreach (var methodBase in incomingMethodBasesToFilter)
			{
				var parameterListForMethodBase = methodBase.GetParameters();
				// ReSharper disable once PossibleMultipleEnumeration
				//// No, there 'aint, ReSharper.
				if (parameterInfos.ParameterInfoListsExactlyMatch(parameterListForMethodBase))
					methodBases.Add(methodBase);
			}
			return methodBases;
		}

		/// <summary>
		/// This one filters on <see cref="MethodInfo"/>s exactly matching.
		/// </summary>
		/// <param name="incomingMethodInfosToFilter">One of us.</param>
		/// <param name="parameterInfos">
		/// List of parameters which must match the methd's EXACTLY,
		/// in
		/// </param>
		/// <param name="returnType">
		/// For definateness, this is checked against the methods return type, if any.
		/// Supply <see langword="null"/> for void return.
		/// </param>
		/// <returns>
		/// Empty collection if no methods of proper characteristics are found or if input
		/// is <see langword="null"/>.
		/// </returns>
		public static IList<MethodInfo> FilterMethodInfosOnParameters(
			this IEnumerable<MethodInfo> incomingMethodInfosToFilter,
			IEnumerable<ParameterInfo> parameterInfos, Type returnType = null)
		{
			var methodInfos = new Collection<MethodInfo>();

			if (incomingMethodInfosToFilter == null)
			{
				return methodInfos;
			}

			foreach (var methodInfo in incomingMethodInfosToFilter)
			{
				var parameterListForMethodInfo = methodInfo.GetParameters();
				// ReSharper disable once PossibleMultipleEnumeration
				//// No, there 'aint, ReSharper.
				if (parameterInfos.ParameterInfoListsExactlyMatch(parameterListForMethodInfo))
					methodInfos.Add(methodInfo);
			}

			if (!methodInfos.Any())
				return methodInfos;

			var returnedMethodInfos = new Collection<MethodInfo>();

			// Now align the return types.
			foreach (var methodInfo in methodInfos)
			{
				if (methodInfo.ReturnType == returnType)
					returnedMethodInfos.Add(methodInfo);
			}
			return returnedMethodInfos;
		}
		/// <summary>
		/// This one filters on <see cref="MethodInfo"/>s on matching name.
		/// </summary>
		/// <param name="incomingMethodInfosToFilter">One of us.</param>
		/// <param name="methodName">
		/// The name of the methods we are seeking.
		/// </param>
		/// <returns>
		/// May be multiple due to optional parameters. Never
		/// <see langword="null"/>.
		/// </returns>
		public static IList<MethodInfo> FilterMethodInfosOnNameMatch(
			this IEnumerable<MethodInfo> incomingMethodInfosToFilter,
			string methodName)
		{
			var methodInfos = new Collection<MethodInfo>();

			if (incomingMethodInfosToFilter == null)
			{
				return methodInfos;
			}

			foreach (var methodInfo in incomingMethodInfosToFilter.Where(methodInfo => string.Equals(methodInfo.Name, methodName, StringComparison.Ordinal)))
			{
				methodInfos.Add(methodInfo);
			}

			return methodInfos;
		}
		/// <summary>
		/// This one filters on <see cref="MethodInfo"/>s on matching names.
		/// </summary>
		/// <param name="incomingMethodInfosToFilter">
		/// One of us. <see langword="null"/> is OK.
		/// </param>
		/// <param name="methodNames">
		/// The names of the methods we are seeking. <see langword="null"/> is OK.
		/// </param>
		/// <returns>
		/// Matching methods. Never <see langword="null"/>.
		/// </returns>
		public static IList<MethodInfo> FilterMethodInfosOnNamesMatch(
			this IEnumerable<MethodInfo> incomingMethodInfosToFilter,
			IEnumerable<string> methodNames)
		{
			var methodInfos = new List<MethodInfo>();

			if ((incomingMethodInfosToFilter == null) || (methodNames == null))
			{
				return methodInfos;
			}

			incomingMethodInfosToFilter = incomingMethodInfosToFilter.ToList();

			foreach (var namedMethodInfoList in methodNames.Select(methodName => incomingMethodInfosToFilter.FilterMethodInfosOnNameMatch(methodName)))
			{
				methodInfos.AddRange(namedMethodInfoList);
			}

			return methodInfos;
		}
		/// <summary>
		/// This one filters on <see cref="MethodInfo"/>s on non-matching names.
		/// </summary>
		/// <param name="incomingMethodInfosToFilter">
		/// One of us. <see langword="null"/> is OK.
		/// </param>
		/// <param name="methodNames">
		/// The names of the methods we are seeking to exclude.
		/// <see langword="null"/> is OK.
		/// </param>
		/// <returns>
		/// Non-Matching methods. Never <see langword="null"/>.
		/// </returns>
		public static IList<MethodInfo> FilterMethodInfosOnNamesNonMatch(
			this IEnumerable<MethodInfo> incomingMethodInfosToFilter,
			IEnumerable<string> methodNames)
		{
			IList<MethodInfo> methodInfos = new List<MethodInfo>();

			if (incomingMethodInfosToFilter == null)
			{
				return methodInfos;
			}

			if (methodNames == null)
				return incomingMethodInfosToFilter.ToList();

			incomingMethodInfosToFilter = incomingMethodInfosToFilter.ToList();

			foreach (var namedMethodInfoList in methodNames.Select(methodName => incomingMethodInfosToFilter.FilterMethodInfosOnNameNonMatch(methodName)))
			{
				// Cull the methods one name at a time.
				methodInfos = methodInfos.RemoveElementsIfPresent(namedMethodInfoList);
			}

			return methodInfos;
		}
		/// <summary>
		/// This one filters on <see cref="MethodInfo"/>s on names NOT matching.
		/// </summary>
		/// <param name="incomingMethodInfosToFilter">One of us.</param>
		/// <param name="methodName">
		/// The name of the methods we are seeking to remove.
		/// </param>
		/// <returns>
		/// May be multiple due to optional parameters. Never
		/// <see langword="null"/>.
		/// </returns>
		public static IList<MethodInfo> FilterMethodInfosOnNameNonMatch(
			this IEnumerable<MethodInfo> incomingMethodInfosToFilter,
			string methodName)
		{
			var methodInfos = new Collection<MethodInfo>();

			if (incomingMethodInfosToFilter == null)
			{
				return methodInfos;
			}

			foreach (var methodInfo in incomingMethodInfosToFilter.Where(methodInfo => !string.Equals(methodInfo.Name, methodName, StringComparison.Ordinal)))
			{
				methodInfos.Add(methodInfo);
			}

			return methodInfos;
		}
	}
}
