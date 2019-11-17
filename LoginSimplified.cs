using System.Collections.Generic;
using CSCommonSecrets;

namespace WhisperDragonWPF
{
	public sealed class LoginSimplified
	{
		// Non visible
		public int indexNumber { get; set; }

		// Visible elements

		public bool IsSecure { get; set; }
		public string Title { get; set; }
		public string URL { get; set; }
		public string Email { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Category { get; set; }
		public string Tags { get; set; }
		public string CreationTime { get; set; }
		public string ModificationTime { get; set; }

		public static List<LoginSimplified> TurnIntoUICompatible(List<LoginInformation> loginInformations, List<LoginInformationSecret> loginInformationSecrets, Dictionary<string, byte[]> derivedPasswords)
		{
			List<LoginSimplified> returnList = new List<LoginSimplified>();

			if (loginInformations != null && loginInformations.Count > 0)
			{
				int runningIndexNumber = 0;
				foreach (LoginInformation loginInformation in loginInformations)
				{
					returnList.Add(new LoginSimplified() {
						indexNumber = runningIndexNumber,
						IsSecure = false,
						Title = loginInformation.GetTitle(),
						URL = loginInformation.GetURL(),
						Email = loginInformation.GetEmail(),
						Username = loginInformation.GetUsername(),
						Password = loginInformation.GetPassword(),
						Category = loginInformation.GetCategory(),
						Tags = loginInformation.GetTags(),
						CreationTime = loginInformation.GetCreationTime().ToString("s", System.Globalization.CultureInfo.InvariantCulture),
						ModificationTime = loginInformation.GetModificationTime().ToString("s", System.Globalization.CultureInfo.InvariantCulture),
					});

					runningIndexNumber++;
				}
			}

			if (loginInformationSecrets != null && loginInformationSecrets.Count > 0)
			{
				int runningIndexNumber = 0;
				foreach (LoginInformationSecret loginInformationSecret in loginInformationSecrets)
				{
					byte[] derivedPassword = derivedPasswords[loginInformationSecret.GetKeyIdentifier()];
					returnList.Add(new LoginSimplified() {
						indexNumber = runningIndexNumber,
						IsSecure = true,
						Title = loginInformationSecret.GetTitle(derivedPassword),
						URL = loginInformationSecret.GetUrl(derivedPassword),
						Email = loginInformationSecret.GetEmail(derivedPassword),
						Username = loginInformationSecret.GetUsername(derivedPassword),
						Password = loginInformationSecret.GetPassword(derivedPassword),
						Category = loginInformationSecret.GetCategory(derivedPassword),
						Tags = loginInformationSecret.GetTags(derivedPassword),
						CreationTime = loginInformationSecret.GetCreationTime(derivedPassword).ToString("s", System.Globalization.CultureInfo.InvariantCulture),
						ModificationTime = loginInformationSecret.GetModificationTime(derivedPassword).ToString("s", System.Globalization.CultureInfo.InvariantCulture),
					});

					runningIndexNumber++;
				}
			}

			return returnList;
		}
	}
}