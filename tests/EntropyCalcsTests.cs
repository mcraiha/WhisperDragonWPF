using NUnit.Framework;
using WhisperDragonWPF;

namespace tests
{
	public class EntropyCalcsTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void CalcutePasswordEntropyTest()
		{
			// Arrange
			string first = "cat";
			string second = "cat";
			string third = "Doggie";

			// Act
			int entropyFirst = EntropyCalcs.CalcutePasswordEntropy(first);
			int entropySecond = EntropyCalcs.CalcutePasswordEntropy(second);
			int entropyThird = EntropyCalcs.CalcutePasswordEntropy(third);

			// Assert
			Assert.AreEqual(entropyFirst, entropySecond);
			Assert.AreNotEqual(entropyFirst, entropyThird);
		}
	}
}