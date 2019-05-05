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
using System.Collections.Generic;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.ErrorAndException
{
	/// <summary>
	///	The default class for implementing <see cref="IPAFStandardExceptionData"/>. This
	/// concrete class is appropriate for use with a standard exception that just has
	/// a message and inner exception - no custom data.
	/// </summary>
	[PAFSerializable(PAFSerializationType.PAFSurrogate)]
	public sealed class PAFStandardExceptionData : PAFAbstractStandardExceptionDataBase
	{
		#region Constructors
		/// <summary>
		/// Constructor sets properties to <see langword="null"/>.
		/// </summary>
		public PAFStandardExceptionData()
		{
		}

		/// <summary>
		/// Constructor sets properties.
		/// </summary>
		/// <param name="logLevel">
		/// <see cref="IPAFStandardExceptionData"/>.
		/// </param>
		/// <param name="extensionData">
		/// Sets <see cref="IPAFStandardExceptionData.ExtensionData"/>.
		/// </param>
		/// <param name="isFatal">
		/// <see cref="IPAFStandardExceptionData"/>.
		/// </param>
		public PAFStandardExceptionData(object extensionData = null, PAFLoggingLevel? logLevel = null,
			bool? isFatal = null) :base(extensionData, logLevel, isFatal)
		{
		}
		#endregion // Constructors
		#region Static Helper Methods
		/// <summary>
		/// Helper for constructors.
		/// </summary>
		/// <param name="data">
		/// If this data is <see langword="null"/> a new default version of
		/// <see cref="IPAFStandardExceptionData"/> is created.
		/// </param>
		/// <returns>
		/// A new version of the interface implementation or the original
		/// incoming version if it is not <see langword="null"/>.
		/// </returns>
		public static IPAFStandardExceptionData DefaultIfNull(IPAFStandardExceptionData data)
		{
			return data ?? new PAFStandardExceptionData();
		}

		#endregion // Static Helper Methods
	}
}