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

using System;
using System.Reflection;
using System.Security;
using PlatformAgileFramework.Manufacturing;

namespace PlatformAgileFramework.TypeHandling
{
	/// <summary>
	/// <para>
	///	Contains needed information about an assembly. Early/Late bound.
	/// </para>
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// <history>
	/// <author> DAP </author>
	/// <date> 10nov2011 </date>
	/// <contribution>
	/// <para>
	/// Changed to hold an actual assembly to be consistent with
	/// style of <see cref="PAFTypeHolder"/>.
	/// </para>
	/// </contribution>
	/// <author> DAP </author>
	/// <date> 06nov2011 </date>
	/// <contribution>
	/// <para>
	/// Made this interface and the provider interface.
	/// </para>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Not thread-safe. On occasion, the set methods must be used. An example would
	/// be if an assembly loader was looking for an assemblies with only partial
	/// information about it. It may then set a specific version, or may be looking
	/// for an assembly by the simple name and fill in details when found and loaded.
	/// </threadsafety>
	public interface IPAFAssemblyHolder
	{
		#region Properties
		/// <summary>
		/// The resolved assembly, which may be <see langword="null"/>.
		/// </summary>
		Assembly Asmbly { get; [SecurityCritical] set; }
		/// <summary>
		/// The assembly checker that is used to verify that a candidate assembly is a suitable
		/// candidate to load. See <see cref="PAFAssemblyHolderBase.SecondAssemblyWillWork"/> is used
		/// as a default in some implementations.
		/// </summary>
		CheckCandidateAssembly AssemblyChecker { get; [SecurityCritical] set; }
		/// <summary>
		/// Holds the assembly culture. May be blank/<see langword="null"/>.
		/// </summary>
		string AssemblyCulture { get; [SecurityCritical] set; }
		/// <summary>
		/// Loader for assemblies. Must never be <see langword="null"/>.
		/// </summary>
		IPAFAssemblyLoader AssemblyLoader { get; [SecurityCritical] set; }
		/// <summary>
		/// Holds the full assembly name string.
		/// </summary>
		string AssemblyNameString { get; [SecurityCritical] set; }
		/// <summary>
		/// Public Key TOKEN, not the full Public Key. May be <see langword="null"/>
		/// or literal string "null"
		/// </summary>
		string AssemblyPublicKeyToken { get; [SecurityCritical] set; }
		/// <summary>
		/// Holds the assembly name. This should normally be the name of the file
		/// without the extension, if the assembly is file-based.
		/// </summary>
		/// <remarks>
		/// This property can be an asterisk (*), in which case a set of
		/// assemblies can be searched for the occurrances of a certain type.
		/// </remarks>
		string AssemblySimpleName { get; [SecurityCritical] set; }
		/// <summary>
		/// Holds the strong name.
		/// </summary>
		string AssemblyStrongName { get; [SecurityCritical] set; }
		/// <summary>
		/// Holds the version in the form X.X.X.X. May be blank/<see langword="null"/>.
		/// </summary>
		string AssemblyVersion { get; [SecurityCritical] set; }
		#endregion // Properties
		#region Methods
		/// <summary>
		/// Resolves an assembly if it is not already resolved. Creates the <see cref="Asmbly"/>.
		/// </summary>
		/// <param name="loader">
		/// Loader that can load an assembly, if needed. If <see langword="null"/>,
		/// the <see cref="AssemblyLoader"/> is tried. If this is
		/// <see langword="null"/>, <see cref="PAFAssemblyLoader.GetDefaultAssemblyLoader"/>
		/// is used.
		/// </param>
		/// <param name="staticLinkedOnly">
		/// <see langword="true"/> to load only from this (current) assembly's manifest
		/// or from assemblies already loaded. Default is <see langword="false"/>.
		/// </param>
		/// <returns>
		/// <see langword="null"/> if assembly not found.
		/// </returns>
		void ResolveMyAssembly(IPAFAssemblyLoader loader = null, bool staticLinkedOnly = false);
		#endregion // Methods
	}
}