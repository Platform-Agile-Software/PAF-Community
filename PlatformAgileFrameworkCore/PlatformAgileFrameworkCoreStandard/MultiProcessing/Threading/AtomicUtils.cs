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

using System.Threading;

#endregion

namespace PlatformAgileFramework.MultiProcessing.Threading
{
	/// <summary>
	/// This class implements simple utilities that do atomic CLI operations
	/// that are useful for synchronized objects.
	/// </summary>
	public class AtomicUtils
	{
		#region Methods
		/// <summary>
		/// This method is used to make an instantaneous copy of an item that
		/// may or may not be <see langword="null"/>.
		/// </summary>
		/// <typeparam name="T">
		/// The type of the item, which must be a reference type.
		/// </typeparam>
		/// <param name="itemToGet">
		/// The object, whose reference is to be returned.
		/// </param>
		/// <returns>
		/// The instantaneous snapshot of the reference.
		/// </returns>
		public static T GetNullableItem<T>(ref T itemToGet)	where T : class
		{
			return Interlocked.CompareExchange(ref itemToGet, null, null);			
		}
		#endregion // Methods
	}
}
