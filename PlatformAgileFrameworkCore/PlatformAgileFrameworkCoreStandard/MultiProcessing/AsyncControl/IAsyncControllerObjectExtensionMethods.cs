//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2019 Icucom Corporation
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

using System.Linq;
using PlatformAgileFramework.Annotations;
using PlatformAgileFramework.Collections;

namespace PlatformAgileFramework.MultiProcessing.AsyncControl
{
	/// <summary>
	/// Extension methods for the <see cref="IAsyncControllerObject"/> interface. Put
	/// them here to avoid too much clutter in <see cref="IAsyncControllerObject"/>.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 04apr2019 </date>
	/// <description>
	/// New.
	/// </description>
	/// </contribution>
	/// </history>
	// ReSharper disable once InconsistentNaming
	public static class IAsyncControllerObjectExtensionMethods
	{
		/// <summary>
		/// Checks to see if all children are terminated.
		/// </summary>
		/// <param name="aco">The controller object.</param>
		public static bool AreChildrenTerminated([NotNull] this IAsyncControllerObject aco)
		{
			if (aco.ControlObjects.SafeCount() == 0)
				return true;

			var retval = aco.ControlObjects.All(controlObject => controlObject.ProcessHasTerminated);

			return retval;
		}
		/// <summary>
		/// Checks to see if all children are started.
		/// </summary>
		/// <param name="aco">The controller object.</param>
		public static bool AreChildrenStarted([NotNull] this IAsyncControllerObject aco)
		{
			if (aco.ControlObjects.SafeCount() == 0)
				return true;

			return aco.ControlObjects.All(controlObject => controlObject.ProcessHasStarted);
		}
	}
}
