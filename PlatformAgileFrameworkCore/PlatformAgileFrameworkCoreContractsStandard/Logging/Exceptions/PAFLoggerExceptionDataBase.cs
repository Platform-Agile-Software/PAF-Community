//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2019 Icucom Corporation
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

using PlatformAgileFramework.ErrorAndException;

namespace PlatformAgileFramework.Logging.Exceptions
{
	/// <summary>
	/// Base class for general logging exceptions.
	/// See <see cref="IPAFLoggerExceptionData"/>.
	/// </summary>
	public abstract class PAFLoggerExceptionDataBase :
		PAFAbstractStandardExceptionDataBase, IPAFLoggerExceptionData
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		internal string m_ProblematicLogFile;
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor builds with the standard arguments plus the
		/// <see cref="IPAFLoggerExceptionData.ProblematicLogFile"/>.
		/// </summary>
		/// <param name="problematicLogFile">
		/// See <see cref="IPAFLoggerExceptionData"/>.
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
		protected PAFLoggerExceptionDataBase(string problematicLogFile,
			object extensionData = null,
			PAFLoggingLevel? pafLoggingLevel = null, bool?
			isFatal = null)
			: base(extensionData, pafLoggingLevel, isFatal)
		{
			m_ProblematicLogFile = problematicLogFile;
		}
		#endregion Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFLoggerExceptionData"/>.
		/// </summary>
		public string ProblematicLogFile
		{
			get { return m_ProblematicLogFile; }
			protected internal set { m_ProblematicLogFile = value; }
		}
		#endregion // Properties
	}
}
