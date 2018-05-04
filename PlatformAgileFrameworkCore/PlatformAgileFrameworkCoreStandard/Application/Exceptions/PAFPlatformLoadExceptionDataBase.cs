//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2017 Icucom Corporation
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

using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Logging;

namespace PlatformAgileFramework.Application.Exceptions
{
    /// <summary>
    /// <para>
    ///	The base class for exception accessing platform-specific assemblies.
    /// </para>
    /// </summary>
    /// <history>
    /// <contribution>
    /// <author> KRM </author>
    /// <date> 05nov17 </date>
    /// <description>
    /// New. Needed a specific exception for not being able to reach the platform.
    /// </description>
	/// </contribution>
    /// </history>
    public abstract class PAFPlatformLoadExceptionDataBase :
		PAFAbstractStandardExceptionDataBase, IPAFPlatformLoadExceptionData
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		internal string m_PlatformAssemblyOrName;
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor builds with the standard arguments plus the
		/// <see cref="IPAFPlatformLoadExceptionData.PlatformAssemblyOrName"/>.
		/// </summary>
		/// <param name="platformAssemblyOrName">
		/// See <see cref="IPAFPlatformLoadExceptionData"/>.
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
		protected PAFPlatformLoadExceptionDataBase(string platformAssemblyOrName,
			object extensionData = null, PAFLoggingLevel? pafLoggingLevel = null, bool?
			isFatal = null)
			: base(extensionData, pafLoggingLevel, isFatal)
		{
			m_PlatformAssemblyOrName = platformAssemblyOrName;
		}
		#endregion Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFPlatformLoadExceptionData"/>.
		/// </summary>
		public string PlatformAssemblyOrName
		{
			get { return m_PlatformAssemblyOrName; }
			protected internal set { m_PlatformAssemblyOrName = value; }
		}
		#endregion // Properties
	}
}
