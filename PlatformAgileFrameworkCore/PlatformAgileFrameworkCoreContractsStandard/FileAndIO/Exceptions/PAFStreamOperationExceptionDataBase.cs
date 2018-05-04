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

using System;
using System.Collections.Generic;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.FileAndIO.FileAndDirectoryService;
using PlatformAgileFramework.Logging;

namespace PlatformAgileFramework.FileAndIO.Exceptions
{
	/// <summary>
	/// Base class for filename exceptions.
	/// </summary>
	public abstract class PAFStreamOperationExceptionDataBase :
		PAFAbstractStandardExceptionDataBase, IPAFStreamOperationExceptionData
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Issued when a client attempts to expand a stream that is not
		/// expandable.
		/// </summary>
		public const string STREAM_NOT_EXPANDABLE = "Stream not expandable";
		/// <summary>
		/// Issued when a client attempts to expand a stream that is at its
		/// limit.
		/// </summary>
		public const string STREAM_SIZE_LIMIT_REACHED = "Stream size limit reached";
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		internal IPAFStorageStream m_ProblematicStream;
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Static constructor loads our tags for extended.
		/// </summary>
		static PAFStreamOperationExceptionDataBase()
		{
			var tags = new List<string>
				{
					STREAM_NOT_EXPANDABLE,
					STREAM_SIZE_LIMIT_REACHED
				};

			RegisterNamedExceptionTagsInternal(tags, typeof(IPAFStreamOperationExceptionData));
		}
		/// <summary>
		/// Constructor builds with the standard arguments plus the
		/// <see cref="IPAFStreamOperationExceptionData.ProblematicStream"/>.
		/// </summary>
		/// <param name="problematicStream">
		/// See <see cref="IPAFStoragePathFormatExceptionData"/>.
		/// </param>
		/// <param name="extensionData">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		/// <param name="pafLoggingLevel">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		/// <param name="isFatal">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		protected PAFStreamOperationExceptionDataBase(IPAFStorageStream problematicStream,
			object extensionData = null, PAFLoggingLevel? pafLoggingLevel = null, bool?
			isFatal = null)
			: base(extensionData, pafLoggingLevel, isFatal)
		{
			m_ProblematicStream = problematicStream;
		}
		#endregion Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFStreamOperationExceptionData"/>.
		/// </summary>
		public IPAFStorageStream ProblematicStream
		{
			get { return m_ProblematicStream; }
			protected internal set { m_ProblematicStream = value; }
		}
		#endregion // Properties
	}
}
