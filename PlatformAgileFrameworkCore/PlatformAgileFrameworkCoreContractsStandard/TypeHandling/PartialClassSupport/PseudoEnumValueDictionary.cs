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


#region Using Directives

using System;
using System.Collections.Generic;
using PlatformAgileFramework.FrameworkServices;

#endregion // Using Directives

namespace PlatformAgileFramework.TypeHandling.PartialClassSupport
{
	/// <summary>
	/// This is an implementation of <see cref="IPseudoEnumValueDictionary"/>. This
	/// is designed to reside somewhere as a singleton to catalog all PE's in an appdomain.
	/// </summary>
	/// <threadsafety>
	/// This class is NOT thread-safe. It is designed to be deployed as as a private
	/// member with the hosting class employing a limited set of access methods that
	/// are synchronized.
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 20aug2015 </date>
	/// <description>
	/// Basically New. Bagged the old-style dictionary entirely. This was written for the
	/// fix of the pseudoenum system.
	/// </description>
	/// </contribution>
	/// </history>
// ReSharper disable once PartialTypeWithSinglePart
// core
	// ReSharper disable once InconsistentNaming
	public partial class PseudoEnumValueDictionary
		: Dictionary<IExtendablePseudoEnumTypeType, IDictionary<IExtendablePseudoEnumTypeType, object>>,
		IPseudoEnumValueDictionary
	{
		#region Constructors
		/// <summary>
		/// Constructor installs our <see cref="ExtendablePseudoEnumTypeComparer"/> to sort our
		/// inner dictionaries.
		/// </summary>
		public PseudoEnumValueDictionary()
			: base(new ExtendablePseudoEnumTypeComparer()) { } 
		#endregion // Constructors
		#region IPAFEnumValueDictionary Implementation
		#region Methods
		/// <summary>
		/// Just returns a standard dictionary. It is, however, specialized
		/// by the installation of our custom comparer, which compares items
		/// by string name.
		/// </summary>
		/// <returns>
		/// Empty dictionary.
		/// </returns>
		private IDictionary<IExtendablePseudoEnumTypeType, object>
			NewInnerDictionary()
		{
			return new Dictionary<IExtendablePseudoEnumTypeType, object>
				(new PseudoEnumValueComparer());
		}
		/// <remarks>
		/// See <see cref="IPseudoEnumValueDictionary"/>
		/// </remarks>
		public virtual void AddPseudoEnum(IExtendablePseudoEnumTypeType pseudoEnumValue)
		{
			IDictionary<IExtendablePseudoEnumTypeType, object> innerDictionary;
			if (ContainsKey(pseudoEnumValue))
			{
				innerDictionary = this[pseudoEnumValue];
			}
			else
			{
				innerDictionary = NewInnerDictionary();
				Add(pseudoEnumValue, innerDictionary);
			}
			// Recall that PE is it's own key.
			innerDictionary.Add(pseudoEnumValue, pseudoEnumValue.EnumValueAsObject);
		}
		#endregion // Methods
		#endregion // IPAFEnumValueDictionary Implementation
	}
}
