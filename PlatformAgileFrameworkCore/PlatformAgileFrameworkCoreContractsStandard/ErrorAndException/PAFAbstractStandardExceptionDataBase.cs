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
using System.Security;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.StringParsing;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;

namespace PlatformAgileFramework.ErrorAndException
{
	/// <summary>
	///	The default abstract class for implementing <see cref="IPAFStandardExceptionData"/>.
	/// </summary>
	[PAFSerializable(PAFSerializationType.PAFSurrogate)]
// ReSharper disable PartialTypeWithSinglePart
	public abstract partial class PAFAbstractStandardExceptionDataBase
		: IPAFStandardExceptionDataInternal
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Backing field.
		/// </summary>
		internal object m_ExtensionData;
		/// <summary>
		/// Backing field.
		/// </summary>
		internal bool? m_IsFatal;
		/// <summary>
		/// Backing field.
		/// </summary>
		internal PAFLoggingLevel? PAF_LOGGING_LEVEL;
		/// <summary>
		/// Backing field. This should be loaded only at construction time
		/// - no need for synchronization.
		/// </summary>
		protected internal static List<string> s_SpecificExceptionTags;
		/// <summary>
		/// Backing field. This should be loaded only at construction time
		/// - no need for synchronization.
		/// </summary>
		protected internal static List<string> s_PropertyFormattingExclusionNames;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor initializes fields.
		/// </summary>
		static PAFAbstractStandardExceptionDataBase()
		{
			s_SpecificExceptionTags = new List<string>();
			s_PropertyFormattingExclusionNames = new List<string>();
			// We don't want to see these in the output.
			s_PropertyFormattingExclusionNames.Add("SpecificExceptionTags");
		}
		/// <summary>
		/// Constructor for inheritors.
		/// </summary>
		protected PAFAbstractStandardExceptionDataBase()
		{
		}
		/// <summary>
		/// Constructor sets properties.
		/// </summary>
		/// <param name="extensionData">
		/// Sets <see cref="IPAFStandardExceptionData.ExtensionData"/>.
		/// </param>
		/// <param name="logLevel">
		/// <see cref="IPAFStandardExceptionData"/>.
		/// </param>
		/// <param name="isFatal">
		/// <see cref="IPAFStandardExceptionData"/>.
		/// </param>
		protected PAFAbstractStandardExceptionDataBase(object extensionData = null, 
			PAFLoggingLevel? logLevel = null, bool? isFatal = null): this()
		{
			m_ExtensionData = extensionData;
			m_IsFatal = isFatal;
			PAF_LOGGING_LEVEL = logLevel;
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// <see cref="IPAFStandardExceptionData"/>.
		/// </summary>
		public virtual object ExtensionData
		{
			get { return m_ExtensionData; }
		}
		/// <summary>
		/// <see cref="IPAFStandardExceptionData"/>.
		/// </summary>
		public virtual bool? IsFatal
		{
			get { return m_IsFatal; }
		}
		/// <summary>
		/// <see cref="IPAFStandardExceptionData"/>.
		/// </summary>
		public virtual PAFLoggingLevel? LogLevel
		{
			get { return PAF_LOGGING_LEVEL; }
		}
		/// <summary>
		/// <see cref="IPAFStandardExceptionData"/>.
		/// </summary>
		public IEnumerable<string> SpecificExceptionTags
		{
			get
			{
			    return s_SpecificExceptionTags?.ToArray();
			}
		}
		/// <summary>
		/// These are the names of properties that are not to be included in
		/// the string representation of the exception.
		/// </summary>
		protected IEnumerable<string> PropertyFormattingExclusionNames
		{
			get
			{
			    return s_PropertyFormattingExclusionNames?.ToArray();
			}
		}
		#endregion // Properties
		#region Methods
		#region Partial Methods
		/// <summary>
		/// Registers the specific exception tag in the resource system. Implemented
		/// in extended.
		/// </summary>
		/// <param name="tag">
		/// String name of the excception.
		/// </param>
		/// <param name="exceptionType">
		/// The type of the calling exception.
		/// </param>
		// TODO - connect back to Ellis records as part of CLR/ECMA
		// ReSharper disable UnusedMember.Local
		// ReSharper disable PartialMethodWithSinglePart
		static partial void RegisterNamedExceptionTag(string tag, Type exceptionType);
		// ReSharper restore PartialMethodWithSinglePart
		// ReSharper restore UnusedMember.Local
		#endregion // Partial Methods
		/// <summary>
		/// <see cref="IPAFStandardExceptionDataInternal"/>.
		/// </summary>
		/// Calls protected virtual method.
		void IPAFStandardExceptionDataInternal.SetExtensionData_Internal(object extensionData)
		{
			SetExtensionData_PV(extensionData);
		}
		/// <summary>
		/// Backing for explicit method.
		/// </summary>
		protected virtual void SetExtensionData_PV(object extensionData)
		{
			m_ExtensionData = extensionData;
		}
		/// <summary>
		/// <see cref="IPAFStandardExceptionDataInternal"/>.
		/// </summary>
		/// Calls protected virtual method.
		void IPAFStandardExceptionDataInternal.SetIsFatal_Internal(bool? isFatal)
		{
			SetIsFatal_PV(isFatal);
		}
		/// <summary>
		/// Backing for explicit method.
		/// </summary>
		protected virtual void SetIsFatal_PV(bool? isFatal)
		{
			m_IsFatal = isFatal;
		}
		/// <summary>
		/// <see cref="IPAFStandardExceptionDataInternal"/>.
		/// Calls protected virtual method.
		/// </summary>
		void IPAFStandardExceptionDataInternal.SetLoggingLevel_Internal(PAFLoggingLevel? pafLoggingLevel)
		{
			SetLoggingLevel_PV(pafLoggingLevel);
		}
		/// <summary>
		/// Backing for explicit method.
		/// </summary>
		protected virtual void SetLoggingLevel_PV(PAFLoggingLevel? pafLoggingLevel)
		{
			PAF_LOGGING_LEVEL = pafLoggingLevel;
		}
		#region IPAFFormattable - Related
		#region Implementation of IPAFFormattable
		/// <summary>
		/// <see cref="IPAFFormattable"/>. This method uses <paramref name="formatProvider"/>
		/// if it is here, to call into <see cref="FormatWithProvider"/> to construct a string.
		/// If it is not here, <see cref="LocalFormatToString"/> is callewd with the incoming
		/// string argument.
		/// </summary>
		/// <param name="format">
		/// <see cref="IPAFFormattable"/>.
		/// </param>
		/// <param name="formatProvider">
		/// <see cref="IPAFFormattable"/>. If this is not <see langword="null"/>, it is used.
		/// If it is <see langword="null"/>, an attempt is made to pull it off <see cref="ExtensionData"/>.
		/// If it is still not found, <see cref="LocalFormatToString"/> is called.
		/// </param>
		/// <returns>A rendering of the exception.</returns>
		public virtual string FormatToString(string format, IPAFFormatProvider formatProvider)
		{
			var provider = formatProvider;
			if (provider == null)
			{
				var pafFormatProviderProvider = ExtensionData as IPAFFormatProviderProvider;
				if (pafFormatProviderProvider != null)
				{
					provider = pafFormatProviderProvider.GetFormatProvider(ExtensionData);
				}
			}

			string formattedException;
			if((provider != null) && ((formattedException = FormatWithProvider(format, provider)) != null))
				return formattedException;

			return LocalFormatToString(format);
		}
		/// <remarks>
		/// <see cref="IPAFFormattable"/>. This method just calls
		/// <see cref="FormatToString(String, IPAFFormatProvider )"/>.
		/// </remarks>
		public virtual string FormatToString(string format)
		{
			return FormatToString(format, null);
		}
		/// <remarks>
		/// <see cref="IPAFFormattable"/>. This method just calls
		/// <see cref="FormatToString(String, IPAFFormatProvider )"/>
		/// with <see langword="null"/>.
		/// </remarks>
		public virtual string FormatToString()
		{
			var str =  FormatToString(null, null);
			if (str == null) return base.ToString();
			return str;
		}
		#endregion // Implementation of IPAFFormattable
		/// <summary>
		/// Virtual method provides a place to use a supplied <see cref="IPAFFormatProvider"/>
		/// and a string to produce a string representation of the exception.
		/// </summary>
		/// <param name="format">See <see cref="IPAFFormatProvider"/>.</param>
		/// <param name="formatProvider">See <see cref="IPAFFormatProvider"/>.</param>
		/// <returns>A formatted representation or <see langword="null"/>.</returns>
		/// <remarks>
		/// This base version returns <see langword="null"/>.
		/// </remarks>
		protected virtual string FormatWithProvider(string format, IPAFFormatProvider formatProvider)
		{
			return null;
		}
		/// <summary>
		/// Method provides local formatting for Core. Prints a heading followed by a
		/// string representation of all public gettable exception properties not in
		/// <see cref="PropertyFormattingExclusionNames"/>. The format is a column of
		/// name/value pairs.
		/// </summary>
		/// <param name="heading">
		/// String that provides input for the formatting process. Normally one of the constant
		/// error description strings for the exception. If the string is <see cref="String.Empty"/>,
		/// no heading is printed. If the string is <see langword="null"/>, the <see cref="Type.FullName"/>
		/// of "this" is used as a heading.
		/// </param>
		/// <returns>
		/// A string representation of the exception.
		/// </returns>
		protected virtual string LocalFormatToString(string heading)
		{
			var outputString
				= TypeExtensions.PublicPropsToString(this, heading, PropertyFormattingExclusionNames);
			return outputString;
		}
		/// <summary>
		/// This override calls <see cref="FormatToString()"/>, which calls
		/// <c>base.ToString()</c> by default. If
		/// <see cref="FormatToString(String, IPAFFormatProvider)"/> is overridden,
		/// you get something else of the inheritor's choosing.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return FormatToString();
		}
		#endregion // IPAFFormattable - Related
		/// <summary>
		/// Registers exception tags in the resource system. This provides an extensibility
		/// mechanism for our filtered exception system. Client frameworks can load
		/// their own tags so new exception data types are less often needed.
		/// </summary>
		/// <param name="tags">
		/// String names of the exceptions. Can be <see langword="null"/>.
		/// </param>
		/// <param name="exceptionType">
		/// The type of the calling exception data type or exception data interface.
		/// </param>
		/// <remarks>
		/// <see cref="SecurityCriticalAttribute"/> for elevated-trust callers.
		/// </remarks>
		[SecurityCritical]
		public static void RegisterNamedExceptionTags(IEnumerable<string> tags, Type exceptionType)
		{
			RegisterNamedExceptionTagsInternal(tags, exceptionType);
		}
		/// <summary>
		/// Internal version of <see cref="RegisterNamedExceptionTagsInternal"/>.
		/// </summary>
		/// <param name="tags">
		/// <see cref="RegisterNamedExceptionTagsInternal"/>
		/// </param>
		/// <param name="exceptionType">
		/// <see cref="RegisterNamedExceptionTagsInternal"/>.
		/// </param>
		internal static void RegisterNamedExceptionTagsInternal(IEnumerable<string> tags, Type exceptionType)
		{
			if (tags == null) return;
			foreach (var tag in tags) {
				// ReSharper disable InvocationIsSkipped
				RegisterNamedExceptionTag(tag, exceptionType);
				// ReSharper restore InvocationIsSkipped
			}
		}
		#endregion Methods
	}
}