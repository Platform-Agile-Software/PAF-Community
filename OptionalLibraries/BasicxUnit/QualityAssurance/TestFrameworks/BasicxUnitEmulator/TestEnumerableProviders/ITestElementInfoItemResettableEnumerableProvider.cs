using System.Collections.Generic;
using PlatformAgileFramework.Collections.Enumerators;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.TestEnumerableProviders
{
	/// <summary>
	/// This interface extends its base by adding a set method.
	/// </summary>
	/// <typeparam name="T">
	/// Constrained to be a <see cref="IPAFTestElementInfo"/> for our testing model.
	/// </typeparam>
	/// <threadsafety>
	/// Since this is used in a concurrent testing facility (if used with Goshaloma),
	/// it should be used in a thread safe manner, if run under conditions of
	/// concurrency. We only need the set method, in our own work, in Goshaloma,
	/// but it's here if others have the same use case.
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 12dec2017 </date>
	/// <description>
	/// Factored this out of Goshaloma, since we needed these on BasicxUnit rewrite.
	/// </description>
	/// </contribution>
	/// </history>
	public interface ITestElementInfoItemResettableEnumerableProvider<T>:
		IPAFResettableEnumerableProvider<T> where T : IPAFTestElementInfo
	{
		/// <summary>
		/// Resets the enumerable.
		/// </summary>
		/// <param name="enumerable">
		/// The new enumerable.
		/// </param>
		void SetEnumerable(IEnumerable<T> enumerable);
	}
}