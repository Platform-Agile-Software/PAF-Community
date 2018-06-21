using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PlatformAgileFramework.Tutorials
{
	/// <summary>
	/// Testing class demonstrating bad and good patterns of using async/await.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 24jan2016 </date>
	/// <description>
	/// New. For client tutelage.
	/// </description>
	/// </contribution>
	/// </history>
	//[TestFixture]
	public class AsyncAwaitTeachingClass
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Allows us to observe an exception from a catch block.
		/// </summary>
		public static Exception s_CatchBlockException;
		#endregion // Fields and Autoproperties
		#region Methods
		#region NUnit Methods
		/// <summary>
		/// Clears statics for each test.
		/// </summary>
		[SetUp]
		public void SetUp()
		{
			s_CatchBlockException = null;
		}
		#endregion // NUnit Methods
		#region TestMethods

		[Test]
		public void ExceptionCaptureTest()
		{
			AsyncVoidCallerWithTryCatch();
		}
		[Test]
		public void OuterExceptionCaptureTest()
		{
			try
			{
				AsyncVoidCallerCaller();
			}
			catch (Exception ex)
			{
				s_CatchBlockException = ex;
			}
		}
		#endregion // TestMethods
		/// <summary>
		/// Calls method with args for failure and success.
		/// </summary>
		/// <remarks>
		/// Problem with this method is that only the last assignment to result
		/// is used.
		/// </remarks>
		public async Task<int> AsyncIntAwaitingCaller()
		{
			var result = await TheAsyncIntMethodUnderScrutiny(2);
			// This call should throw an exception when the result is accesed.
			// The way await works is to retrow this exception on the caller's
			// SynchronizationContext.
			result = await TheAsyncIntMethodUnderScrutiny(1);
			result = await TheAsyncIntMethodUnderScrutiny(0);
			return result;
		}
		/// <summary>
		/// Calls method with args for failure and success.
		/// </summary>
		public async void AsyncVoidCaller()
		{
			var two = await TheAsyncIntMethodUnderScrutiny(2);
			// This call should throw an exception when the result is accesed.
			// The way await works is to retrow this exception on the caller's
			// SynchronizationContext.
			var one = await TheAsyncIntMethodUnderScrutiny(1);
			var zero = await TheAsyncIntMethodUnderScrutiny(0);
		}
		/// <summary>
		/// Calls <see cref="AsyncVoidCaller()"/> method.
		/// </summary>
		public void AsyncVoidCallerCaller()
		{
			AsyncVoidCaller();
		}
		/// <summary>
		/// Calls <see cref="AsyncVoidCaller()"/> method with args for failure
		/// and success and catches exceptions.
		/// </summary>
		public async void AsyncVoidCallerWithTryCatch()
		{
			try
			{
				var two = await TheAsyncIntMethodUnderScrutiny(2);
				// This call should throw an exception when the result is accesed.
				// The way await works is to retrow this exception on the caller's
				// SynchronizationContext.
				var one = await TheAsyncIntMethodUnderScrutiny(1);
				var zero = await TheAsyncIntMethodUnderScrutiny(0);
			}
			catch (Exception ex)
			{
				s_CatchBlockException = ex;
			}
		}
		/// <summary>
		/// The async method under scrutiny.
		/// </summary>
		/// <param name="testInt">
		/// 0 generates an exception. >1 returns a set result. 1 returns an
		/// exception with the exception message = "1".
		/// </param>
		/// <returns>
		/// A task with an int payload (<see cref="Task{int}"/>).
		/// </returns>
		public Task<int> TheAsyncIntMethodUnderScrutiny(int testInt)
		{
			if (testInt == 0) throw new Exception("0");
			var tcs = new TaskCompletionSource<int>();
			if (testInt == 1)
				// Note: if this exception is not "observed" before it falls out
				// of scope in certain versions of PCLs and certain versions of
				// < .Net 4.5.1, it will cause an unobserved task exception
				// to be thrown.
				tcs.SetException(new Exception("1"));
			if (testInt > 1)
				tcs.SetResult(testInt);
			return tcs.Task;
		}
		/// <summary>
		/// The async method under scrutiny.
		/// </summary>
		/// <param name="testInt">
		/// 0 generates an exception. 1 returns <see langword="false"/>. >1 returns
		/// <see langword="true"/>.
		/// </param>
		/// <returns>
		/// A task with a bool payload (<see cref="Task{bool}"/>).
		/// </returns>
		public Task<bool> TheAsyncBoolMethodUnderScrutiny(int testInt)
		{
			if (testInt == 0) throw new Exception("0");
			var tcs = new TaskCompletionSource<bool>();
			if (testInt == 1)
				// Note: if this exception is not "observed" before it falls out
				// of scope in certain versions of PCLs and certain versions of
				// < .Net 4.5.1, it will cause an unobserved task exception
				// to be thrown.
				tcs.SetException(new Exception("1"));
			if (testInt > 1)
				tcs.SetResult(true);
			return tcs.Task;
		}
		#endregion // Methods
	}
}