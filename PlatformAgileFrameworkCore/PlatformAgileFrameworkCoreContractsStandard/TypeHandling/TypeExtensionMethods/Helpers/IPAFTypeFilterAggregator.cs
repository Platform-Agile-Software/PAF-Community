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
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

#region Using Directives

using System;
using PlatformAgileFramework.TypeHandling.Filters;

#endregion

namespace PlatformAgileFramework.TypeHandling.TypeExtensionMethods.Helpers
{
	/// <summary>
	/// This interface is a closure of <see cref="IPAFItemFilterAggregator{T}"/>.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> JAW(P) </author>
	/// <date> 30jun15 </date>
	/// <description>
	/// Helping Kurt to change stuff for the reflection library
	/// changes in Windows Phone. Now we work with typeinfo, not type.
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
	public interface IPAFTypeFilterAggregator :IPAFItemFilterAggregator<Type>, IPAFTypeFilter
	{
	}
}
