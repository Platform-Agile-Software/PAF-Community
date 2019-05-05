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

using System.Collections.Generic;
using PlatformAgileFramework.Collections.KeyedCollections;
using PlatformAgileFramework.Serializing.Interfaces;

namespace PlatformAgileFramework.Serializing.HelperCollections
{
	/// <summary>
	/// This class provides storage for serialization surrogates. It is designed to
	/// be accessed by a single thread and is generally held and used inside the
	/// framework. It holds <see cref="IPAFSerializationSurrogateKey"/>'s and
	/// is keyed by the name and type.
	/// </summary>
	/// <threadsafety>
	/// This class subclasses an ordinary dictionary and is thus not thread-safe.
	/// </threadsafety>
	public class PAFSerializationSurrogateDictionary : Dictionary<IPAFNameAndTypeKeyedObject,
		IPAFSerializationSurrogateKey>
	{
		#region Constructors
		/// <summary>
		/// This constructor builds the dictionary with a case-insensitive name
		/// comparison.
		/// </summary>
		public PAFSerializationSurrogateDictionary()
			:base(new PAFNTKeyedObjectComparer<IPAFNameAndTypeKeyedObject>(true))
		{}
		#endregion // Constructors
	}
}
