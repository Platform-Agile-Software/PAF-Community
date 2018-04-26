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
using PlatformAgileFramework.TypeHandling.PartialClassSupport;

#endregion

namespace PlatformAgileFramework.TypeHandling.Filters
{
	/// <summary>
	/// This class determines how the filter results are to be combined
	/// to form an aggregate result. The class is designed for extensibility
	/// either through inheritance or use of partial classes.
	/// </summary>
	// ReSharper disable PartialTypeWithSinglePart
	public sealed partial class ItemFilterCombinationMode : ExtendablePseudoEnumInt32

	// ReSharper restore PartialTypeWithSinglePart
	{
		#region Class Fields And Autoproperties
		//public static readonly PEValue<Int32, ItemFilterCombinationMode> OR_TRUE
		//	= GenerateNewEnumValue<ItemFilterCombinationMode>("OR_TRUE ", 1);
		/// <summary>
		/// Positive returns on an item are to be ORed together. The
		/// item passes if any filter passes it.
		/// </summary>
		public static readonly ItemFilterCombinationMode OR_TRUE
		= new ItemFilterCombinationMode("OR_TRUE", 1);
		#endregion // Class Fields And Autoproperties
	}
}
