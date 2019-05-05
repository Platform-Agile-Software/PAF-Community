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

using System.Reflection;
using PlatformAgileFramework.TypeHandling.Filters;

#endregion

namespace PlatformAgileFramework.TypeHandling.MemberExtensionMethods.Helpers
{
	/// <summary>
	/// This interface defines a container for a <see cref="TypeHandlingUtils.GenericTypeFilter{T}"/>
	/// and its auxiliary data. It is mostly a helper container for the member extensions. It is a
	/// specialization of <see cref="IPAFItemFilter{T}"/>.
	/// </summary>
	/// <typeparam name="T">
	/// This is the type that the filters are designed to operate on. In this derived
	/// interface, it is constrained to be a <see cref="MemberInfo"/>.
	/// </typeparam>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 31jul2012 </date>
	/// <description>
	/// New.
	/// Support interface for the aggregator.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IPAFMemberFilter<in T>: IPAFItemFilter<T> where T: MemberInfo
	{
	}
}
