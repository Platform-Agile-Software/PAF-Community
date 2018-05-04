//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 Icucom Corporation
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
using System.Reflection;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator
{
	/// <summary>
	/// Subclass holds static information for each method. Can be late-bound.
	/// </summary>
	/// <threadsafety>
	/// This subclass contains essentially read-only information except that set
	/// at time of late binding. Late binding should only be done at startup
	/// on a single thread. Otherwise the design is probably wrong.
	/// </threadsafety>
	public class PAFTestFrameworkMethodInfo : PAFTestElementInfo,
		IPAFTestMethodInfo
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal PAFTypeHolderBase m_HostType;
		/// <summary>
		/// Backing. Just use a superclass container to avoid code bloat.
		/// </summary>
		protected internal PAFMemberHolderBase m_MethodHolder;
        #endregion // Class Fields and Autoproperties
        #region Constructors
        /// <summary>
        /// Builds with necessary parameters for later construction of all members.
        /// </summary>
        /// <param name="hostType">
        /// The early or late-bound <see cref="Type"/> that the member is attached to.
        /// Can be <see langword="null"/> and actually will be ignored if <paramref name="holder"/>
        /// is early-bound.
        /// </param>
        /// <param name="holder">
        /// An early or late-bound method. Not <see langword="null"/>.
        /// </param>
        /// <param name="parent">
        /// The parent, which is usually an <see cref="IPAFTestFixtureInfo"/> in
        /// a standard xUnit configuration.
        /// </param>
        /// <param name="name">
        /// Optional name for the <see cref="IPAFTestElementInfo"/> node. Defaults to the
        /// method name.
        /// </param>
        /// <exceptions>
        /// <exception cref="ArgumentNullException">"holder"</exception>
        /// </exceptions>
		public PAFTestFrameworkMethodInfo(PAFTypeHolderBase hostType, PAFMemberHolderBase holder,
			IPAFTestElementInfo parent = null, string name = null)
			:base(GetNameFromNames(holder, name), null, parent)
		{

			// Overload for early bound.
			if (holder.MmbrInfo != null) {
				hostType = holder.HostType;
			}

			// Us.
			m_HostType = hostType;
			m_MethodHolder = holder;
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFTestMethodInfo"/>.
		/// </summary>
		/// <remarks>
		/// Virtual for remote/local peer.
		/// </remarks>
		/// <threadsafety>
		/// Unsynchronized. Should be set by only one thread at startup.
		/// </threadsafety>
		public virtual string FrameworkMethodName
		{ get { return m_MethodHolder.MemberName; } }
		/// <summary>
		/// See <see cref="IPAFTestMethodInfo"/>.
		/// </summary>
		/// <remarks>
		/// Virtual for remote/local peer.
		/// </remarks>
		/// <threadsafety>
		/// Unsynchronized. Should be set by only one thread at startup or at the
		/// time of late binding.
		/// </threadsafety>
		/// <remarks>
		/// The set method also resets the <see cref="HostType"/>
		/// to correspond to the declaring type of the method.
		/// </remarks>
		public virtual MethodInfo FrameworkMethodInfo
		{
			// Cast is OK because method info is all we can ever contain.
			get { return (MethodInfo)m_MethodHolder.MmbrInfo; }
			set
			{
				if (value != null) {
					m_HostType = value.DeclaringType;
				}
				m_MethodHolder.MmbrInfo = value;
			}
		}
		/// <summary>
		/// See <see cref="IPAFTestMethodInfo"/>.
		/// </summary>
		/// <remarks>
		/// Virtual for remote/local peer.
		/// </remarks>
		/// <threadsafety>
		/// Unsynchronized. Should be set by only one thread at startup or at the
		/// time of late binding.
		/// </threadsafety>
		public virtual PAFTypeHolderBase HostType
		{
			get { return m_HostType; }
		}
        #endregion // Properties
        #region Methods

        /// <summary>
        /// Little helper method just checks holder for null and chooses between
        /// <paramref name="name"/> and the name on the holder.
        /// </summary>
        /// <param name="holder">Not <see langword="null"/></param>
        /// <param name="name">Arbitrary string or <see langword="null"/>.</param>
        /// <returns>
        /// <paramref name="name"/> if not <see cref = "string.Empty"/> or <see langword="null"/>.
        /// <see cref="PAFMemberHolderBase.MemberName"/> otherwise.
        /// </returns>
        /// <exceptions>
        /// <exception cref="ArgumentNullException">"holder"</exception>
        /// </exceptions>
	    public static string GetNameFromNames(PAFMemberHolderBase holder, string name)
	    {
            if(holder == null) throw new ArgumentNullException("holder");
	        if (!string.IsNullOrEmpty(name)) return name;
	        return holder.MemberName;

	    }
	    #endregion // Methods
        #region Conversion Operators
        /// <summary>
        /// Calls <c>PAFTestFrameworkMethodInfo(info.DeclaringType, info)</c>.
        /// </summary>
        /// <param name="info">
        /// The info to be wrapped. Not <see langword="null"/>.
        /// </param>
        /// <returns>
        /// One of us.
        /// </returns>
        /// <exceptions>
        /// <exception cref="ArgumentNullException"> is thrown if <paramref name="info"/>.
        /// is <see langword="null"/>.
        /// </exception>
        /// </exceptions>
        public static implicit operator PAFTestFrameworkMethodInfo(MemberInfo info)
		{
			if (info == null)
				throw new ArgumentNullException("info");
			return new PAFTestFrameworkMethodInfo(info.DeclaringType, info);
		}
		#endregion // Conversion Operators
	}
}
