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
using PlatformAgileFramework.TypeHandling.PartialClassSupport;

#endregion

namespace PlatformAgileFramework.TypeHandling.Delegates
{
	/// <summary>
	/// <para>
	/// This class describes what states an "undisciplined subscriber" can be in.
	/// </para>
	/// <para>
	/// Subscribers can be fed on threads and if they fail to return within a
	/// timeout or generate exceptions, they can be disconnected or marked
	/// as rogue.
	/// </para>
	/// </summary>
	// ReSharper disable PartialTypeWithSinglePart
	public sealed partial class SubscriberState: ExtendablePseudoEnumInt32
	// ReSharper restore PartialTypeWithSinglePart
	{
		#region Class Fields And Autoproperties
		/// <summary>
		/// Set this bit to prevent transmission to this subscriber.
		/// </summary>
		// ReSharper disable once InconsistentNaming
		public static readonly SubscriberState IGNORED
			= new SubscriberState("IGNORED", 1, true);
		/// <summary>
		/// This subscriber is to be permanently removed from the subscriber store
		/// at the next opportunity.
		/// </summary>
		// ReSharper disable once InconsistentNaming
		public static readonly SubscriberState DISCONNECTED
			= new SubscriberState("DISCONNECTED", 2, true);
		#endregion // Class Fields And Autoproperties
		/// <remarks>
		/// See base.
		/// </remarks>
		public SubscriberState(string name, int value)
			: base(name, value)
		{
		}
		/// <remarks>
		/// See base.
		/// </remarks>
		internal SubscriberState(string name, int value, bool addToDictonary)
			: base(name, value, addToDictonary)
		{
		}

	}
}
