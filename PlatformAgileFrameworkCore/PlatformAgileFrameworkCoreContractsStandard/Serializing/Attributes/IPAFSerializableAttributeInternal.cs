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

using System;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.Serializing.Attributes
{
	/// <summary>
	/// Interface for accessing PAFAttributes. Internal version for the framework.
	/// </summary>
	/// <threadsafety>
	/// Manipulation of the type through this interface is normally done only by the
	/// framework and typically does not need to be synchronized.
	/// </threadsafety>
	internal interface IPAFSerializableAttributeInternal: IPAFSerializableAttribute
	{
		#region Properties
		/// <summary>
		/// Holds the state of the serialization.
		/// </summary>
		object SerializationState { get; set; }
		#endregion // Properties
		#region Methods
		/// <summary>
		/// Sets the type of serialization to be employed on the type/member.
		/// </summary>
		void SetSerializationTypeInUse(PAFSerializationType type);
		/// <summary>
		/// Sets the type of custom serialization to be employed on the type/member.
		/// </summary>
		/// <param name="customSerializer">Custom serializer for the type.</param>
		void SetCustomSerializer(IPAFTypeProps customSerializer);
		#endregion // Methods
	}
}
