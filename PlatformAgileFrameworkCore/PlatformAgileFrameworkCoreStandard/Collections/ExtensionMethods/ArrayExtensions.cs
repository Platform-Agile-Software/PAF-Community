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
using System.Collections.Generic;

// ReSharper disable CheckNamespace
namespace PlatformAgileFramework.Collections
// ReSharper restore CheckNamespace
{
	/// <summary>
	/// This class contain some extension methods for the <see cref="ICollection{T}"/>.
	/// </summary>
// ReSharper disable InconsistentNaming
	public static class ArrayExtensions
// ReSharper restore InconsistentNaming
	{
		/// <summary>
		/// This method allows an array of <typeparamref name="T"/> to be reversed.
		/// </summary>
		/// <typeparam name="T">The type of items in the collection.</typeparam>
		/// <param name="array">The array to be reversed. <see langword="null"/> gets <see langword="null"/> out. </param>
		/// <returns>
		/// An array with elements in reverse order.
		/// </returns>
		public static T[] PAFReverseArray<T> (this T[] array)
		{
			if(array == null) return null;

			var outArray = new T[array.Length];

			for(var i = 0; i < array.Length; i++){
				outArray[array.Length - 1 - i] = array[i];
			}
			return outArray;
		}
	}
}
