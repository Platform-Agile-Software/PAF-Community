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
using System.Collections.ObjectModel;

#endregion

namespace PlatformAgileFramework.TypeHandling.Filters
{
	/// <summary>
	/// This class provides a container for a <see cref="TypeHandlingUtils.GenericTypeFilter{T}"/>
	/// and its auxiliary data.
	/// </summary>
	/// <typeparam name="T">
	/// This is the type that the filters are designed to operate on.
	/// </typeparam>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 31jul2012 </date>
	/// <description>
	/// New.
	/// Support class for the aggregator.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Immutable type - safe if auxiliary data is not modified.
	/// </threadsafety>
	public class PAFItemFilter<T> :IPAFItemFilter<T>
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// The name of the filter. This is used for reporting purposes.
		/// </summary>
		public string FilterName { get; protected set; }
		/// <summary>
		/// This is the optional auxiliary data that can be used by the filter. This
		/// can be set when the class is constructed so that the filter can have
		/// its own private data. This is the default case.
		/// </summary>
		public object FilterAuxiliaryData { get; protected set; }
		/// <summary>
		/// This is the delegate to use for filtering.
		/// </summary>
		public TypeHandlingUtils.GenericTypeFilter<T> FilteringDelegate { get; protected set; }
		/// <summary>
		/// <see langword="true"/> to use the common aux data from either a call on an external
		/// container or on the external container itself.
		/// </summary>
		public bool UseExternalData { get; protected set; }
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// For inheritors
		/// </summary>
		protected PAFItemFilter()
		{
		}

		/// <summary>
		/// Main constructor.
		/// </summary>
		/// <param name="filterName">
		/// This is a name for the filter. <see langword="null"/> or
		/// blank results in the delegete method declaring type and method name
		/// to be concatenated and used. The name obviously need not be unique
		/// within a collection of filters.
		/// </param>
		/// <param name="filteringDelegate">
		/// Loads <see cref="FilteringDelegate"/>.
		/// </param>
		/// <param name="filterAuxiliaryData">
		/// Loads <see cref="FilterAuxiliaryData"/>.
		/// </param>
		/// <param name="useCommonData">
		/// Loads <see cref="UseExternalData"/>.
		/// </param>
		public PAFItemFilter(TypeHandlingUtils.GenericTypeFilter<T> filteringDelegate,
			string filterName = null, object filterAuxiliaryData = null, bool useCommonData = false)
		{
			if(filteringDelegate == null)
				throw new ArgumentNullException("filteringDelegate");
			// Note that the filtering delegate must be set first, since the
			// name may be set from it.
			FilteringDelegate = filteringDelegate;

			if (string.IsNullOrEmpty(filterName))
// ReSharper disable PossibleNullReferenceException
// methodinfo's declaring type is never null.
				filterName = filteringDelegate.GetHashCode().ToString();
// ReSharper restore PossibleNullReferenceException
			FilterName = filterName;
			FilterAuxiliaryData = filterAuxiliaryData;
			UseExternalData = useCommonData;
		}
		#endregion Constructors
		#region Methods
		/// <summary>
		/// Applies the filter to a set of items. Returns only those
		/// that pass.
		/// </summary>
		/// <param name="items">The set of items.</param>
		/// <param name="auxData">
		/// Auxilliary data that will be used if not <see langword="null"/>
		/// and <see cref="UseExternalData"/> is <see langword="true"/>.
		/// </param>
		/// <returns>
		/// List of items that pass the filtering test or <see langword="null"/>.
		/// </returns>
		public virtual IList<T> ApplyFilter(IEnumerable<T> items, object auxData = null)
		{
			if (items == null) return null;
			var list = new Collection<T>();
			var data = FilterAuxiliaryData;
			if ((UseExternalData) && (auxData != null)) data = auxData;
			foreach (var item in items) {
				if (FilteringDelegate(item, data))
					list.Add(item);
			}
			return list;
		}
		#endregion // Methods
	}
}
