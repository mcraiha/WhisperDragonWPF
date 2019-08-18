using System.Collections.Generic;
using System.Security.Cryptography;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.ComponentModel;
using WhisperDragonWPF;

public class CreatePasswordViewModel : INotifyPropertyChanged
{
	public string PasswordLength { get; set; } = "16";

	public bool IncludeUpperCaseLatinLetters { get; set; } = true;

	public bool IncludeLowerCaseLatinLetters { get; set; } = true;

	public bool IncludeDigits { get; set; } = true;

	public bool IncludeSpecialCharactersASCII { get; set; } = true;

	public bool IncludeEmojis { get; set; } = false;

	private bool visiblePassword = true;
	public bool VisiblePassword 
	{ 
		get
		{
			return this.visiblePassword;
		}
		
		set
		{
			this.visiblePassword = value;
			// Update to cause onpropertychange
			GeneratedPassword = generatedPassword;
		} 
	}

	private string generatedPassword = "";
	public string GeneratedPassword 
	{ 
		get
		{
			if (VisiblePassword)
			{
				return this.generatedPassword;
			}
			
			return string.Create(this.generatedPassword.Length, '*', (chars, buf) => {
    																	for (int i=0;i<chars.Length;i++) chars[i] = buf;
					});
		}
		set         
		{
			this.generatedPassword = value;
			OnPropertyChanged(nameof(GeneratedPassword));
		}
	
	}

	public event PropertyChangedEventHandler PropertyChanged;

	private static readonly List<char> upperCaseLatinLetters = new List<char>()
	{
		'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
	};

	private static readonly List<char> lowerCaseLatinLetters = new List<char>()
	{
		'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
	};

	private static readonly List<char> digits = new List<char>()
	{
		'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
	};

	private static readonly List<char> specialCharactersASCII = new List<char>()
	{
		'!', '"', '#', '$', '%', '\'', '(', ')', '*', '+', ',', '-', '.', '/', ':', ';', '<', '>', '?', '@', '[', '\\', ']', '^', '_', '`', '{', '|', '}', '~'
	};

	#region Buttons

	private ICommand generatePasswordCommand;
	public ICommand GeneratePasswordCommand
	{
		get
		{
			return generatePasswordCommand 
				?? (generatePasswordCommand = new ActionCommand(() =>
				{
					int howManyChars = int.Parse(PasswordLength);

					List<char> generated = new List<char>(howManyChars);

					List<char> possibleChars = new List<char>();

					using (var generator = RandomNumberGenerator.Create())
					{
						if (IncludeUpperCaseLatinLetters)
						{
							int index = GetPositiveRandomInt(generator) % upperCaseLatinLetters.Count;
							generated.Add(upperCaseLatinLetters[index]);
							possibleChars.AddRange(upperCaseLatinLetters);
						}

						if (IncludeLowerCaseLatinLetters)
						{
							int index = GetPositiveRandomInt(generator) % lowerCaseLatinLetters.Count;
							generated.Add(lowerCaseLatinLetters[index]);
							possibleChars.AddRange(lowerCaseLatinLetters);
						}

						if (IncludeDigits)
						{
							int index = GetPositiveRandomInt(generator) % digits.Count;
							generated.Add(digits[index]);
							possibleChars.AddRange(digits);
						}

						if (IncludeSpecialCharactersASCII)
						{
							int index = GetPositiveRandomInt(generator) % specialCharactersASCII.Count;
							generated.Add(specialCharactersASCII[index]);
							possibleChars.AddRange(specialCharactersASCII);
						}

						// Reorder all possible chars
						possibleChars = possibleChars.OrderBy(x => GetPositiveRandomInt(generator)).ToList();

						while (generated.Count < howManyChars)
						{
							int index = GetPositiveRandomInt(generator) % possibleChars.Count;
							generated.Add(possibleChars[index]);
						}

						// Reorder all generated chars
						generated = generated.OrderBy(x => GetPositiveRandomInt(generator)).ToList();
					}

					GeneratedPassword = new String(generated.ToArray());
					copyToClipboardCommand.RaiseCanExecuteChanged();
				}));
		}
	}


	private ActionConditionalCommand copyToClipboardCommand;
	public ActionConditionalCommand CopyToClipboardCommand
	{
		get
		{
			return copyToClipboardCommand 
				?? (copyToClipboardCommand = new ActionConditionalCommand(() =>
				{
					Clipboard.SetText(this.generatedPassword);
				}, () => this.generatedPassword.Length > 1));
		}
	}


	private ICommand closeCommand;
	public ICommand CloseCommand
	{
		get
		{
			return closeCommand 
				?? (closeCommand = new ActionCommand(() =>
				{
					
				}));
		}
	}

	#endregion // Buttons

	#region Property changed

	// Create the OnPropertyChanged method to raise the event
	protected void OnPropertyChanged(string name)
	{
		PropertyChangedEventHandler handler = PropertyChanged;
		if (handler != null)
		{
			handler(this, new PropertyChangedEventArgs(name));
		}
	}

	#endregion // Property changed

/* 	public static void Randomize<T>(List<T> target, RandomNumberGenerator rng)
	{
		int n = target.Count;
        for (int i = 0; i < n; i++)
        {
            int r = i + _random.Next(n - i);

            T t = target[r];
            target[r] = target[i];
            target[i] = t;
        }
	}*/

	private static int GetPositiveRandomInt(RandomNumberGenerator rng)
	{
		int returnValue = -1;

		byte[] byteArray = new byte[4];

		while (returnValue < 0)
		{
			rng.GetBytes(byteArray);
			returnValue = BitConverter.ToInt32(byteArray, 0);
		}

		return returnValue;
	}
}