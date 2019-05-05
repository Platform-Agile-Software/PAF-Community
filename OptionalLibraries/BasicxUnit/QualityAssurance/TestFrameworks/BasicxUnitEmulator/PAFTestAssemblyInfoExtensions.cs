//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2017 Icucom Corporation
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
	/// This is a new helper class for <see cref="IPAFTestAssemblyInfo"/>s
	/// helpers that were moved to a separate class so the folks could find them.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 21dec2017 </date>
	/// <desription>
	/// New.
	/// </desription>
	/// </contribution>
	/// </history>
	public static class PAFTestAssemblyInfoExtensions
	{
		/// <summary>
		/// Just grabs fixtures from the child collection.
		/// </summary>
		/// <param name="assemblyInfo">
		/// test assembly info.
		/// </param>
		/// <returns>
		/// Test fixture infos - never <see langword="null"/>.
		/// </returns>
		public static IList<IPAFTestFixtureInfo> GetFixtures
			(this IPAFTestAssemblyInfo assemblyInfo)
		{
			return assemblyInfo.GetChildInfoSubtypesOfType<IPAFTestElementInfo, IPAFTestFixtureInfo>();
		}
		/// <summary>
		/// Just grabs wrappers from the child collection.
		/// </summary>
		/// <param name="assemblyInfo">
		/// test assembly info.
		/// </param>
		/// <returns>
		/// test wrappers - never <see langword="null"/>.
		/// </returns>
		public static IList<IPAFTestFixtureWrapper> GetWrappers
			(this IPAFTestAssemblyInfo assemblyInfo)
		{
			return assemblyInfo.GetChildInfoSubtypesOfType<IPAFTestElementInfo, IPAFTestFixtureWrapper>();
		}
	}
}
