//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2017 Icucom Corporation
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

using System.Reflection;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.StringParsing;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.AssemblyHandling
{
	/// <summary>
	/// <para>
	/// This class just has some helpers for assemblies.
	/// </para>
	/// </summary>
	/// <history>
	/// <author> KRM </author>
	/// <date> 21dec2017 </date>
	/// <contribution>
	/// <description>
	/// New
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Conditionally thread-safe. See member descriptions.
	/// </threadsafety>
	// ReSharper disable PartialTypeWithSinglePart
	public static partial class AssemblyExtensions
	// ReSharper restore PartialTypeWithSinglePart
	{
		/// <summary>
		/// This method just wraps an assembly in a <see cref="PAFAssemblyHolder"/>
		/// and returns the interface.
		/// </summary>
		/// <param name="assembly">"this" reference from Assembly class.</param>
		/// <returns>
		/// The interface.
		/// </returns>
		public static IPAFAssemblyHolder ToAssemblyholder(this Assembly assembly)
		{
			return new PAFAssemblyHolder(assembly);
		}
		/// <summary>
		/// This method picks just the assembly base name without version info or
		/// anything else.
		/// </summary>
		/// <param name="assembly">"this" reference from Assembly class.</param>
		/// <returns>
		/// Name. <see langword="null"/> gets <see langword="null"/>. 
		/// </returns>
		public static string AssemblySimpleName(this Assembly assembly)
		{
			if (assembly == null)
				return null;

			// Full name always has the ",".
			var name = assembly.FullName.BreakStringInTwo()[0];
			return name;
		}
	}
}
