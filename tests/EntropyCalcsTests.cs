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

		[Test]
		public void GetPasswordSecurityLevelTest()
		{
			// Arrange
			string first = "cat";
			string second = "cat!24TBTB1214b!DF¤";

			// Act
			PasswordSecurityLevel securityLevelFirst = EntropyCalcs.GetPasswordSecurityLevel(EntropyCalcs.CalcutePasswordEntropy(first));
			PasswordSecurityLevel securityLevelSecond = EntropyCalcs.GetPasswordSecurityLevel(EntropyCalcs.CalcutePasswordEntropy(second));

			// Assert
			Assert.AreEqual(PasswordSecurityLevel.Very_Weak, securityLevelFirst);
			Assert.AreNotEqual(securityLevelFirst, securityLevelSecond);
		}
	}
}