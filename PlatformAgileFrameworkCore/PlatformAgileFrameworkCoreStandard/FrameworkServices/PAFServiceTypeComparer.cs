//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010-2011 Icucom Corporation
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

using PlatformAgileFramework.Collections;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.FrameworkServices.Exceptions;

#region Using directives

using System;
using System.Collections.Generic;
using PlatformAgileFramework.Collections.Comparers;
#endregion

#region Exception Shorthand
using IPAFSED = PlatformAgileFramework.FrameworkServices.Exceptions.IPAFServiceExceptionData;
using PAFSED = PlatformAgileFramework.FrameworkServices.Exceptions.PAFServiceExceptionData;
using PAFSEDB = PlatformAgileFramework.FrameworkServices.Exceptions.PAFServiceExceptionDataBase;
#endregion // Exception Shorthand

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// This class implements a comparison of <see cref="IPAFServiceDescription"/>s.
	/// We compare the interface type. This is designed to be used in the
	/// outer dictionary of a two-level service dictionary.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 20jun2013 </date>
	/// <description>
	/// Built for the two-level dictionary scheme.
	/// </description>
	/// </contribution>
	/// </history>
	public class PAFServiceTypeComparer : AbstractComparerShell<IPAFNamedAndTypedObject>
	{
		/// <summary>
// ReSharper disable once CSharpWarnings::CS1584
// ReSharper problem.
		/// Compares the <see cref="IPAFNamedAndTypedObject.ObjectType"/>s.
		/// </summary>
		/// <param name="firstKey">
		/// The first <see cref="IPAFNamedAndTypedObject"/> to compare.
		/// </param>
		/// <param name="secondKey">
		/// The other <see cref="IPAFNamedAndTypedObject"/> to compare.
		/// </param>
		/// <returns>
		/// See <see cref="IComparer{T}"/>
		/// </returns>
		/// <exceptions>
		/// <exception> <see cref="PAFStandardException{IPAFServiceExceptionData}"/>
		/// <see cref="PAFServiceExceptionDataBase.TYPE_IS_NOT_RESOLVED"/>
		/// Dictionary throws a cryptic exception, so a better one is thrown here.
		/// </exception>
		/// <exception> <see cref="ArgumentException"/> is thrown if either
		/// <paramref name="firstKey"/> or <paramref name="secondKey"/>
		/// is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		public override int DefaultMainCompare(IPAFNamedAndTypedObject firstKey,
			IPAFNamedAndTypedObject secondKey)
		{
			if (firstKey == null)
				throw new ArgumentNullException("firstKey");
			if (secondKey == null)
				throw new ArgumentNullException("secondKey");

			var firstInterfaceType = firstKey.ObjectType;
			var secondInterfaceType = secondKey.ObjectType;

			// Types must be resolved.
			if (firstInterfaceType == null)
			{
				var data = new PAFSED(firstKey.GetServiceDescriptionInterface());
				throw new PAFStandardException<IPAFSED>(
					data, PAFServiceExceptionMessageTags.TYPE_IS_NOT_RESOLVED);
			}
			if (secondInterfaceType == null)
			{
				var data = new PAFSED(secondKey.GetServiceDescriptionInterface());
				throw new PAFStandardException<IPAFSED>(
                    data, PAFServiceExceptionMessageTags.TYPE_IS_NOT_RESOLVED);
			}
			if (secondInterfaceType.GetHashCode() == firstInterfaceType.GetHashCode())
				return 0;
			if (secondInterfaceType.GetHashCode() > firstInterfaceType.GetHashCode())
				return -1;
			return 1;
		}

		/// <summary>
		/// This override gets the hash code of the service interface type, because
		/// that is what we are sorting by.
		/// </summary>
		/// <param name="obj">
		/// An <see cref="IPAFNamedAndTypedObject"/>.
		/// </param>
		/// <returns>The hash code of the service interface type.</returns>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">
		/// "obj"
		/// </exception>
		/// <exception cref="ArgumentException">
		/// "Not a 'IPAFNamedAndTypedObject'"
		/// </exception>
		/// <exception> <see cref="PAFStandardException{IPAFServiceExceptionData}"/>
		/// <see cref="PAFServiceExceptionDataBase.TYPE_IS_NOT_RESOLVED"/>. The
		/// interface type must be resolved for this method. Dictionary throws
		/// a cryptic exception, so a better one is thrown here.
		/// </exception>
		/// </exceptions>
		public override int GetHashCode(object obj)
		{
			if (obj == null) throw new ArgumentNullException("obj");
			var ntod = obj as IPAFNamedAndTypedObject;
			if (ntod == null) throw new ArgumentException("Not a 'IPAFNamedAndTypedObject'");
			// Types must be resolved.
			var interfaceType = ntod.ObjectType;
			if (interfaceType == null)
			{
				var data = new PAFSED(ntod.GetServiceDescriptionInterface());
				throw new PAFStandardException<IPAFSED>(
                    data, PAFServiceExceptionMessageTags.TYPE_IS_NOT_RESOLVED);
			}
			return interfaceType.GetHashCode();
		}
	}
}
