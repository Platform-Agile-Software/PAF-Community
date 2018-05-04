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


namespace PlatformAgileFramework.TypeHandling.PartialClassSupport
{
	/// <summary>
	/// This class holds a non-Generic dictionary of every instance of every
	/// enum type in the system. Basically the same sort of system as in MS's
	/// implementation of the built-in enum system.
	/// </summary>
	/// <threadsafety>
	/// Uses locks for the dictionary - safe.
	/// </threadsafety>
	// ReSharper disable once PartialTypeWithSinglePart
	public abstract partial class ExtendablePseudoEnumNonGenericBase
	{
		#region Class Fields And Autoproperties
		/// <summary>
		/// This holds the static pseuduenum values.
		/// </summary>
		internal static readonly IPseudoEnumValueDictionary s_PseudoEnumValueDictionary; 
		#endregion // Class Fields And Autoproperties
		#region Constructors
		/// <summary>
		/// Static constructor just builds the dictionary.
		/// </summary>
		static ExtendablePseudoEnumNonGenericBase()
		{
			s_PseudoEnumValueDictionary = new PseudoEnumValueDictionary();
		}
		#endregion // Constructors
		#region Methods

		/// <remarks>
		/// See <see cref="IPseudoEnumValueDictionary"/>
		/// </remarks>
		protected internal static void AddPseudoEnum(IExtendablePseudoEnumTypeType pseudoEnumValue)
		{
			lock (s_PseudoEnumValueDictionary)
			{
				s_PseudoEnumValueDictionary.AddPseudoEnum(pseudoEnumValue);
			}
		}
		#endregion // Methods
	}
}
