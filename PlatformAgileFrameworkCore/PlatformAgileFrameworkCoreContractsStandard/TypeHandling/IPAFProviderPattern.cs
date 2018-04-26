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

using System;

namespace PlatformAgileFramework.TypeHandling
{
	/// <summary>
	/// Class that holds an item of a certain type.
	/// </summary>
	/// <typeparam name="T">
	/// The type of the item provided.
	/// </typeparam>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 21aug2015 </date>
	/// <description>
	/// Took out setting and synchronization - this should be implemented in specific applications
	/// where the provider needs to be reset - don't know why it was in here since we didn't
	/// use it in PAF and can't find where any legacy code used it.
	/// </description>
	/// </contribution>
	public interface IPAFProviderPattern<T>
	{
		#region Properties
		/// <summary>
		/// The item.
		/// </summary>
		T ProvidedItem { get; }
		#endregion // Properties
		#region Methods
		/// <summary>
		/// Gets the item - needed for value types. On occasion the item is never set.
		/// </summary>
		bool TryGetProvidedItem(out T item);
		#endregion // Methods
	}
}
