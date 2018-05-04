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

using PlatformAgileFramework.TypeHandling.TypeExtensionMethods.Helpers;

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// This interface provides internal access to manager parent.
	/// </summary>
	// ReSharper disable once PartialTypeWithSinglePart
	// Addition of services moved to extended and to inheritors.
	internal partial interface IPAFServiceManagerInternal : IPAFServiceManagerExtended
	{
		/// <remarks>
		/// See <see cref="IPAFServiceManagerExtended"/>.
		/// </remarks>
		IPAFServiceDescription CreateServiceInternal(
			IPAFServicePipelineObject servicePipelineObject,
			IPAFServiceDescription serviceDescription,
			IPAFTypeFilter typeFilter,
			PAFLocalServiceInstantiator localServiceInstantiator);
		/// <remarks>
		/// See <see cref="IPAFServiceManagerExtended"/>.
		/// </remarks>
		IPAFServiceManager ParentManagerInternal{get; set;}

	}
}