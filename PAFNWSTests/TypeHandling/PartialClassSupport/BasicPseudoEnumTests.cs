using System;
using NUnit.Framework;

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.TypeHandling.PartialClassSupport.Tests
{
	[TestFixture]
	public class BasicPseudoEnumTests
	{
		#region Fields and AutoProps

		/// <summary>
		/// Just to show how we can prestore bit patterns from a static method,
		/// which is obviously callable by a static constructor.
		/// </summary>
		private static TestPE2 s_AllBitsOn; 
		#endregion // Fields and AutoProps
		#region Basic Construction
		/// <summary>
		/// This test is designed to check that the global dictionary is being populated.
		/// </summary>
		[Test]
		public void CheckDictionaryIsPopulated()
		{
			var dict = ExtendablePseudoEnumNonGenericBase.s_PseudoEnumValueDictionary;

			// Construct an "external" pe to use as a key.
			var newTestPE1Value = new TestPE1("TEST_NAME", 0);

			// Checks to see if we can get the dictionary of TestPE1 values.
			var testPE1Dict = dict[newTestPE1Value];

			// TestPE1 has two values, including both parts.
			Assert.IsTrue(testPE1Dict.Count == 2);

			// Ensure we find one of 'em.
			Assert.IsTrue(testPE1Dict.ContainsKey(TestPE1.TESTPE1_VALUE1));

			// Ensure we don't find the one that was constructed externally.
			Assert.IsTrue(!testPE1Dict.ContainsKey(newTestPE1Value));
			var threwException = false;
			try
			{
				// Here we construct a PE with the same name as one in th dictionary and
				// try to put it in the dictionary.
				// ReSharper disable once ObjectCreationAsStatement
				new TestPE1("TESTPE1_VALUE1", 10, true);
			}
			catch (Exception)
			{
				threwException = true;
			}
			Assert.IsTrue(threwException);
		}
		/// <summary>
		/// This test is designed to check that we can AND/OR PEs and get a value.
		/// </summary>
		[Test]
		public void TestBitwiseOPs()
		{

			// Oring bit 1 and 2 should give us 3.
			var resultingPE1 = TestPE1.TESTPE1_VALUE1 | TestPE1.TESTPE1_VALUE2;
			Assert.IsTrue(resultingPE1 == 3);

			// Anding a enum of three with value two should give 2.
			var testPE1WithValue3 = new TestPE1("VALUEOFTHREE", 3);

			var oredValue = testPE1WithValue3 & TestPE1.TESTPE1_VALUE2;
			Assert.IsTrue(oredValue == 2);

		}
		/// <summary>
		/// This test is designed to check that we can build composite base enums.
		/// </summary>
		[Test]
		public void TestCompositeEnumerableTypes()
		{

			// Or bit 1 and 2 should give us 3.
			var resultingPE1 = TestPE1.TESTPE1_VALUE1.BitwiseOR(TestPE1.TESTPE1_VALUE2);
			Assert.IsTrue(resultingPE1.EnumValueAsGeneric == 3);

			// Check the name.
			Assert.IsTrue(resultingPE1.Name.Equals("TESTPE1_VALUE1 | TESTPE1_VALUE2"));

			// Anding a enum of three with value two should give 2.
			var testPE1WithValue3 = new TestPE1("VALUEOFTHREE", 3);
			var resultingPE2 = TestPE1.TESTPE1_VALUE2.BitwiseAND(testPE1WithValue3);
			Assert.IsTrue(resultingPE2.EnumValueAsGeneric == 2);

			// Check the name.
			Assert.IsTrue(resultingPE2.Name.Equals("TESTPE1_VALUE2 & VALUEOFTHREE", StringComparison.Ordinal));
		}

		/// <summary>
		/// This test is designed to check that we can build composite PEs.
		/// </summary>
		[Test]
		public void TestCompositePEs()
		{

			// Or bit 1 and 2 should give us 3.
			var resultingPE1 = new TestPE1(TestPE1.TESTPE1_VALUE1.BitwiseOR(TestPE1.TESTPE1_VALUE2));
			Assert.IsTrue(resultingPE1.Value == 3);

			// Check the name.
			Assert.IsTrue(resultingPE1.Name.Equals("TESTPE1_VALUE1 | TESTPE1_VALUE2", StringComparison.Ordinal));

			// Anding a enum of three with value two should give 2.
			var testPE1WithValue3 = new TestPE1("VALUEOFTHREE", 3);
			var resultingPE2 = new TestPE1(TestPE1.TESTPE1_VALUE2.BitwiseAND(testPE1WithValue3));
			Assert.IsTrue(resultingPE2.Value == 2);

			// Check the name.
			Assert.IsTrue(resultingPE2.Name.Equals("TESTPE1_VALUE2 & VALUEOFTHREE", StringComparison.Ordinal));
		}
		/// <summary>
		/// This test is designed to show how to build a static bit combo.
		/// </summary>
		[Test]
		public void TestBuildStaticBitCombo()
		{
			BuildStaticBitPattern();
			// Or bit 1 and 2 should give us 21.
			Assert.IsTrue(s_AllBitsOn.Value == 21);
		}
		#endregion  //Basic Construction
		/// <summary>
		/// Static helper that just builds a PE statically.
		/// </summary>
		public static void BuildStaticBitPattern()
		{
			s_AllBitsOn = new TestPE2(TestPE2.TESTPE2_VALUE1.BitwiseOR(TestPE2.TESTPE2_VALUE2).BitwiseOR(TestPE2.TESTPE2_VALUE3));
		}

	}
}

