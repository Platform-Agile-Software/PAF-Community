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


namespace PlatformAgileFramework.TypeHandling.Filters
{
	/// <summary>
	/// <para>
	/// This interface defines a type-safe aggregator for
	/// <see cref="TypeHandlingUtils.GenericTypeFilter{T}"/>'s. This interface supports
	/// hierarchical composition of filters into a logic tree. Simplest form is an
	/// and/or tree, which is what we support in Core OOB. Inheritors have fun!!
	/// </para>
	/// <para>
	/// Note that an aggregator is also a filter and implementors must devise internal
	/// methods that will combine results from contained <see cref="IPAFItemFilter{T}"/>s
	/// according to the <see cref="CombinationMode"/>.
	/// </para>
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
	public interface IPAFItemFilterAggregator<T> : IPAFItemFilter<T>
	{
		#region Properties
		/// <summary>
		/// Contains the combination mode.
		/// Default = <see cref="ItemFilterCombinationMode.AND_FALSE"/>.
		/// </summary>
		ItemFilterCombinationMode CombinationMode { get; }
		#endregion // Properties
		#region Methods
		/// <summary>
		/// This method adds a filter to the collection.
		/// </summary>
		/// <param name="filter">
		/// Filter to be added. <see langword="null"/>s are ignored.
		/// </param>
		void AddFilter(IPAFItemFilter<T> filter);
		#endregion // Methods
	}
}
