using System;
using PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator;

namespace Xamarin.FormsTestRunner.Models
{
    public class TestResultSummary : ITestResultSummary
	{
		#region Fields and Autoproperties
		private IPAFTestElementInfo m_TestElementInfo;
		private string m_Id;
		private string m_TestElementName;
		private string m_Status;

		#endregion Fields and Autoproperties

		#region Constructors
		/// <summary>
		/// Default for construct and set.
		/// </summary>
		public TestResultSummary()
		{
		}   
		/// <summary>
		/// Pulls props off the test info and generates a random GUID for
		/// testing.
		/// </summary>
		public TestResultSummary(IPAFTestElementInfo testInfo)
		{
			m_TestElementInfo = testInfo;
			m_Id = new Guid().ToString();
			m_Status = testInfo.TestElementStatus.ToString();
			m_TestElementName = testInfo.TestElementName;
		}


		#endregion // Constructors
		public virtual IPAFTestElementInfo TestElementInfo
		{
			get { return m_TestElementInfo; }
			set { m_TestElementInfo = value; }
		}

		public virtual string Id
		{
			get { return m_Id; }
			set { m_Id = value; }
		}

		public virtual string TestElementName
		{
			get { return m_TestElementName; }
			set { m_TestElementName = value; }
		}

		public virtual string Status
		{
			get { return m_Status; }
			set { m_Status = value; }
		}

		public IPAFTestElementInfo ProvidedItem
		{
			get { return m_TestElementInfo; }
			// ReSharper disable once ValueParameterNotUsed
			// We don't set this one.
			set { ; }
		}
	}
}
