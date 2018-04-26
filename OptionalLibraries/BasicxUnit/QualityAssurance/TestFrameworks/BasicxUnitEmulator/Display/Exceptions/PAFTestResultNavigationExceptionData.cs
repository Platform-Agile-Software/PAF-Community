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

using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display.Exceptions
{
	/// <summary>
	///	See <see cref="IPAFTestResultNavigationExceptionData"/>. This class doesn't
	/// do much except carry messages.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 11jan2018 </date>
	/// <description>
	/// New. Mostly for errors in console-based navigation.
	/// </description>
	/// </contribution>
	/// </history>
	[PAFSerializable]
	public sealed class PAFTestResultNavigationExceptionData :
		PAFAbstractStandardExceptionDataBase, IPAFTestResultNavigationExceptionData
	{
		#region Constructors
		/// <summary>
		/// Constructor builds with the standard arguments.
		/// </summary>
		/// <param name="extensionData">
		/// Loads <see cref="IPAFStandardExceptionData.ExtensionData"/>.
		/// </param>
		/// <param name="loggingLevel">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		/// <param name="isFatal">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		public PAFTestResultNavigationExceptionData(
			object extensionData = null, PAFLoggingLevel? loggingLevel = null, bool? isFatal = null)
			: base(extensionData, loggingLevel, isFatal)
		{
		}
		#endregion Constructors
	}
}