using System;
using NUnit.Framework;

namespace PlatformAgileFramework.TypeHandling.WeakBindings
{
	/// <summary>
	/// This test fixture tests whether weak references and things using them are released
	/// properly.
	/// </summary>
	[TestFixture]
	public class BasicWeakReferenceTests
	{
		#region Fields and AutoProps
		/// <summary>
		/// Test field to indicate that reference has been released by a finalizer.
		/// </summary>
		public static bool s_WeakReferenceFinalized; 
		#endregion // Fields and AutoProps

		/// <summary>
		/// This test is designed to check that we release weak references.
		/// </summary>
		[Test]
		public void TestReleaseWeakReference()
		{
			var holder = new WeakReferenceHolder();
			var bindee = new WeakBindee();
			holder.m_WeakReference = new PAFWeakReference<WeakBindee>(bindee);

			// Shouldn't be finalized if we hold the strong reference.
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();

			Assert.IsTrue(!s_WeakReferenceFinalized);

			// Should be finalized if we release the strong reference.
			// ReSharper disable once RedundantAssignment
			bindee = null;
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();

			Assert.IsTrue(s_WeakReferenceFinalized);

		}
	}


	/// <summary>
	/// Simple class to act as a target for the binding.
	/// </summary>
	public class WeakBindee
	{
		~WeakBindee()
		{
			BasicWeakReferenceTests.s_WeakReferenceFinalized = true;
		}
	}

	/// <summary>
	/// Simple class to act as a handle for the binding.
	/// </summary>
	public class WeakReferenceHolder
	{
		public PAFWeakReference<WeakBindee> m_WeakReference;
	}
}

