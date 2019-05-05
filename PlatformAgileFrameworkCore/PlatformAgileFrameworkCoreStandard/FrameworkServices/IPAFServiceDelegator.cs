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
namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	///	Interface for a service that can delegate its functions to another type.
	/// </summary>
	/// <typeparam name="T">
	/// Generic type that implements <see cref="IPAFService"/> and can be
	/// more general than <typeparamref name="U"/>
	/// </typeparam>
	/// <typeparam name="U">
	/// A framework service.
	/// </typeparam>
	public interface IPAFServiceDelegator<in T, out U> where T: IPAFService, U
		where U: IPAFService
	{
		/// <summary>
		/// A service that is attached to the implementing type to provide the actual service
		/// functionality that the class will forward calls to.
		/// </summary>
		T ServiceDelegate { set; }
		/// <summary>
		/// Returns a service that the external callers can request to make calls on. Should
		/// return <see langword="null"/> if <see cref="ServiceDelegate"/> is not installed.
		/// </summary>
		U DelegatedService { get; }
	}
}