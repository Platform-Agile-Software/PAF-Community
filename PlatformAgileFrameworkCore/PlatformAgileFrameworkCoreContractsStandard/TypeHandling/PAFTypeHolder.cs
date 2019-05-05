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
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.Manufacturing;
using PlatformAgileFramework.Serializing;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.TypeHandling
{
	/// <summary>
	/// Sealed version of <see cref="PAFTypeHolderBase"/>.
	/// </summary>
	/// <history>
	/// <author> DAP </author>
	/// <date> 05nov2011 </date>
	/// <contribution>
	/// <para>
	/// New sealed version.
	/// </para>
	/// </contribution>
	/// </history>
// ReSharper disable PartialTypeWithSinglePart
	[PAFSerializable(PAFSerializationType.PAFSurrogate)]
	public sealed partial class PAFTypeHolder : PAFTypeHolderBase
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Constructors
		/// <summary>
		/// Constructor sets props - just calls base.
		/// </summary>
		/// <param name="typeName">
		/// See base.
		/// </param>
		/// <param name="typeType">
		/// See base.
		/// </param>
		/// <param name="checker">
		/// See base.
		/// </param>
		/// <param name="assemblyLoader">
		/// See base.
		/// </param>
		/// <param name="assemblyHolder">
		/// See base.
		/// </param>
		/// <exceptions>
		/// See base.
		/// </exceptions>
		public PAFTypeHolder(Type typeType, string typeName = null, IPAFAssemblyHolder assemblyHolder = null,
			CheckCandidateAssembly checker = null, IPAFAssemblyLoader assemblyLoader = null)
			: base(typeType, typeName, checker, assemblyLoader, assemblyHolder) { }
		/// <summary>
		/// Constructor uses type name - just calls base.
		/// </summary>
		/// <param name="typeName">
		/// See base.
		/// </param>
		/// <exceptions>
		/// See base.
		/// </exceptions>
		public PAFTypeHolder(string typeName): base(typeName) { }
		/// <summary>
		/// Copy constructor builds from the interface.
		/// </summary>
		/// <param name="typeHolder">
		/// One of us.
		/// </param>
		public PAFTypeHolder(IPAFTypeHolder typeHolder)
			: base(typeHolder) { }
		/// <summary>
		/// Copy constructor. See base.
		/// </summary>
		/// <param name="typeHolderBase">
		/// See base.
		/// </param>
		public PAFTypeHolder(PAFTypeHolderBase typeHolderBase)
			: base(typeHolderBase) { }
		#endregion // Constructors
		#region Methods
		/// <summary>
		/// Generates a <see cref="IPAFTypeHolder"/> by calling the main constructor with
		/// a <see cref="Type"/> argument.
		/// </summary>
		/// <param name="typeType">
		/// See constructor.
		/// </param>
		/// <returns>
		/// <see cref="IPAFTypeHolder"/>.
		/// </returns>
		// ReSharper disable once InconsistentNaming
		public static IPAFTypeHolder IHolder(Type typeType)
		{
			return new PAFTypeHolder(typeType);
		}
		#endregion // Methods
		#region Conversion Operators
		/// <summary>
		/// Calls <c>PAFTypeHolder(type)</c>.
		/// </summary>
		/// <param name="type">
		/// The type to be wrapped. Not <see langword="null"/>.
		/// </param>
		/// <returns>
		/// One of us.
		/// </returns>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if <paramref name="type"/>
		/// is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		public static implicit operator PAFTypeHolder(Type type)
		{
			if (type == null)
				throw new ArgumentNullException(nameof(type));
			return new PAFTypeHolder(type);
		}
		/// <summary>
		/// Calls <c>PAFTypeHolder(typeName)</c>.
		/// </summary>
		/// <param name="typeName">
		/// The type name. Can not be <see langword="null"/>.
		/// </param>
		/// <returns>
		/// One of us.
		/// </returns>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if <paramref name="typeName"/>
		/// is <see langword="null"/> or blank.
		/// </exception>
		/// </exceptions>
		public static implicit operator PAFTypeHolder(string typeName)
		{
			if (string.IsNullOrEmpty(typeName))
				throw new ArgumentNullException(nameof(typeName));
			return new PAFTypeHolder(null, typeName);
		}
		#endregion // Conversion Operators
		#region Static Helpers
		/// <summary>
		/// Calls <c>PAFTypeHolder(IPAFNamedAndTypedObject.ObjectType)</c>.
		/// </summary>
		/// <param name="nto">
		/// The named and typed object. Can not be <see langword="null"/>.
		/// </param>
		/// <returns>
		/// One of us.
		/// </returns>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if <paramref name="nto"/>
		/// is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		public static IPAFTypeHolder FromNTO(IPAFNamedAndTypedObject nto)
		{
			if (nto == null)
				throw new ArgumentNullException(nameof(nto));
			return new PAFTypeHolder(nto.ObjectType);
		}
		#endregion // Static Helpers
	}
}