using System;using System.ComponentModel;using NUnit.Framework;using PlatformAgileFramework.Events.EventTestHelpers;using PlatformAgileFramework.FrameworkServices.Tests;namespace PlatformAgileFramework.Events{
	/// <summary>	/// Tests for the standard <see cref="INotifyPropertyChanged"/> event.	/// </summary>    [TestFixture]	public class MulticastNotifyPropertyChangedEventTests : BasicServiceManagerTestFixtureBase	{				/// <summary>
		/// Want to run enough broadcasts so we get an accurate timing.
		/// </summary>		public const int NUM_ITERATIONS = 10000;

		// These are two communicating peers, attempting to respond
		// to each others' property changes.		public MulticastNotifyPropertyChangedTestClass m_Peer1;
		public MulticastNotifyPropertyChangedTestClass m_Peer2;		/// <summary>		/// Build the peers and set counts.		/// </summary>        [SetUp]		public override void SetUp()		{			base.SetUp();			m_Peer1 = new MulticastNotifyPropertyChangedTestClass();			m_Peer2 = new MulticastNotifyPropertyChangedTestClass();		}

		/// <summary>		/// Need to unsubscribe and dispose.		/// </summary>        [TearDown]		public virtual void TearDown()		{			m_Peer1.Dispose();			m_Peer2.Dispose();		}		/// <summary>
		/// Every fixture has to have one test.
		/// </summary>		[Test]		public void DummyTest()		{		}		/// <summary>		/// Just a speed test.		/// </summary>		//[Test]		public void SpeedTest()		{			// Peer 1 listens to peer 2.			m_Peer1.OneWayBindToMyPeer(m_Peer2);			// Peer 2 listens to peer 1.			m_Peer2.OneWayBindToMyPeer(m_Peer1);			var startTime = DateTime.Now;			for (var iterationNum = 0; iterationNum < NUM_ITERATIONS; iterationNum++)			{				// Set peer1's info.				m_Peer1.AnAge = iterationNum*2;				// Set peer1's info.				m_Peer1.Aname = (iterationNum * 2).ToString();				// Set peer2's info.				m_Peer2.AnAge = iterationNum*2+1;				// Set peer2's info.				m_Peer2.Aname = (iterationNum * 2+1).ToString();			}			var stopTime = DateTime.Now;			var elapsedTime = stopTime - startTime;			// Just a quick and dirty way to print the time - should			// be put into a log file.			Assert.IsTrue(false, "Elapsed Time = " + elapsedTime);		}

	}}