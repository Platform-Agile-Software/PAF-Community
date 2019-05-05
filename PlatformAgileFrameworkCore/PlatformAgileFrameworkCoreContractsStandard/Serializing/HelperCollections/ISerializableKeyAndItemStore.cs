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
using System.Collections.Generic;
using PlatformAgileFramework.Serializing.Interfaces;

namespace PlatformAgileFramework.Serializing.HelperCollections
{
	/// <summary>
	/// Plugin delegate for checking serializability of incoming keys
	/// and items.
	/// </summary>
	/// <param name="typeToBeChecked">
	/// The type to check.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if type is serializable.
	/// </returns>
	public delegate bool SerializabilityChecker(Type typeToBeChecked);
	/// <summary>
	/// <para>
	/// This interface exposes members that allow a simple typed object store
	/// to be accessed. The intent is to allow objects to be stored in
	/// this collection that are checked at the time of installation, rather
	/// than having a serializer throw an exception when serialization
	/// is attempted.
	/// </para>
	/// <para>
	/// This interface is designed for an implementation of a collection
	/// containing elements that are serializable. The serializability check
	/// is made at run-time, not compile-time, since in .Net serializability
	/// checks are difficult to make at compile-time. The purpose of any
	/// implementation is to examine elements installed in the collection
	/// at the time the collection is loaded, before serialization takes
	/// place.
	/// </para>
	/// </summary>
	/// <typeparam name="Tkey">
	/// The key by which the item is located.
	/// </typeparam>
	/// <typeparam name="Titem">
	/// The type of the item located in the store.
	/// </typeparam>
	/// <remarks>
	/// For a serialiazability-safe Hash table, both type arguments can be
	/// <see cref="object"/>s.
	/// </remarks>
	public interface ISerializableKeyAndItemStore<Tkey, Titem>: IPAFSerializableCollection<KeyValuePair<Tkey, Titem>>
	{
		#region Properties
		/// <summary>
		/// Gets the keys of the installed items. Can be <see langword="null"/> if collection
		/// is empty.
		/// </summary>
		Tkey[] Keys { get; }
		/// <summary>
		/// A method that is designed to check items to be added to the store.
		/// A typical implementation could default to <c>TypeExtensions.IsTypeSerializable</c>,
		/// for both Key and item.
		/// The method is exposed so clients can check whether an object is serializable
		/// in a current operating environment BEFORE adding it to the store
		/// </summary>
		SerializabilityChecker ObjectIsSerializable { get; }
		#endregion // Properties
		#region Methods
		/// <summary>
		/// Installs a <typeparamref name="Titem"/> that must be serializable. This
		/// method will throw an exception if the incoming object is not serializable.
		/// This provides a bit of up-front protection that is better than letting an
		/// exception be thrown deep in the serialization system. The
		/// <typeparamref name="Tkey"/> by which the item is acessed must also be
		/// serializable.
		/// </summary>
		/// <param name="key">
		/// The <typeparamref name="Tkey"/> the item will be referenced by.
		/// </param>
		/// <param name="item">The serializable item.</param>
		/// <returns>
		/// <see langword="true"/> if item was successfully added. <see langword="false"/> if an
		/// entry with the same key was already in the dictionary. This method
		/// style is needed for multi-threaded applications.
		/// </returns>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if either argument
		/// is <see langword="null"/>.
		/// </exception>
		/// <exception cref="ArgumentException"> is thrown if <paramref name="item"/>
		/// is not serializable.
		/// </exception>
		/// </exceptions>
		bool Add(Tkey key, Titem item);
		/// <summary>
		/// Retrieves a keyed item from the store.
		/// </summary>
		/// <param name="key">The key of the item to look up.</param>
		/// <returns>
		/// Found item or "default&lt;T&gt;".
		/// </returns>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if the argument
		/// is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		Titem GetItem(Tkey key);
		/// <summary>
		/// Retrieves a keyed item from the store. Try/Get style needed for
		/// value types or in multi-threaded applications.
		/// </summary>
		/// <param name="key">The key of the item to look up.</param>
		/// <param name="item">
		/// Returned item if found or "default&lt;T&gt;".</param>
		/// <returns>
		/// <see langword="true"/> if item found.
		/// </returns>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if the argument
		/// is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		bool TryGetItem(Tkey key, out Titem item);
		#endregion // Methods
	}
}
