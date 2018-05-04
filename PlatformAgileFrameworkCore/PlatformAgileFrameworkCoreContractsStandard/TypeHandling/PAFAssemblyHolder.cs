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
using PlatformAgileFramework.Serializing;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.TypeHandling
{
	/// <summary>
	/// Sealed version of base.
	/// </summary>
	/// <threadsafety>
	/// See base.
	/// </threadsafety>
	/// <history>
	/// <author> DAP </author>
	/// <date> 07nov2011 </date>
	/// <contribution>
	/// <para>
	/// New for direct PAF use.
	/// </para>
	/// </contribution>
	/// </history>
	[PAFSerializable(PAFSerializationType.PAFSurrogate)]
// ReSharper disable CSharpWarnings::CS0660
// Overridden on base.
	public sealed class PAFAssemblyHolder : PAFAssemblyHolderBase
// ReSharper restore CSharpWarnings::CS0660
	{
		#region Constructors
		/// <summary>
		/// For serializer.
		/// </summary>
		internal PAFAssemblyHolder(){}
		/// <summary>
		/// See base.
		/// </summary>
		/// <param name="assemblyName">
		/// See base.
		/// </param>
		/// <param name="assembly">
		/// See base.
		/// </param>
		/// <param name="asmblyChecker">
		/// See base.
		/// </param>
		/// <param name="assemblyLoader">
		/// See base.
		/// </param>
		/// <exceptions>
		/// See base.
		/// </exceptions>
		public PAFAssemblyHolder(string assemblyName, Assembly assembly,
			CheckCandidateAssembly asmblyChecker = null, IPAFAssemblyLoader assemblyLoader = null)
			: base(assemblyName, assembly, asmblyChecker, assemblyLoader) { }
		/// <summary>
		/// See base.
		/// </summary>
		/// <param name="assemblyHolder">
		/// See base.
		/// </param>
		public PAFAssemblyHolder(PAFAssemblyHolder assemblyHolder):base(assemblyHolder){}
		#endregion Constructors
		#region Conversion Operators
		/// <summary>
		/// Calls <c>PAFAssemblyName(Assembly.FullName)</c>.
		/// </summary>
		/// <param name="assembly">
		/// The assembly to be represented. Not <see langword="null"/>.
		/// </param>
		/// <returns>
		/// One of us.
		/// </returns>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if <paramref name="assembly"/>
		/// is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		public static implicit operator PAFAssemblyHolder(Assembly assembly)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");
			return new PAFAssemblyHolder(assembly.FullName, assembly);
		}
		#endregion // Conversion Operators
	}
}