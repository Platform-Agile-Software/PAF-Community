using NUnit.Framework;

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.TestingSamples
{
	public class MyAdder
	{
		public static int AddTwoNumbersTogether(int firstNumber, int secondNumber)
		{
			return firstNumber + secondNumber;
		}
	}
	[TestFixture]
	public class MyAdderTests
	{
		/// <summary>
		/// This test is designed to test the adder (not a good test).
		/// </summary>
		[Test]
		public virtual void CheckResult()
		{
			Assert.IsTrue(MyAdder.AddTwoNumbersTogether(2, 2) != 0);
		}
		/// <summary>
		/// This test is designed to test the adder (a good test).
		/// </summary>
		[Test]
		public virtual void CheckResultMeaningful()
		{
			Assert.IsTrue(MyAdder.AddTwoNumbersTogether(2, 2) == 4);
		}
		/// <summary>
		/// This test is designed to test extreme conditions.
		/// </summary>
		[Test]
		public virtual void CheckResultExtreme()
		{
			var result = MyAdder.AddTwoNumbersTogether(int.MaxValue, int.MaxValue);
			Assert.IsTrue(result == int.MaxValue);
		}
	}
	public class MyAccumulatingAdder
	{
		protected internal int m_Accumulator;
		public virtual void AccumulateNumber(int numberToAccumulate)
		{
			m_Accumulator += numberToAccumulate;
		}
		public virtual void ClearAccumulator()
		{
			m_Accumulator = 0;
		}
		public virtual int GetResult()
		{
			return m_Accumulator;
		}
	}
	[TestFixture]
	public class MyAccumulatingAdderTests
	{
		/// <summary>
		/// Tests the intermediate result of the accumulating adder.
		/// </summary>
		[Test]
		public void CheckIntermediateResult()
		{
			var myMoreComplexAdder = new MyAccumulatingAdder();
			myMoreComplexAdder.AccumulateNumber(1); 
			Assert.IsTrue(myMoreComplexAdder.m_Accumulator == 1);
		}
	}
}

