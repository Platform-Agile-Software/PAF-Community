using System;
using PlatformAgileFramework.TypeHandling;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;

namespace PlatformAgileFramework.Execution.Pipeline
{
	/// <summary>
	/// This class provides utility functions for types participating in the
	/// PAF "execution pipeline".
	/// </summary>
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
	/// <para>
	/// Completely redesigned for SL, since we no longer have access to stack info.
	/// </para>
	/// </contribution>
	/// </history>
	// Core part
// ReSharper disable PartialTypeWithSinglePart
	public static partial class PAFExecutionPipelineExtensionMethods
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Methods
		/// <summary>
		/// Provides exception service for <see cref="IPAFBaseExePipelineInitialize{T}"/>'s
		/// whose properties are attempted to be set after initialization. Throws an
		/// <see cref="InvalidOperationException"/> if the <paramref name="pipelinedObject"/>
		/// has already been initialized. This method is generally called if properties
		/// are set after initialization, but it's possible that some methods might need
		/// to be called only before initialization.
		/// </summary>
		/// <param name="pipelinedObject">
		/// The instance of the Type implementing <see cref="IPAFBaseExePipelineInitialize{T}"/>.
		///  </param>
		/// <param name="methodOrPropertyName">
		/// The name of the property or method that is being called after initialization.
		/// Can be <see langword="null"/>, but more helpful if not.
		/// </param>
		public static void ThrowExceptionOnInitialized<T>(this IPAFBaseExePipelineInitialize<T> pipelinedObject,
			string methodOrPropertyName) where T: class
		{
			if (!pipelinedObject.IsExePipelineInitialized) return;
			// TODO KRM specific exception.
			var ex = new Exception();
			throw ex;
		}
		/// <summary>
		/// Provides exception service for <see cref="IPAFBaseExePipelineInitialize{T}"/>'s
		/// whose methods are attempted to be called before initialization. Throws an
		/// <see cref="InvalidOperationException"/> if the <paramref name="pipelinedObject"/>
		/// has not been initialized. This method is generally called if methods
		/// are called before initialization, but it's possible that some properties
		/// might need to be set only after initialization.
		/// </summary>
		/// <param name="pipelinedObject">
		/// The instance of the Type implementing <see cref="IPAFBaseExePipelineInitialize{T}"/>.
		///  </param>
		/// <param name="methodOrPropertyName">
		/// The name of the property or method that is being called before initialization.
		/// Can be <see langword="null"/>, but more helpful if not.
		/// </param>
		public static void ThrowExceptionOnUninitialized<T>(
			this IPAFBaseExePipelineInitialize<T> pipelinedObject,
			string methodOrPropertyName) where T: class
		{
			if (!pipelinedObject.IsExePipelineInitialized)
			{
				// TODO KRM specific exception.
				var ex = new Exception();
				throw ex;
			}
		}
		/// <summary>
		/// Provides exception service for <see cref="IPAFBaseExePipelineInitialize{T}"/>'s
		/// whose mandatory properties have not been set or have been set to invalid
		/// values.
		/// </summary>
		/// <param name="pipelinedObject">
		/// The instance of the Type implementing <see cref="IPAFBaseExePipelineInitialize{T}"/>.
		///  </param>
		/// <param name="propertyName">
		/// The name of the property that has not been set correctly.
		/// Can be <see langword="null"/>, but more helpful if not.
		/// </param>
		/// <remarks>
		/// Unconditionally throws an exception.
		/// </remarks>
		public static void ThrowExceptionOnPropertyNotSet<T>(this IPAFBaseExePipelineInitialize<T> pipelinedObject,
			string propertyName) where T: class
		{
			// TODO KRM specific exception.
			var ex = new Exception();
			throw ex;
		}

		/// <summary>
		/// Factory to create objects that wear <see cref="IPAFBaseExePipelineInitialize{T}"/>.
		/// This factory employs reflection to instantiate and optionally initialize types
		/// that wear the interface and have a public constructor with the single argument
		/// <see cref="IPAFPipelineParams{T}"/>.
		/// </summary>
		/// <typeparam name="T">
		/// The class wearing <see cref="IPAFBaseExePipelineInitialize{T}"/>.
		/// </typeparam>
		/// <typeparam name="U">
		/// The class wearing <see cref="IPAFBaseExePipelineInitialize{T}"/>.
		/// </typeparam>
		/// <typeparam name="V">
		/// Specific application parameters.
		/// </typeparam>
		/// <param name="iPAFPipelineParams">
		/// The pipeline's <see cref="IPAFPipelineParams{V}"/>.
		/// </param>
		/// <returns>A new instance of <typeparamref name="T"/>.</returns>
		/// <remarks>
		/// <para>
		/// In accordance with the pipeline protocol, the <see cref="IPAFBaseExePipelineInitialize{V}.InitializeExePipeline"/>
		/// method is called after construction if the <paramref name="iPAFPipelineParams"/>
		/// is non-<see langword="null"/> and has the <see cref="IPAFPipelineParams{V}.ShouldInitializeAfterConstruction"/>
		/// property set.
		/// </para>
		/// <para>
		/// This method can construct reference types only.
		/// </para>
		/// </remarks>
		/// <exceptions>
		/// <exception cref="InvalidOperationException">
		/// Thrown if the type does not have the required constructor.
		/// "Can't find IPAFPipeline constructor for Type: TYPE".
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// Thrown if the type was not constructed.
		/// "Can't construct Type: TYPE".
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// Thrown if an exception occurred in the initialization phase.
		/// "Pipeline initialization failed for Type: TYPE"
		/// </exception>
		/// </exceptions>
		public static T CreatePipelineObject<T, U, V>(U iPAFPipelineParams) 
			where T: class, IPAFBaseExePipelineInitialize<V> where U: class, IPAFPipelineParams<V>
			where V: class
		{
			var typeofT = typeof (T);
			var constructorInfo = typeofT.GetCopyConstructor(ExecutionUtils.IsElevatedTrust());
			if (constructorInfo == null)
			{
				// TODO KRM specific exception.
				var ex = new Exception();
				throw ex;
			}
			T newT;
			try
			{
				newT = (T) constructorInfo.Invoke(new object[] {iPAFPipelineParams});
			}
			catch(Exception ex)
			{
				// TODO KRM specific exception.
				var errorException = new Exception(ex.Message);
				throw errorException;
			}

			if ((iPAFPipelineParams != null) && (iPAFPipelineParams.ShouldInitializeAfterConstruction))
			{
				try
				{
					var provider
						= new PAFProviderPattern<IPAFPipelineParams<V>>(iPAFPipelineParams);
					newT.InitializeExePipeline(provider);
				}
				catch(Exception ex)
				{
					// TODO KRM specific exception.
					var errorException = new Exception(ex.Message);
					throw errorException;
				}
			}
			return newT;
		}
		#endregion // Methods
	}
}
