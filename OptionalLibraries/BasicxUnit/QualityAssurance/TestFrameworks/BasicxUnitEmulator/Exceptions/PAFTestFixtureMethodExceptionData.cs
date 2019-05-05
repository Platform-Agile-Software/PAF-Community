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
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Exceptions
{
	/// <summary>
	///	See <see cref="IPAFTestFixtureMethodExceptionData"/>.
	/// </summary>
	/// <remarks>
	/// Need for aggregation on this exception type, so interface is defined.
	/// </remarks>
	[PAFSerializable]
	// ReSharper disable PartialTypeWithSinglePart
	// Core part.;
	public sealed partial class PAFTestFixtureMethodExceptionData :
		// ReSharper restore PartialTypeWithSinglePart
		PAFAbstractStandardExceptionDataBase, IPAFTestFixtureMethodExceptionData
	{
		#region Fields and Autoproperties

		/// <summary>
		/// Backing for the prop.
		/// </summary>
		internal IPAFTypeHolder m_TestFixtureType;

		/// <summary>
		/// Backing for the prop.
		/// </summary>
		internal string m_TestFixtureMethodName;

		#endregion // Fields and Autoproperties

		#region Constructors

		/// <summary>
		/// Constructor builds with the standard arguments plus the <see cref="IPAFTypeHolder"/>.
		/// </summary>
		/// <param name="testFixtureType">
		/// Loads <see cref="TestFixtureType"/>. May not be <see langword="null"/>.
		/// </param>
		/// <param name="testFixtureMethodName">
		/// Loads <see cref="TestFixtureMethodName"/>. May be <see langword="null"/>.
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
		/// <exception cref="ArgumentNullException"> is thrown if <paramref name="testFixtureType"/>
		/// is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		public PAFTestFixtureMethodExceptionData(IPAFTypeHolder testFixtureType, string testFixtureMethodName = null,
			object extensionData = null, PAFLoggingLevel? loggingLevel = null, bool? isFatal = null)
			: base(extensionData, loggingLevel, isFatal)
		{
			m_TestFixtureType = testFixtureType ?? throw new ArgumentNullException(nameof(testFixtureType));
			m_TestFixtureMethodName = testFixtureMethodName;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// See <see cref="IPAFTestFixtureMethodExceptionData"/>.
		/// </summary>
		public IPAFTypeHolder TestFixtureType
		{
			get { return m_TestFixtureType; }
			internal set { m_TestFixtureType = value; }
		}

		/// <summary>
		/// See <see cref="IPAFTestFixtureMethodExceptionData"/>.
		/// </summary>
		public string TestFixtureMethodName
		{
			get { return m_TestFixtureMethodName; }
			internal set { m_TestFixtureMethodName = value; }
		}

		#endregion // Properties
	}
}
