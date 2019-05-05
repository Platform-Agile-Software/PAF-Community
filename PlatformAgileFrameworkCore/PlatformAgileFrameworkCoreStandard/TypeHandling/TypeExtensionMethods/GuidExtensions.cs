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

#endregion

namespace PlatformAgileFramework.TypeHandling.TypeExtensionMethods
{
	/// <summary>
	/// This class implements extensions for Guids.
	/// </summary>
	/// <history>
	/// <author> DAP </author>
	/// <date> 02jul2011 </date>
	/// <contribution>
	/// Wanted to make sure WHOEVER's implementation we were using, we
	/// didn't get empty Guids.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Safe.
	/// </threadsafety>
// ReSharper disable PartialTypeWithSinglePart
// For Silverlight.
	public static partial class GuidExtensions
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Methods
		/// <summary>
		/// This method generates a non-empty Guid. Not really an extension
		/// method, but this is a good place for it.
		/// </summary>
		/// <returns>
		/// A non - empty Guid.
		/// </returns>
		public static Guid RandomNonEmptyGuid()
		{
			Guid possibleGuid;
			do
			{
				possibleGuid = Guid.NewGuid();
			}while (possibleGuid == Guid.Empty);
			return possibleGuid;
		}
		#endregion // Methods
	}
}
