using System;
using System.Collections.Generic;
using CSCommonSecrets;

namespace WhisperDragonWPF
{
	public sealed class LoginSimplified
	{
		// Non visible
		public int zeroBasedIndexNumber { get; set; }

		// Visible elements

		public bool IsSecure { get; set; }
		public string Title { get; set; }
		public string URL { get; set; }
		public string Email { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Notes { get; set; }
		public byte[] Icon { get; set; }
		public string Category { get; set; }
		public string Tags { get; set; }
		public string CreationTime { get; set; }
		public string ModificationTime { get; set; }

		public static List<LoginSimplified> TurnIntoUICompatible(List<LoginInformation> loginInformations, List<LoginInformationSecret> loginInformationSecrets, Dictionary<string, byte[]> derivedPasswords, SettingsData settingsData)
		{
			List<LoginSimplified> returnList = new List<LoginSimplified>();

			if (loginInformations != null && loginInformations.Count > 0)
			{
				int runningIndexNumber = 0;
				foreach (LoginInformation loginInformation in loginInformations)
				{
					returnList.Add(new LoginSimplified() {
						zeroBasedIndexNumber = runningIndexNumber,
						IsSecure = false,
						Title = FormatTextWithFilter(loginInformation.GetTitle(), settingsData.TitleShowMode),
						URL = FormatTextWithFilter(loginInformation.GetURL(), settingsData.UrlShowMode),
						Email = FormatTextWithFilter(loginInformation.GetEmail(), settingsData.EmailShowMode),
						Username = FormatTextWithFilter(loginInformation.GetUsername(), settingsData.UsernameShowMode),
						Password = FormatTextWithFilter(loginInformation.GetPassword(), settingsData.PasswordShowMode),
						Notes = loginInformation.GetNotes(),
						Icon = loginInformation.GetIcon(),
						Category = FormatTextWithFilter(loginInformation.GetCategory(), settingsData.CategoryShowMode),
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
						zeroBasedIndexNumber = runningIndexNumber,
						IsSecure = true,
						Title = FormatTextWithFilter(loginInformationSecret.GetTitle(derivedPassword), settingsData.TitleShowMode),
						URL = FormatTextWithFilter(loginInformationSecret.GetURL(derivedPassword), settingsData.UrlShowMode),
						Email = FormatTextWithFilter(loginInformationSecret.GetEmail(derivedPassword), settingsData.EmailShowMode),
						Username = FormatTextWithFilter(loginInformationSecret.GetUsername(derivedPassword), settingsData.UsernameShowMode),
						Password = FormatTextWithFilter(loginInformationSecret.GetPassword(derivedPassword), settingsData.PasswordShowMode),
						Notes = loginInformationSecret.GetNotes(derivedPassword),
						Icon = loginInformationSecret.GetIcon(derivedPassword),
						Category = FormatTextWithFilter(loginInformationSecret.GetCategory(derivedPassword), settingsData.CategoryShowMode),
						Tags = loginInformationSecret.GetTags(derivedPassword),
						CreationTime = loginInformationSecret.GetCreationTime(derivedPassword).ToString("s", System.Globalization.CultureInfo.InvariantCulture),
						ModificationTime = loginInformationSecret.GetModificationTime(derivedPassword).ToString("s", System.Globalization.CultureInfo.InvariantCulture),
					});

					runningIndexNumber++;
				}
			}

			return returnList;
		}

		private static readonly char passwordChar = '\u2022';

		private static readonly Random rng = new Random(Seed: 2047);

		private static string FormatTextWithFilter(string input, ShowMode showMode)
		{
			if (showMode == ShowMode.ShowFull)
			{
				return input;
			}
			else if (showMode == ShowMode.ShowFirstFour)
			{
				if (input.Length < 5)
				{
					return input;
				}
				else
				{
					return input.Substring(0, 4);
				}
			}
			else if (showMode == ShowMode.ShowFirst)
			{
				if (input.Length < 2)
				{
					return input;
				}
				else
				{
					return input.Substring(0, 1);
				}
			}
			else if (showMode == ShowMode.HiddenCorrectLength)
			{
				return new string(passwordChar, input.Length);
			}
			else if (showMode == ShowMode.HiddenConstantLenght)
			{
				return new string(passwordChar, 8);
			}
			else if (showMode == ShowMode.HiddenRandomLength)
			{
				return new string(passwordChar, rng.Next(1, 16));
			}

			throw new NotImplementedException("Missing ShowMode");
		}
	}
}