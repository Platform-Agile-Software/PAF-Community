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
using System.Collections.Generic;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.Collections.Enumerators
{
	/// <summary>
	///	Implementation of <see cref="IPAFSealableEnumerable{T}"/>.
	/// </summary>
	/// <typeparam name="T">
	/// <see cref="IPAFSealableEnumerable{T}"/>.
	/// </typeparam>
	/// <history>
	/// <author> KRM </author>
	/// <date> 24jun2013 </date>
	/// <contribution>
	/// Built the implementation.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Unsafe - designed to be loaded by one accessing thread at a time.
	/// </threadsafety>
	[PAFSerializable]
	public class PAFSealableEnumerable<T>
		: IPAFSealableEnumerable<T> where T: class
	{
		#region Fields and Autoproperties
		/// <summary>
		/// The items.
		/// </summary>
		protected internal List<T> m_Items;
		/// <summary>
		/// Capability to seal list after receiving a <see langword="null"/> entry.
		/// </summary>
		protected internal bool m_ListIsSealed;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor builds and optionally loads with items.
		/// </summary>
		/// <param name="items">
		/// Loads <see cref="Items"/>. May be <see langword="null"/>. If this enumeration
		/// is not <see langword="null"/> and terminates with a <see langword="null"/>
		/// item, no more items may be added with the <see cref="AddItem"/> method.
		/// The terminating <see langword="null"/> is not added to the list.
		/// </param>
		public PAFSealableEnumerable(IEnumerable<T> items = null)
		{
			m_Items = items != null ? new List<T>(items) : new List<T>();

			if ((m_Items.Count <= 0) || (m_Items[m_Items.Count - 1] != null)) return;

			// Remove the last entry and seal the list.
			m_ListIsSealed = true;
			m_Items.RemoveAt(m_Items.Count - 1);
		}

		#endregion Constructors
		#region Properties
		/// <summary>
		/// Gets the items.
		/// </summary>
		public IEnumerable<T> Items
		{
			get { return m_Items; }
		}
		#endregion // Properties
		#region  Methods
		/// <summary>
		/// This method adds an item to the list of items if the list is not
		/// sealed. If the item passed to this method is <see langword="null"/>,
		/// this seals the list. The <see langword="null"/> item is not
		/// added to the list.
		/// </summary>
		/// <param name="item">Item to be added.</param>
		public void AddItem(T item)
		{
			if(m_ListIsSealed) throw new InvalidOperationException("SealableEnumerable List is sealed");
			m_Items.Add(item);
			if (item == null)
			{
				m_ListIsSealed = true;
			}
			m_Items.Add(item);
		}
		#endregion // Methods
	}
}