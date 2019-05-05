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

#region Using Directives
using PlatformAgileFramework.TypeHandling.PartialClassSupport;
#endregion

namespace PlatformAgileFramework.FileAndIO
{
	/// <summary>
	/// <para>
	/// This class describes what we can do to a "file". Named with a prefix to avoid
	/// confusion with .Net enums. "Files" are general storage areas in PAF. There are only
	/// a limited number of bit fields here, since we cover files and also other storage
	/// mechanisms.
	/// </para>
	/// <para>
	/// The standard .Net file modes do not really make sense on some of the platforms
	/// we run on, since we are not able to control access in such ways.
	/// </para>
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 04may2019 </date>
	/// <description>
	/// Added history. Original from Silverlight days, I believe. Also needed to add/change
	/// DOCs to reflect updated behavior WRT replace/append.
	/// we assumed.
	/// </description>
	/// </contribution>
	/// </history>
	// ReSharper disable PartialTypeWithSinglePart
	public sealed partial class PAFFileAccessMode : ExtendablePseudoEnumInt32
	// ReSharper restore PartialTypeWithSinglePart
	{
		#region Class Fields And Autoproperties
		/// <summary>
		/// Set this bit to prevent writing. Invalid when used with <see cref="REPLACE"/>.
		/// </summary>
		// ReSharper disable once InconsistentNaming
		public static readonly PAFFileAccessMode READONLY
			= new PAFFileAccessMode("READONLY", 1, true);
		/// <summary>
		/// Scrolls to the end of a file if a file exists. Applicable to open
		/// operations. New files should be created if they do not exist
		/// and cursor left at the beginning.
		/// </summary>
		// ReSharper disable once InconsistentNaming
		public static readonly PAFFileAccessMode APPEND
			= new PAFFileAccessMode("APPEND", 4, true);
		/// <summary>
		/// Replaces a file if it already exists. Applicable to open
		/// operations.
		/// </summary>
		// ReSharper disable once InconsistentNaming
		public static readonly PAFFileAccessMode REPLACE
			= new PAFFileAccessMode("REPLACE", 16, true);
		#endregion // Class Fields And Autoproperties
		/// <remarks>
		/// See base.
		/// </remarks>
		public PAFFileAccessMode(string name, int value)
			: base(name, value)
		{
		}
		/// <remarks>
		/// See base.
		/// </remarks>
		internal PAFFileAccessMode(string name, int value, bool addToDictionary)
			: base(name, value, addToDictionary)
		{
		}

		/// <summary>
		/// Validates whether basic bits are compatible.
		/// </summary>
		/// <param name="pafFileAccessMode">Paf mode.</param>
		/// <returns><see langword="false"/> for inconsistent bits.</returns>
		public static bool ValidateFileAccessMode(PAFFileAccessMode pafFileAccessMode)
		{
			// Can't have readonly with replace.
			if (((pafFileAccessMode & READONLY) != 0)
				&& ((pafFileAccessMode & REPLACE) != 0))
				return false;

			// Can't have readonly with append.
			if (((pafFileAccessMode & READONLY) != 0)
				&& ((pafFileAccessMode & APPEND) != 0))
				return false;

			return true;
		}
	}
}
