//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 Icucom Corporation
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
using System.Reflection;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator
{
	/// <summary>
	/// Holds information for each test fixture method, including setup/teardown
	/// methods and actual test method. Also applies to harness methods, etc.
	/// </summary>
	/// <summary>
	/// </summary>
	/// <threadsafety>
	/// Implementations are NOT necessarily expected to be thread-safe.
	/// However, the members defined by this interface are expected to be
	/// set once and only by one thread. If not, there is probably something
	/// wrong with the design or the usage of the implementation.
	/// </threadsafety>
	/// <history>
	/// <author> KRM </author>
	/// <date> 10aug2012 </date>
	/// <contribution>
	/// <para>();
	/// Added history. Cleaned up DOCs. Redid with new interfaces.
	/// </para>
	/// </contribution>
	/// </history>
	/// <remarks>
	/// We don't carry any information about parameters or return types or
	/// Generic types in methods. All third-party test frameworks that we
	/// wish to support use attributes to identify non-Generic methods
	/// with no parameters returning void. PAF ExtendedxUnit is restricted
	/// to this support and Goshaloma employs interfaces, so we don't
	/// see the need. Framework builders are free to extend the interface
	/// and implmentations.
	/// </remarks>
	public interface IPAFTestMethodInfo: IPAFTestElementInfo
	{
		#region Properties
		/// <summary>
		/// Method info. for the framework method. Note the method info is
		/// never serialized for the local/remote peer. Public set method for
		/// late binding.
		/// </summary>
		MethodInfo FrameworkMethodInfo { get; set; }
		/// <summary>
		/// Stringful representation of the method, which is serialized. If meaningful
		/// names are employed for the methods, these names can be used for
		/// <see cref="IPAFTestElementInfo.TestElementName"/> as well.
		/// </summary>
		string FrameworkMethodName { get; }
		/// <summary>
		/// Early/Late bound declaring type for the method.
		/// </summary>
		PAFTypeHolderBase HostType { get; }
		#endregion Properties
	}
}
