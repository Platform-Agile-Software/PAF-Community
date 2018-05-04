using System;
using System.Collections;
using System.Collections.Generic;
using PlatformAgileFramework.Collections;

namespace PlatformAgileFramework.Serializing.ECMAReplacements
{
	/// <summary>
	/// An interface to enable carrying of serialized data. This interface has the
	/// same exposed methods as the classical "System.Runtime.Serialization.SerializationInfo"
	/// in order to make it easy to convert legacy code. Implementations can hold primitive
	/// data types that can be collected by a classical CLR-style procedure whereby all
	/// fields, public and non-public can be scooped up through reflection or by a
	/// data-contract style serialization procedure applied to public members.
	/// The reflection on non-public fields can obviously only be performed within
	/// elevated-trust environments, or by a surrgate assembly having internal
	/// access.
	/// </summary>
	/// <threadsafety>
	/// Implementations are not expected to be thread-safe. Not intended for multi-thread access.
	/// </threadsafety>
	/// <remarks>
	/// <see cref="IEnumerable"/> has been added to the implementation requirement to enumerate over
	/// <see cref="IPAFSerializationEntry"/>'s
	/// </remarks>
	public interface IPAFSerializationInfoCLS: IEnumerable<IPAFSerializationEntry>
	{
		#region Properties
		/// <summary>
		/// Manipulates name of assembly. Does not allow a null to be set.
		/// </summary>
		/// <remarks>Legacy method.</remarks>
		/// <exception>
		/// <see cref="ArgumentNullException"/> thrown if input is <see langword="null"/>.
		/// "value"
		/// </exception>
		string AssemblyName { get; set; }

		/// <summary>
		/// Manipulates the full type name. Does not allow a null to be set.
		/// </summary>
		/// <remarks>Legacy method.</remarks>
		/// <exception>
		/// <see cref="ArgumentNullException"/> thrown if input is <see langword="null"/>.
		/// "value"
		/// </exception>
		string FullTypeName { get; set; }

		/// <summary>
		/// Fetches the number of members that have been serialized for the type.
		/// </summary>
		/// <remarks>Legacy method.</remarks>
		int MemberCount { get; }
		#endregion // Properties
		#region Methods
		#region Collection Manipulation Methods
		/// <summary>
		/// Adds an <typeparamref name="T"/> to the serialization info.
		/// </summary>
		/// <typeparam name="T">The type of the value to be stored.</typeparam>
		/// <param name="name">
		/// Name to be stored under. Normally the name of the serialized member.
		/// </param>
		/// <param name="value">Value of the <typeparamref name="T"/>.</param>
		/// <remarks>
		/// Novel method.
		/// The incoming <paramref name="value"/> can be <see langword="null"/> for
		/// reference type.
		/// </remarks>
		void AddValue<T>(string name, T value);

		/// <summary>
		/// Adds an <see cref="Object"/> to the serialization info.
		/// </summary>
		/// <param name="name">
		/// Name to be stored under. Normally the name of the serialized member.
		/// </param>
		/// <param name="value">Value of the member.</param>
		/// <remarks>
		/// Legacy method.
		/// If the incoming <paramref name="value"/> is <see langword="null"/>, the
		/// type recorded is "typeof(Object)". If the object is not <see langword="null"/>
		/// the type of the object is stored. Use <see cref="AddValue(String, Object, Type)"/>
		/// to store type info.
		/// </remarks>
		void AddValue(string name, object value);
		/// <summary>
		/// Main method to add an entry to the serialized element collection. This
		/// method may be used to construct a "slot" in the serialization info array
		/// to hold a typed object which will be later constructed or to signal a
		/// missing member which could be set to a default upon deserialization.
		/// </summary>
		/// <param name="name">Name of the element.</param>
		/// <param name="value">The value of the object. May be <see langword="null"/>.</param>
		/// <param name="type">The type of the object.</param>
		/// <exceptions>
		/// <exception>
		/// <see cref="ArgumentNullException"/> thrown if <paramref name="name"/> is <see langword="null"/>.
		/// "name"
		/// </exception>
		/// <exception>
		/// <see cref="ArgumentNullException"/> thrown if <paramref name="type"/> is <see langword="null"/>.
		/// "type"
		/// </exception>
		/// </exceptions>
		/// <remarks>Legacy method.</remarks>
		void AddValue(string name, object value, Type type);
		/// <summary>
		/// Fetches an entry from the serialization store by name, with optional
		/// exception service.
		/// </summary>
		/// <param name="name">
		/// Name of the entry.
		/// </param>
		/// <param name="throwException">
		/// <see langword="true"/> if an exception is to be thrown if the entry is not found.
		/// </param>
		/// <returns>
		/// The entry, or <see langword="null"/>.
		/// </returns>
		/// <remarks>
		/// Novel method.
		/// </remarks>
		IPAFSerializationEntry GetEntry(string name, bool throwException);
		/// <summary>
		/// This is the main method for fetching and converting objects using
		/// the type converters which may be accessible within the
		/// <see cref="IPAFFormatterConverterCLS.Convert(Object, Type)"/> method.
		/// </summary>
		/// <param name="name">The name of the serialized entry.</param>
		/// <param name="type">
		/// The OUTPUT type we desire. This may be different than the internal
		/// type of the object and conversion may have to be performed. If
		/// the type of the stored object is assignable to the the DESIRED type,
		/// the object is returned directly.
		/// </param>
		/// <returns>An object of the specified type.</returns>
		/// <remarks>
		/// Legacy method.
		/// </remarks>
		/// <exceptions>
		/// Exceptions galore can occur in this method. We don't attempt to catch
		/// or wrap them, since it is desired to reproduce original behavior.
		/// </exceptions>
		object GetValue(string name, Type type);
		/// <summary>
		/// Generic version.
		/// </summary>
		/// <param name="name">
		/// See <see cref="GetValue(String, Type)"/>.
		/// </param>
		/// <returns>
		/// "default(T)" ifs entry not found.
		/// </returns>
		/// <remarks>
		/// Novel method.
		/// </remarks>
		T GetValueNoThrow<T>(string name);
		/// <summary>
		/// See <see cref="GetValue(String, Type)"/>.
		/// The only difference between this method and that is that this method
		/// returns a <see langword="null"/> when an entry is not found.
		/// </summary>
		/// <param name="name">
		/// See <see cref="GetValue(String, Type)"/>.
		/// </param>
		/// <param name="type">
		/// See <see cref="GetValue(String, Type)"/>.
		/// </param>
		/// <returns>
		/// See <see cref="GetValue(String, Type)"/>.
		/// </returns>
		/// <remarks>
		/// Legacy internal method. We have exposed this method publicly, since it provides
		/// useful functionality.
		/// </remarks>
		object GetValueNoThrow(string name, Type type);
		/// <summary>
		/// Resets the serialized type.
		/// </summary>
		/// <param name="type">The new type.</param>
		/// <remarks>Legacy method.</remarks>
		void SetType(Type type);
		/// <summary>
		/// This is the try-get style for value types.
		/// </summary>
		/// <typeparam name="T">The type of the value to be stored.</typeparam>
		/// <param name="name">The name of the serialized entry.</param>
		/// <param name="value">
		/// The located item or "default(T)".
		/// </param>
		/// <returns><see langword="true"/> if the item was found.</returns>
		/// <remarks>
		/// Novel method.
		/// </remarks>
		/// <exceptions>
		/// No exceptions will occur.
		/// </exceptions>
		bool TryGetValue<T>(string name, out T value);
		#endregion // Collection Manipulation Methods
		#region Type-safe Entry Addition Methods
		/// <summary>
		/// Adds a boolean to the serialization info
		/// </summary>
		/// <param name="name">
		/// Name to be stored under. Normally the name of the serialized member.
		/// </param>
		/// <param name="value">Value of the member.</param>
		/// <remarks>Legacy method.</remarks>
		void AddValue(string name, bool value);

		/// <summary>
		/// Adds a byte to the serialization info
		/// </summary>
		/// <param name="name">
		/// Name to be stored under. Normally the name of the serialized member.
		/// </param>
		/// <param name="value">Value of the member.</param>
		void AddValue(string name, byte value);

		/// <summary>
		/// Adds a Char to the serialization info.
		/// </summary>
		/// <param name="name">
		/// Name to be stored under. Normally the name of the serialized member.
		/// </param>
		/// <param name="value">Value of the member.</param>
		/// <remarks>Legacy method.</remarks>
		void AddValue(string name, char value);

		/// <summary>
		/// Adds a DateTime to the serialization info.
		/// </summary>
		/// <param name="name">
		/// Name to be stored under. Normally the name of the serialized member.
		/// </param>
		/// <param name="value">Value of the member.</param>
		void AddValue(string name, DateTime value);

		/// <summary>
		/// Adds a decimal to the serialization info.
		/// </summary>
		/// <param name="name">
		/// Name to be stored under. Normally the name of the serialized member.
		/// </param>
		/// <param name="value">Value of the member.</param>
		/// <remarks>Legacy method.</remarks>
		void AddValue(string name, decimal value);

		/// <summary>
		/// Adds a double-precision floating point value to the serialization info.
		/// </summary>
		/// <param name="name">
		/// Name to be stored under. Normally the name of the serialized member.
		/// </param>
		/// <param name="value">Value of the member.</param>
		/// <remarks>Legacy method.</remarks>
		void AddValue(string name, double value);

		/// <summary>
		/// Adds a 16-bit integer to the serialization info.
		/// </summary>
		/// <param name="name">
		/// Name to be stored under. Normally the name of the serialized member.
		/// </param>
		/// <param name="value">Value of the member.</param>
		/// <remarks>Legacy method.</remarks>
		void AddValue(string name, short value);
		/// <summary>
		/// Adds a 32-bit integer to the serialization info.
		/// </summary>
		/// <param name="name">
		/// Name to be stored under. Normally the name of the serialized member.
		/// </param>
		/// <param name="value">Value of the member.</param>
		/// <remarks>Legacy method.</remarks>
		void AddValue(string name, int value);
		/// <summary>
		/// Adds a 64-bit integer to the serialization info.
		/// </summary>
		/// <param name="name">
		/// Name to be stored under. Normally the name of the serialized member.
		/// </param>
		/// <param name="value">Value of the member.</param>
		void AddValue(string name, long value);
		/// <summary>
		/// Adds a single-precision floating-point value to the serialization info.
		/// </summary>
		/// <param name="name">
		/// Name to be stored under. Normally the name of the serialized member.
		/// </param>
		/// <param name="value">Value of the member.</param>
		/// <remarks>Legacy method.</remarks>
		void AddValue(string name, float value);
		#endregion // Type-safe Entry Addition Methods

		#region Type-safe Entry Fetch Methods
		/// <summary>
		/// Retrives a boolean value from an entry. If the entry cannot be found
		/// or cannot be converted, an exception is thrown.
		/// </summary>
		/// <param name="name">
		/// Name of the entry.
		/// </param>
		/// <returns>The located and converted boolean.</returns>
		/// <remarks>
		/// The type of the <see cref="IPAFNamedAndTypedObject.ObjectValue"/> and
		/// the associated stored <see cref="IPAFNamedAndTypedObject.ObjectType"/>
		/// must match or a cast exception will occur - Legacy method.
		/// </remarks>
		/// <remarks>Legacy method.</remarks>
		bool GetBoolean(string name);
		/// <summary>
		/// Retrives a byte value from an entry. If the entry cannot be found
		/// or cannot be converted, an exception is thrown.
		/// </summary>
		/// <param name="name">
		/// Name of the entry.
		/// </param>
		/// <returns>The located and converted byte.</returns>
		/// <remarks>
		/// The type of the <see cref="IPAFNamedAndTypedObject.ObjectValue"/> and
		/// the associated stored <see cref="IPAFNamedAndTypedObject.ObjectType"/>
		/// must match or a cast exception will occur - Legacy method.
		/// </remarks>
		byte GetByte(string name);
		/// <summary>
		/// Retrives a char value from an entry. If the entry cannot be found
		/// or cannot be converted, an exception is thrown.
		/// </summary>
		/// <param name="name">
		/// Name of the entry.
		/// </param>
		/// <returns>The located and converted char.</returns>
		/// <remarks>
		/// The type of the <see cref="IPAFNamedAndTypedObject.ObjectValue"/> and
		/// the associated stored <see cref="IPAFNamedAndTypedObject.ObjectType"/>
		/// must match or a cast exception will occur - Legacy method.
		/// </remarks>
		char GetChar(string name);
		/// <summary>
		/// Retrives a DateTime value from an entry. If the entry cannot be found
		/// or cannot be converted, an exception is thrown.
		/// </summary>
		/// <param name="name">
		/// Name of the entry.
		/// </param>
		/// <returns>The located and converted DateTime.</returns>
		/// <remarks>
		/// The type of the <see cref="IPAFNamedAndTypedObject.ObjectValue"/> and
		/// the associated stored <see cref="IPAFNamedAndTypedObject.ObjectType"/>
		/// must match or a cast exception will occur - Legacy method.
		/// </remarks>
		DateTime GetDateTime(string name);
		/// <summary>
		/// Retrives a decimal value from an entry. If the entry cannot be found
		/// or cannot be converted, an exception is thrown.
		/// </summary>
		/// <param name="name">
		/// Name of the entry.
		/// </param>
		/// <returns>The located and converted Decimal.</returns>
		/// <remarks>
		/// The type of the <see cref="IPAFNamedAndTypedObject.ObjectValue"/> and
		/// the associated stored <see cref="IPAFNamedAndTypedObject.ObjectType"/>
		/// must match or a cast exception will occur - Legacy method.
		/// </remarks>
		decimal GetDecimal(string name);
		/// <summary>
		/// Retrives a Double value from an entry. If the entry cannot be found
		/// or cannot be converted, an exception is thrown.
		/// </summary>
		/// <param name="name">
		/// Name of the entry.
		/// </param>
		/// <returns>The located and converted Double.</returns>
		/// <remarks>
		/// The type of the <see cref="IPAFNamedAndTypedObject.ObjectValue"/> and
		/// the associated stored <see cref="IPAFNamedAndTypedObject.ObjectType"/>
		/// must match or a cast exception will occur - Legacy method.
		/// </remarks>
		double GetDouble(string name);
		/// <summary>
		/// Retrives an Int16 value from an entry. If the entry cannot be found
		/// or cannot be converted, an exception is thrown.
		/// </summary>
		/// <param name="name">
		/// Name of the entry.
		/// </param>
		/// <returns>The located and converted Int16.</returns>
		/// <remarks>
		/// The type of the <see cref="IPAFNamedAndTypedObject.ObjectValue"/> and
		/// the associated stored <see cref="IPAFNamedAndTypedObject.ObjectType"/>
		/// must match or a cast exception will occur - Legacy method.
		/// </remarks>
		short GetInt16(string name);
		/// <summary>
		/// Retrives an Int32 value from an entry. If the entry cannot be found
		/// or cannot be converted, an exception is thrown.
		/// </summary>
		/// <param name="name">
		/// Name of the entry.
		/// </param>
		/// <returns>The located and converted Int32.</returns>
		/// <remarks>
		/// The type of the <see cref="IPAFNamedAndTypedObject.ObjectValue"/> and
		/// the associated stored <see cref="IPAFNamedAndTypedObject.ObjectType"/>
		/// must match or a cast exception will occur - Legacy method.
		/// </remarks>
		int GetInt32(string name);
		/// <summary>
		/// Retrives an Int64 value from an entry. If the entry cannot be found
		/// or cannot be converted, an exception is thrown.
		/// </summary>
		/// <param name="name">
		/// Name of the entry.
		/// </param>
		/// <returns>The located and converted Int64.</returns>
		/// <remarks>
		/// The type of the <see cref="IPAFNamedAndTypedObject.ObjectValue"/> and
		/// the associated stored <see cref="IPAFNamedAndTypedObject.ObjectType"/>
		/// must match or a cast exception will occur - Legacy method.
		/// </remarks>
		long GetInt64(string name);
		/// <summary>
		/// Retrives a single-precision floating point value value from an entry.
		/// If the entry cannot be found or cannot be converted, an exception is thrown.
		/// </summary>
		/// <param name="name">
		/// Name of the entry.
		/// </param>
		/// <returns>The located and converted float.</returns>
		/// <remarks>
		/// The type of the <see cref="IPAFNamedAndTypedObject.ObjectValue"/> and
		/// the associated stored <see cref="IPAFNamedAndTypedObject.ObjectType"/>
		/// must match or a cast exception will occur - Legacy method.
		/// </remarks>
		/// <remarks>
		/// The type of the <see cref="IPAFNamedAndTypedObject.ObjectValue"/> and
		/// the associated stored <see cref="IPAFNamedAndTypedObject.ObjectType"/>
		/// must match or a cast exception will occur - Legacy method.
		/// </remarks>
		float GetSingle(string name);
		/// <summary>
		/// Retrives a String value from an entry. If the entry cannot be found
		/// or cannot be converted, an exception is thrown.
		/// </summary>
		/// <param name="name">
		/// Name of the entry.
		/// </param>
		/// <returns>The located and converted String.</returns>
		/// <remarks>
		/// The type of the <see cref="IPAFNamedAndTypedObject.ObjectValue"/> and
		/// the associated stored <see cref="IPAFNamedAndTypedObject.ObjectType"/>
		/// must match or a cast exception will occur - Legacy method.
		/// </remarks>
		string GetString(string name);
		#endregion // Type-safe Entry Fetch Methods
		#endregion Methods
	}
}