//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2005 - 2018 Icucom Corporation
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

//using PlatformAgileFramework.Connectors.BaseConnectors;

namespace PlatformAgileFramework.Connections.BaseConnectors
{
	/// <summary>
	/// This is the Generic base interface for all connectors in PAF.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date>23jan2018 </date>
	/// <description>
	/// Had to inherit from the non - Generic, which is the way it should
	/// have always been.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date>09jan2017 </date>
	/// <description>
	/// Changed names of the generics so we don't have to keep
	/// doing explicit implementation all the time.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> DAP </author>
	/// <date>11jan2012 </date>
	/// <description>
	/// Added history - Extracted this from the simulator. Interface was used to
	/// support data flows from one block to another. Made this type-safe version.
	/// </description>
	/// </contribution>
	/// </history>
	// ReSharper disable once PartialTypeWithSinglePart
	//// Second part is in the simulator.
	public partial interface IPAFConnector<TSource, TSink>
		: IPAFConnector
	{
		/// <summary>
		/// Flows can be symmetric (bi-directional) but this will be
		/// the default source for uni-directional flows. Cannot be
		/// immutable by class design, since it must be dynamically
		/// set. Reset should be guarded, however.
		/// </summary>
		TSource UniDirectionalSourceItem { get; set; }

		/// <summary>
		/// Flows can be symmetric (bi-directional) but this will be
		/// the default sink for uni-directional flows.  Cannot be
		/// immutable by class design, since it must be dynamically
		/// set. Reset should be guarded, however.
		/// </summary>
		TSink UniDirectionalSinkItem { get; set; }

	}
}
