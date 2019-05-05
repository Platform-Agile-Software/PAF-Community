//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 -2015 Icucom Corporation
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
using System.Reflection;

namespace PlatformAgileFramework.Manufacturing
{
	/// <summary>
	/// Version for Xamarin mobile that allows access to the <see cref="AppDomain"/> class.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> Brian T. </author>
	/// <date> 22sep2015 </date>
	/// <desription>
	/// New.
	/// </desription>
	/// </contribution>
	/// </history>
// ReSharper disable PartialTypeWithSinglePart
// TODO - KRM change to internal when in the SM.
	public partial class XamarinManufacturingUtilsInstance :ManufacturingUtilsInstance
	// ReSharper restore PartialTypeWithSinglePart
	{
		/// <remarks>
		/// <see cref="ManufacturingUtils"/>. This override is dynamic for the cases
		/// where we do dynamic loading and the set of loaded assemblies is changing.
		/// Calling <see cref="AppDomain.GetAssemblies"/> isn't very expensive and this
		/// is a low-traffic area. 
		/// </remarks>
		public override IEnumerable<Assembly> GetAppDomainAssemblies()
		{
			var assys = AppDomain.CurrentDomain.GetAssemblies();
			foreach(var assy in assys)
			{
				AddAssemblyToAssembliesLoadedInternal(assy);
			}
			return base.GetAppDomainAssemblies();

		}
	}
}
