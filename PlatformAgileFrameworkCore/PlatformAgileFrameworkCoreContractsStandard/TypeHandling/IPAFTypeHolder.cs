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
using PlatformAgileFramework.Manufacturing;

namespace PlatformAgileFramework.TypeHandling
{
	/// <summary>
	/// Interface for the type holder. We parse an assembly-qualified name
	/// into pieces so we can fiddle with the components. We usually just
	/// send <see cref="IPAFTypeProps.AssemblyQualifiedTypeName"/> across
	/// the wire for .Net type serialization.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 30nov2012 </date>
	/// <description>
	/// New.
	/// Needed to rearrange the type holder and put a piece in CoreContracts.
	/// </description>
	/// </contribution>
	/// </history>
	/// <remarks>
	/// We do not use optional parameters in this interface in order not
	/// to preclude explicit implementation.
	/// </remarks>
	// KRM TODO - someone has to check that this new provider style did not
	// break the serialization surrogate - looks like it probably did.
	public interface IPAFTypeHolder : IPAFTypeProps, IPAFAssemblyHolderProvider
	{
		#region Properties
		/// <summary>
		/// Holds the namespace punctuated with the usual dots.
		/// </summary>
		string Namespace { get; }
		/// <summary>
		/// Holds the namespace-qualified type name. Namespace prefixed on
		/// type with the usual dot.
		/// </summary>
		string NamespaceQualifiedTypeName { get; }
		/// <summary>
		/// Holds the type name. No namespace - no dots.
		/// </summary>
		string SimpleTypeName { get; }
		#endregion // Properties

		#region Methods
		/// <summary>
		/// Resolves a type from either the set of assemblies already loaded in
		/// an app domain or from an assembly loaded by the <paramref name="loader"/>.
		/// </summary>
		/// <param name="loader">
		/// A supplied loader. The implementation may choose to ignore this loader
		/// in preference for another it has access to. This parameter may be
		/// <see langword="null"/>, depending on the implementation.
		/// </param>
		/// <param name="staticLinkedOnly">
		/// If <see langword="true"/>, the loader will not load any assemblies
		/// into the appdomain - the type's assembly must already be here.
		/// </param>
		/// <returns>
		/// The resolved type or <see langword="null"/>. No exceptions are thrown in
		/// this method, but none are normally caught. If <see langword="null"/> is
		/// returned, this normally means that the type was simply not found.
		/// </returns>
		Type ResolveType(IPAFAssemblyLoader loader, bool staticLinkedOnly);
		/// <remarks>
		/// Normally calls "ResolveType(loader, false)".
		/// </remarks>
		Type ResolveType(IPAFAssemblyLoader loader);
		/// <remarks>
		/// Normally calls "ResolveType(null, staticLinkedOnly)".
		/// </remarks>
		Type ResolveType(bool staticLinkedOnly);
		/// <remarks>
		/// Normally calls "ResolveType(null, false)".
		/// </remarks>
		Type ResolveType();
		#endregion Methods
	}
}