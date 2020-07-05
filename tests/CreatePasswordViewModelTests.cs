using NUnit.Framework;
using WhisperDragonWPF;

namespace tests
{
	public class CreatePasswordViewModelTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void GeneratePasswordCommandTest()
		{
			// Arrange
			CreatePasswordViewModel cpvmDefault = new CreatePasswordViewModel(null, null);

			// Act
			cpvmDefault.GeneratePasswordCommand.Execute(null);
			string passwordDefault = cpvmDefault.GeneratedPassword;

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(passwordDefault));
		}
	}
}