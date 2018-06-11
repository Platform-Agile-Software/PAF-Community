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

#endregion

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.TypeHandling.PartialClassSupport
{
	/// <summary>
	/// A few extensions that build a composite enum.
	/// </summary>
	public static class ExtendablePseudoEnumInt32Extensions
	{
		/// <summary>
		/// Bitwise "OR".
		/// </summary>
		/// <param name="me">"this"</param>
		/// <param name="otherPE">A different PE.</param>
		/// <returns>
		/// Value of the "OR" of the values. Names are concatenated with a " | ".
		/// </returns>
		// ReSharper disable once InconsistentNaming
		public static IExtendablePseudoEnumTypeType<int> BitwiseOR(this IExtendablePseudoEnumTypeType<int> me,
			IExtendablePseudoEnumTypeType<int> otherPE)
		{
			var value = me.EnumValueAsGeneric | otherPE.EnumValueAsGeneric;
			var name = me.Name + " | " + otherPE.Name;
			return new ExtendablePseudoEnum<int>(name, value);
		}
		/// <summary>
		/// Bitwise "OR" that just returns the value.
		/// </summary>
		/// <param name="me">"this"</param>
		/// <param name="otherPE">A different PE.</param>
		/// Value of the "OR" of the values.
		// ReSharper disable once InconsistentNaming
		public static int BitwiseORValue(this IExtendablePseudoEnumTypeType<int> me,
			IExtendablePseudoEnumTypeType<int> otherPE)
		{
			return me.EnumValueAsGeneric | otherPE.EnumValueAsGeneric;
		}

		/// <summary>
		/// Bitwise "AND".
		/// </summary>
		/// <param name="me">"this"</param>
		/// <param name="otherPE">A different PE.</param>
		/// Value of the "AND" of the values. Names are concatenated with a " & ".
		// ReSharper disable once InconsistentNaming
		public static IExtendablePseudoEnumTypeType<int> BitwiseAND(this IExtendablePseudoEnumTypeType<int> me,
			IExtendablePseudoEnumTypeType<int> otherPE)
		{
			var value = me.EnumValueAsGeneric & otherPE.EnumValueAsGeneric;
			var name = me.Name + " & " + otherPE.Name;
			return new ExtendablePseudoEnum<int>(name, value);
		}
		/// <summary>
		/// Bitwise "AND" that just returns the value.
		/// </summary>
		/// <param name="me">"this"</param>
		/// <param name="otherPE">A different PE.</param>
		/// Value of the "AND" of the values.
		// ReSharper disable once InconsistentNaming
		public static int BitwiseANDValue(this IExtendablePseudoEnumTypeType<int> me,
			IExtendablePseudoEnumTypeType<int> otherPE)
		{
			return me.EnumValueAsGeneric & otherPE.EnumValueAsGeneric;
		}
	}
}
