using System;
using PlatformAgileFramework.TypeHandling.BasicValueTypes;

namespace PlatformAgileFramework.TypeHandling.TypeComparison.Comparators
{
	/// <summary>
	/// Interface for comparing complex floats adds a parameterized comparator.
	/// Expose this interface from its implementing class to
	/// allow local overrides of a <see cref="IPAFEquatable{T}"/>.
	/// </summary>
	/// <history>
	/// <author> BMC </author>
	/// <date> 02jun2017 </date>
	/// <contribution>
	/// <description>
	/// New. Rework/reformat of the <see cref="IComparable{T}"/> helpers.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IPAFComplexFloatEquatable: IPAFEquatable<PAFComplexFloatNumber>
	{
		/// <summary>
		/// Compares with the epsilon suppled as a parameter.
		/// </summary>
		/// <param name="value">
		/// First value to be compared.
		/// </param>
		/// <param name="otherValue">
		/// Other value to be compared.
		/// </param>
		/// <param name="epsilon">
		/// epsilon to be used in the comparison.
		/// </param>
		/// <returns>
		/// True if some function of the comparands is within <paramref name="epsilon"/>.
		/// This is implementation-dependent.
		/// </returns>
		bool AreEqual(PAFComplexFloatNumber value, PAFComplexFloatNumber otherValue, float epsilon);
	}
}
