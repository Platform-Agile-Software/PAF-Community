﻿using System;
using NUnit.Framework;
using PlatformAgileFramework.Events.EventTestHelpers;
using PlatformAgileFramework.FrameworkServices.Tests;
using PlatformAgileFramework.Notification.SubscriberStores;
using PlatformAgileFramework.Notification.SubscriberStores.EventSubscriberStores;

namespace PlatformAgileFramework.Events
{
	/// <summary>
	/// Tests for subscribing and releasing weak event handlers.
	/// Generic version. Here we also test an unclosed Generic subscriber's capability
	/// to pull the proper data out of the payloads. This uses the "standard style" of
	/// events, where event args are directly exposed.
	/// </summary>
	[TestFixture]
	public class WeakUnconstrainedGenericEventTests : BasicServiceManagerTestFixtureBase
	{
		/// <summary>
		/// The subscriber will add to this count.
		/// </summary>
		public static int s_SumOfInts;

		/// <summary>
		/// This indicates that the subscriber was finalized.
		/// </summary>
		public static int s_NumSubscribersFinalized;

		public static int s_EventsTriggered;
		public WeakGenericEventSubscriberTestClass<object> m_ObjectSubscriber;
		public WeakGenericEventSubscriberTestClass<int> m_IntSubscriber;

		public IPayloadWeakableSubscriberStore<Action<object, object>, object>
			m_ObjectPublisher;

		public IPayloadWeakableSubscriberStore<Action<object, int>, int>
			m_IntPublisher;

		public WeakUnconstrainedGenericEventTests()
		{
			m_ObjectSubscriber = new WeakGenericEventSubscriberTestClass<object>();
			m_ObjectPublisher = new GenericEventArgsSubscriberStore<object, WeakUnconstrainedGenericEventTests>(this);
			// This will box an integer.
			m_ObjectPublisher.Payload = 1;

			m_IntSubscriber = new WeakGenericEventSubscriberTestClass<int>();
			m_IntPublisher = new GenericEventArgsSubscriberStore<int, WeakUnconstrainedGenericEventTests>(this);
			// This just loads an int into an int.
			m_IntPublisher.Payload = 1;

		}

		/// <summary>
		/// Gets around the problem of NUnit being crippled without testfixture setups.
		/// Base manages the fixture setup. This methods dumps all the subscribers in both stores.
		/// </summary>
		[SetUp]
		public override void SetUp()
		{
			m_ObjectPublisher.ClearSubscribers();
			m_IntPublisher.ClearSubscribers();
			s_EventsTriggered = 0;

			base.SetUp();
		}

		/// <summary>
		/// This one tests two subscribers to see if they can process
		/// int/object payloads correctly and the publishers can clear
		/// their subscribers.
		/// </summary>
		[Test]
		public void TestGenericAndSubscriptionClearing()
		{

			// Hook up our subscribers.
			m_ObjectSubscriber.WeaklySubscribe(m_ObjectPublisher);
			m_IntSubscriber.WeaklySubscribe(m_IntPublisher);

			// Do four notifcations.
			m_ObjectPublisher.NotifySubscribers();
			m_IntPublisher.NotifySubscribers();
			m_ObjectPublisher.NotifySubscribers();
			m_IntPublisher.NotifySubscribers();

			Assert.IsTrue(s_EventsTriggered == 4, "s_EventTriggered == 4");

			// Clear subscribers. 
			m_ObjectPublisher.ClearSubscribers();
			m_IntPublisher.ClearSubscribers();

 
			// Subscribers should be vanished by now.
			m_ObjectPublisher.NotifySubscribers();
			m_IntPublisher.NotifySubscribers();

			// Count should not change.
			Assert.IsTrue(s_EventsTriggered == 4, "s_EventTriggered == 4 (still)");
		}
	}
}
