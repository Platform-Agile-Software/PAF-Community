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

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.TypeHandling.Disposal.Tests
{
	/// <summary>
	/// <para>
	/// Singleton class to illustrate hiding the ability to
	/// dispose the <see cref="IDisposable"/>.
	/// </para>
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 19mar2019 </date>
	/// <description>
	/// Rebuilt this demo class as a lazy singleton.
	/// Lazy singleton allows statics to be set by applications before instantiation.
	/// </description>
	/// </contribution>
	/// </history>
	public class DisposableSingletonDemoClass
	{
		#region Class Fields and AutoProps
		/// <summary>
		/// Holds a reference to the instance if constructed. Internal
		/// for testing.
		/// </summary>
		internal static DisposableSingletonDemoClass s_InstanceReference { get; set; }
		/// <summary>
		/// This is a weird one for sure. Set it so it can be read internally.
		/// Can be pushed on to the class before construction.
		/// </summary>
		public static Guid SecretKey { internal get; set; }
		/// <summary>
		/// This a thread-safe wrapper for constructing the singleton.
		/// </summary>
		/// <remarks>
		/// Lazy class is thread-safe by default. Internal for testing.
		/// </remarks>
		internal static Lazy<DisposableSingletonDemoClass> s_Singleton =
			new Lazy<DisposableSingletonDemoClass>(ConstructSingletonDemoClass);
		#region Constructors
		/// <summary>
		/// Not quite a constructor - a factory for the lazy construction.
		/// </summary>
		private static DisposableSingletonDemoClass ConstructSingletonDemoClass()
		{
			s_InstanceReference = new DisposableSingletonDemoClass();

			// Register for disposal using the secret key that was pushed in.
			DisposalRegistry.RegisterForDisposal(PrivateDispose, SecretKey);
			return s_InstanceReference;
		}

		/// <summary>
		/// Private constructor enforces the singleton condition.
		/// </summary>
		private DisposableSingletonDemoClass()
		{
			// Put things here that need to built for the instance.
		}
		#endregion
		#region Properties
		/// <summary>
		/// Get the singleton instance of the class.
		/// </summary>
		/// <returns>The singleton.</returns>
		public static DisposableSingletonDemoClass Instance => s_Singleton.Value;
		#endregion
		internal void ReleaseUnmanagedResources()
		{
			// TODO release unmanaged resources here
		}
		/// <summary>
		/// Typical Dispose method can be static or instance.
		/// Static allows us to skip disposal if class was never
		/// instantiated.
		/// </summary>
		/// <param name="disposing">The usual thing....</param>
		private static void PrivateDispose(bool disposing)
		{
			if (s_InstanceReference == null)
				return;
			s_InstanceReference.ReleaseUnmanagedResources();
			if (disposing)
			{
			}

			// We don't even really care that much if a singleton instance
			// is held in memory, but we clean it up to be neat.
			s_Singleton = null;
			s_InstanceReference = null;
		}
		/// <summary>
		/// Typical Dispose method can be static or instance.
		/// Static allows us to skip disposal if class was never
		/// instantiated.
		/// </summary>
		private static void PrivateDispose()
		{
			if (s_InstanceReference == null)
				return;
			PrivateDispose(true);
			GC.SuppressFinalize(s_InstanceReference);
		}
		~DisposableSingletonDemoClass()
		{
			PrivateDispose(false);
		}
	}
	#endregion
}
