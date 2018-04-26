//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2017 Icucom Corporation
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
using System.Linq;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.Collections.Enumerators;
using PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.TestEnumerableProviders;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator
{
	/// <summary>
	/// Class has a few helper extensions for test elements.
	/// </summary>
	/// <threadsafety>
	/// See individual methods.
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 12dec2017 </date>
	/// <description>
	/// Factored this out of Goshaloma, since we need these on BasicxUnit.
	/// </description>
	/// </contribution>
	/// </history>
	public static class PAFTestElementInfoExtensions
	{
		#region Class Fields and Autoproperties
		public const string METHOD_ELEMENT_TAG = "Method";
		public const string FIXTURE_ELEMENT_TAG = "Fixture";
		public const string ASSEMBLY_ELEMENT_TAG = "Assembly";
		public const string ASSEMBLYSET_ELEMENT_TAG = "AssemblySet";
		public const string HARNESS_ELEMENT_TAG = "Harness";
		#endregion // Class Fields and Autoproperties
		#region Methods
		/// <summary>
		/// Helper method that alphabetizes <see cref="IPAFTestElementInfo"/> arrays.
		/// </summary>
		/// <param name="elementInfos">
		/// Incoming collection. <see langword="null"/> gets an empty collection - never <see langword="null"/>.
		/// </param>
		/// <returns>Alphabetized collection.</returns>
		/// <exceptions>
		/// None caught, none thrown. An exception is guaranteed
		/// if elements with repeated names are passed in.
		/// </exceptions>
		public static ICollection<T> AlphabetizeMethodInfos<T>(this IEnumerable<T> elementInfos)
			where T : IPAFTestElementInfo

		{
			var dict = new Dictionary<string, T>();
			var alphabetizedMethodInfos = new List<T>();
			// ReSharper disable LoopCanBeConvertedToQuery
			foreach (var methodInfo in elementInfos)
				// ReSharper restore LoopCanBeConvertedToQuery
			{
				dict.Add(methodInfo.TestElementName, methodInfo);
			}
			// Now just pull from the dict...
			foreach (var methodInfoKVP in dict)
			{
				alphabetizedMethodInfos.Add(methodInfoKVP.Value);
			}
			return alphabetizedMethodInfos;
		}
		/// <summary>
		/// Returns elements that are active.
		/// </summary>
		/// <param name="testElementInfos">One of us.</param>
		/// <returns>0 for no parents.</returns>
		/// <threadsafety>
		/// Safe.
		/// </threadsafety>
		public static IList<T> GetActiveElements<T>
			(this IEnumerable<T> testElementInfos)
			where T: IPAFTestElementInfo
		{
			var infos = new List<T>();
			if (testElementInfos == null) return infos;
			foreach (var info in testElementInfos)
			{
				if(info.TestElementStatus == TestElementRunnabilityStatus.Active)
					infos.Add(info);
			}
			return infos;
		}
		/// <summary>
		/// Gets all children of a node. 
		/// </summary>
		/// <typeparam name="T">
		/// Constrained to be a subtype of <see cref="IPAFTestElementInfo"/>.
		/// </typeparam>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementInfo"/>.
		/// </param>
		/// <returns>
		/// List of <typeparamref name="T"/>s
		/// </returns>
		public static IList<T> GetAllElementChildren<T>
			(this T info) where T : IPAFTestElementInfo
		{
			return info.GetChildInfoSubtypesOfType<T, T>();
		}
		/// <summary>
		/// Gets children that are a subtype of a subtype of
		/// <see cref="IPAFTestElementInfo"/>. 
		/// </summary>
		/// <typeparam name="T">
		/// Constrained to be a subtype of <see cref="IPAFTestElementInfo"/>.
		/// </typeparam>
		/// <typeparam name="U">
		/// Constrained to be a subtype of <see cref="IPAFTestElementInfo"/>.
		/// </typeparam>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementInfo"/>.
		/// </param>
		/// <returns>
		/// List of <typeparamref name="U"/>s
		/// </returns>
		public static IList<U> GetTypedChildInfosOfType<T, U>
			(this IPAFTestElementInfo<T> info) where T : IPAFTestElementInfo where U : IPAFTestElementInfo
		{
			var infos = new List<U>();
			if (!info.AllChildren.Any())
				return infos;
			var colOfU = info.AllChildren.EnumIntoSubtypeList<IPAFTestElementInfo, U>();
 			return colOfU;
		}
		/// <summary>
		/// Gets children that are <see cref="IPAFTestElementInfo"/>. 
		/// </summary>
		/// <typeparam name="T">
		/// Constrained to be a subtype of <see cref="IPAFTestElementInfo"/>.
		/// </typeparam>
		/// <typeparam name="U">
		/// Constrained to be a subtype of <see cref="IPAFTestElementInfo"/>.
		/// </typeparam>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementInfo"/>.
		/// </param>
		/// <returns>
		/// List of <see cref="IPAFTestElementInfo"/>s that are actually
		/// <typeparamref name="U"/>s. Generic can only be closed on
		/// <typeparamref name="U"/> at the point where it's used and it
		/// has to return a non-Generic.
		/// </returns>
		public static IList<IPAFTestElementInfo> GetUntypedChildInfosOfType<T, U>
			(this T info) where T : IPAFTestElementInfo where U : IPAFTestElementInfo
		{
			var infos = new List<IPAFTestElementInfo>();
			if (!info.AllChildren.Any())
				return infos;
			var colOfU = info.AllChildren.EnumIntoSubtypeList<IPAFTestElementInfo, U>();
			return colOfU.EnumIntoSupertypeList<IPAFTestElementInfo, U>();
		}
		/// <summary>
		/// Gets children that are a subtype of a subtype of
		/// <see cref="IPAFTestElementInfo"/>. 
		/// </summary>
		/// <typeparam name="T">
		/// Constrained to be a subtype of <see cref="IPAFTestElementInfo"/>.
		/// </typeparam>
		/// <typeparam name="U">
		/// Constrained to be a subtype of <typeparamref name="T"/>.
		/// </typeparam>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementInfo"/>.
		/// </param>
		/// <returns>
		/// List of <typeparamref name="U"/>s
		/// </returns>
		public static IList<U> GetChildInfoSubtypesOfType<T, U>
			(this T info) where T : IPAFTestElementInfo where U : T
		{
			var infos = new List<U>();
			if (!info.AllChildren.Any())
				return infos;
			var colOfU = info.AllChildren.EnumIntoSubtypeList<IPAFTestElementInfo, U>();
			return colOfU;
		}
		/// <summary>
		/// Just figures out how far down we are from the root.
		/// </summary>
		/// <param name="testElementInfo">One of us.</param>
		/// <returns>0 for no parents.</returns>
		/// <threadsafety>
		/// Safe.
		/// </threadsafety>
		public static int GetDepthDownFromRoot(this IPAFTestElementInfo testElementInfo)
		{
			var depth = 0;
			while ((testElementInfo = testElementInfo.Parent) != null)
			{
				depth++;
			}
			return depth;
		}
		/// <summary>
		/// Just returns the root.
		/// </summary>
		/// <param name="testElementInfo">One of us.</param>
		/// <returns>Root of the tree.</returns>
		/// <threadsafety>
		/// Safe.
		/// </threadsafety>
		public static IPAFTestElementInfo GetRootOfTestElementTree(this IPAFTestElementInfo testElementInfo)
		{
			var returnedTestElementInfo = testElementInfo;
			while ((testElementInfo = testElementInfo.Parent) != null)
			{
				returnedTestElementInfo = testElementInfo;
			}
			return returnedTestElementInfo;
		}
		/// <summary>
		/// Just a little helper to do the cast from a possibly explicit interface in one place.
		/// </summary>
		/// <param name="providerProvider">
		/// A <see cref="ITestElementInfoItemResettableEnumerableProviderProvider{T}"/>.
		/// </param>
		/// <returns>
		/// The provider provider - cannot be <see langword="null"/>, since we
		/// just does an implicit cast.
		/// </returns>
		// NOTE KRM - this was a wierd way to do things. Why not just
		// use the MeAs construct on the class itself like we always did.
		public static ITestElementInfoItemResettableEnumerableProviderProvider<T>
			GetTestElementInfoResettableEnumerableProviderProvider<T>
			(this ITestElementInfoItemResettableEnumerableProviderProvider<T> providerProvider)
			where T : IPAFTestElementInfo
		{
			return providerProvider;
		}
		/// <summary>
		/// Just a little helper to do the cast from a possibly explicit interface in one place.
		/// </summary>
		/// <param name="providerProvider">
		/// A <see cref="ITestElementInfoItemResettableEnumerableProviderProvider{T}"/>.
		/// </param>
		/// <returns>
		/// The provider provider - can be <see langword="null"/>.
		/// </returns>
		public static ITestElementInfoItemResettableEnumerableProviderProvider<T>
			EnsureTestElementInfoResettableEnumerableProvider<T>
			(this ITestElementInfoItemResettableEnumerableProviderProvider<T> providerProvider)
			where T : IPAFTestElementInfo
		{
			return providerProvider;
		}
		/// <summary>
		/// Just a little helper to do the cast from a possibly explicit interface in one place.
		/// </summary>
		/// <param name="providerProvider">
		/// A <see cref="ITestElementInfoItemEnumerableProviderProvider{T}"/>.
		/// </param>
		/// <returns>
		/// The provider provider - can be <see langword="null"/>.
		/// </returns>
		public static ITestElementInfoItemEnumerableProviderProvider<T>
			GetTestElementInfoEnumerableProviderProvider<T>
			(this ITestElementInfoItemEnumerableProviderProvider<T> providerProvider)
			where T: IPAFTestElementInfo
		{
			return providerProvider;
		}

		/// <summary>
		/// Generates an exception if <paramref name="testElementInfo"/> is
		/// <see langword="null"/> or name is blank or <see langword="null"/>.
		/// Initial use was in constructors.
		/// </summary>
		/// <param name="testElementInfo">One of us.</param>
		/// <returns>The name if no problems.</returns>
		/// <threadsafety>
		/// Safe.
		/// </threadsafety>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="testElementInfo"/> cannot be <see langword="null"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// </exception>
		/// "Name cannot be null or blank"
		/// <paramref name="testElementInfo"/> must have a valid name property.
		/// </exceptions>
		public static string EnsureTestElementName(this IPAFTestElementInfo testElementInfo)
		{
			if(testElementInfo == null)
				throw new ArgumentNullException(nameof(testElementInfo));
			if(string.IsNullOrEmpty(testElementInfo.TestElementName))
				throw new ArgumentException(nameof(testElementInfo),
					"Name cannot be null or blank");
			return testElementInfo.TestElementName;
		}
		/// <summary>
		/// Just a little shorthand for setting stuff on test elements.
		/// </summary>
		/// <param name="testElementInfo">info we are operating on.</param>
		/// <param name="invalidate">
		/// <see langword="true"/> to set the element status to <see cref="TestElementRunnabilityStatus.ExcludedByErrors"/>.
		/// Default is <see langword="false"/>.
		/// </param>
		/// <param name="exception">
		/// Exceptions to add to the exception collection.
		/// </param>
		/// <param name="passed">
		/// If this has a value, it will be used to set the
		/// <see cref="IPAFTestElementInfo.Passed"/>. If no value, it is not
		/// changed. Default = <see langword="null"/> (no value).
		/// </param>
		/// <threadsafety>
		/// Unsafe.
		/// </threadsafety>
		public static void SetElementInfoErrors(this IPAFTestElementInfo testElementInfo,
			bool invalidate = false, Exception exception = null, bool? passed = null)
		{
			if(invalidate) testElementInfo.TestElementStatus = TestElementRunnabilityStatus.ExcludedByErrors;
			if(exception != null) testElementInfo.AddTestException(exception);
			if (passed.HasValue) testElementInfo.Passed = passed.Value;
		}

		/// <summary>
		/// Wraps an enumerable in our default provider and provider provider
		/// classes.
		/// </summary>
		/// <typeparamref name="T">
		/// Any <see cref="IPAFTestElementInfo"/>
		/// </typeparamref>
		/// <param name="enumerable">
		/// An enumerable of <typeparamref name="T"/>.
		/// </param>
		/// <returns>A provider provider.</returns>
		/// <threadsafety>
		/// Safe.
		/// </threadsafety>
		public static ITestElementInfoItemEnumerableProviderProvider<T>
			WrapEnumerator<T>(this IEnumerable<T> enumerable)
			where T : IPAFTestElementInfo
		{
			var provider = new TestElementInfoItemEnumerableProvider<T>(enumerable);
			var providerProvider
				= new TestElementInfoItemEnumerableProviderProvider<T>(provider);
			return providerProvider;
		}
		/// <summary>
		/// Wraps an enumerable in our default provider provider
		/// classes.
		/// </summary>
		/// <typeparamref name="T">
		/// Any <see cref="IPAFTestElementInfo"/>
		/// </typeparamref>
		/// <param name="enumerableProvider">
		/// An enumerable provider of <typeparamref name="T"/>.
		/// </param>
		/// <returns>A provider provider.</returns>
		/// <threadsafety>
		/// Safe.
		/// </threadsafety>
		public static ITestElementInfoItemEnumerableProviderProvider<T>
			WrapEnumeratorProvider<T>(this IPAFEnumerableProvider<T> enumerableProvider)
			where T : IPAFTestElementInfo
		{
			var providerProvider
				= new TestElementInfoItemEnumerableProviderProvider<T>(enumerableProvider);
			return providerProvider;
		}
		#endregion // Methods

	}
}
