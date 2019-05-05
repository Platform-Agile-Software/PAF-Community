//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2014 Icucom Corporation
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

#region Using Directives
using System;
using System.Collections.Generic;

#endregion

namespace PlatformAgileFramework.TypeHandling.PartialClassSupport
{
	/// <summary>
	/// <para>
	/// This class acts as a sort of enum that is extensible. The class must
	/// be closed with a CLS-compliant integer or a run-time exception will
	/// be thrown upon load. Unfortunately, .Net does not allow more
	/// comprehensive Generic constraints so a runtime check is necessary.
	/// </para>
	/// <para>
	/// In this class, multiple names can map to the same integer value. Names must
	/// obviously be unique, however.
	/// </para>
	/// <para>
	/// This is a class, so we can have a nullable enum naturally, without
	/// introducing special values.
	/// </para>
	/// <para>
	/// The class design involves the use of a nested <see langword="struct"/>
	/// representing the actual enum value, which is a string/int pair.
	/// </para>
	/// </summary>
	/// <typeparam name="T"> This must be a <see cref="byte"/>, <see cref="short"/>,
	/// <see cref="int"/> or <see cref="long"/>.
	/// </typeparam>
	/// <remarks>
	/// Interface implementation is explicit to hide some gory details
	/// from the casual user.
	/// </remarks>
	public class ExtendablePseudoEnum<T> :
		ExtendablePseudoEnumNonGenericBase,
		IExtendablePseudoEnumTypeType<T> where T : struct
	{
		#region Class Fields And Autoproperties
		/// <summary>
		/// Gets the integer value for the enum.
		/// </summary>
		public T Value { get; private set; }
		/// <summary>
		/// Gets the string name for the enum.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Saves a little bit of typing to get ourselves as the interface.
		/// </summary>
		protected internal IExtendablePseudoEnumTypeType<T> AsPEInterface { get; set; }

		#endregion // Class Fields And Autoproperties
		#region Constructors
		/// <summary>
		/// This public constructor builds the type just to act as a
		/// <see cref="IExtendablePseudoEnumTypeType{T}"/>, without
		/// installing it into out internal global catalog of enums. 
		/// </summary>
		/// <param name="name">
		/// The name of the enum value.
		/// </param>
		/// <param name="value">
		/// Integer value of the enum.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="name"/> cannot be <see langword="null"/>
		/// or blank.
		/// </exception>
		/// </exceptions>
		public ExtendablePseudoEnum(string name, T value)
				: this(name, value, false)
		{
		}

		/// <summary>
		/// Builds with a string representation of the int value. Internal
		/// so not every Tom, Dick or Harry can construct it.
		/// </summary>
		/// <param name="name">
		/// The name of the enum value.
		/// </param>
		/// <param name="value">
		/// Integer value of the enum.
		/// </param>
		/// <param name="addToDictionary">
		/// Set to <see langword="true"/> if this is a "singular"  enum, as 
		/// is defined as static on top-level enum classes.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="name"/> cannot be <see langword="null"/>
		/// or blank.
		/// </exception>
		/// </exceptions>
		protected internal ExtendablePseudoEnum(string name, T value, bool addToDictionary)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			Name = name;
			Value = value;
			if(addToDictionary)
				AddPseudoEnum(this);
			AsPEInterface = this;
		}
		/// <summary>
		/// Static constructor just checks to see if we have closed this with a valid
		/// integer type.
		/// </summary>
		/// <exceptions>
		/// <exception cref="ArgumentOutOfRangeException">
		/// "Not a valid CLS-compliant integer".
		/// </exception>
		/// </exceptions>
		static ExtendablePseudoEnum()
		{
			var typeofT = typeof(T);
			if (
				(typeofT != typeof(long))
				&&
				(typeofT != typeof(int))
				&&
				(typeofT != typeof(short))
				&&
				(typeofT != typeof(byte))
				)
			{
				throw new ArgumentOutOfRangeException(string.Format("{0}T", "ARG0"),
					"Not a valid CLS-compliant integer");
			}
		}
		#endregion // Constructors
		#region IExtendablePseudoEnumTypeType Implementation
		/// <remarks/>
		Type IExtendablePseudoEnumTypeType.EnumIntegerType
		{ get { return typeof(T); } }
		/// <remarks/>
		T IExtendablePseudoEnumTypeType<T>.EnumValueAsGeneric
		{ get { return Value; } }
		/// <remarks/>
		object IExtendablePseudoEnumTypeType.EnumValueAsObject
		{ get { return Value; } }
		/// <remarks/>
		Type IExtendablePseudoEnumTypeType.EnumType
		{ get { return GetType(); } }

		#endregion // IExtendablePseudoEnumTypeType Implementation
		#region Conversion Operators
		/// <summary>
		/// Just pulls the name out of the enum.
		/// </summary>
		/// <param name="pseudoEnumValue">
		/// The value of the enum. Not <see langword="null"/>.
		/// </param>
		/// <returns>
		/// One of us.
		/// </returns>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if
		/// <paramref name="pseudoEnumValue"/> is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		public static implicit operator string(ExtendablePseudoEnum<T> pseudoEnumValue)
		{
			if(pseudoEnumValue == null)
				throw new ArgumentNullException("pseudoEnumValue");
			return pseudoEnumValue.Name;
		}
		/// <summary>
		/// Just pulls the value out of the <see cref="ExtendablePseudoEnum{T}"/>.
		/// </summary>
		/// <param name="pseudoEnumValue">
		/// The value of the enum. Not <see langword="null"/>.
		/// </param>
		/// <returns>
		/// One of us.
		/// </returns>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if
		/// <paramref name="pseudoEnumValue"/> is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		public static implicit operator T(ExtendablePseudoEnum<T> pseudoEnumValue)
		{
			if (pseudoEnumValue == null)
				throw new ArgumentNullException("pseudoEnumValue");
			return pseudoEnumValue.Value;
		}

		/// <summary>
		/// Creates a key/value pair for the dictionary.
		/// </summary>
		/// <param name="pseudoEnumValue">
		/// The value of the enum. Name not <see langword="null"/>.
		/// </param>
		/// <returns>
		/// One of us.
		/// </returns>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if
		/// <paramref name="pseudoEnumValue"/> is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		public static implicit operator KeyValuePair<string, object>(ExtendablePseudoEnum<T> pseudoEnumValue)
		{
			if (pseudoEnumValue == null)
				throw new ArgumentNullException("pseudoEnumValue");
			return new KeyValuePair<string, object>(pseudoEnumValue.Name, pseudoEnumValue.Value);
		}
		#endregion // Conversion Operators
		#region Overrides for Equals, ToString and Hash Code
		/// <summary>
		/// Determines whether the specified <see cref="object"/> is equal to the
		/// current <see cref="object"/>.
		/// </summary>
		/// <param name="obj">
		/// The <see cref="object"/> to compare with the current
		/// <see cref="ExtendablePseudoEnum{T}"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if <paramref name="obj"/> is
		/// a <see cref="ExtendablePseudoEnum{T}"/> and has
		/// a name and EnumType equal to this <see cref="ExtendablePseudoEnum{T}"/>.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals (this, obj))
				return true;
			if (!(obj is ExtendablePseudoEnum<T>)) return false;
			var pev = (ExtendablePseudoEnum<T>)obj;
			if (string.CompareOrdinal(Name, pev.Name) != 0) return false;
			if (pev.AsPEInterface.EnumType != AsPEInterface.EnumType) return false;
			return true;
		}
		/// <summary>
		/// Let to ToString() return something meaningful.
		/// </summary>
		/// <returns>
		/// The Enum name.
		/// </returns>
		public override string ToString()
		{
			return Name;
		}
		/// <summary>
		/// Must be here to shut up the compiler/resharper.
		/// </summary>
		/// <returns>
		/// The hash code.
		/// </returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		#endregion // Overrides for Equals, ToString and Hash Code
		#region Bitwise Operators
		/// <summary>
		/// Bitwise "OR" operator.
		/// </summary>
		/// <param name="pe1">The <see cref="ExtendablePseudoEnum{T}"/>.</param>
		/// <param name="pe2">The <see cref="ExtendablePseudoEnum{T}"/>.</param>
		/// <returns>
		/// OR'd value of arguments.
		/// </returns>
		public static T operator |(ExtendablePseudoEnum<T> pe1, ExtendablePseudoEnum<T> pe2)
		{
			if (typeof(T) == typeof(byte))
			{
				var pevValue1 = (byte)(object)pe1.Value;
				var pevValue2 = (byte)(object)pe2.Value;
				return (T)(object)(pevValue1 | pevValue2);
			}
			if (typeof(T) == typeof(short))
			{
				var pevValue1 = (short)(object)pe1.Value;
				var pevValue2 = (short)(object)pe2.Value;
				return (T)(object)(pevValue1 | pevValue2);
			}
			if (typeof(T) == typeof(int))
			{
				var pevValue1 = (int)(object)pe1.Value;
				var pevValue2 = (int)(object)pe2.Value;
				return (T)(object)(pevValue1 | pevValue2);
			}
			if (typeof(T) == typeof(int))
			{
				var pevValue1 = (int)(object)pe1.Value;
				var pevValue2 = (int)(object)pe2.Value;
				return (T)(object)(pevValue1 | pevValue2);
			}
			return default(T);
		}
		#endregion // Bitwise Operators

		/// <summary>
		/// Equality op calls PEsEqual
		/// </summary>
		/// <param name="pev1">The <see cref="ExtendablePseudoEnum{T}"/>.</param>
		/// <param name="pev2">The <see cref="ExtendablePseudoEnum{T}"/>.</param>
		/// <returns>
		/// <see langword="true"/> if PEs are equal.
		/// </returns>
		public static bool operator ==(ExtendablePseudoEnum<T> pev1,
			ExtendablePseudoEnum<T> pev2)
		{
			return Equals(pev1, pev2);
		}
		/// <summary>
		/// Obligatory InEquality op calls PEsEqual - lightweight.
		/// </summary>
		/// <param name="pev1">The <see cref="ExtendablePseudoEnum{T}"/>.</param>
		/// <param name="pev2">The <see cref="ExtendablePseudoEnum{T}"/>.</param>
		/// <returns>
		/// <see langword="true"/> if PEs are NOT equal.
		/// </returns>
		public static bool operator !=(ExtendablePseudoEnum<T> pev1,
			ExtendablePseudoEnum<T> pev2)
		{
			return !Equals(pev1, pev2);
		}

	}
}
