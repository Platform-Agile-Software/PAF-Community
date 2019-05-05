//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2005 - 2017 Icucom Corporation
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
using PlatformAgileFramework.Notification.Connectors;

namespace PlatformAgileFramework.Connections.BaseConnectors
{
    /// <summary>
    /// This is a base class for a connector in PAF. The simplest implementation
    /// of <see cref="IPAFConnector"/>. This class was retrofitted to have virtual
    /// props to support the overlay of Generics sometime around 2007.
    /// </summary>
    /// <history>
    /// <contribution>
    /// <author> KRM </author>
    /// <date> 04jun2013 </date>
    /// <description>
    /// Documented this class fully. Added history.
    /// </description>
    /// </contribution>
    /// </history>
    // ReSharper disable once PartialTypeWithSinglePart
	//// NOTE: KRM - this class still has some explicit implementation wierdness that
	// I can't see a reason for....... 23jan2018
	public partial class PAFConnector : IPAFConnector
	{
        #region Class Fields and Autoproperties
        /// <summary>
        /// Disposal flag must be volatile for thread safety.
        /// </summary>
        private volatile bool m_IsDisposed;
		/// <summary>
		/// Backing for the source prop.
		/// </summary>
		private object m_Source;
		/// <summary>
		/// Backing for the sink prop.
		/// </summary>
		private object m_Sink;
		/// <summary>
		/// Backing.
		/// </summary>
		private TransferDirection m_DataTransferDirection;

		private readonly IPAFConnector m_MeAsMyInterface;
		#endregion // Class Fields and Autoproperties
		#region Constructors

		/// <summary>
		/// Default doesn't do a thing except set an interface reference.
		/// </summary>
		public PAFConnector()
		{
			m_MeAsMyInterface = this;
		}

		/// <summary>
		/// Constructor loads properties.
		/// </summary>
		/// <param name="source">
		/// Loads <see cref="IPAFConnector.UniDirectionalSource"/>. Not <see langword="null"/>.
		/// </param>
		/// <param name="sink">
		/// Loads <see cref="IPAFConnector.UniDirectionalSink"/>.  Not <see langword="null"/>.
		/// </param>
		/// <param name="dataTransferDirection">
		/// Loads <see cref="IPAFConnector.DataTransferDirection"/>.
		/// <see langword="null"/> is two-way.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">"source"</exception>
		/// <exception cref="ArgumentNullException">"sink"</exception>
		/// </exceptions>
		public PAFConnector(object source, object sink, TransferDirection dataTransferDirection = null)
		:this()
		{
			m_MeAsMyInterface.UniDirectionalSource = source;
			m_MeAsMyInterface.UniDirectionalSink = sink;
			if(dataTransferDirection == null)
				dataTransferDirection = TransferDirection.TWO_WAY;
			m_MeAsMyInterface.DataTransferDirection = dataTransferDirection;
		}
		#endregion // Constructors

		#region IPAFConnector implementation
		object IPAFConnector.UniDirectionalSource
        { get { return m_Source; } set { m_Source = value; } }
		object IPAFConnector.UniDirectionalSink
		{ get { return m_Sink; } set { m_Sink = value; } }
		public virtual TransferDirection DataTransferDirection
		{
			get { return m_DataTransferDirection; }
			set { m_DataTransferDirection = value; }
		}

		#endregion // IPAFConnector implementation
        /// <summary>
        /// Backing for the interface.
        /// </summary>
		protected virtual object UniDirectionalSourcePV
		{
			get { return m_Source; }
			set { m_Source = value; }
		}
		/// <summary>
		/// Backing for the interface.
		/// </summary>
		protected virtual object UniDirectionalSinkPV
		{
			get { return m_Sink; }
			set { m_Sink = value; }
		}
		#region IDispose implementation
		public void Dispose()
		{
			if (m_IsDisposed)
				return;
			m_IsDisposed = true;
			Dispose(true);
			// We manually disposed stuff, so we needn't be finalized.
			GC.SuppressFinalize(this);
		}
        #endregion // IDispose implementation
        protected virtual void Dispose(bool disposing)
		{

		}
	}
}
