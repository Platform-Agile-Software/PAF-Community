using NUnit.Framework;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
using PlatformAgileFramework.TypeHandling;

// Exception shorthand.
using PAFTED = PlatformAgileFramework.ErrorAndException.CoreCustomExceptions.PAFTypeExceptionData;
using PAFTEDB = PlatformAgileFramework.ErrorAndException.CoreCustomExceptions.PAFTypeExceptionDataBase;
using IPAFTED = PlatformAgileFramework.ErrorAndException.CoreCustomExceptions.IPAFTypeExceptionData;


// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.FrameworkServices.ErrorAndException.Tests
{
	[TestFixture]
	public class BasicExceptionTests
	{
		#region TryCatch
		/// <summary>
		/// This test is designed to test that PAF exceptions are caught
		/// correctly when their Generic is an interface. This is really
		/// a test of the CLR implementation. We use a completely arbitrary
		/// exception from the core.
		/// </summary>
		[Test]
		public void CatchExceptionByPayloadInterface()
		{
			bool didCatch;
			var data = new PAFTED(PAFTypeHolder.IHolder(typeof(int)));
			var exceptionToThrow = new PAFStandardException<IPAFTED>(data, PAFTypeExceptionMessageTags.TYPE_NOT_AN_INTERFACE_TYPE);
			try
			{
				throw exceptionToThrow;
			}
			catch (PAFStandardException<IPAFTED>)
			{
				didCatch = true;
			}
			Assert.IsTrue(didCatch);
		}
		/// <summary>
		/// This test is designed to test that PAF exceptions are not caught
		/// correctly when their Generic is an interface that the payload
		/// does not wear. This is really a test of the CLR implementation.
		/// We use a completely arbitrary exception from the core.
		/// </summary>
		[Test]
		public void DontCatchExceptionByInterface()
		{
			var didCatch = false;
			var data = new PAFTED(PAFTypeHolder.IHolder(typeof(int)));
			var exceptionToThrow = new PAFStandardException<IPAFTED>(data, PAFTypeExceptionMessageTags.TYPE_NOT_AN_INTERFACE_TYPE);
			try
			{
				throw exceptionToThrow;
			}
			catch (PAFStandardException<IPAFConstructorExceptionData>)
			{
				didCatch = true;
			}
				// ReSharper disable once EmptyGeneralCatchClause
			catch
			{
			}
			Assert.IsTrue(!didCatch);
		}
		/// <summary>
		/// This test is designed to test that PAF exceptions are caught
		/// correctly when their Generic is an interface. This is really
		/// a test of the CLR implementation. We use a completely arbitrary
		/// exception from the core.
		/// </summary>
		[Test]
		public void CatchExceptionByClass()
		{
			bool didCatch;
			var data = new PAFTED(PAFTypeHolder.IHolder(typeof(int)));
            var exceptionToThrow = new PAFStandardException<PAFTED>(data, PAFTypeExceptionMessageTags.TYPE_NOT_AN_INTERFACE_TYPE);
			try
			{
				throw exceptionToThrow;
			}
			catch (PAFStandardException<PAFTED>)
			{
				didCatch = true;
			}
			Assert.IsTrue(didCatch);
		}
		#endregion  //TryCatch
	}
}

