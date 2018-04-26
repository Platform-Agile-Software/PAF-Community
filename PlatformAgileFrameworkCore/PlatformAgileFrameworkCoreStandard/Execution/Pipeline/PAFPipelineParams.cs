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

// ReSharper disable once RedundantUsingDirective
using System;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.Execution.Pipeline
{
	/// <summary>
	/// Default implementation of <see cref="IPAFPipelineParams{T}"/>.
	/// </summary>
	/// <remarks>
	/// See the interface.
	/// </remarks>
	/// <history>
	/// <author> KRM </author>
	/// <date> 12sep2014 </date>
	/// <contribution>
	/// Added history and changed class to be immutable to close a potettial security hole.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// This implementation is not thread-safe. Normally only one thread will be
	/// accessing this class.
	/// </threadsafety>
	public class PAFPipelineParams<T>
		: IPAFPipelineParamsInternal<T> , IPAFProviderPattern<IPAFPipelineParams<T>> where T : class
	{
		#region Constructors
		/// <summary>
		/// Default for inheritors.
		/// </summary>
		public PAFPipelineParams()
		{
		}
		/// <summary>
		/// Constructor builds with params.
		/// </summary>
		public PAFPipelineParams(T applicationParameters,
			bool shouldInitializeAfterConstruction = false)
		{
			ApplicationParameters = applicationParameters;
			ShouldInitializeAfterConstruction = shouldInitializeAfterConstruction;
		}

		#endregion // Constructors
		/// <summary>
		/// See <see cref="IPAFPipelineParams{T}"/>.
		/// </summary>
		public bool ShouldInitializeAfterConstruction { get; protected set; }

		/// <summary>
		/// See <see cref="IPAFPipelineParams{T}"/>.
		/// </summary>
		public T ApplicationParameters { get; protected set; }

		/// <summary>
		/// See <see cref="IPAFPipelineParams{T}"/>.
		/// </summary>
		public IPAFPipelineParams<T> ReparameterizedCopy(T newParams)
		{
			var parameters = new PAFPipelineParams<T>();
			parameters.ApplicationParameters = newParams;
			parameters.ShouldInitializeAfterConstruction = ShouldInitializeAfterConstruction;
			return parameters;
		}

		#region Implementation of IPAFProviderPattern<IPAFPipelineParams<T>>
		/// <summary>
		/// See <c>IPAFProviderPattern{IPAFPipelineParams{T}}</c>
		/// </summary>
		public IPAFPipelineParams<T> ProvidedItem
		{
			get { return this; }
		}

		/// <summary>
		/// See <c>IPAFProviderPattern{IPAFPipelineParams{T}}</c>
		/// </summary>
		public bool TryGetProvidedItem(out IPAFPipelineParams<T> item)
		{
			item = this;
			if (ApplicationParameters == default(T))
				return false;
			return true;
		}

        #endregion // Implementation of IPAFProviderPattern<IPAFPipelineParams<T>>

        #region Implementation of IPAFProviderPatternInternal<IPAFPipelineParams<T>>
        /// <summary>
        /// See <c>IPAFProviderPatternInternal{IPAFPipelineParams{T}}</c>
        /// </summary>
        void IPAFPipelineParamsInternal<T>.SetShouldInitializeAfterConstruction(bool initialize)
	    {
	        ShouldInitializeAfterConstruction = initialize;
	    }
	    /// <summary>
	    /// See <c>IPAFProviderPatternInternal{IPAFPipelineParams{T}}</c>
	    /// </summary>

        void IPAFPipelineParamsInternal<T>.SetApplicationParameters(T parameters)
	    {
	        ApplicationParameters = parameters;
	    }
	    #endregion // Implementation of IPAFProviderPattern<IPAFPipelineParams<T>>

    }
}
