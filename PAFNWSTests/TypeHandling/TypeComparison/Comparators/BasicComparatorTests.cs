using System;
using NUnit.Framework;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
using PlatformAgileFramework.TypeHandling.BasicValueTypes;
// Exception shorthand.
using PAFTED = PlatformAgileFramework.ErrorAndException.CoreCustomExceptions.PAFTypeExceptionData;
using IPAFTED = PlatformAgileFramework.ErrorAndException.CoreCustomExceptions.IPAFTypeExceptionData;



namespace PlatformAgileFramework.TypeHandling.TypeComparison.Comparators
{
	[TestFixture]
	public class BasicComparatorTests
	{
		#region Fields and Autoproperties
		/// <summary>
		/// This is the base number to compare to.
		/// </summary>
		public static readonly double s_DoubleNumberToCompare = 1.0;
		/// <summary>
		/// This number should pass the comparision.
		/// </summary>
		public static readonly double s_DoubleNumberCloseTo = 1.0 + double.Epsilon;
		/// <summary>
		/// This number should fail the comparision.
		/// </summary>
		public static readonly double s_DoubleNumberNotCloseTo = 1.0 + 0.1;
		/// <summary>
		/// This is the base number to compare to.
		/// </summary>
		public static PAFComplexDoubleNumber s_ComplexDoubleNumberToCompare;
		/// <summary>
		/// This number should pass the comparision.
		/// </summary>
		public static PAFComplexDoubleNumber s_ComplexDoubleNumberCloseTo;
		/// <summary>
		/// This number should fail the comparision.
		/// </summary>
		public static PAFComplexDoubleNumber s_ComplexDoubleNumberNotCloseTo;
		/// <summary>
		/// This is the base number to compare to.
		/// </summary>
		public static readonly float s_FloatNumberToCompare = 1.0F;
		/// <summary>
		/// This number should pass the comparision.
		/// </summary>
		public static readonly float s_FloatNumberCloseTo = 1.0F + float.Epsilon;
		/// <summary>
		/// This number should fail the comparision.
		/// </summary>
		public static readonly float s_FloatNumberNotCloseTo = 1.0F + 0.1F;
		/// <summary>
		/// This is the base number to compare to.
		/// </summary>
		public static PAFComplexFloatNumber s_ComplexFloatNumberToCompare;
		/// <summary>
		/// This number should pass the comparision.
		/// </summary>
		public static PAFComplexFloatNumber s_ComplexFloatNumberCloseTo;
		/// <summary>
		/// This number should fail the comparision.
		/// </summary>
		public static PAFComplexFloatNumber s_ComplexFloatNumberNotCloseTo;
		#endregion // Fields and Autoproperties
		#region Constructors
		static BasicComparatorTests()
		{
			s_ComplexDoubleNumberToCompare
				= new PAFComplexDoubleNumber(s_DoubleNumberToCompare, s_DoubleNumberToCompare);
			s_ComplexDoubleNumberCloseTo
				= new PAFComplexDoubleNumber(s_DoubleNumberCloseTo, s_DoubleNumberCloseTo);
			s_ComplexDoubleNumberNotCloseTo
				= new PAFComplexDoubleNumber(s_DoubleNumberNotCloseTo, s_DoubleNumberNotCloseTo);
			s_ComplexFloatNumberToCompare
				= new PAFComplexFloatNumber(s_FloatNumberToCompare, s_FloatNumberToCompare);
			s_ComplexFloatNumberCloseTo
				= new PAFComplexFloatNumber(s_FloatNumberCloseTo, s_FloatNumberCloseTo);
			s_ComplexFloatNumberNotCloseTo
				= new PAFComplexFloatNumber(s_FloatNumberNotCloseTo, s_FloatNumberNotCloseTo);
		}
		#endregion // Constructors
		#region TestMethods
		[Test]
		public void TestDoubleCompare()
		{
			IPAFDoubleEquatable doubleComparer = new PAFDoubleEquatable();
			var isCloseTo = doubleComparer.AreEqual(s_DoubleNumberToCompare, s_DoubleNumberCloseTo);
			Assert.IsTrue(isCloseTo, "double close to");
			isCloseTo = doubleComparer.AreEqual(s_DoubleNumberToCompare, s_DoubleNumberNotCloseTo);
			Assert.IsTrue(!isCloseTo, "double not close to");
		}
		[Test]
		public void TestComplexDoubleCompare()
		{
			IPAFComplexDoubleEquatable complexDoubleComparer = new PAFComplexDoubleEquatable();
			var isCloseTo = complexDoubleComparer.AreEqual(s_ComplexDoubleNumberToCompare, s_ComplexDoubleNumberCloseTo);
			Assert.IsTrue(isCloseTo, "complex double close to");
			isCloseTo = complexDoubleComparer.AreEqual(s_ComplexDoubleNumberToCompare, s_ComplexDoubleNumberNotCloseTo);
			Assert.IsTrue(!isCloseTo, "complex double not close to");
		}
		[Test]
		public void TestFloatCompare()
		{
			IPAFFloatEquatable doubleComparer = new PAFFloatEquatable();
			var isCloseTo = doubleComparer.AreEqual(s_FloatNumberToCompare, s_FloatNumberCloseTo);
			Assert.IsTrue(isCloseTo, "float close to");
			isCloseTo = doubleComparer.AreEqual(s_FloatNumberToCompare, s_FloatNumberNotCloseTo);
			Assert.IsTrue(!isCloseTo, "float not close to");
		}
		[Test]
		public void TestComplexFloatCompare()
		{
			IPAFComplexFloatEquatable complexFloatComparer = new PAFComplexFloatEquatable();
			var isCloseTo = complexFloatComparer.AreEqual(s_ComplexFloatNumberToCompare, s_ComplexFloatNumberCloseTo);
			Assert.IsTrue(isCloseTo, "complex float close to");
			isCloseTo = complexFloatComparer.AreEqual(s_ComplexFloatNumberToCompare, s_ComplexFloatNumberNotCloseTo);
			Assert.IsTrue(!isCloseTo, "complex float not close to");
		}
		#endregion // TestMethods
	}
}

