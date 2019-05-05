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

#region Using Directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PlatformAgileFramework.Collections.ExtensionMethods;

#endregion

namespace PlatformAgileFramework.TypeHandling.Filters
{
	/// <summary>
	/// This class provides a type-safe aggregator for
	/// <see cref="TypeHandlingUtils.GenericTypeFilter{T}"/>'s. This class supports
	/// hierarchical composition of filters into a logic tree. Simplest form is an
	/// and/or tree, which is what we support in Core OOB. Inheritors have fun!!
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 31jul2012 </date>
	/// <description>
	/// New.
	/// Needed to build an aggregator so we could build and store often-used
	/// filter combinations.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Safe if the individual filters are safe.
	/// </threadsafety>
	/// <remarks>
	/// Note that the filters are contravariant on their item type, so an aggregator
	/// closed with a derived type can aggregate filters with the same type or
	/// a base type. 
	/// </remarks>
	public class PAFItemFilterAggregator<T> : PAFItemFilter<T>, IPAFItemFilterAggregator<T>
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// See <see cref="IPAFItemFilterAggregator{T}"/>.
		/// </summary>
		public ItemFilterCombinationMode CombinationMode { get; protected set; }
		/// <summary>
		/// Contains a list of filter containers that are called in the
		/// same sequence they are installed. We expose only an add method, since
		/// someone is sure to fiddle with the collection while it is being read
		/// and ask why they have concurrency errors.
		/// </summary>
		protected IList<IPAFItemFilter<T>> Filters { get; set; }
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor builds list and sets props.
		/// </summary>
		/// <param name="filterName">
		/// See <see cref="PAFItemFilter{T}"/>. An aggregator is just another container.
		/// </param>
		/// <param name="filterContainers">
		/// Optional set of filters that can be added at construction time.
		/// Default = <see langword="null"/>
		/// </param>
		/// <param name="combinationMode">
		/// Sets <see cref="CombinationMode"/>.
		/// Default = <see cref="ItemFilterCombinationMode.AND_FALSE"/>.
		/// </param>
		/// <param name="filterAuxiliaryData">
		/// See <see cref="PAFItemFilter{T}"/>. An aggregator is just another container.
		/// </param>
		/// <param name="useCommonData">
		/// See <see cref="PAFItemFilter{T}"/>. An aggregator is just another container.
		/// </param>
		public PAFItemFilterAggregator(string filterName = null, IEnumerable<IPAFItemFilter<T>> filterContainers = null,
			ItemFilterCombinationMode combinationMode = default(ItemFilterCombinationMode), object filterAuxiliaryData = null, bool useCommonData = false)
		{
			if (combinationMode == default(ItemFilterCombinationMode)) combinationMode = ItemFilterCombinationMode.AND_FALSE;
			CombinationMode = combinationMode;
			// Note that the filtering delegate must be set first, since the
			// name may be set from it.
			if (combinationMode == ItemFilterCombinationMode.AND_FALSE)
				FilteringDelegate = AndItemFilterFalseResults;
			else
				FilteringDelegate = OrItemFilterTrueResults;

			if (string.IsNullOrEmpty(filterName))
				FilterName = FilteringDelegate.GetHashCode().ToString();

			FilterAuxiliaryData = filterAuxiliaryData;
			UseExternalData = useCommonData;
			Filters = new Collection<IPAFItemFilter<T>>();

			if (filterContainers == null) return;
			foreach(var cont in filterContainers)
			{
				Filters.Add(cont);
			}
		}
		#endregion // Constructors
		#region Methods
		/// <summary>
		/// See <see cref="IPAFItemFilterAggregator{T}"/>.
		/// </summary>
		/// <param name="filter">
		/// See <see cref="IPAFItemFilterAggregator{T}"/>.
		/// </param>
		public void AddFilter(IPAFItemFilter<T> filter)
		{
			lock(Filters)
			{
				Filters.AddNoNulls(filter);
			}
		}
		/// <summary>
		/// This method will return <see langword="false"/> if any aggregated filters return
		/// <see langword="false"/>.
		/// </summary>
		/// <param name="item">
		/// See <typeparamref name="T"/>
		/// </param>
		/// <param name="auxData">
		/// An object carrying auxiliary information.
		/// </param>
		/// <returns>
		/// <see langword="true"/> only if no filters return <see langword="false"/>.
		/// </returns>
		/// <remarks>
		/// This method is exposed as public so it can be called, independent of
		/// the construction parameter
		/// </remarks>
		public virtual bool AndItemFilterFalseResults(T item, object auxData)
		{
			lock (Filters)
			{
				foreach (var container in Filters)
				{
					var data = container.FilterAuxiliaryData;
					if (container.UseExternalData) data = auxData;
					if (!container.FilteringDelegate(item, data))
						return false;
				}
			}
			return true;
		}
		/// <summary>
		/// This method will return <see langword="false"/> if all aggregated filters return
		/// <see langword="false"/>.
		/// </summary>
		/// <param name="item">
		/// See <typeparamref name="T"/>
		/// </param>
		/// <param name="auxData">
		/// An object carrying auxiliary information.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if any filters return <see langword="true"/>.
		/// </returns>
		/// <remarks>
		/// This method is exposed as public so it can be called, independent of
		/// the construction parameter
		/// </remarks>
		public virtual bool OrItemFilterTrueResults(T item, object auxData)
		{
			lock (Filters)
			{
				foreach (var container in Filters)
				{
					var data = container.FilterAuxiliaryData;
					if (container.UseExternalData) data = auxData;
					if (container.FilteringDelegate(item, data))
						return true;
				}
			}
			return false;
		}
		#endregion // Methods
	}
}
