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

namespace PlatformAgileFramework.FrameworkServices
{
	#region Delegates
	/// <summary>
	/// This delegate is the common method signature that corresponds to the
	/// pipeline methods, load, initialize, etc.
	/// </summary>
	/// <param name="servicePipelineObject">
	/// Object that is passed to the pipeline methods.
	/// </param>
	public delegate void PAFServicePipelineDelegate(IPAFServicePipelineObject
		servicePipelineObject);

	/// <summary>
	/// This delegate accepts the common method signature that corresponds to the
	/// pipeline methods, load, initialize, etc.
	/// </summary>
	/// <param name="pipelineDelegate">
	/// The delegate.
	/// </param>
	/// <param name="servicePipelineObject">
	/// Object that is passed to the pipeline methods.
	/// </param>
	public delegate void PAFServicePipelineDelegateDelegate(PAFServicePipelineDelegate
		pipelineDelegate, IPAFServicePipelineObject servicePipelineObject);
	#endregion // Delegates
	/// <summary>
	/// <para>
	///	Interface for an an object providing services to services during the
	/// <see cref="IPAFServiceExtended"/> pipeline stages.
	/// </para>
	/// </summary>
	public interface IPAFServicePipelineObject
	{
		#region Properties
		/// <summary>
		/// Carries an exception that may be generated during the execution of a
		/// pipeline stage. This is <see langword="null"/> if all is well.
		/// </summary>
		Exception ExecutionException { get; set; }
		/// <summary>
		/// This is the stage of the pipeline our task is working on. Can
		/// be <see langword="null"/> if the work is not stage-related.
		/// </summary>
		ServicePipelineStage PipelineStage { get; }
		/// <summary>
		/// Returns a base service manager.
		/// </summary>
		IPAFServiceManager ServiceManager { get; }
		#endregion // Properties
	}
}