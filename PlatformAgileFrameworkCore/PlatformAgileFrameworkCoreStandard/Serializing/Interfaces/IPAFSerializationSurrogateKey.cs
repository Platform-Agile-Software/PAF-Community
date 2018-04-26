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
using PlatformAgileFramework.Collections.KeyedCollections;

namespace PlatformAgileFramework.Serializing.Interfaces
{
	/// <summary>
	/// Interface allows a surrogate to be indexed by type, name and context state.
	/// Implementations are normally surrogates and they wear the key directly
	/// to make resorting of elements in collections a bit easier.
	/// </summary>
	/// <threadsafety>
	/// The implementations of this interface are not anticipated to be thread-safe.
	/// Normally, only one worker thread is accessing the implementing class during
	/// the serialization or deserialization process.
	/// </threadsafety>
	// ReSharper disable once PartialTypeWithSinglePart
	// partial for injecting COM stuff in ECMACLR
	public partial interface IPAFSerializationSurrogateKey: IPAFNameAndTypeKeyedObject
	{
	}
}
