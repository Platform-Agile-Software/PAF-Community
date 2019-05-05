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
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-
#if NunitLinked
// This was used for NUnit - it's here if anybody wants it....
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace GoshalomaCore.QualityAssurance.TestFrameworks.Nunit
{
	/// <summary>
	/// This class defines an attribute that can be placed on methods, assemblies
	/// or classes to indicate that long-running tests are contained within. A
	/// specialized class is needed in order to provide a segway into Nunit extensions.
	/// These extensions can leverage the extensibility model of Nunit to provide
	/// instrumentation and reports about tests.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true)]
	public sealed class LongRunningTestAttribute : CategorizedTestAttribute
	{
	}
	/// <summary>
	/// This class defines a base attribute class that is designed for use with Nunit
	/// and other framework's extensibility mechanism. Please use a subclass as an
	/// attribute on your test methods.
	/// </summary>
	/// <remarks>
	/// This abstract class does not offer the parametrized constructor that the base
	/// <see cref="CategoryAttribute"/>offers. This is because the name of the category
	/// is defined by the string in <see cref="CategoryAttribute"/>-derived attributes.
	/// We wish to limit test categories to only those that have been decided upon by
	/// management, in consultation with the continuous integration team. The use of
	/// the pre-defined categories can be enforced with a Gendarme rule on large
	/// projects.
	/// </remarks>
	public abstract class CategorizedTestAttribute : CategoryAttribute
	{
		#region Class Fields and AutoProperties
		/// <summary>
		/// Dictionary for the extensions.
		/// </summary>
		protected internal IDictionary<String, Object> ObjectBag { get; set; }
		#endregion // Class Fields and AutoProperties
	}
}
#endif
