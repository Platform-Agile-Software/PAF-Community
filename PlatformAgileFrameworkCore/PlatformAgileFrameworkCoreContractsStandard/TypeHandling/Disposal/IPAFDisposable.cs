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
using System.Security;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Security;

namespace PlatformAgileFramework.TypeHandling.Disposal
{
	#region Delegates
	/// <summary>
	/// This is the signature for a method that will call <see cref="IPAFDisposable.PAFDispose"/>
	/// in conjunction with an <see cref="IPAFDisposable"/> implementation.
	/// </summary>
	/// <param name="disposing">
	/// Should be <see langword="true"/> when called from <see cref="IDisposable.Dispose"/>.
	/// </param>
	/// <param name="obj">
	/// Arbitrary payload. This argument can be <see langword="null"/> depending on the
	/// implementation.
	/// </param>
	/// <returns>
	/// Implementors may elect to return an exception if one occurs in the
	/// disposal process. An <see cref="PAFStandardException{PAFAggregateExceptionData}"/>
	/// is helpful here.
	/// </returns>
	public delegate Exception PAFDisposerMethod(bool disposing, object obj);
	/// <summary>
	/// This is the signature for the ordinary <see cref="IDisposable.Dispose"/> method.
	/// </summary>
	public delegate void DisposeMethod();
	#endregion// Delegates
	/// <summary>
	/// Interface supporting the PAF dispose pattern.
	/// </summary>
	/// <history>
	/// <author> KRM </author>
	/// <date> 29apr2012 </date>
	/// <contribution>
	/// Factored out of typehandling for use in core. Anybody doing .Net
	/// programming should have what's left in here.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Any implementations should NORMALLY be made thread-safe.
	/// </threadsafety>
	/// <remarks>
	/// Interface is partial for extension in full-trust environments.
	/// </remarks>
// ReSharper disable PartialTypeWithSinglePart
	public partial interface IPAFDisposable: IDisposable
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Properties
		/// <summary>
		/// Determines if an object has already been disposed.
		/// </summary>
		bool IsPAFObjectDisposed { get; }
		#endregion // Properties
		#region Methods
		/// <summary>
		/// Allows the fetch of the internal dispose method that is not decorated with
		/// the <see cref="SecurityCriticalAttribute"/>.
		/// </summary>
		/// <param name="obj">
		/// An additional parameter that can carry/accept information about the dispose operation.
		/// This parameter may be <see langword="null"/> and implementations should expect that. It is
		/// often a <see cref="IPAFSecretKey"/>.
		/// </param>
		/// <returns>
		/// <see langword="null"/> if an internal security check fails.
		/// <see langword="null"/>if the object has already been disposed..
		/// </returns>
		DisposeMethod GetDisposeMethod(object obj);
		/// <summary>
		/// This method is normally called by both the <see cref="IDisposable.Dispose"/> method
		/// and by the finalizer if one is implemented. See standard references for the
		/// logic involved in freeing unmanaged versus managed resources.
		/// </summary>
		/// <param name="disposing">
		/// This variable is traditionally set to <see langword="true"/> if the method is called
		/// from <see cref="IDisposable.Dispose"/> to indicate that managed objects can be
		/// accessed (e.g. to have their <see cref="IDisposable"/> implementations called.
		/// </param>
		/// <param name="obj">
		/// An additional parameter that can carry/accept information about the dispose operation.
		/// This parameter may be <see langword="null"/> and implementations should expect that. It is
		/// often a <see cref="IPAFDisposable"/>, but in many cases it is something
		/// different entirely, such as a <see cref="IPAFSecretKey"/>.
		/// </param>
		/// <remarks>
		/// <para>
		/// The usual rules for calling base classes' PAFDispose should be followed in derived
		/// classes. It should thus be implemented as a virtual method.
		/// </para>
		/// <returns>
		/// Implementors may elect to return an exception if one occurs in the
		/// disposal process. An <see cref="PAFStandardException{PAFAggregateExceptionData}"/>
		/// is helpful here.
		/// </returns>
		/// </remarks>
		/// <exceptions>
		/// Dispose operations should not throw exceptions or allow exceptions to be thrown
		/// capriciously. In many scenarios, it is necessary to examine the type of exception
		/// that is being thrown and the circumstances under which it is thrown to determine
		/// whether the application should be terminated. Generally speaking, dispose operations
		/// should not terminate if an exception is encountered during the disposal of a single
		/// resource. It is useful to catch and catalog any thrown exceptions by wrapping child
		/// disposal operations in try/catch blocks. With this technique, the dispose call does
		/// not terminate on the first exception encountered. At the end of the <see cref="PAFDispose"/>
		/// method, an assessment can be made as to whether the situation requires application shutdown
		/// or merely a log entry or some other action. Of course, this logic/criteria can
		/// vary, depending on whether the application is in service or under test.
		/// </exceptions>
		Exception PAFDispose(bool disposing, object obj);
		/// <summary>
		/// Allows the disposal function for the Type to indicate the object is disposed. This
		/// method is intended to employ some sort of synchronized lock on a "disposed" flag.
		/// </summary>
		/// <param name="obj">
		/// An additional parameter that can carry/accept information about the dispose operation.
		/// This parameter may be <see langword="null"/> and implementations should expect that. It is
		/// often a <see cref="IPAFDisposable"/>, but in many cases it is something
		/// different entirely, such as a <see cref="IPAFSecretKey"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if this object was NOT already disposed by another thread. If
		/// <see langword="true"/>, the object can be disposed by the calling thread.
		/// </returns>
		bool SetPAFObjectDisposed(object obj);
		#endregion // Methods
	}

}

