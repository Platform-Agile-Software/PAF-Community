//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2005 - 2017 Icucom Corporation
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

namespace PlatformAgileFramework.Connections.BaseConnectors
{
	/// <summary>
	/// This is the base interface for all connectors in PAF. The connector must
	/// be disposable. since implementations may contain waveform files and other
	/// items that must be disposed. Dispose should be callable multiple times
	/// without exceptions. Use a guard..... 
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date>11jun2017 </date>
	/// <description>
	/// Redid with a pseudoenum
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> DAP </author>
	/// <date>11jan2012 </date>
	/// <description>
	/// Added history - Extracted this from the simulator. Interface was used to
	/// support data flows from one block to another. Made some mods to allow
	/// bi-directional flows.
	/// </description>
	/// </contribution>
	/// </history>
	// ReSharper disable once PartialTypeWithSinglePart
	//// Second part is in the simulator.
	public partial interface IPAFConnector
	{
		/// <summary>
		/// Flows can be symmetric (bi-directional) but this will be
		/// the source for uni-directional flows. Cannot be
		/// immutable by class design, since it must be dynamically
		/// set. Reset should be guarded, however.
		/// </summary>
		object UniDirectionalSource { get; set; }

		/// <summary>
		/// Flows can be symmetric (bi-directional) but this will be
		/// the sink for uni-directional flows. Cannot be
		/// immutable by class design, since it must be dynamically
		/// set. Reset should be guarded, however.
		/// </summary>
		object UniDirectionalSink { get; set; }

		/// <summary>
		/// Transfer direction, which should be thread-safe, since it can
		/// be set dynamically.
		/// </summary>
		TransferDirection DataTransferDirection { get; set; }
	}
}
