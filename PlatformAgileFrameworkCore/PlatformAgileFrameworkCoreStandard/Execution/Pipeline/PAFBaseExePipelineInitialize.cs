
using System;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.Execution.Pipeline
{
	/// <summary>
	/// Default implementation of the interface. Often used as a nested class.
	/// </summary>
	/// <remarks>
	/// See the interface.
	/// </remarks>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 21aug2015 </date>
	/// <description>
	/// Took out synchronization - this is to be used in applications where the generic
	/// is either immutable or synchronized.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> BMC </author>
	/// <date> 21aug2011 </date>
	/// <description>
	/// Built for nested class support.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// This implementation is not thread-safe. Normally only one thread will be
	/// accessing this class.
	/// </threadsafety>
	public abstract class PAFBaseExePipelineInitialize<T>: IPAFBaseExeParameterizedPipelineInitialize<T>
		where T : class
	{
		#region Clsss Fields
		/// <summary>
		/// The payload.
		/// </summary>
		private IPAFPipelineParams<T> m_PipelineParams;
		#endregion // Clsss Fields
		#region Constructors
		/// <summary>
		/// Constructor builds with props.
		/// </summary>
		/// <param name="pipelineParams">Parameter-providing interface.</param>
		protected PAFBaseExePipelineInitialize(IPAFPipelineParams<T> pipelineParams)
		{
			m_PipelineParams = pipelineParams;
		}

		/// <summary>
		/// Constructor builds with raw props. Builds an internal <see cref="PAFPipelineParams{T}"/>.
		/// </summary>
		/// <param name="pipelineParam">Generic pipeline param.</param>
		/// <param name="shouldInitializeAfterConstruction">
		/// See <see cref="IPAFPipelineParams{T}"/>.
		/// </param>
		protected PAFBaseExePipelineInitialize(T pipelineParam,
			bool shouldInitializeAfterConstruction = false)
		{
			var pipelineClass = new PAFPipelineParams<T>(pipelineParam, 
				shouldInitializeAfterConstruction);
			PipelineParams = pipelineClass;
		}

		#endregion Constructors
		#region Properties
		/// <summary>
		/// <see cref="IPAFBaseExePipelineInitialize{T}"/>.
		/// </summary>
		public bool IsExePipelineInitialized { get; protected set; }
		/// <summary>
		/// <see cref="IPAFBaseExePipelineInitialize{T}"/>.
		/// </summary>
		public IPAFPipelineParams<T> PipelineParams { get { return m_PipelineParams; }
			protected set { m_PipelineParams = value; } }
		#endregion // Properties
		#region Methods
		/// <remarks>
		/// <see cref="IPAFBaseExePipelineInitialize{T}"/>.
		/// </remarks>
		public abstract void InitializeExePipeline(IPAFProviderPattern<IPAFPipelineParams<T>> provider);
		#endregion // Methods

		#region IPAFProviderPattern<IPAFPipelineParams<T>> Implementation
		/// <remarks>
		/// See <see cref="IPAFProviderPattern{IPAFPipelineParams}"/>
		/// </remarks>
		public IPAFPipelineParams<T> ProvidedItem
		{
			get { return m_PipelineParams; }
		}
		/// <remarks>
		/// See <see cref="IPAFProviderPattern{IPAFPipelineParams}"/>. In this class, item
		/// is a reference type, so we always return <see langword="true"/>.
		/// </remarks>
		public bool TryGetProvidedItem(out IPAFPipelineParams<T> item)
		{
			item = m_PipelineParams;
			return true;
		}
		#endregion // IPAFProviderPattern<IPAFPipelineParams<T>> Implementation
	}
}
