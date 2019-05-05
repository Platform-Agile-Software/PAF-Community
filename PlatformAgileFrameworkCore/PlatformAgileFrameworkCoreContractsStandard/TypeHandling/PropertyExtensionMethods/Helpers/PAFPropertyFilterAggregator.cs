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
using System.Reflection;
using PlatformAgileFramework.TypeHandling.Filters;
using PlatformAgileFramework.TypeHandling.MemberExtensionMethods;
using PlatformAgileFramework.TypeHandling.MemberExtensionMethods.Helpers;

#endregion

namespace PlatformAgileFramework.TypeHandling.PropertyExtensionMethods.Helpers
{
	/// <summary>
	/// This class provides a type-safe aggregator for
	/// <see cref="MemberExtensions.FilterMember"/>'s. It is mostly a helper class for
	/// the member extensions. It is a specialization of <see cref="PAFItemFilterAggregator{T}"/>.
	/// </summary>
	/// <typeparam name="T">
	/// This is the type that the filters are designed to operate on. In this derived
	/// class, it is constrained to be a <see cref="MemberInfo"/>.
	/// </typeparam>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 31jul2012 </date>
	/// <description>
	/// New.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Safe if the individual filters are safe.
	/// </threadsafety>
	public class PAFPropertyFilterAggregator<T> : PAFMemberFilterAggregator<T> where T : PropertyInfo
	{
		/// <remarks>
		/// Constructor is a direct pass-through to base. See <see cref="PAFItemFilterAggregator{T}"/>
		/// </remarks>
		public PAFPropertyFilterAggregator(string filterName = null, IEnumerable<PAFItemFilter<T>> filterContainers = null,
		ItemFilterCombinationMode combinationMode = default(ItemFilterCombinationMode), object filterAuxiliaryData = null, bool useCommonData = false)
			: base(filterName, filterContainers, combinationMode, filterAuxiliaryData, useCommonData)
		{
		}
	}
}
