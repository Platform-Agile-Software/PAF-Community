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

namespace PlatformAgileFramework.FileAndIO.Exceptions
{
    /// <summary>
    /// <para>
    ///	The base class for STORAGE AREA NAME exception data.
    /// </para>
    /// </summary>
    /// <history>
    /// <contribution>
    /// <author> KRM </author>
    /// <date> 09jul2015 </date>
    /// <description>
    /// </description>
    /// I changed ALL the names having to do with FILEs, since it was confusing
    /// clients who were using this for NON-FILE storage. We had brought the exception
    /// over from ECMA and it was confusing folks.
    /// </contribution>
    /// </history>
    public abstract class PAFStoragePathFormatExceptionDataBase :
        PAFAbstractStandardExceptionDataBase, IPAFStoragePathFormatExceptionData
    {
        #region Class Fields and Autoproperties
        /// <summary>
        /// Backing for the prop.
        /// </summary>
        internal string m_ProblematicStoragePath;
        #endregion // Class Fields and Autoproperties
        #region Constructors
        /// <summary>
        /// Constructor builds with the standard arguments plus the
        /// <see cref="IPAFStoragePathFormatExceptionData.ProblematicStoragePath"/>.
        /// </summary>
        /// <param name="problematicStoragePath">
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
        protected PAFStoragePathFormatExceptionDataBase(string problematicStoragePath,
            object extensionData = null, PAFLoggingLevel? pafLoggingLevel = null, bool?
            isFatal = null)
            : base(extensionData, pafLoggingLevel, isFatal)
        {
            m_ProblematicStoragePath = problematicStoragePath;
        }
        #endregion Constructors
        #region Properties
        /// <summary>
        /// See <see cref="IPAFStoragePathFormatExceptionData"/>.
        /// </summary>
        public string ProblematicStoragePath
        {
            get { return m_ProblematicStoragePath; }
            protected internal set { m_ProblematicStoragePath = value; }
        }
        #endregion // Properties
    }
}
