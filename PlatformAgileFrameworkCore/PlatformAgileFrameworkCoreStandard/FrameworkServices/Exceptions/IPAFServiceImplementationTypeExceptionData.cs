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
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.FrameworkServices.Exceptions
{
	/// <summary>
	///	Exceptions that occur handling services when the implementation type is known.
	/// Use <see cref="IPAFServiceImplementationTypeExceptionData"/>
	/// to describe errors encountered when loading a service implementing an interface
	/// (described by <see cref="IPAFServiceExceptionData.ProblematicService"/>
	/// that is implemented by a specific concrete type.
	/// Note that because our service discovery process can involve a partial type
	/// description, this type description can be an assembly-qualified type name,
	/// a namespace-qualified type name or just a namespace or just an unqualified
	/// type name. This data should give whatever information we have about the
	/// concrete implementing type we are dealing with.
	/// </summary>
	[PAFSerializable]
	public interface IPAFServiceImplementationTypeExceptionData : IPAFServiceExceptionData
	{
		#region Properties
		/// <summary>
		/// Assembly name.
		/// </summary>
		string ProblematicImplementationTypeAssemblyName { get; }
		/// <summary>
		/// Portion before the dot in a namespace-qualified type name.
		/// (e.g. the "System.Collections" in "System.Collections.Generic").
		/// No terminating dot, please.
		/// </summary>
		string ProblematicImplementationTypeNameSpace { get; }
		/// <summary>
		/// Last segment beyond the dot in a namespace-qualified type name.
		/// (e.g. the "String" in "System.String").
		/// No dots, please.
		/// </summary>
		string ProblematicImplementationTypeSimpleName { get; }
		#endregion // Properties
	}
}