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

// ReSharper disable once RedundantUsingDirective
using System;

namespace PlatformAgileFramework.TypeHandling
{
	/// <summary>
	/// Interface that provides access to a reference type.
	/// </summary>
	/// <typeparam name="T">
	/// The type of the item provided. Contrained to be a "class".
	/// </typeparam>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 21aug2017 </date>
	/// <description>
	/// Lose the value type baggage, since we never use it in PAF.
	/// </description>
	/// </contribution>
	public interface IPAFClassProviderPattern<out T> where T: class
	{
		#region Properties
		/// <summary>
		/// The class.
		/// </summary>
		T ProvidedItem { get; }
		#endregion // Properties
	}
}
