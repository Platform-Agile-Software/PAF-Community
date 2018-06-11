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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

#region Using Directives
using System;
using System.Collections.Generic;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
using PlatformAgileFramework.TypeHandling.Exceptions;
// Exception shorthand.
using PAFTED = PlatformAgileFramework.ErrorAndException.CoreCustomExceptions.PAFTypeExceptionData;
#endregion

namespace PlatformAgileFramework.TypeHandling.PartialClassSupport
{
    /// <summary>
    /// Contains the value of a Pseudoenum - name and int value. Struct that carries just the info
    /// so we can fool .Net into thinking we are not overriding base class or interface conversion
    /// operations....
    /// </summary>
    /// <typeparam name="T"> This must be a <see cref="byte"/>, <see cref="short"/>,
    /// <see cref="int"/> or <see cref="long"/>.
    /// </typeparam>
    /// <history>
    /// <contribution>
    /// <author> KRM </author>
    /// <date> 22jan2012 </date>
    /// <description>
    /// Added history and "TOODO" note.
    /// </description>
    /// </contribution>
    /// </history>
	public struct PEValue<T> where T : struct
	{
		#region Class Fields And Autoproperties
		/// <summary>
		/// Gets the integer value for the enum.
		/// </summary>
		public T Value { get; }
		/// <summary>
		/// Gets the string name for the enum.
		/// </summary>
		public string Name { get; }
		#endregion // Class Fields And Autoproperties
		#region Constructors

	    static PEValue()
	    {
		    if ((typeof(T) == typeof(byte)) || (typeof(T) == typeof(short)) || (typeof(T) == typeof(int)) ||
	            (typeof(T) == typeof(long))) return;
	        var data = new PAFTED(typeof(T));
	        throw new PAFStandardException<PAFTED>
	            (data, PAFTypeMismatchExceptionMessageTags.GENERIC_MUST_BE_SIGNED_INTEGER_TYPE);
	    }
		/// <summary>
		/// Builds with a string representation of the int value.
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
		public PEValue(string name, T value)
			: this()
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			Name = name;
			Value = value;
		}

		/// <summary>
		/// Builds from the interface.
		/// </summary>
		/// <param name="pseudoEnumInterface">
		/// Interface carrying the data for a PE.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="pseudoEnumInterface"/>
		/// cannot be <see langword="null"/> or blank.
		/// </exception>
		/// </exceptions>
		public PEValue(IExtendablePseudoEnumTypeType<T> pseudoEnumInterface )
			: this()
		{
			if (pseudoEnumInterface == null)
				throw new ArgumentNullException("pseudoEnumInterface");
			Name = pseudoEnumInterface.Name;
			Value = pseudoEnumInterface.EnumValueAsGeneric;
		}
		#endregion // Constructors
		#region Conversion Operators
		/// <summary>
		/// Just pulls the name out of the <see cref="PEValue{T}"/>.
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
		public static implicit operator string(PEValue<T> pseudoEnumValue)
		{
			return pseudoEnumValue.Name;
		}
		/// <summary>
		/// Just pulls the value out of the <see cref="PEValue{T}"/>.
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
		public static implicit operator T(PEValue<T> pseudoEnumValue)
		{
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
		public static implicit operator KeyValuePair<string, object>(PEValue<T> pseudoEnumValue)
		{
			return new KeyValuePair<string, object>(pseudoEnumValue.Name, pseudoEnumValue.Value);
		}
		#endregion // Conversion Operators
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
	}
}
