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

using System;
using System.Collections;
using System.Collections.Generic;
using PlatformAgileFramework.Collections.Enumerators;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.FrameworkServices.Exceptions
{
	/// <summary>
	///	Sealed implementation of <see cref="IPAFServicesExceptionData"/>.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 11oct2014 </date>
	/// New.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Safe after locking.
	/// </threadsafety>
	[PAFSerializable]
	public sealed class PAFServicesExceptionData : PAFServicesExceptionDataBase
	{
		#region Constructors
		/// <summary>
		/// See base.
		/// </summary>
		/// <param name="services">
		/// See base.
		/// </param>
		/// <param name="extensionData">
		/// See base.
		/// </param>
		/// <param name="pafLoggingLevel">
		/// See base.
		/// </param>
		/// <param name="isFatal">
		/// See base.
		/// </param>
		public PAFServicesExceptionData(IEnumerable<IPAFServiceDescription> services = null,
			object extensionData = null, PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null )
// ReSharper disable once PossibleMultipleEnumeration
			:base(services, extensionData, pafLoggingLevel, isFatal )
		{
		}
		#endregion Constructors
	}
}