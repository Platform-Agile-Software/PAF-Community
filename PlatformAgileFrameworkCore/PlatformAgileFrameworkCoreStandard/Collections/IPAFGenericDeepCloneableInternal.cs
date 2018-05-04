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
namespace PlatformAgileFramework.Collections
{
	/// <summary>
	/// Interface does deep cloning. This interface extends the <see cref="IPAFGenericDeepCloneable{T}"/>
	/// interface by adding a method whose purpose is to clone all non-public fields of the object
	/// that are not normally exposed to the client.
	/// </summary>
	/// <remarks>
	/// The reason for the split interface is that clients often don't need
	/// the entire internal infrastructure of a type in order to examine its
	/// public characteristics. It matters when this internal infrastructure
	/// is large or complex.
	/// </remarks>
	internal interface IPAFDeepCloneableInternal<out T>: IPAFGenericDeepCloneable<T>
	{
		/// <summary>
		/// Makes a deep clone of a type, including public and non-public fields.
		/// </summary>
		/// <returns>
		/// An independent copy of a type.
		/// </returns>
		T DeepCloneAll();
	}
}