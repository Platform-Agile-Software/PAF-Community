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

namespace PlatformAgileFramework.Execution.Pipeline
{
	/// <summary>
	/// This interface prescribes methods which PAF application classes must implement to
	/// participate in the "execution pipeline". The naming convention used
	/// here anticipates the extension of the interface to include specific additional
	/// stages in the pipeline.
	/// </summary>
	/// <typeparam name="T">The actual application parameters.</typeparam>
	public interface IPAFBaseExePipeline<T> : IPAFBaseExePipelineInitialize<T>
		where T: class
	{
		#region Properties
		/// <summary>
		/// Provides an indication of whether the type has already been uninitialized.
		/// This will be <see langword="true"/> only after the <see cref="UninitializeExePipeline"/>
		/// method has been called. If this property is <see langword="true"/> it generally means
		/// that the PAF component can no longer be accessed.
		/// </summary>
		bool IsExePipelineUninitialized { get; }
		/// <summary>
		/// Provides an indication of whether the type has run at least once.
		/// </summary>
		bool HasPipelinedObjectRun { get; }
		/// <summary>
		/// Provides an indication of whether the type is currently executing.
		/// For scenarios where multiple threads are accessing the pipelined object.
		/// </summary>
		bool IsPipelinedObjectRunning { get; }
		#endregion // Properties
		#region Methods
		/// <summary>
		/// Runs (executes) the object.
		/// </summary>
		/// <param name="obj">
		/// Optional data.
		/// </param>
		void RunPipelinedObject(object obj);
		/// <summary>
		/// Provides an Uninitialization function where things can be shut down.
		/// This can often be used to call a dispose method, but can be different if
		/// needed. One scenario is when the type is expensive to create and it's
		/// desired to undo it's parameterization, then redo it's parameterization
		/// and run it again.
		/// </summary>
		void UninitializeExePipeline();
		#endregion // Methods
	}
}
