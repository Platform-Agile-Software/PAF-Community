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

#endregion

namespace PlatformAgileFramework.TypeHandling.Filters
{
	/// <summary>
	/// This defines a container for a <see cref="TypeHandlingUtils.GenericTypeFilter{T}"/>
	/// and its auxiliary data.
	/// </summary>
	/// <typeparam name="T">
	/// This is the type that the filters are designed to operate on.
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
	public interface IPAFItemFilter<in T>
	{
		#region Properties
		/// <summary>
		/// The name of the filter. This is used for reporting purposes.
		/// </summary>
		string FilterName { get;}
		/// <summary>
		/// This is the optional auxiliary data that can be used by the filter. This
		/// can be set when the class is constructed so that the filter can have
		/// its own private data. This is the default case.
		/// </summary>
		object FilterAuxiliaryData { get; }
		/// <summary>
		/// This is the delegate to use for filtering.
		/// </summary>
		TypeHandlingUtils.GenericTypeFilter<T> FilteringDelegate { get; }
		/// <summary>
		/// <see langword="true"/> to use the common aux data from either a call on an external
		/// container or on the external container itself.
		/// </summary>
		bool UseExternalData { get; }
		#endregion // Properties
	}
}
