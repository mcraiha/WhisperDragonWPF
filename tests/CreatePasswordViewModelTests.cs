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
		public void GeneratePasswordCommandLowerCaseTest()
		{
			// Arrange
			CreatePasswordViewModel cpvmOnlyLowerCase = new CreatePasswordViewModel(null, null);
			cpvmOnlyLowerCase.IncludeLowerCaseLatinLetters = true;
			cpvmOnlyLowerCase.IncludeUpperCaseLatinLetters = false;
			cpvmOnlyLowerCase.IncludeDigits = false;
			cpvmOnlyLowerCase.IncludeSpecialCharactersASCII = false;
			cpvmOnlyLowerCase.IncludeEmojis = false;

			// Act
			cpvmOnlyLowerCase.GeneratePasswordCommand.Execute(null);
			string passwordLowerCase = cpvmOnlyLowerCase.GeneratedPassword;

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(passwordLowerCase));
			Assert.AreEqual(int.Parse(cpvmOnlyLowerCase.PasswordLength), passwordLowerCase.Length);

			Assert.IsTrue(passwordLowerCase.All(c => char.IsLower(c) && char.IsLetter(c)));
		}

		[Test]
		public void GeneratePasswordCommandUpperCaseTest()
		{
			// Arrange
			CreatePasswordViewModel cpvmOnlyUpperCase = new CreatePasswordViewModel(null, null);
			cpvmOnlyUpperCase.IncludeUpperCaseLatinLetters = true;
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

		[Test]
		public void GeneratePasswordCommandDigitsTest()
		{
			// Arrange
			CreatePasswordViewModel cpvmOnlyDigits = new CreatePasswordViewModel(null, null);
			cpvmOnlyDigits.IncludeUpperCaseLatinLetters = false;
			cpvmOnlyDigits.IncludeLowerCaseLatinLetters = false;
			cpvmOnlyDigits.IncludeDigits = true;
			cpvmOnlyDigits.IncludeSpecialCharactersASCII = false;
			cpvmOnlyDigits.IncludeEmojis = false;

			// Act
			cpvmOnlyDigits.GeneratePasswordCommand.Execute(null);
			string passwordDigits = cpvmOnlyDigits.GeneratedPassword;

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(passwordDigits));
			Assert.AreEqual(int.Parse(cpvmOnlyDigits.PasswordLength), passwordDigits.Length);

			Assert.IsTrue(passwordDigits.All(c => char.IsDigit(c)));
		}

		[Test]
		public void GeneratePasswordCommandSpecialCharactersTest()
		{
			// Arrange
			CreatePasswordViewModel cpvmOnlySpecialChars = new CreatePasswordViewModel(null, null);
			cpvmOnlySpecialChars.IncludeUpperCaseLatinLetters = false;
			cpvmOnlySpecialChars.IncludeLowerCaseLatinLetters = false;
			cpvmOnlySpecialChars.IncludeDigits = false;
			cpvmOnlySpecialChars.IncludeSpecialCharactersASCII = true;
			cpvmOnlySpecialChars.IncludeEmojis = false;

			// Act
			cpvmOnlySpecialChars.GeneratePasswordCommand.Execute(null);
			string passwordSpecialChars = cpvmOnlySpecialChars.GeneratedPassword;

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(passwordSpecialChars));
			Assert.AreEqual(int.Parse(cpvmOnlySpecialChars.PasswordLength), passwordSpecialChars.Length);

			Assert.IsTrue(passwordSpecialChars.All(c => !char.IsDigit(c) && !char.IsLetter(c)));
		}

		[Test]
		public void GeneratePronounceablePasswordCommandTest()
		{
			// Arrange
			CreatePasswordViewModel cpvmPronounceable2Words = new CreatePasswordViewModel(null, null);
			cpvmPronounceable2Words.HowManyWords = "2";

			CreatePasswordViewModel cpvmPronounceable9Words = new CreatePasswordViewModel(null, null);
			cpvmPronounceable9Words.HowManyWords = "9";

			// Act
			cpvmPronounceable2Words.GeneratePronounceablePasswordCommand.Execute(null);
			string password2Words = cpvmPronounceable2Words.GeneratedPronounceablePassword;

			cpvmPronounceable9Words.GeneratePronounceablePasswordCommand.Execute(null);
			string password9Words = cpvmPronounceable9Words.GeneratedPronounceablePassword;

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(password2Words));
			Assert.Greater(password2Words.Length, 5);

			Assert.IsFalse(string.IsNullOrEmpty(password9Words));
			Assert.Greater(password9Words.Length, 21);

			Assert.AreNotEqual(password2Words, password9Words);
		}
	}
}