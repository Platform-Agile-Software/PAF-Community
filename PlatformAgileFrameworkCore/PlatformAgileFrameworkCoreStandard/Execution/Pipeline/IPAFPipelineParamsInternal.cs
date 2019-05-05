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

// ReSharper disable once RedundantUsingDirective
// Needed for full ECMA
using System;

namespace PlatformAgileFramework.Execution.Pipeline
{
	/// <summary>
	/// This interface allows access to set parameters that are used in certain
	/// PAF application class constructors and factories and at other stages in the
	/// pipeline. This internal interface is used mostly by test fixtures.
	/// </summary>
	/// <typeparam name="T">The actual application parameters.</typeparam>
	/// <threadsafety>
	/// The construction parameters are not normally touched by multiple threads.
	/// Thread safety needn't be guaranteed in this scenario. If they are written
	/// after construction, they must be synchronized.
	/// </threadsafety>
	internal interface IPAFPipelineParamsInternal<T>
        : IPAFPipelineParams<T> where T: class
	{
	    /// <summary>
	    /// This property tells if the class should be initialized after it is
	    /// constructed. This propery is used mostly by factories, but can be set by a test fixture.
	    /// </summary>
	    void SetShouldInitializeAfterConstruction(bool initialize);

        /// <summary>
        /// Sets the applications specific parameters without going through the
        /// <see cref="IPAFPipelineParams{T}.ReparameterizedCopy"/> path.
        /// </summary>
        void SetApplicationParameters(T parameters);
	}
}
