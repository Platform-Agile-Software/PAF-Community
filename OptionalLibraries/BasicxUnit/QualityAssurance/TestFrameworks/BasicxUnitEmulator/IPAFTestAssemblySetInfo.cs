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

using System.Collections.Generic;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator
{
	/// <summary>
	///	A container for a set of assemblies containing tests that are to be
	/// grouped in one run.
	/// </summary>
	public interface IPAFTestAssemblySetInfo: IPAFTestElementInfo
	{
		#region Methods
		/// <summary>
		/// This method gets all the <see cref="IPAFTestAssemblyInfo"/>'s from
		/// the instance. The implementation may contain variegated
		/// <see cref="IPAFTestElementInfo"/>'s, but test assemblies are the
		/// most common, so we provide this method for convenience.
		/// </summary>
		/// <returns>The collection.</returns>
		ICollection<IPAFTestAssemblyInfo> GetAssemblies();
		/// <summary>
		/// This method gets all the <see cref="IPAFTestAssemblyInfo"/>'s from
		/// the instance that are "active".
		/// </summary>
		/// <returns>The collection.</returns>
		ICollection<IPAFTestAssemblyInfo> GetActiveAssemblies();
		#endregion // Methods
	}
}