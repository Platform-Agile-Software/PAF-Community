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


using System;
using PlatformAgileFramework.Collections.Dictionaries;

namespace PlatformAgileFramework.TypeHandling.PartialClassSupport
{
	/// <summary>
	/// This interface provides storage for <see cref="ExtendablePseudoEnum{T}"/>'s.
	/// </summary>
	/// <threadsafety>
	/// Implementations do not necessarily have to be thread-safe. Implementations
	/// may be designed to be used inside a lock. Document, please!!!!!
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 20aug2015 </date>
	/// <description>
	/// New - the Pseudoenum stuff had big problems - just wasn't written right.
	/// We need to have a catalog of all PE's in the system in a static dictionary.
	/// </description>
	/// </contribution>
	/// </history>
// ReSharper disable once PartialTypeWithSinglePart
	public partial interface IPseudoEnumValueDictionary 
		: IPAFTwoLevelDictionary<IExtendablePseudoEnumTypeType, IExtendablePseudoEnumTypeType, object>
	{
		/// <summary>
		/// Normally, this method will partition the incoming items into separate
		/// dictionaries, based on the type.
		/// </summary>
		/// <param name="pseudoEnumValue">The PE to add.</param>
		/// <exceptions>
		/// Normal exceptions will propagate up from dictionaries concerning duplicates.
		/// These are not caught. No exceptions need be caught or thrown from the
		/// implementation.
		/// </exceptions>
		void AddPseudoEnum(IExtendablePseudoEnumTypeType pseudoEnumValue);
	}
}
