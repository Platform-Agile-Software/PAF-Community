﻿//@#$&+
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
using PlatformAgileFramework.TypeHandling.Filters;
using PlatformAgileFramework.TypeHandling.MemberExtensionMethods;
using PlatformAgileFramework.TypeHandling.MemberExtensionMethods.Helpers;

#endregion

namespace PlatformAgileFramework.TypeHandling.TypeExtensionMethods.Helpers
{
	/// <summary>
	/// This class provides a type-safe aggregator for
	/// <see cref="MemberExtensions.FilterMember"/>'s. It is mostly a helper class for
	/// the type extensions. It is a closure of <see cref="PAFItemFilterAggregator{T}"/>.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> JAW(P) </author>
	/// <date> 30jun15 </date>
	/// <description>
	/// Helping Kurt to change stuff for the reflection library
	/// changes in Windows Phone. Can't use memberinfo base classes anymore.
	/// </description>
	/// </contribution>
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
	public class PAFTypeFilterAggregator
		: PAFItemFilterAggregator<Type>, IPAFTypeFilterAggregator
	{
		/// <remarks>
		/// Constructor is a direct pass-through to base. See <see cref="PAFMemberFilterAggregator{T}"/>
		/// </remarks>
		public PAFTypeFilterAggregator( string filterName = null,
			IEnumerable<IPAFItemFilter<Type>> filterContainers = null,
			ItemFilterCombinationMode combinationMode = default(ItemFilterCombinationMode),
			object filterAuxiliaryData = null, bool useCommonData = false)
			: base(filterName, filterContainers, combinationMode, filterAuxiliaryData,
			useCommonData)
		{
		}

	}
}
