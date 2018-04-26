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
using PlatformAgileFramework.Manufacturing;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.TypeHandling
{
	/// <summary>
	/// <para>
	///	Simple container to hold just the props for a "PAFTypeHolder".
	/// </para>
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 30nov2012 </date>
	/// <description>
	/// New.
	/// Needed just a small container to export in the contracts assy.
	/// </description>
	/// </contribution>
	/// </history>
	[PAFSerializable]
	public class PAFTypeProps: IPAFTypeProps
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal string m_AssemblyQualifiedTypeName;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal Type m_TypeType;
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Builds with a string representation of a type or with an actual
		/// <see cref="Type"/>, if available.
		/// The format is:
		/// <c>NamespaceQualifiedTypeName, SimpleAssemblyName {,Culture = CultureInfo} {,Version = Major.Minor.Build.Revision} {,StrongName} {,PublicKeyToken}</c>,
		/// where braces indicate optional fields.
		/// </summary>
		/// <param name="assemblyQualifiedTypeName">
		/// String to parse and build from.
		/// </param>
		/// <param name="typeType">
		/// Type of the type, if available.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if
		/// <paramref name="assemblyQualifiedTypeName"/>
		/// is <see langword="null"/> or blank and <paramref name="typeType"/>
		/// is <see langword = "null"/>. 
		/// </exception>
		/// <exception cref="ArgumentException"> is thrown if
		/// <paramref name="assemblyQualifiedTypeName"/>
		/// is malformed and <paramref name="typeType"/>
		/// is <see langword = "null"/>. 
		/// </exception>
		/// </exceptions>
		public PAFTypeProps(string assemblyQualifiedTypeName,
			Type typeType = null)
		{
			// Early bound solves everything!
			if (typeType != null) {
				assemblyQualifiedTypeName = typeType.AssemblyQualifiedName;
			}
			else if(string.IsNullOrEmpty(assemblyQualifiedTypeName))
			{
				throw new ArgumentNullException("assemblyQualifiedTypeName");
			}

			m_AssemblyQualifiedTypeName = assemblyQualifiedTypeName;

			m_TypeType = typeType;
		}
		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="typeProps">
		/// Instance of us.
		/// </param>
		/// <remarks>
		/// Simple implementation just causes a reparse of the string. If
		/// the old one was good, the new one will be, too.
		/// </remarks>
		protected PAFTypeProps(IPAFTypeProps typeProps)
			: this(typeProps.AssemblyQualifiedTypeName, typeProps.TypeType){}
		#endregion Constructors
		#region Properties
		/// <summary>
		/// Holds the full type name.
		/// </summary>
		public string AssemblyQualifiedTypeName
		{ 
			get { return m_AssemblyQualifiedTypeName; }
			protected internal set
			{
				m_AssemblyQualifiedTypeName = value;
			}
		}
		/// <summary>
		/// Holds the type, if available. Will return <see langword="null"/> if the type
		/// is not local or has not been resolved.
		/// </summary>
		public virtual Type TypeType
		{
			get { return m_TypeType; }
			set
			{
				if (value != null) {
					AssemblyQualifiedTypeName = value.AssemblyQualifiedName;
				}
				m_TypeType = value;
			}
		}
		#endregion // Properties
		#region Static Helpers
		/// <summary>
		/// Tells if the holder is <see langword="null"/> or has no content.
		/// </summary>
		/// <param name="typeProps">
		/// <see cref="PAFTypeProps"/> to check.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if invalid.
		/// </returns>
		public static bool IsNullOrInvalid(PAFTypeProps typeProps)
		{
			if (typeProps == null) return true;
			if ((typeProps.AssemblyQualifiedTypeName == null) && typeProps.TypeType == null)
				return true;
			return false;
		}
		#endregion // Static Helpers
		#region Methods
		#region Conversion Operators
		/// <summary>
		/// Calls <c>PAFTypeProps(type.AssemblyQualifiedName)</c>.
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
		public static implicit operator PAFTypeProps(Type type)
		{
			if (type == null)
				throw new ArgumentNullException("type");
			return new PAFTypeProps(type.AssemblyQualifiedName);
		}
		#endregion // Conversion Operators
		#region Obligatory Patch for Equals and Hash Code
		/// <summary>
		/// Determines whether the specified <see cref="Object"/> is equal to the
		/// current <see cref="Object"/>.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if the specified <see cref="Object"/> is equal to the current
		/// <see cref="Object"/>; otherwise, false.
		/// </returns>
		/// <param name="obj">
		/// The <see cref="Object"/> to compare with the current <see cref="Object"/>.
		/// </param>
		/// <remarks>
		/// Patch for Microsoft's mistake.
		/// </remarks>
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			// ReSharper disable BaseObjectEqualsIsObjectEquals
			return GetType() == obj.GetType() && base.Equals(obj);
			// ReSharper restore BaseObjectEqualsIsObjectEquals
		}
		/// <summary>
		/// We are a reference type so just call base to shut up the compiler/tools.
		/// </summary>
		/// <returns>
		/// The original hash code.
		/// </returns>
		public override int GetHashCode()
		{
			// ReSharper disable BaseObjectGetHashCodeCallInGetHashCode
			return base.GetHashCode();
			// ReSharper restore BaseObjectGetHashCodeCallInGetHashCode
		}
		#endregion // Obligatory Patch for Equals and Hash Code
		#endregion // Methods
	}
}