using System;
using System.Threading;
using PlatformAgileFramework.Notification.SubscriberStores;
using PlatformAgileFramework.Properties;

namespace PlatformAgileFramework.Events.EventTestHelpers
{
	/// <summary>
	/// This is a "subscriber" class that takes an unconstrained Generic as
	/// the payload.
	/// </summary>
	/// <typeparam name="TPayload">Any type.</typeparam>
	public class WeakGenericEventSubscriberTestClass<TPayload>
	{
		private void LogEventOccurance(object obj, IPAFEventArgsProvider<TPayload> args)
		{
			// We have to use an interlocked method, since this poor
			// test class might get invoked on multiple threads.
			Interlocked.Add(ref WeakUnconstrainedGenericEventTests.s_EventsTriggered, 1);
		}

		public void WeaklySubscribe(
			[NotNull] IPayloadWeakableSubscriberStore<Action<object, IPAFEventArgsProvider<TPayload>>, TPayload> publisher)
		{
			publisher.WeaklySubscribe(Execute);
		}

		public void StronglySubscribe(
			[NotNull] IPayloadWeakableSubscriberStore<Action<object, IPAFEventArgsProvider<TPayload>>, TPayload> publisher)
		{
			publisher.Subscribe(Execute, false);
		}

		/// <summary>
		/// In this method, we fetch the payload. We then add it to the sum.
		/// The payload must be an <see cref="int"/>, either as a value type or a boxed value type.
		/// </summary>
		/// <param name="obj">Unused.</param>
		/// <param name="args">The generic payload interface to examine.</param>
		public void Execute(object obj, [NotNull] IPAFEventArgsProvider<TPayload> args)
		{
			var payload = args.Value;

			// Can't cast an open Generic.
			// This is good for value type payloads, since
			// a boxing is not required.
			var valueTypePayload = payload as ValueType;
			
			// ReSharper disable once PossibleNullReferenceException
			//  -- Constraint specified in Docs.
			var result = (int)valueTypePayload;

			Interlocked.Add(ref WeakUnconstrainedGenericEventTests.s_SumOfInts, result);
			LogEventOccurance(obj, args);
		}

		~WeakGenericEventSubscriberTestClass()
		{
			// We have to use an interlocked method, since this poor
			// test class might get invoked on multiple threads.
			Interlocked.Add(ref WeakUnconstrainedGenericEventTests.s_NumSubscribersFinalized, 1);
		}
	}

}