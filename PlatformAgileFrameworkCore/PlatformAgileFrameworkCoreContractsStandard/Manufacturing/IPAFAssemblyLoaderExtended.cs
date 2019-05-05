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
#pragma warning restore 1587

using System;
using System.Collections.Generic;
using System.Reflection;

namespace PlatformAgileFramework.Manufacturing
{
    /// <summary>
    /// This interface prescribes members that all Platform-agile assembly loaders for PAF
    /// must implement. <see cref="IPAFAssemblyLoaderExtended"/>s are <see cref="IPAFAssemblyResolver"/>s and can
    /// thus be used to hook the <c>AppDomain.AssemblyResolve</c> event.
    /// </summary>
    /// <threadsafety>
    /// Implementations and typically NOT thread-safe.
    /// </threadsafety>
    /// <remarks>
    /// This interface adds the capability to load from an arbitrary file.
    /// </remarks>
    public interface IPAFAssemblyLoaderExtended: IPAFAssemblyLoader
	{
		#region Properties
		/// <summary>
		/// The list of directories/URI's to search for loading unresolved DLL's. Try
		/// to make this set of directories ordered, starting from the most likely
		/// place the DLL would be found. The directories are checked in order.
		/// </summary>
		/// <remarks>
		/// Setter allows the list to be <see langword="null"/>ed. Default is <see langword="null"/>.
		/// If this parameter is <see langword="null"/>, and the <see cref="IPAFAssemblyLoader.LoadAssembly"/>
		/// method is in use, the directory of the loaded assembly is added to
		/// the application's static probe list for assembly resolution. See
		/// Pratscher, Chapter 7. 
		/// </remarks>
		IList<string> OrderedDirectoryList { get; set; }
        #endregion // Properties
        #region Methods
        /// <summary>
        /// <para>
        /// This is an assembly loader that attempts to locate an assembly from a
        /// set of directories. It's operation differs, depending on the format of
        /// the <paramref name="assemblyName"/> passed to it. The assembly name can
        /// be a simple name, without version or other information (e.g. "myAssembly")
        /// or a fully-qualified name, such as would come from <see cref="Assembly.FullName"/>.
        /// The directories are searched in the order they appear in the incoming list.
        /// </para>
        /// <para>
        /// In the case of a simple name, the method will try every directory in the
        /// directory search list and attempt to load first a file with a platform-specific
        /// executable extension appended, then with a platform-specific dynamic load
        /// library extension (e.g. ".dll", .dylib, .so, etc.). The first file it finds,
        /// it returns. If <see cref="IPAFAssemblyLoader.ProcessSimpleAssemblyName"/> is <see langword="false"/>, the
        /// method will return <see langword="null"/>. A simple path can be rooted, in which case,
        /// it is used directly and directories are ignored.
        /// </para>
        /// <para>
        /// In the case of a full <see cref="Assembly.FullName"/> this method attempts to locate
        /// an assembly from a set of directories with the correct simple name. It tries loading
        /// an assembly from one directory after another until it finds one with the correct
        /// matching <see cref="Assembly.FullName"/> information, including version, etc..
        /// </para>
        /// <para>
        /// This method loads assemblies into the current "AppDomain""/>
        /// </para>
        /// <para>
        /// This method assumes either one or the other of an exe or a dll will be present
        /// in any given directory. It looks for exe first and if it does not find it,only
        /// then does it look for a DLL. If both can be present, your design is probably
        /// bad.
        /// </para>
        /// </summary>
        /// <param name="directoriesToCheck">
        /// Directories to check for assemblies. This set of directories is added to the base set of
        /// directories handled by the <see cref="OrderedDirectoryList"/> property. If these are both
        /// <see langword="null"/> or empty, an empty list is created and no directories are searched and the
        /// method TYPICALLY returns <see langword="null"/> although specific implementations may behave differently.
        /// Duplicate directories are removed from the aggregated list.
        /// </param>
        /// <param name="assemblyName">
        /// Assembly reference string. The expectation of the format is consistent with the string
        /// handed back from <c>AppDomain.AssemblyResolve</c> callback, so this method can be
        /// used as a resolver. See the summary for further details on allowable names.
        /// If an executable or dynamic library extension is already appended to the name,
        /// it is not replaced. Be aware that extensions are platform-specific.
        /// </param>
        /// <returns>
        /// The loaded assembly or <see langword="null"/> if unsuccessful. Normally, exceptions
        /// are generated from the runtime if assembly load is not successful if a file
        /// is found. <see langword="null"/> usually means that no file with the prescribed base name
        /// was found in the search paths.
        /// </returns>
        /// <exceptions>
        /// <exception cref="ArgumentNullException"> is thrown if <paramref name="assemblyName"/>
        /// is <see langword="null"/> or blank. Message is "assemblyName".
        /// </exception>
        /// <exception cref="BadImageFormatException">s are supressed when <see cref="IPAFAssemblyLoader.IgnoreBadFormat"/>
        /// is set.
        /// </exception>
        /// <exception cref="Exception">s are generated within the framework for various reasons
        /// when assemblies cannot be loaded and this method does not catch them. The exception is
        /// the <exception cref="BadImageFormatException"/> mentioned above, when parameters are
        /// properly set.
        /// </exception>
        /// </exceptions>
        /// <remarks>
        /// Internally, this method typically calls <c>Assembly.LoadFrom(String)</c> which can load an assembly
        /// from an arbitrary directory. Noted that the <c>Assembly.LoadFrom(String)</c> is a security-critical
        /// method and will fail in partial-trust environments
        /// </remarks>
        Assembly LoadAssemblyFrom(ICollection<string> directoriesToCheck, string assemblyName);
		#endregion // Methods
	}
}