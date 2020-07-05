using NUnit.Framework;
using WhisperDragonWPF;
using System.Linq;

namespace tests
{
	public class CreatePasswordViewModelTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void GeneratePasswordCommandDefaultTest()
		{
			// Arrange
			CreatePasswordViewModel cpvmDefault1 = new CreatePasswordViewModel(null, null);
			CreatePasswordViewModel cpvmDefault2 = new CreatePasswordViewModel(null, null);

			// Act
			cpvmDefault1.GeneratePasswordCommand.Execute(null);
			string passwordDefault1 = cpvmDefault1.GeneratedPassword;

			cpvmDefault2.GeneratePasswordCommand.Execute(null);
			string passwordDefault2 = cpvmDefault2.GeneratedPassword;

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(passwordDefault1));
			Assert.AreEqual(int.Parse(cpvmDefault1.PasswordLength), passwordDefault1.Length);

			Assert.AreNotEqual(passwordDefault1, passwordDefault2);
		}

		[Test]
		public void GeneratePasswordCommandShortTest()
		{
			// Arrange
			CreatePasswordViewModel cpvmShort = new CreatePasswordViewModel(null, null);
			cpvmShort.PasswordLength = "8";

			// Act
			cpvmShort.GeneratePasswordCommand.Execute(null);
			string passwordShort = cpvmShort.GeneratedPassword;

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(passwordShort));
			Assert.AreEqual(int.Parse(cpvmShort.PasswordLength), passwordShort.Length);
		}

		[Test]
		public void GeneratePasswordCommandUpperCaseTest()
		{
			// Arrange
			CreatePasswordViewModel cpvmOnlyUpperCase = new CreatePasswordViewModel(null, null);
			cpvmOnlyUpperCase.IncludeLowerCaseLatinLetters = false;
			cpvmOnlyUpperCase.IncludeDigits = false;
			cpvmOnlyUpperCase.IncludeSpecialCharactersASCII = false;
			cpvmOnlyUpperCase.IncludeEmojis = false;

			// Act
			cpvmOnlyUpperCase.GeneratePasswordCommand.Execute(null);
			string passwordUpperCase = cpvmOnlyUpperCase.GeneratedPassword;

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(passwordUpperCase));
			Assert.AreEqual(int.Parse(cpvmOnlyUpperCase.PasswordLength), passwordUpperCase.Length);

			Assert.IsTrue(passwordUpperCase.All(c => char.IsUpper(c) && char.IsLetter(c)));
		}
	}
}