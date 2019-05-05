using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using PlatformAgileFramework.FrameworkServices.Tests;
using PlatformAgileFramework.Notification.Helpers;
using PlatformAgileFramework.Notification.TestHelpers;

// ReSharper disable once CheckNamespace

namespace PlatformAgileFramework.Events.Tests{
	/// <summary>	/// Tests for the <see cref="RecurringActionTimer"/>.	/// </summary>    [TestFixture]	public class RecurringActionTimerTests : BasicServiceManagerTestFixtureBase	{
		/// <summary>
		/// Time factor used as a multiplier for delays;
		/// </summary>
        public static double s_TimeFactor = 1;
		// This indicates the number of calls the have been made
		// on our test method target.
		public static int s_NumCalls;
		// This indicates the number of calls on the "test body"
		// that are to be run in our multiple run test method.
		public static int s_NumRunsOfTestBodyToDo = 10;
		// This indicates the number of calls on the "test body"
		// that have actually been performed.
		public static int s_NumRunsOfTestBodyPerformed;

		/// <summary>
		/// This is an instance of our testing subclass.
		/// </summary>        public RecurringActionTimerTestingSubclass m_RecurringActionTimerTestingSubclass;
		/// <summary>		/// We just create the subclass after making sure it is		/// disposed from any prior test.		/// </summary>        [SetUp]		public override void SetUp()		{			base.SetUp();

			// Kill timer if alive.
			DisposeActionTimer();			m_RecurringActionTimerTestingSubclass				= new RecurringActionTimerTestingSubclass(TestActionTarget);			s_NumCalls = 0;		}

		/// <summary>		/// We just dispose the action timer so we don't destabilize		/// our tests.		/// </summary>        [TearDown]		public virtual void TearDown()		{
			// Kill timer if alive.
			DisposeActionTimer();		}
		/// <summary>		/// This one tests the count of calls. We have a 4 second delay		/// with a repetition interval of half a second on a vacuous method.		/// We should get about 8 calls. We also check to see if the		/// timer's internal count of calls is correct.		/// </summary>        [Test]		public void TestCallCountOnVacuousMethod()		{			m_RecurringActionTimerTestingSubclass.SetRecurranceTime((int)(500 * s_TimeFactor));			Task.Delay((int)(4000 * s_TimeFactor)).Wait();

			// Now shut the thing off.
			m_RecurringActionTimerTestingSubclass.SetRecurranceTime(0);

			// Delay a second to see if more calls accrue.
			Task.Delay((int)(1000 * s_TimeFactor)).Wait();			var isCountWithinRange = (s_NumCalls < 9) && (s_NumCalls > 7);			Assert.IsTrue(isCountWithinRange, "isCountWithinRange");			Assert.IsTrue(s_NumCalls == m_RecurringActionTimerTestingSubclass.m_NumCalls,				"Calls match");		}

		/// <summary>		/// This one tests the ability to do just one invocation		/// if passed the proper parameter.		/// </summary>        [Test]		public void TestOneShotCapability()		{			m_RecurringActionTimerTestingSubclass.SetRecurranceTime(-1);

			// We'll wait one half second - that should be plenty.
			Task.Delay((int)(500 * s_TimeFactor)).Wait();			Assert.IsTrue(s_NumCalls == 1, "Is one shot count 1");		}

		/// <summary>
		/// This one tests whether a "one-shot" activation, invoked
		/// after many activations have been made on a timer, will occur.
		/// </summary>
        [Test]
		public void TestOneShotAfterMany()
		{
			m_RecurringActionTimerTestingSubclass.SetRecurranceTime((int)(500 * s_TimeFactor));
			Task.Delay((int)(4000 * s_TimeFactor)).Wait();

			// Now shut the thing off.
			m_RecurringActionTimerTestingSubclass.SetRecurranceTime(0);

			// Delay a second to see if more calls accrue.
			Task.Delay((int)(1000 * s_TimeFactor)).Wait();

			// Now do the one-shot.
			m_RecurringActionTimerTestingSubclass.SetRecurranceTime(-1);

			// Delay a second to see if more calls accrue.
			Task.Delay((int)(1000 * s_TimeFactor)).Wait();

			var isCountWithinRange = (s_NumCalls < 10) && (s_NumCalls > 8);

			Assert.IsTrue(isCountWithinRange, "One shot after many (num calls) ");
		}
		/// <summary>
		/// This one tests whether a "one-shot" activation, invoked
		/// DURING actions running on a timer, will occur AFTER
		/// the timer has stopped. It runs the test body once.
		/// </summary>
		[Test]
		public void TestOneShotScheduledDuringTimerRunningManyTest()
		{
			TestOneShotScheduledDuringTimerRunningManyTestBody();
		}
		/// <summary>
		/// This one tests whether a "one-shot" activation, invoked
		/// DURING actions running on a timer, will occur AFTER
		/// the timer has stopped. It runs the test body multiple times.
		/// </summary>
		// This one has to wait until we reintroduce the
		// LongRunningTestAttribute. 
		[Test]
		public void TestOneShotScheduledDuringTimerRunningManyTestMultiple()
		{
			// ReSharper disable once NotAccessedVariable
			var numTimesNumCallsGT5 = 0;
			// ReSharper disable once NotAccessedVariable
			var numTimesNumCallsLT5 = 0;
			var numTimesToRun = s_NumRunsOfTestBodyToDo;
			s_NumRunsOfTestBodyPerformed = 0;
			while (numTimesToRun-- > 0)
			{
				s_NumCalls = 0;

				// Shut the timer off and wait for any possible schedules to stop.
				m_RecurringActionTimerTestingSubclass.SetRecurranceTime(0);
				Task.Delay((int)(600 * s_TimeFactor)).Wait();
				TestOneShotScheduledDuringTimerRunningManyTestBody();
				s_NumRunsOfTestBodyPerformed++;
				if (s_NumCalls > 5) numTimesNumCallsGT5++;
				if (s_NumCalls < 5) numTimesNumCallsLT5++;
			}
		}

		public void TestOneShotScheduledDuringTimerRunningManyTestBody()
		{
			m_RecurringActionTimerTestingSubclass.SetRecurranceTime((int)(500 * s_TimeFactor));
			Task.Delay((int)(1950 * s_TimeFactor)).Wait();

			// We should have time for 4 calls to have completed or started.
			// This test doesn't work when the timing is scaled down. Tests
			// that call it multiple times need to be attributed as long running.
			var isCountWithinRange = (s_NumCalls < 5) && (s_NumCalls > 3);
			Assert.IsTrue(isCountWithinRange, "One shot in the middle of many (num calls(3-4)) ");

			// Now register the one-shot.
			m_RecurringActionTimerTestingSubclass.SetRecurranceTime(-1);

			// Now shut the timer off. We should have had time for just
			// 4 activations to have occurred on the timer.
			m_RecurringActionTimerTestingSubclass.SetRecurranceTime(0);

			// Delay to wait for calls to finish.
			Task.Delay((int)(5000 * s_TimeFactor)).Wait();

			// The one shot schedule should have been "remembered" and
			// run after the scheduler is shut down.
			isCountWithinRange = s_NumCalls == 5;			Assert.IsTrue(isCountWithinRange, " (num calls(5)) ");		}
		/// <summary>		/// This one tests the ability to change schedule. We do it in the middle of 		/// an action to see that the change doesn't take effect until after		/// the action has completed.		/// </summary>        [Test]		public void TestTimerScheduleChange()		{			m_RecurringActionTimerTestingSubclass.SetRecurranceTime((int)(500 * s_TimeFactor));			Task.Delay((int)(1750 * s_TimeFactor)).Wait();

			// We should have time for about 3 calls to complete and be
			// in the middle of the 4'th. However, num calls is updated BEFORE
			// the delay, so we expect 4.
			var isCountWithinRange = s_NumCalls == 4;			Assert.IsTrue(isCountWithinRange, " (num calls(4)) ");

			// Now extend the recurrance time to 1 second.
			m_RecurringActionTimerTestingSubclass.SetRecurranceTime((int)(1000 * s_TimeFactor));
			// Wait half a second for the last of the first four actions and
			// the next one running on the new schedule to complete.
			Task.Delay((int)(500 * s_TimeFactor)).Wait();			isCountWithinRange = s_NumCalls == 5;			Assert.IsTrue(isCountWithinRange, " (num calls(5)) ");

			// Now shut it off.
			m_RecurringActionTimerTestingSubclass.SetRecurranceTime(0);

			// Now wait a couple seconds to see that the thing is not still running.
			Task.Delay((int)(2000 * s_TimeFactor)).Wait();			isCountWithinRange = s_NumCalls == 5;			Assert.IsTrue(isCountWithinRange, " (num calls(5)) (not still running) ");		}

		/// <summary>		/// This one tests the ability to change schedule. We do it in the middle of 		/// an action to see that the effect doesn't take effect until after		/// the action has completed.		/// </summary>        [Test]		public void TestStopAndChangeScheduleAndRestart()		{			m_RecurringActionTimerTestingSubclass.SetRecurranceTime((int)(500 * s_TimeFactor));			Task.Delay((int)(1250 * s_TimeFactor)).Wait();

			// We should have time for about 2 calls to complete and be
			// in the middle of the 3'th. However, num calls is updated BEFORE
			// the delay, so we expect 3.
			var isCountWithinRange = s_NumCalls == 3;			Assert.IsTrue(isCountWithinRange, " (num calls(3)) ");

			// Now shut it off. This should make it stop before the fourth
			// starts so we'll only see three.
			m_RecurringActionTimerTestingSubclass.SetRecurranceTime(0);
			// Wait half a second for completion.
			Task.Delay((int)(500 * s_TimeFactor)).Wait();			isCountWithinRange = s_NumCalls == 3;			Assert.IsTrue(isCountWithinRange, " (num calls(3) (still)) ");
			// Wait some more for completion.
			Task.Delay((int)(1000 * s_TimeFactor)).Wait();			isCountWithinRange = s_NumCalls == 3;			Assert.IsTrue(isCountWithinRange, " (num calls(3) (still still)) ");

			// Now extend the recurrance time to 1 second.
			m_RecurringActionTimerTestingSubclass.SetRecurranceTime((int)(1000 * s_TimeFactor));
			// Wait one and one half second for first task to be finished and second
			// task to be started.
			Task.Delay((int)(1500 * s_TimeFactor)).Wait();			isCountWithinRange = s_NumCalls == 5;			Assert.IsTrue(isCountWithinRange, " (num calls(5)) ");

			// Now shut it off again, in the middle of the second task.
			// This should preclude it from running the third time.
			m_RecurringActionTimerTestingSubclass.SetRecurranceTime(0);

			// Now wait a couple seconds to see that the thing is not still running.
			Task.Delay((int)(2000 * s_TimeFactor)).Wait();			isCountWithinRange = s_NumCalls == 5;			Assert.IsTrue(isCountWithinRange, " (num calls(5)) (not still running) ");		}

		/// <summary>		/// Tests whether the correct number of task creations and disposals have
		/// been made. 		/// </summary>        [Test]		public void TestProperTaskCreationAndDisposal()		{			m_RecurringActionTimerTestingSubclass.SetRecurranceTime((int)(500 * s_TimeFactor));			Task.Delay((int)(1250 * s_TimeFactor)).Wait();

			// We should have time for about 2 calls to complete and be
			// in the middle of the 3'th. We expect 1 task started but not yet disposed.
			var isCountWithinRange = m_RecurringActionTimerTestingSubclass.m_NumMainTasks == 1;
			Assert.IsTrue(isCountWithinRange, " (num main tasks(1)) ");
			isCountWithinRange = m_RecurringActionTimerTestingSubclass.m_NumDisposals == 0;
			Assert.IsTrue(isCountWithinRange, " (num disposals(0)) ");

			// Now shut it off.
			m_RecurringActionTimerTestingSubclass.SetRecurranceTime(int.MinValue);
			// Wait half a second for completion.
			Task.Delay((int)(500 * s_TimeFactor)).Wait();			isCountWithinRange = m_RecurringActionTimerTestingSubclass.m_NumDisposals == 1;			Assert.IsTrue(isCountWithinRange, " (num disposals(1)) ");

			// Now restart and wait a couple of times.
			m_RecurringActionTimerTestingSubclass.SetRecurranceTime((int)(500 * s_TimeFactor));
			// Time to start up.
			Task.Delay((int)(100 * s_TimeFactor)).Wait();
			m_RecurringActionTimerTestingSubclass.SetRecurranceTime(int.MinValue);
			// Time to quit.
			Task.Delay((int)(600 * s_TimeFactor)).Wait();
			m_RecurringActionTimerTestingSubclass.SetRecurranceTime((int)(500 * s_TimeFactor));
			Task.Delay((int)(100 * s_TimeFactor)).Wait();
			m_RecurringActionTimerTestingSubclass.SetRecurranceTime(int.MinValue);
			Task.Delay((int)(600 * s_TimeFactor)).Wait();
			isCountWithinRange = m_RecurringActionTimerTestingSubclass.m_NumMainTasks == 3;
			Assert.IsTrue(isCountWithinRange, " (num main tasks(3)) ");
			isCountWithinRange = m_RecurringActionTimerTestingSubclass.m_NumDisposals == 3;
			Assert.IsTrue(isCountWithinRange, " (num main disposals(3)) ");

		}		public void TestActionTarget()		{
			// Addition is NOT atomic.
			Interlocked.Add(ref s_NumCalls, 1);		}

		/// <summary>
		/// Just kills the timer without killing the Service base.
		/// </summary>        public void DisposeActionTimer()		{			m_RecurringActionTimerTestingSubclass?.Dispose();			m_RecurringActionTimerTestingSubclass = null;		}		public override void Dispose()		{			DisposeActionTimer();			base.Dispose();		}	}}