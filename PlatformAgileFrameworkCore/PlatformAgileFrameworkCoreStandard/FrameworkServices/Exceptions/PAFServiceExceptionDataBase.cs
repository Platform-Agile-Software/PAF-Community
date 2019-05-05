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

using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Logging;

namespace PlatformAgileFramework.FrameworkServices.Exceptions
{
	/// <summary>
	/// Base class for service exceptions concerning multiple services. See
	/// <see cref="IPAFServicesExceptionData"/>.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 11oct2014 </date>
	/// <description>
	/// New. Implementation of <see cref="IPAFServicesExceptionData"/>
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Uses a locked collection - safe after locking.
	/// </threadsafety>
	public abstract class PAFServiceExceptionDataBase :
		PAFAbstractStandardExceptionDataBase, IPAFServiceExceptionData
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		internal IPAFServiceDescription m_ProblematicService;
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor builds with the standard arguments plus the
		/// <see cref="IPAFServiceExceptionData.ProblematicService"/>.
		/// </summary>
		/// <param name="problematicService">
		/// See <see cref="IPAFServiceExceptionData"/>.
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
		protected PAFServiceExceptionDataBase(IPAFServiceDescription problematicService,
			object extensionData = null, PAFLoggingLevel? pafLoggingLevel = null, bool?
			isFatal = null)
			: base(extensionData, pafLoggingLevel, isFatal)
		{
			m_ProblematicService = problematicService;
		}
		#endregion Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFServiceExceptionData"/>.
		/// </summary>
		public IPAFServiceDescription ProblematicService
		{
			get { return m_ProblematicService; }
			protected internal set { m_ProblematicService = value; }
		}
		#endregion // Properties
	}
}
