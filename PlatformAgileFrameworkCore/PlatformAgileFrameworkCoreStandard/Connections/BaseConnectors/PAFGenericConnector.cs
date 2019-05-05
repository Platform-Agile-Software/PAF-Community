//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2005 - 2018 Icucom Corporation
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
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Notification.Connectors;
using PlatformAgileFramework.TypeHandling;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;
using PAFTypeMismatchExceptionMessageTags = PlatformAgileFramework.TypeHandling.Exceptions.PAFTypeMismatchExceptionMessageTags;

#region Exception Shorthand
using ITMMED = PlatformAgileFramework.TypeHandling.Exceptions.IPAFTypeMismatchExceptionData;
using TMMED = PlatformAgileFramework.TypeHandling.Exceptions.PAFTypeMismatchExceptionData;
#endregion // Exception Shorthand

namespace PlatformAgileFramework.Connections.BaseConnectors
{
	/// <summary>
	/// This is a base class for a connector in PAF. The simplest implementation
	/// of <see cref="IPAFConnector{TNode1, TNode2}"/>.
	/// </summary>
	/// <typeparam name="TNode1">1 node in an arbitrary network.</typeparam>
	/// <typeparam name="TNode2">A different node in an arbitrary network.</typeparam>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 04jun2013 </date>
	/// <description>
	/// Part of the move of stuff from the circuit simulator to the general framework.
	/// Documented this class fully. Added history.
	/// </description>
	/// </contribution>
	/// </history>
	// ReSharper disable once PartialTypeWithSinglePart
	public partial class PAFConnector<TNode1, TNode2> : PAFConnector, IPAFConnector<TNode1, TNode2>
        where TNode1: class where TNode2: class
	{
		#region Class Fields and Autoprops
        /// <summary>
        /// Shortcut to our base.
        /// </summary>
		private readonly IPAFConnector m_MeAsMyBase;
		#endregion // Class Fields and Autoprops
		#region Constructors
		/// <summary>
		/// Default just staples in an interface reference.
		/// </summary>
		public PAFConnector()
		{
			m_MeAsMyBase = this;
		}
		/// <summary>
		/// Staples in an interface reference and sets all the props.
		/// </summary>
		/// <param name="node1">
		/// Loads <see cref="IPAFConnector.UniDirectionalSource"/>. Not <see langword="null"/>.
		/// </param>
		/// <param name="node2">
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
		public PAFConnector(TNode1 node1, TNode2 node2, TransferDirection dataTransferDirection = null)
			:base(node1, node2, dataTransferDirection)
		{
			m_MeAsMyBase = this;
		}
		#endregion // Constructors
		#region Generic IPAFConnector implementation
		public virtual TNode1 UniDirectionalSourceItem
		{
			get
			{
				return (TNode1)m_MeAsMyBase.UniDirectionalSource;
			}
			set
			{
				m_MeAsMyBase.UniDirectionalSource = value;
			}
		}
		public virtual TNode2 UniDirectionalSinkItem
		{
			get
			{
				return (TNode2)m_MeAsMyBase.UniDirectionalSink;
			}
			set
			{
				m_MeAsMyBase.UniDirectionalSink = value;
			}
		}
		#endregion // IPAFConnector Generic implementation
		#region Overridden Helpers.
		/// <summary>
		/// Setter checks type consistency.
		/// </summary>
		protected override object UniDirectionalSourcePV
		{
            get { return m_MeAsMyBase.UniDirectionalSource; }
			set
			{
				// Allow value to be nulled.
				if (value == null)
				{
					m_MeAsMyBase.UniDirectionalSource = null;
					return;
				}
				var typeOfValue = value.GetType();
				var typeOfTSource = typeof(TNode1);
				if (!typeOfTSource.IsTypeAssignableFrom(typeOfValue))
				{
					var data = new TMMED(PAFTypeHolder.IHolder(typeOfValue), PAFTypeHolder.IHolder(typeOfTSource));
					throw new PAFStandardException<ITMMED>(data, PAFTypeMismatchExceptionMessageTags.FIRST_TYPE_NOT_CASTABLE_TO_SECOND_TYPE);
				}
                m_MeAsMyBase.UniDirectionalSource = value;

			}
		}
		/// <summary>
		/// Setter checks type consistency.
		/// </summary>
		protected override object UniDirectionalSinkPV
		{
            get { return m_MeAsMyBase.UniDirectionalSink; }
			set
			{
                // Allow value to be nulled.
                if(value == null)
                {
					m_MeAsMyBase.UniDirectionalSink = null;
                    return;
				}
				var typeOfValue = value.GetType();
				var typeOfTSink = typeof(TNode2);
				if (!typeOfTSink.IsTypeAssignableFrom(typeOfValue))
				{
					var data = new TMMED(PAFTypeHolder.IHolder(typeOfValue), PAFTypeHolder.IHolder(typeOfTSink));
					throw new PAFStandardException<ITMMED>(data, PAFTypeMismatchExceptionMessageTags.FIRST_TYPE_NOT_CASTABLE_TO_SECOND_TYPE);
				}
                m_MeAsMyBase.UniDirectionalSink = value;
			}
		}
		#endregion //  Overridden Helpers.
		protected override void Dispose(bool disposing)
		{

		}
	}
}
