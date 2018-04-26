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

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.Serializing
{
	#region Enums
	/// <summary>
	/// Specifies the type of serialization to be emulated. This is a <see cref="FlagsAttribute"/>
	/// <c>enum</c>, since multiple types of serialization may be in use. A type can be configured
	/// to be serializable in multiple ways.
	/// </summary>
	[Flags]
	public enum PAFSerializationType :long
	{
		/// <summary>
		/// Indicates that this member should not be serialized. Normally set dynamically
		/// only by the framework.
		/// </summary>
		None = -1,
		/// <summary>
		/// Default value. Serialization possibilities are dynamically discovered
		/// What's actually used is determined by the serializer settings.
		/// </summary>
		Unspecified = 0,
		// TODO KRM - discuss surrogates.
		/// <summary>
		/// Classical .Net serialization scoops up all public and non-public fields not marked
		/// with "System.Runtime.Serialization.NonSerialized. Note that we get only public fields in
		/// Silverlight.
		/// </summary>
		Net = 1,
		/// <summary>
		/// WCF data contract serializer emulation.
		/// </summary>
		DataCtr = 2,
		/// <summary>
		/// Uses a surrogate installed or described by the client or specified
		/// on the type.
		/// </summary>
		PAFSurrogate = 4,
		/// <summary>
		/// Uses the PAF internal serialization system.
		/// </summary>
		PAFInternal = 8,
		/// <summary>
		/// Custom type of serialization mechanism uses a string ID.
		/// </summary>
		Custom = 32768
	}
	#endregion // Enums
}
