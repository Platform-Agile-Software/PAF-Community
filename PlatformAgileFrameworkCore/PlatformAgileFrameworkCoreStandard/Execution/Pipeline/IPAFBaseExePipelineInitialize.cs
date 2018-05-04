
using System;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.Execution.Pipeline
{
	/// <summary>
	/// <para>
	/// This interface prescribes methods which PAF application classes must implement to
	/// participate in the first stage of the "execution pipeline". The naming convention used
	/// here anticipates the extension of the interface to include specific additional
	/// stages in the pipeline. The "pipeline" provides a set of standard properties and
	/// methods that help application developers avoid doing a lot of work in constructors.
	/// The basic usage pattern is to construct an object, then set its properties, then call
	/// <see cref="InitializeExePipeline"/>. This provides considerably more flexibility
	/// in parameterizing types at specific times in specific ways.
	/// </para>
	/// </summary>
	/// <typeparam name="T">The actual application parameters.</typeparam>
	/// <history>
	/// <author> BMC </author>
	/// <date> 09aug2011 </date>
	/// <contribution>
	/// <para>
	/// Added history.
	/// </para>
	/// <para>
	/// Broke this out of the full pipeline for use in core. Was needed in the peice of
	/// serialization we broke out for core.
	/// </para>
	/// </contribution>
	/// </history>
	public interface IPAFBaseExePipelineInitialize<T> where T:class
	{
		#region Properties
		/// <summary>
		/// Provides an indication of whether the type has already been initialized.
		/// </summary>
		bool IsExePipelineInitialized { get; }
		/// <summary>
		/// Storage for params passed in by pipeline constructor.
		/// </summary>
		IPAFPipelineParams<T> PipelineParams { get; }
		#endregion // Properties
		#region Methods
		/// <summary>
		/// Provides an initialization function where services are loaded and
		/// other things can be done to prepare the class for use. The initialize
		/// method can be expected to throw a variety of exceptions when mandatory
		/// properties are not set for typical implementations.
		/// </summary>
		/// <param name="provider">
		/// Allows a provider to be specified at the initialization stage.
		/// </param>
		void InitializeExePipeline(IPAFProviderPattern<IPAFPipelineParams<T>> provider);
		#endregion // Methods
	}
}
