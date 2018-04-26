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

namespace PlatformAgileFramework.Collections.Dictionaries
{
	/// <summary>
	/// This interface provides storage for objects that are keyed by name and type.
	/// Usually there are many objects of the same type with different names,
	/// but this dictionary can used for general purposes, depending on the
	/// comparer that is installed. This dictionary is convenient when the object
	/// itself implements <see cref="IPAFNamedAndTypedObject"/> directly or
	/// aggregates a token that provides it.
	/// </summary>
	/// <threadsafety>
	/// Implementations need not be thread-safe.
	/// </threadsafety>
	public interface IPAFNamedAndTypedObjectDictionary<T>
		: IDictionary<IPAFNamedAndTypedObject, T>
	{
	}
}
