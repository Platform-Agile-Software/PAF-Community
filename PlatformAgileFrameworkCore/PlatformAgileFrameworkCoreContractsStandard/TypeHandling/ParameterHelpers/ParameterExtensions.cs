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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.Collections.ExtensionMethods;

#endregion

namespace PlatformAgileFramework.TypeHandling.ParameterHelpers
{
	/// <summary>
	/// This class implements extensions for <see cref="ParameterInfo"/>s.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 31jul2012 </date>
	/// <description>
	/// New.
	/// Needed a bit more infrastructure for reflection on parameters. Cleaned
	/// up and integrated stuff from prior frameworks. Parameter filtering is
	/// the main thing for now - more later.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Safe.
	/// </threadsafety>
// ReSharper disable PartialTypeWithSinglePart
// For Silverlight.
	public static partial class ParameterExtensions
// ReSharper restore PartialTypeWithSinglePart
	{	
		#region Delegates
		/// <summary>
		/// This is a delegate for filtering <see cref="ParameterInfo"/>'s with an arbitrary
		/// auxiliary object.
		/// </summary>
		/// <param name="parameter">
		/// This is a <see cref="ParameterInfo"/> which may be <see langword="null"/>, in which
		/// case the method returns <see langword="true"/>.
		/// </param>
		/// <param name="obj">
		/// This is an arbitrary auxiliary object that can provide data to aid
		/// in the filtering. It may be <see langword="null"/>, depending on the design of the filter.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the parameter passes the criteria.
		/// </returns>
		public delegate bool FilterParameter(ParameterInfo parameter, object obj);
		/// <summary>
		/// This is a delegate for filtering <see cref="ParameterInfo"/>'s with a type-safe
		/// auxiliary object.
		/// </summary>
		/// <typeparam name="U">
		/// This is a <see cref="Type"/> with no particular constraints.
		/// </typeparam>
		/// <param name="parameterInfo">
		/// An info to subject to our pass/fail criteria.
		/// </param>
		/// <param name="auxObj">
		/// This is an auxiliary object of <typeparamref name="U"/> that can provide data to aid
		/// in the filtering. It may be <see langword="null"/>, depending on the design of the filter.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the member passes the criteria.
		/// </returns>
		public delegate bool FilterParameter<in U>(ParameterInfo parameterInfo, U auxObj);
		#endregion // Delegates
		#region Methods
		/// <summary>
		/// This orders parameters in their canonical order. Parameter with
		/// position property 1 first, etc.
		/// </summary>
		/// <returns>
		/// The ordered list.
		/// </returns>
		/// <remarks>
		/// Not completely lightweight, since we have to instantiate
		/// a comparator.
		/// </remarks>
		public static IList<ParameterInfo> SortParameters
			(this IEnumerable<ParameterInfo> parameters)
		{
			IComparer<ParameterInfo> isFirstParamGreater = null;
			var orderedList = new List<ParameterInfo>();
			if (parameters == null) return null;
			parameters = parameters.BuildCollection();

			if (parameters.Any()) isFirstParamGreater
				= new ParameterInfoPositionalComparer();

			foreach (var paramInfo in parameters)
			{
				orderedList.AddItemInOrder(paramInfo, isFirstParamGreater);
			}
			return orderedList;
		}
		/// <summary>
		/// This determines if lists of parameters exactly match in length
		/// as well as <see cref="ParameterInfosExactlyMatch"/> ed-ness.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if a match.
		/// </returns>
		/// <remarks>
		/// Not completely lightweight, since we have to instantiate
		/// two comparators.
		/// </remarks>
		public static bool ParameterInfoListsExactlyMatch
		(this IEnumerable<ParameterInfo> parameters,
			IEnumerable<ParameterInfo> otherParametersToCompare)
		{

			parameters = parameters.ToList();
			otherParametersToCompare = otherParametersToCompare.ToList();
			// No real work to do?
			if (parameters.Count() != otherParametersToCompare.Count())
				return false;

			var sortedParameters = parameters.SortParameters();
			var sortedOtherParameters = otherParametersToCompare.SortParameters();


			for (var paramNum = 0; paramNum < sortedParameters.Count; paramNum++)
			{
				var param = sortedParameters[paramNum];
				var otherParam = sortedOtherParameters[paramNum];
				if (!param.ParameterInfosExactlyMatch(otherParam))
					return false;
			}

			return true;
		}
		/// <summary>
		/// This examines two <see cref="PropertyInfo"/>s to see if
		/// they match.
		/// </summary>
		/// <param name="parameter">One of us.</param>
		/// <param name="otherParameterToCompare">
		/// Another one of us to compare.
		/// </param>
		/// <returns>
		/// <see langword="false"/> if ANY props (type,in/out,optional,retval)
		/// do not match.
		/// </returns>
		/// <remarks>
		/// Lightweight, since we don't have to build anything.
		/// </remarks>
		public static bool ParameterInfosExactlyMatch
		(this ParameterInfo parameter, ParameterInfo otherParameterToCompare)
		{
			if (parameter.GetType() != otherParameterToCompare.GetType())
				return false;
			if (parameter.IsIn != otherParameterToCompare.IsIn)
				return false;
			if (parameter.IsOut != otherParameterToCompare.IsOut)
				return false;
			if (parameter.IsOptional != otherParameterToCompare.IsOptional)
				return false;
			if (parameter.IsRetval != otherParameterToCompare.IsRetval)
				return false;
			return true;

		}
		#endregion // Methods
	}
}
