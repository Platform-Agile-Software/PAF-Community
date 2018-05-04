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

using System;

namespace PlatformAgileFramework.TypeHandling.Disposal
{
	/// <summary>
	/// Basic utilities to support object disposal.
	/// </summary>
	public class PAFDisposalUtils
	{
		#region Methods
		/// <summary>
		/// This helper acts upon a disposable instance to dispose it and catches any
		/// disposal exceptions. It optionally <see langword="null"/>s the instance.
		/// </summary>
		/// <typeparam name="U">
		/// This is the type to be disposed. It must be a reference type (so we can
		/// <see langword="null"/> it and must be <see cref="IDisposable"/> so we can dispose it.
		/// </typeparam>"/>
		/// <param name="disposable">
		/// This is the <see cref="IDisposable"/> instance. <see langword="null"/> exits without
		/// processing.
		/// </param>
		/// <param name="nullTheInstance">
		/// <see langword="true"/> to <see langword="null"/> the instance regardless of whether we get
		/// an exception or not.
		/// </param>
		/// <returns>
		/// An exception that may have occurred in the dispose method. <see langword="null"/>
		/// indicates all went well.
		/// </returns>
		public static Exception Disposer<U>(ref U disposable, bool nullTheInstance)
			where U: class, IDisposable
		{
			if (disposable == null) return null;
			Exception caughtException = null;
			try {
				disposable.Dispose();
			}
			catch (Exception ex)
			{
				caughtException = ex;
			}
			if (nullTheInstance) disposable = null;
			return caughtException;
		}
		/// <summary>
		/// Little helper just throws an exception when we try to access the class
		/// after disposal.
		/// </summary>
		/// <param name="typeName">
		/// The name of the type that is already disposed. Can be
		/// <see langword="null"/> or blank.
		/// </param>
		/// <param name="oneForDisposed">
		/// Integaer that has a value of 1 if the type is disposed.
		/// </param>
		public static void DisposalGuard(string typeName, int oneForDisposed)
		{
			if (typeName == null) typeName = "";
			if (oneForDisposed == 1)
				throw new ObjectDisposedException(typeName + " instance has been disposed");
		}
		#endregion // Methods
	}
}

