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
	#region Delegates
	/// <summary>
	/// This delegate is the common method signature that corresponds to the
	/// pipeline methods, load, initialize, etc.
	/// </summary>
	/// <param name="servicePipelineObject">
	/// Object that is passed to the pipeline methods.
	/// </param>
	/// <typeparam name="T">
	/// Subinterface of <see cref="IPAFService"/>.
	/// </typeparam>
	public delegate void PAFServicePipelineDelegate<T>(IPAFServicePipelineObject<T>
		servicePipelineObject) where T: class, IPAFService;
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
	public delegate void PAFGenericServicePipelineDelegateDelegate<T>(PAFServicePipelineDelegate<T>
		pipelineDelegate, IPAFServicePipelineObject<T> servicePipelineObject)
			where T : class, IPAFService;
	#endregion // Delegates
	/// <summary>
	/// <para>
	///	Interface for an an object providing services to services during the
	/// <see cref="IPAFService"/> pipeline stages.
	/// </para>
	/// </summary>
	/// <typeparam name="T">
	/// Subinterface of <see cref="IPAFService"/>.
	/// </typeparam>
	public interface IPAFServicePipelineObject<in T>:
		IPAFServicePipelineObject where T: class, IPAFService
	{
		#region Properties
		/// <summary>
		/// Returns a subtyped service manager.
		/// </summary>
		IPAFServiceManager<T> GenericServiceManager { get; }
		#endregion // Properties
	}
}