//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 Icucom Corporation
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
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Exceptions;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator
{
	/// <summary>
	///	See <see cref="IPAFTestAssemblyInitializationExceptionData"/>.
	/// </summary>
	/// <remarks>
	/// Need for aggregation on this exception type, so interface is defined.
	/// </remarks>
	[PAFSerializable]
	public sealed class PAFTestAssemblyInitializationExceptionData :
		PAFAbstractStandardExceptionDataBase, IPAFTestAssemblyInitializationExceptionData
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		internal IPAFAssemblyHolder m_TestAssemblyInitializing;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor builds with the standard arguments plus the <see cref="PAFAssemblyHolder"/>.
		/// </summary>
		/// <param name="testAssemblyInitializing">
		/// Loads <see cref="TestAssemblyInitializing"/>. May not be <see langword="null"/>.
		/// </param>
		/// <param name="extensionData">
		/// Loads <see cref="IPAFStandardExceptionData.ExtensionData"/>.
		/// </param>
		/// <param name="loggingLevel">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		/// <param name="isFatal">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if <paramref name="testAssemblyInitializing"/>
		/// is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		public PAFTestAssemblyInitializationExceptionData(IPAFAssemblyHolder testAssemblyInitializing,
			object extensionData = null, PAFLoggingLevel? loggingLevel = null, bool? isFatal = null)
			: base(extensionData, loggingLevel, isFatal)
		{
		    m_TestAssemblyInitializing = testAssemblyInitializing ?? throw new ArgumentNullException(nameof(testAssemblyInitializing));
		}
		#endregion Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFTestFixtureMethodExceptionData"/>.
		/// </summary>
		public IPAFAssemblyHolder TestAssemblyInitializing
		{
			get { return m_TestAssemblyInitializing; }
			internal set { m_TestAssemblyInitializing = value; }
		}
		#endregion // Properties
	}
}