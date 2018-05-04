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

namespace PlatformAgileFramework.TypeHandling
{
	/// <remarks>
	/// See <see cref="PAFProviderPatternBase{T}"/>.
	/// </remarks>
	public sealed class PAFClassProviderPattern<T>
		: PAFProviderPatternBase<T>, IPAFClassProviderPattern<T>
		where T: class
	{
		#region Constructors
		/// <remarks>
		/// See <see cref="PAFProviderPatternBase{T}"/>.
		/// </remarks>
		public PAFClassProviderPattern()
		{
		}
		/// <remarks>
		/// See <see cref="PAFProviderPatternBase{T}"/>.
		/// </remarks>
		public PAFClassProviderPattern(T item)
			: base(item)
		{
		}
		#endregion // Constructors
	}
}
