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



// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.Platform
{
	/// <summary>
	/// This class provides platform-specific information. There are certain constants
	/// that need to be loaded before an application can even start.  Thus
	/// we create the platform-agility by swapping files.
	/// </summary>
	public interface IPlatformInfo
	{
		#region Properties
		/// <summary>
		/// This represents the name of the current platform.
		/// </summary>
		string CurrentPlatformName { get; }

		/// <summary>
		/// This represents the name of the current platform binding assembly file,
		/// without path.
		/// </summary>
		string CurrentPlatformAssyName { get; }

		/// <summary>
		/// This represents the default mapping for the c drive for
		/// the platform. This is legacy support stuff for "porting"
		/// windows apps to mono.
		/// </summary>
		string CDriveMapping { get; }

		/// <summary>
		/// This represents the default mapping for the d drive for
		/// the platform. This is legacy stuff for "porting" windows apps to
		/// mono.
		/// </summary>
		string DDriveMapping { get; }

		/// <summary>
		/// This represents the executable extension for
		/// the platform.
		/// </summary>
		string ExecutableExtension { get; }

		/// <summary>
		/// This represents the dynamically linked library extension for
		/// the platform.
		/// </summary>
		string DllExtension { get; }

		/// <summary>
		/// This represents the "main" directory separator character for
		/// the platform.
		/// </summary>
		char MainDirSepChar { get; }

		/// <summary>
		/// This represents the "alternative" directory separator character for
		/// the platform. Our conversion stuff on unix accepts backslash.
		/// </summary>
		char AltDirSepChar { get; }
		#endregion // Properties
	}
}
