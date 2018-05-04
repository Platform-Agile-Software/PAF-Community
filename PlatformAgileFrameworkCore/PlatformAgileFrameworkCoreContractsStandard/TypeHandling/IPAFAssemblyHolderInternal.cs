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
using System.Reflection;
using PlatformAgileFramework.Manufacturing;

namespace PlatformAgileFramework.TypeHandling
{
	/// <summary>
	/// <para>
	///	Internal setters for setting stuff post-construction and for serializers.
	/// </para>
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// <history>
	/// <contribution>
	/// <author> DAP </author>
	/// <date> 21nov2011 </date>
	/// <description>
	/// Made this interface for serialization.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Not safe. This is only for privileged callers.
	/// </threadsafety>
	internal interface IPAFAssemblyHolderInternal: IPAFAssemblyHolder
	{
		#region Methods
		/// <summary>
		/// Setter for the prop. See <see cref="IPAFAssemblyHolder"/>.
		/// </summary>
		void SetAssembly(Assembly asmbly);
		/// <summary>
		/// Setter for the prop. See <see cref="IPAFAssemblyHolder"/>.
		/// </summary>
		void SetAssemblyChecker(CheckCandidateAssembly assemblyChecker);
		/// <summary>
		/// Setter for the prop. See <see cref="IPAFAssemblyHolder"/>.
		/// </summary>
		void SetAssemblyCulture(string assemblyCulture);
		/// <summary>
		/// Setter for the prop. See <see cref="IPAFAssemblyHolder"/>.
		/// </summary>
		void SetAssemblyLoader(IPAFAssemblyLoader assemblyLoader);
		/// <summary>
		/// Setter for the prop. See <see cref="IPAFAssemblyHolder"/>.
		/// </summary>
		void SetAssemblyNameString(string assemblyNameString);
		/// <summary>
		/// Setter for the prop. See <see cref="IPAFAssemblyHolder"/>.
		/// </summary>
		void SetAssemblyPublicKeyToken(string assemblyPublicKeyToken);
		/// <summary>
		/// Setter for the prop. See <see cref="IPAFAssemblyHolder"/>.
		/// </summary>
		void SetAssemblySimpleName(string assemblySimpleName);
		/// <summary>
		/// Setter for the prop. See <see cref="IPAFAssemblyHolder"/>.
		/// </summary>
		void SetAssemblyStrongName(string assemblyStrongName);
		/// <summary>
		/// Setter for the prop. See <see cref="IPAFAssemblyHolder"/>.
		/// </summary>
		void SetAssemblyVersion(string assemblyVersion);
		#endregion // Methods
	}
}