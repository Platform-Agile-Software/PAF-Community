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

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.Platform
{
	/// <summary>
	/// <para>
	/// This class provides platform-specific information. There are certain constants
	/// that need to be loaded before an applcation can even start.  Thus
	/// we create the platform-agility by swapping files.
	/// </para>
	/// <para>
	/// This is the WindowsDesktop version.
	/// </para>
	/// </summary>
	public class WinPlatformInfo :IPlatformInfo
	{
		#region Fields and Autoproperties
		/// <summary>
		/// This represents the name of the current platform.
		/// </summary>
		public const string CURRENT_PLATFORM_NAME = "Win";

		/// <summary>
		/// This represents the name of the current platform binding assembly.
		/// </summary>
		public const string CURRENT_PLATFORM_ASSY = "ECMA";

		/// <summary>
		/// This represents the default mapping for the c drive for
		/// the platform. This is what the single-letter directory spec
		/// will be mapped to when put on the front end of a path, so it
		/// must have the colon.
		/// </summary>
		public const string C_DRIVE_MAPPING = "c:";

		/// <summary>
		/// This represents the default mapping for the d drive for
		/// the platform.  This is what the single-letter directory spec
		/// will be mapped to when put on the front end of a path, so it
		/// must have the colon.
		/// </summary>
		public const string D_DRIVE_MAPPING = "d:";

		/// <summary>
		/// This represents the executable extension for
		/// the platform.
		/// </summary>
		public const string EXECUTABLE_EXTENSION = ".exe";

		/// <summary>
		/// This represents the dynamically linked library extension for
		/// the platform.
		/// </summary>
		public const string DLL_EXTENSION = ".dll";

		/// <summary>
		/// This represents the "main" directory separator character for
		/// the platform.
		/// </summary>
		public const char MAIN_DIR_SEP_CHAR = '\\';

		/// <summary>
		/// This represents the "alternative" directory separator character for
		/// the platform.
		/// </summary>
		public const char ALT_DIR_SEP_CHAR = '/';
		#endregion // Fields and Autoproperties
		#region Implementation of IPlatformInfo
		/// <summary>
		/// Default for <see cref="IPlatformInfo"/>.
		/// </summary>
		public virtual string CurrentPlatformName
		{
			get { return CURRENT_PLATFORM_NAME; }
		}

		/// <summary>
		/// Default for <see cref="IPlatformInfo"/>.
		/// </summary>
		public virtual string CurrentPlatformAssyName
		{
			get { return CURRENT_PLATFORM_ASSY; }
		}

		/// <summary>
		/// Default for <see cref="IPlatformInfo"/>.
		/// </summary>
		public virtual string CDriveMapping
		{
			get { return C_DRIVE_MAPPING; }
		}

		/// <summary>
		/// Default for <see cref="IPlatformInfo"/>.
		/// </summary>
		public virtual string DDriveMapping
		{
			get { return D_DRIVE_MAPPING; }
		}

		/// <summary>
		/// Default for <see cref="IPlatformInfo"/>.
		/// </summary>
		public virtual string ExecutableExtension
		{
			get { return EXECUTABLE_EXTENSION; }
		}

		/// <summary>
		/// Default for <see cref="IPlatformInfo"/>.
		/// </summary>
		public virtual string DllExtension
		{
			get { return DLL_EXTENSION; }
		}

		/// <summary>
		/// Default for <see cref="IPlatformInfo"/>.
		/// </summary>
		public virtual char MainDirSepChar
		{
			get { return MAIN_DIR_SEP_CHAR; }
		}

		/// <summary>
		/// Default for <see cref="IPlatformInfo"/>.
		/// </summary>
		public virtual char AltDirSepChar
		{
			get { return ALT_DIR_SEP_CHAR; }
		}
		#endregion
	}
}
