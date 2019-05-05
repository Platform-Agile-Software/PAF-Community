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
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;
using System.Collections.Generic;
using System.Threading;

namespace PlatformAgileFramework.TypeHandling.TypeComparison.Comparators
{
	/// <summary>
	/// Comparator for types. This class is designed to be used, by default
	/// as a singleton. However, in our work, we need to construct a comparator
	/// with parameters sometimes, so the constructor is not private. This class
	/// is non-Generic base class to hold static default closed comparators
	/// </summary>
	/// <history>
	/// <author> BMC </author>
	/// <date> 02jun2017 </date>
	/// <contribution>
	/// <description>
	/// New. 
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Safe.
	/// </threadsafety>
	public class PAFEquatable
	{
		#region Fields and Autoprops
		/// <summary>
		/// This comparator will be used as the default in calls on
		/// the singleton, but it is accessible through its interface extension.
		/// </summary>
		public static IPAFFloatEquatable DefaultFloatEquatable { get; protected set; }
		/// <summary>
		/// This comparator will be used as the default in calls on
		/// the singleton, but it is accessible through its interface extension.
		/// </summary>
		public static IPAFDoubleEquatable DefaultDoubleEquatable { get; protected set; }
		/// <summary>
		/// This comparator will be used as the default in calls on
		/// the singleton, but it is accessible through its interface extension.
		/// </summary>
		public static IPAFComplexFloatEquatable DefaultComplexFloatEquatable { get; protected set; }
		/// <summary>
		/// This comparator will be used as the default in calls on
		/// the singleton, but it is accessible through its interface extension.
		/// </summary>
		public static IPAFComplexDoubleEquatable DefaultComplexDoubleEquatable { get; protected set; }
		#endregion // Fields and Autoprops
		#region Constructors

		/// <summary>
		/// Builds our DEFAULT static defaults.
		/// </summary>
		static PAFEquatable()
		{
			DefaultFloatEquatable = new PAFFloatEquatable();
			DefaultDoubleEquatable = new PAFDoubleEquatable();
			DefaultComplexFloatEquatable = new PAFComplexFloatEquatable();
			DefaultComplexDoubleEquatable = new PAFComplexDoubleEquatable();

		}
		#endregion // Constructors
	}
}
