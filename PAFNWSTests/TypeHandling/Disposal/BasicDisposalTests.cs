//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2019 Icucom Corporation
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
using NUnit.Framework;
using PlatformAgileFramework.TypeHandling.Disposal.Tests;
namespace PlatformAgileFramework.TypeHandling.Disposal
{
	/// <summary>
	/// Tests to show how the disposal mechanism is to be used.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 16mar2019 </date>
	/// <description>
	/// New. Putting sanitized tests for the disposal mechanism
	/// into PAF Community for release.
	/// </description>
	/// </contribution>
	/// </history>
	/// <remarks>
	/// We now use ONLY <see cref="Guid"/>s for secret keys in PAF Community.
	/// </remarks>
	[TestFixture]
	public class BasicDisposalTests
	{
		#region Fields and AutoProps
		public static Guid s_DisposalGuid1 =
			Guid.NewGuid();
		public static Guid s_DisposalGuid2 =
			Guid.NewGuid();
		public static Guid s_SingletonDisposalGuid =
			Guid.NewGuid();
		#endregion // Fields and AutoProps
		#region Methods
		/// <summary>
		/// This test is designed to check that we can dispose two static
		/// classes correctly and observe any disposal errors.
		/// </summary>
		[Test]
		public void DisposeTwoStaticsByDisposables()
		{
			// We handle the disposers by their necessary interfaces.
			IPAFDisposableDisposalClientProvider staticClass1Disposable
				= new StaticClass1.DisposerClass1(s_DisposalGuid1);
			IPAFDisposableDisposalClientProvider staticClass2Disposable
				= new StaticClass2.DisposerClass2(s_DisposalGuid2);

			DisposalRegistry.RegisterForDisposal(staticClass1Disposable);
			DisposalRegistry.RegisterForDisposal(staticClass2Disposable);

			DisposalRegistry.DisposeRegistrantsInternal(s_DisposalGuid2);
			DisposalRegistry.DisposeRegistrantsInternal(s_DisposalGuid1);

			var contains1
				= DisposalRegistry.GetExceptions().Keys.Contains(typeof(StaticClass1.DisposerClass1));
			Assert.IsTrue(contains1, "Exception disposing class1");
			var contains2
				= DisposalRegistry.GetExceptions().Keys.Contains(typeof(StaticClass2.DisposerClass2));
			Assert.IsTrue(contains2, "Exception disposing class2");
		}
		/// <summary>
		/// This test is designed to check that we can dispose a singleton
		/// by passing in a private disposal method.
		/// </summary>
		[Test]
		public void DisposeASingletonByDelegate()
		{
			// Set our secret key on the static field.
			DisposableSingletonDemoClass.SecretKey = s_SingletonDisposalGuid;

			// This will construct the singleton and do the registration internally.
			var instance = DisposableSingletonDemoClass.Instance;

			// Dispose the singleton by its key.
			DisposalRegistry.DisposeRegistrantsInternal(s_SingletonDisposalGuid);

			var disposed = DisposableSingletonDemoClass.s_InstanceReference == null;
			Assert.IsTrue(disposed, "Disposable Singleton Disposed");
		}
		#endregion // Methods
	}
}

