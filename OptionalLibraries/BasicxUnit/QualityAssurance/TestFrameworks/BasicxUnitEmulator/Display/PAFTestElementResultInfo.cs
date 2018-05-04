//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 -2017 Icucom Corporation
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
using System.Collections.Generic;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display
{
	/// <summary>
	/// Default implementation of <see cref="IPAFTestElementResultInfo"/>.
	/// </summary>
	/// <threadsafety>
	/// NOT thread-safe.
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 22nov2017 </date>
	/// <description>
	/// New from old - added DOCS.
	/// </description>
	/// </contribution>
	/// </history>
	public class PAFTestElementResultInfo : IPAFTestElementResultInfo
	{
		#region Fields and Autoproperties
		#region Statics
		/// <summary>
		/// Provided as a default gatherer for <see cref="IPAFTestElementInfo"/>s
		/// </summary>
		public static Func<IPAFTestElementResultInfo, IList<IPAFTestElementResultInfo>> DefaultChildGatherer { get; set; }
			= PAFTestElementResultInfoExtensions.GenerateChildResults;
		/// <summary>
		/// Provided as a default printer for <see cref="IPAFTestElementInfo"/>s.
		/// </summary>
		public static Func<IPAFTestElementResultInfo, bool, int, bool, string> DefaultCustomPrinter { get; set; }
			= (testElementResultInfo, displayChildNumber, detailLevel, printHierarchy)
                => testElementResultInfo.PrintResultAtNode(displayChildNumber, detailLevel, printHierarchy);
		#endregion // Statics
		/// <summary>
		/// Backing...
		/// </summary>
		protected IPAFTestElementInfo m_ElementInfo;
		/// <summary>
		/// Backing...
		/// </summary>
		protected bool? m_ShouldDisplay;

		/// <summary>
		/// Backing...
		/// </summary>
		protected Func<IPAFTestElementResultInfo, bool, int, bool, string>
			m_CustomPrinter = DefaultCustomPrinter;

		/// <summary>
		/// Backing...
		/// </summary>
		protected Func<IPAFTestElementResultInfo, IList<IPAFTestElementResultInfo>> 
			m_ChildGatherer = DefaultChildGatherer;

		/// <remarks>
		/// <see cref="IPAFTestElementResultInfo"/>. Initialized to
		/// -1.
		/// </remarks>
		public int ChildElementNumber { get; set; } = -1;

		/// <remarks>
		/// <see cref="IPAFTestElementResultInfo"/>. Initialized to
		/// <see langword="true"/>.
		/// </remarks>
		public bool ShouldDisplaySuccesses { get; set; } = true;
		/// <remarks>
		/// <see cref="IPAFTestElementResultInfo"/>. Initialized to
		/// <see langword="true"/>.
		/// </remarks>
		public bool ShouldDisplayFailures { get; set; } = true;
		/// <remarks>
		/// <see cref="IPAFTestElementResultInfo"/>. Initialized to
		/// <see langword="true"/>.
		/// </remarks>
		public bool ShouldDisplayInactive { get; set; } = true;


		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Default for construct and set style. Sets default custom printer
		/// and default child gatherer.
		/// </summary>
		public PAFTestElementResultInfo()
		{
		}
		/// <summary>
		/// Just wraps the info.
		/// </summary>
		/// <param name="elementInfo">wrapped info</param>
		public PAFTestElementResultInfo(IPAFTestElementInfo elementInfo)
			:this()
		{
			m_ElementInfo = elementInfo;
		}
		#endregion // Constructors
		#region Properties
        /// <summary>
        /// <see cref="IPAFTestElementResultInfo"/>
        /// </summary>
        public Func<IPAFTestElementResultInfo, bool, int, bool, string> CustomPrinter
        {
            get { return m_CustomPrinter; }
            set
            {
                m_CustomPrinter = value ?? throw new ArgumentNullException("CustomPrinter");
            }
        }

        /// <summary>
        /// <see cref="IPAFTestElementResultInfo"/>.
        /// </summary>
        public Func<IPAFTestElementResultInfo, IList<IPAFTestElementResultInfo>> DisplayChildResultGatherer
        {
            get { return m_ChildGatherer; }
            set
            {
                m_ChildGatherer = value ?? throw new ArgumentNullException("ChildGatherer");
            }
        }

		/// <remarks>
		/// <see cref="IPAFTestElementResultInfo"/>.
		/// </remarks>
		public IPAFTestElementInfo ElementInfo
		{
			get { return m_ElementInfo; }
			set { m_ElementInfo = value; }
		}
        /// <remarks>
        /// <see cref="IPAFTestElementResultInfo"/>.
        /// </remarks>
        public virtual string ElementTypeTag
        {
            get { return this.GetElementTypeTag(); }
        }
		/// <summary>
		/// <see cref="IPAFTestElementResultInfo"/>.
		/// The getter will return a value if one has been set. Otherwise it
		/// examines the state of the test and the other "ShouldDisplay"
		/// properties to figure it out. 
		/// </summary>
		public bool? ShouldDisplay
		{
			get
			{
				if (m_ShouldDisplay.HasValue)
					return m_ShouldDisplay.Value;
				if ((ElementInfo.Passed == false) && ShouldDisplayFailures)
					return true;
				if ((ElementInfo.Passed == true) && ShouldDisplaySuccesses)
					return true;
				if ((ElementInfo.TestElementStatus != TestElementRunnabilityStatus.Active)
					&& ShouldDisplayInactive)
					return true;
				return false;
			}
			set { m_ShouldDisplay = value; }
		}

		#endregion Properties
		#region Methods
		/// <summary>
		/// This override outputs a header for each node. If the node
		/// has no children, this is all that gets printed. If node has
		/// printable children, they will be printed by themselves. This
		/// is in general consonance with the use of ToString for debugger
		/// display and other things.
		/// </summary>
		/// <returns>Node Info string.</returns>
		public override string ToString()
		{
			return this.PrependToInfoString(null);
		}
		#endregion // Methods
	}
}
