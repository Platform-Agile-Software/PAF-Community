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
using PlatformAgileFramework.Collections.Enumerators;
using PlatformAgileFramework.Execution.Pipeline;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.MultiProcessing.AsyncControl;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator
{
	/// <summary>
	///	Default implementation of <see cref="IPAFTestFixtureWrapperParameters"/>.
	/// </summary>
	/// <remarks>
	/// Always use distinguishable names in interfaces members that will be
	/// aggregated.
	/// </remarks>
	public class PAFTestFixtureWrapperParameters : PAFPipelineParams<IPAFServiceManager<IPAFService>>,
		IPAFTestFixtureWrapperParameters
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// See <see cref="IPAFTestFixtureWrapperParameters"/>.
		/// </summary>
		public PAFTypeHolder TestFixtureType { get; set; }
		/// <summary>
		/// See <see cref="IPAFTestFixtureWrapperParameters"/>.
		/// </summary>
		public IPAFTestFrameworkBehavior FrameworkBehavior { get; set; }
		/// <summary>
		/// See <see cref="IPAFTestFixtureWrapperParameters"/>.
		/// TODO convert info builders for non-PAF. Currently, only PAF is auto-generated.
		/// </summary>
		public IPAFTestFixtureInfo FixtureInfo { get; set; }
		/// <summary>
		/// See <see cref="IPAFTestFixtureWrapperParameters"/>.
		/// </summary>
		public long MaxTestMethodCalls { get; set; }
		/// <summary>
		/// See <see cref="IPAFTestFixtureWrapperParameters"/>.
		/// </summary>
		public IPAFResettableEnumerableProvider<MethodInfo> TestMethodEnumerable { get; set; }
		/// <summary>
		/// See <see cref="IPAFTestFixtureWrapperParameters"/>.
		/// </summary>
		public PAFTestRunner TestRunner { get; set; }
		/// <summary>
		/// See <see cref="IPAFTestFixtureWrapperParameters"/>.
		/// </summary>
		public IAsyncControlObject AsyncTestControlObject { get; set; }
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Default constructor sticks PAFUnit framework behavior as default.
		/// Does not load other properties. Reload framework behavior if you
		/// want something else. Loads default for <see cref="MaxTestMethodCalls"/>
		/// of -1 implying no iteration stopping criterion is used.
		/// </summary>
		public PAFTestFixtureWrapperParameters()
		{
			FrameworkBehavior = PAFTestFrameworkBehavior.GetStandardPAFUnitParams();
			MaxTestMethodCalls = -1;
		}
		#endregion // Constructors
	}
}