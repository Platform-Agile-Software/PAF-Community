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
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.Serializing.Attributes
{
	/// <summary>
	/// Interface for accessing PAFAttributes.
	/// </summary>
	public interface IPAFSerializableAttribute
	{
		#region Properties
		/// <summary>
		/// Specifies the type of serialization possible on the type/member.
		/// </summary>
		PAFSerializationType SerializationTypesEnabled { get; }
		/// <summary>
		/// Specifies the type of custom serialization to be employed on the type/member.
		/// This allows a <see cref="Type"/> to specify a serializer for itself without
		/// it being stored in the surrogate dictionary and also to override any surrogate
		/// specifications for superclasses. Using the config file is a better solution,
		/// generally, but customer needs drive this piece of functionality.
		/// </summary>
		IPAFTypeProps CustomSerializer { get; }
		#endregion // Fields and Autoproperties
	}
}
