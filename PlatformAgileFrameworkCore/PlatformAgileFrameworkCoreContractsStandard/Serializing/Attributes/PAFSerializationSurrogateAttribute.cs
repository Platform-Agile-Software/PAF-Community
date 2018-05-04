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
using System.Collections.Generic;
using PlatformAgileFramework.TypeHandling;

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.Serializing.Attributes
{
	/// <summary>
	/// Attribute placed on classes to signal that they are a surrogate
	/// for another.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	// ReSharper disable once InconsistentNaming
#pragma warning disable CS3015 // Type has no accessible constructors which use only CLS-compliant types
	public sealed class PAFSerializationSurrogateAttribute : Attribute
#pragma warning restore CS3015 // Type has no accessible constructors which use only CLS-compliant types
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Assembly-qualified names of types we can serialize. Defaults to <see langword="null"/>.
		/// </summary>
		private readonly IList <IPAFTypeProps> m_TypesWeCanSerialize;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Sets streaming states and types serializable.
		/// </summary>
		/// <param name="typesWeCanSerialize">
		/// Assembly-qualified names of types we can serialize.
		/// </param>
		public PAFSerializationSurrogateAttribute(IList<IPAFTypeProps> typesWeCanSerialize = null)
		{
			m_TypesWeCanSerialize = typesWeCanSerialize;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Assembly-qualified names of types we can serialize. Defaults to
		/// <see langword="null"/>. This OPTIONAL field makes it easier for the serializer
		/// to find a surrogate.
		/// </summary>
		public IEnumerable<IPAFTypeProps> TypesWeCanSerialize
		{
			get { return m_TypesWeCanSerialize; }
		}
		#endregion // Properties
	}
}
