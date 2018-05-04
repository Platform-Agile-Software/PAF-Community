//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2017 Icucom Corporation
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
using PlatformAgileFramework.TypeHandling.TypeComparison.Comparators;

namespace PlatformAgileFramework.TypeHandling.TypeExtensionMethods.Helpers
{
	/// <summary>
	/// Helpers for types.
	/// </summary>
	/// <history>
	/// <author> KRM </author>
	/// <date> 11jun2017 </date>
	/// <contribution>
	/// <description>
	/// New.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Unsafe.
	/// </threadsafety>
	public static class TypeHelpers
	{
		/// <summary>
		/// Checks if a new value is different from the old. Sets the value
		/// only when different.
		/// </summary>
		/// <typeparam name="T">Type of the value.</typeparam>
		/// <param name="oldValue">
		/// Reference to a value that is typically a class field.
		/// </param>
		/// <param name="newValue">
		/// Desired value for the item.
		/// </param>
		/// <returns>
		/// True if the value was changed, false if the existing value matched the
		/// desired value.
		/// </returns>
		/// <threadsafety>
		/// Unsafe. <typeparamref name="T"/> has no contraints, so can't be accessed atomically.
		/// </threadsafety>
		/// <remarks>
		/// This resolves race conditions between communicating elements. Taken from the
		/// Spice extensions.
		/// </remarks>
		public static bool SetValueIfChanged<T>(ref T oldValue, T newValue)
		{
			if (PAFEquatable<T>.Instance.AreEqual(oldValue, newValue)) return false;
			oldValue = newValue;
			return true;
		}
	}
}
