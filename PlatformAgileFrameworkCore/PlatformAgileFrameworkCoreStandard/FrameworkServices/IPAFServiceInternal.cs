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
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// Remnants of the internal interface that must be provided for the SL model.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> Brian T. </author>
	/// <date> 02jan2019 </date>
	/// <description>
	/// Refactored this to contain just "IsDefault" manipulation. Old interface
	/// hierarchy was not right.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> DAP </author>
	/// <date> 04jan2012 </date>
	/// <description>
	/// Took a lot of stuff out of here and moved it to extended.
	/// Added history.
	/// </description>
	/// </contribution>
	/// </history>
	// ReSharper disable once PartialTypeWithSinglePart
	internal partial interface IPAFServiceInternal : IPAFService
	{
		#region Methods
		/// <summary>
		/// Should service be the default service for the type?
		/// </summary>
		/// <param name="serviceIsDefault">
		/// <see langword = "true"/> to mark the service as default.
		/// </param>
		/// <return>
		///<see langword = "true"/> if a change was made.
		/// </return>
		bool SetServiceAsDefault(bool serviceIsDefault);
		/// <summary>
		/// Gets the manager the service is contained in.
		/// </summary>
		IPAFServiceManagerInternal ServiceManager { get; set; }
		#endregion // Methods
	}
}
