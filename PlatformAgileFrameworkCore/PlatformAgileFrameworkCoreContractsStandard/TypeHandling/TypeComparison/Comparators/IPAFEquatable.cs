namespace PlatformAgileFramework.TypeHandling.TypeComparison.Comparators
{
	/// <summary>
	/// This interface allows a specific implementation of a comparator
	/// to be provided as a service. Necessary for parameterized comparators.
	/// </summary>
	/// <typeparam name="T">Arbitrary Generic.</typeparam>
	/// <history>
	/// <author> BMC </author>
	/// <date> 02jun2017 </date>
	/// <contribution>
	/// <description>
	/// New. Needed to support our custom structs.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IPAFEquatable<in T>
	{
		/// <summary>
		/// Compares two values for equality.
		/// </summary>
		/// <param name="valueItem">First value to be compared.</param>
		/// <param name="otherValueItem">Another value to be compared.</param>
		/// <returns>
		/// <see langword="true"/> if equal ACCORDING TO THE IMPLEMENTATION.
		/// </returns>
		/// <remarks>
		/// Folks are confused when the answer is not what they think.
		/// CHECK THE IMPLEMENTATION!
		/// </remarks>
		bool AreEqual(T valueItem, T otherValueItem);
	}
}