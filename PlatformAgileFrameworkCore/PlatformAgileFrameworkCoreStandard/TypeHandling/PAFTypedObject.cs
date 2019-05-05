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

using System;
using PlatformAgileFramework.Collections;

namespace PlatformAgileFramework.TypeHandling
{
	/// <summary>
	/// Class that holds an object, which may be <see langword="null"/> and its
	/// Type.
	/// </summary>
	public class PAFTypedObject: PAFTypeHolderBase, IPAFTypedObject
	{
		#region Class AutoProperties
		/// <summary>
		/// The object, which may be <see langword="null"/>.
		/// </summary>
		public object Object { get; set; }
		#endregion // Class AutoProperties
		#region Constructors
		/// <summary>
		/// Constructor just set the fields.
		/// </summary>
		/// <param name="obj">
		/// The contained object. May be <see langword="null"/>.
		/// </param>
		/// <param name="objectType">
		/// The type of the object (not <see langword="null"/>).
		/// </param>
		public PAFTypedObject (object obj, Type objectType)
			:base(objectType)
		{
			Object = obj;
		}
		#endregion Constructors
		#region Static Helpers
		/// <summary>
		/// Calls <c>PAFTypedObject(IPAFNamedAndTypedObject.Object, IPAFNamedAndTypedObject.ObjectType)</c>.
		/// </summary>
		/// <param name="nto">
		/// The named and typed object. Can not be <see langword="null"/>.
		/// </param>
		/// <returns>
		/// One of us.
		/// </returns>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if <paramref name="nto"/>
		/// is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		public static IPAFTypedObject FromNTO(IPAFNamedAndTypedObject nto)
		{
			if (nto == null)
				throw new ArgumentNullException("nto");
			return new PAFTypedObject(nto.ObjectValue, nto.ObjectType);
		}
		#endregion // Static Helpers

	}
}
