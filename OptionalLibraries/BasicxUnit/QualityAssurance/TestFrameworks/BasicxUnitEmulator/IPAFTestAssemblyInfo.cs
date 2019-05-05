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

using System;
using System.Collections.Generic;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator
{
	/// <summary>
	/// Extends <see cref="IPAFTestElementInfo"/> to describe a test assembly.
	/// </summary>
	/// <threadsafety>
	/// Implementations are NOT necessarily expected to be thread-safe.
	/// </threadsafety>
	/// <history>
	/// <author> KRM </author>
	/// <date> 04aug2012 </date>
	/// <contribution>
	/// <para>
	/// Added history.
	/// </para>
	/// </contribution>
	/// </history>
	public interface IPAFTestAssemblyInfo: IPAFTestElementInfo<IPAFTestAssemblyInfo>
	{
		#region Properties
		/// <summary>
		/// Is assy a harness?. <see langword="null"/> before loading if not
		/// previously examined through reflection.
		/// </summary>
		bool? IsHarness { get; }
		/// <summary>
		/// Identity of the assy.
		/// </summary>
		IPAFAssemblyHolder AsmName { get; }
		/// <summary>
		/// Gatherer that can be pushed in from outside.
		/// </summary>
		Func<IPAFAssemblyHolder, IPAFTestFrameworkBehavior, IList<IPAFTypeHolder>> 
			FixtureGatherer { get; set; }
		/// <summary>
		/// Runner that can be pushed in from outside.
		/// </summary>
		Action<IPAFTestAssemblyInfo, object> FixtureRunner { get; set; }
		/// <summary>
		/// Testframework behavior. This can have the framework name undefined.
		/// The initialization method in the PAF testing infrastructure will
		/// dynamically determine the framework in use. We make this an assembly-wide
		/// property, since we want to enforce the necessity of having only one
		/// framework supported in each assembly.
		/// </summary>
		IPAFTestFrameworkBehavior TestFrameworkBehavior { get; }
		#endregion Properties
	}
}
