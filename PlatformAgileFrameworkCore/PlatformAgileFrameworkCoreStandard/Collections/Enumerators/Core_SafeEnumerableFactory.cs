using System;
using System.Collections;
using System.Collections.Generic;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;

namespace PlatformAgileFramework.Collections.Enumerators
{
	/// <summary>
	/// Implementation of <see cref="IEnumerableFactory{T}"/> that generates deep copy
	/// enumerators. Standard problem in developing thread-safe collections is that the
	/// enumerator is still exposed. This enumerator factory is designed to give access
	/// to three different copy enumerators so collections can be iterated over with a
	/// foreach statement while the collection is being modified by other threads. The
	/// enumerators called internally may be employed in a collection if the client
	/// wishes a specific scheme to be forced, instead of selected automatically.
	/// </summary>
	/// <typeparam name="T">Generic type in the collection.</typeparam>
// Silverlight
// ReSharper disable PartialTypeWithSinglePart
	public partial class SafeEnumerableFactory<T> : IEnumerableFactory<T>
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Methods
		#region Partial Methods
		// ReSharper disable PartialMethodWithSinglePart
		/// <summary>
		/// This method implements a clone enumerator, which operates on the
		/// "ICloneable" types. This interface is generally deprecated
		/// and is not included in Silverlight. We include it here for backward
		/// compatibility with some PAF apps that use it.
		/// </summary>
		/// <param name="enumerable">Incoming enumeration to build from.</param>
		/// <param name="cloneEnumerable">An enumerator that hands out clones of the
		/// original type by calling <c>ICloneable.Clone()</c>.</param>
		/// <remarks>
		/// Noted: This is the DEFINITION of a partial method which can have an
		/// IMPLEMENTATION in ONE other part of the class. In PAF, we implement
		/// it in the ECMA/CLR part of core.
		/// </remarks>
		partial void BuildCloneEnumerable(IEnumerable<T> enumerable, ref IEnumerable<T> cloneEnumerable);
// ReSharper restore PartialMethodWithSinglePart
		#endregion // Partial Methods
		/// <summary>
		/// This constructor builds a thread-safe enumerator based on the characteristics
		/// of <typeparamref name="T"/>. These characteristics are checked in the order
		/// listed here.
		/// <list type="number">
		/// <item>
		/// <term><see cref="IPAFGenericDeepCloneable{T}"/></term>
		/// <description>
		/// Builds a cloning enumerator based on "deep" cloning.
		/// </description>
		/// </item>
		/// <item>
		/// <term>Has a copy constructor</term>
		/// <description>
		/// Builds a copy enumerator that copies elements into a separate array.
		/// This enumerator does not give access to the original copies, so it
		/// is useful only for readonly access to a snapshot of the collection.
		/// </description>
		/// </item>
		/// <item>
		/// <term> Is "ICloneable"</term>
		/// <description>
		/// Builds a cloning enumerator based on "ICloneable".
		/// Not available in the Silverlight model.
		/// </description>
		/// </item>
		/// <item>
		/// <term>Neither</term>
		/// <description>
		/// Builds a copy enumerator that copies element references into a separate
		/// array. This does not make elements themselves safe, but ensures that
		/// the collection structure is safe.
		/// </description>
		/// </item>
		/// </list>
		/// </summary>
		/// <param name="enumerable">
		/// Incoming enumeration to build from. <see langword="null"/> gets <see langword="null"/>.
		/// </param>
		/// <returns>
		/// A thread-safe enumerator. The enumerator will be thread-safe in accordance with
		/// the type of clone the enumerator makes. If it is a truly deep clone, the type
		/// hierarchy will be completely replicated, with a completely new hierachy
		/// of references. Other clone methodologies may just make a reference copy,
		/// etc.. The client must understand the type of copying that will be done,
		/// based on the characteristics of the type, <typeparamref name="T"/>.
		/// This method never returns <see langword="null"/>
		/// </returns>
		/// <remarks>
		/// In a Silverlight environment, the interface "ICloneable" is not defined
		/// publicly and thus objects may not be cloned by this interface. The ECMA/CLR
		/// augmentation of this class does provide the ability to produce
		/// enumerable factories that will utilize "ICloneable".
		/// </remarks>
		public virtual IEnumerable<T> BuildEnumerable(IEnumerable<T> enumerable)
		{
			if (enumerable == null) return null;
			// Avoid double fetch.
			enumerable = enumerable.IntoArray();
			// Needed for constructors.
			var theEnumerator = new object[] { enumerable };
			var typeOfT = new[] { typeof(T) };
			//// We prefer our special interface, IDeepCloneable.
			if (typeof(IPAFGenericDeepCloneable<T>).IsTypeAssignableFrom(typeOfT[0])) {
				var typeOfClosedEnumerator
					= typeof(DeepCloneEnumerable<>).MakeGenericType(typeOfT);
				var deepCloneEnumerator
					= (IEnumerable<T>)Activator.CreateInstance(typeOfClosedEnumerator, theEnumerator);
				return deepCloneEnumerator;
			}
			//// Copy enumerator
			var info = typeOfT[0].GetCopyConstructor();
			// If so, use the copy enumerator.
			if (info != null) {
				// TODO adjust for elevated priviledge.
				// KRM - put copyenumerable back.
				//var copyEnumerator = new PAFCopyEnumerable<T>(enumerable, true);
				//return copyEnumerator.GetEnumerable();
			}
			//// Attempt to build a clone enumerator if we are in an ECMA CLR environment.
			IEnumerable<T> cloningEnumerable = null;
// ReSharper disable InvocationIsSkipped
			BuildCloneEnumerable(enumerable, ref cloningEnumerable);
// ReSharper restore InvocationIsSkipped
			// Done if we got an enumerator.
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode
			if (cloningEnumerable != null) return cloningEnumerable;
// ReSharper restore HeuristicUnreachableCode
// ReSharper restore ConditionIsAlwaysTrueOrFalse

			// Don't know how to copy items - use the array enumerator.
			var shallowCopyEnumerator = new ShallowCopyEnumerator<T>(enumerable);
			return shallowCopyEnumerator;
		}
		/// <summary>
		/// This method just culls all the non - <typeparamref name="T"/>
		/// items from the input and builds with the other method.
		/// </summary>
		/// <param name="enumerable">
		/// Non - Generic enumerable.
		/// </param>
		/// <returns>
		/// Generic enumerator.
		/// </returns>
		public virtual IEnumerable<T> BuildEnumerable(IEnumerable enumerable)
		{
			var generic = enumerable.FilterEnumerable<T>();
			return BuildEnumerable(generic);
		}
		#endregion // Methods
	}
}