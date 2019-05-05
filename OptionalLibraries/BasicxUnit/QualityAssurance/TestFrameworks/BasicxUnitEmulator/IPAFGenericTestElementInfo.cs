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

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator
{

	/// <summary>
	/// Holds information for each test item(harness, suite, assy, test). Use
	/// this one if we know the type of our children. True for the standard
	/// xUnit hierarchy.
	/// </summary>
	/// <threadsafety>
	/// Implementations are NOT necessarily expected to be thread-safe.
	/// </threadsafety>
	/// <history>
	/// <author> KRM </author>
	/// <date> 07aug2012 </date>
	/// <contribution>
	/// <para>
	/// New for type-safety.
	/// </para>
	/// </contribution>
	/// </history>
	public interface IPAFTestElementInfo<T>: IPAFTestElementInfo
		,ITestElementInfoItemEnumerableProviderProvider<T>
		where T: IPAFTestElementInfo
	{
			#region Properties
		/// <summary>
		/// Children perhaps stored in the main collection, but filtered by type.
		/// </summary>
		IEnumerable<T> TypedChildren { get; }
		#endregion Properties
		#region Methods
		/// <summary>
		/// Adds an element to the node.
		/// </summary>
		/// <param name="testElementInfo">
		/// Element info to add.
		/// </param>
		void AddTestElement(T testElementInfo);
		/// <summary>
		/// Allows elements to decide on which children are displayed.
		/// </summary>
		Func<IPAFTestElementInfo<T>, IList<IPAFTestElementInfo>> GetDisplayChildElementItems { get; set; }
		#endregion // Methods
	}
}
