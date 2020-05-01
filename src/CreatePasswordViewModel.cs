using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	public Visibility VisibilityUsePassword { get; set; }

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


	private string generatedPronounceablePassword = "";
	public string GeneratedPronounceablePassword 
	{ 
		get
		{
			if (VisiblePassword)
			{
				return this.generatedPronounceablePassword;
			}
			
			return string.Create(this.generatedPronounceablePassword.Length, '*', (chars, buf) => {
    																	for (int i=0;i<chars.Length;i++) chars[i] = buf;
					});
		}
		set         
		{
			this.generatedPronounceablePassword = value;
			OnPropertyChanged(nameof(GeneratedPronounceablePassword));
		}
	}

	public ObservableCollection<string> Languages { get; }

	public string selectedLanguage;

	public string SelectedLanguage
	{
        get
        {
            return this.selectedLanguage;
        }
        set
        {
            if (this.selectedLanguage != value)
            {
                this.selectedLanguage = value;
                OnPropertyChanged(nameof(SelectedLanguage));
            }
        }
    }

	public bool StartWithUpperCase { get; set; } = true;

	public bool IncludeNumbers { get; set; } = true;

	public bool IncludeSpecialCharSimple { get; set; } = true;

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

	private static readonly List<char> specialCharactersPronounceable = new List<char>()
	{
		'!', '#', '$', '%', '*', '+', '-', '.', '?', '@', '_',
	};

	// Generated during runtime in ConstructEmojiList(), See https://en.wikipedia.org/wiki/Emoticons_(Unicode_block)
	private static readonly List<string> emoticonsUnicodeBlock = new List<string>();

	private static readonly List<(string, string)> languageToCommonWords = new List<(string, string)>()
	{
		("English", "CommonWords/English-Common.txt")
	};

	private readonly Action callOnClose;
	private readonly Action<string> passwordCallback;

	public CreatePasswordViewModel(Action closeAction, Action<string> passwordCallback)
	{
		this.callOnClose = closeAction;
		this.passwordCallback = passwordCallback;
		this.VisibilityUsePassword = (passwordCallback != null) ? Visibility.Visible : Visibility.Hidden;

		this.Languages = CreateLanguages();
		this.selectedLanguage = Languages[0];
	}

	private static ObservableCollection<string> CreateLanguages()
	{
		ObservableCollection<string> returnList = new ObservableCollection<string>();

		foreach (var pair in languageToCommonWords)
		{
			returnList.Add(pair.Item1);
		}

		return returnList;
	}

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

					List<string> generated = new List<string>(howManyChars);

					List<string> possibleChars = new List<string>();

					using (var generator = RandomNumberGenerator.Create())
					{
						if (IncludeUpperCaseLatinLetters)
						{
							int index = GetPositiveRandomInt(generator) % upperCaseLatinLetters.Count;
							generated.Add(upperCaseLatinLetters[index].ToString());
							possibleChars.AddRange(Array.ConvertAll<char, string>(upperCaseLatinLetters.ToArray(), element => element.ToString()));
						}

						if (IncludeLowerCaseLatinLetters)
						{
							int index = GetPositiveRandomInt(generator) % lowerCaseLatinLetters.Count;
							generated.Add(lowerCaseLatinLetters[index].ToString());
							possibleChars.AddRange(Array.ConvertAll<char, string>(lowerCaseLatinLetters.ToArray(), element => element.ToString()));
						}

						if (IncludeDigits)
						{
							int index = GetPositiveRandomInt(generator) % digits.Count;
							generated.Add(digits[index].ToString());
							possibleChars.AddRange(Array.ConvertAll<char, string>(digits.ToArray(), element => element.ToString()));
						}

						if (IncludeSpecialCharactersASCII)
						{
							int index = GetPositiveRandomInt(generator) % specialCharactersASCII.Count;
							generated.Add(specialCharactersASCII[index].ToString());
							possibleChars.AddRange(Array.ConvertAll<char, string>(specialCharactersASCII.ToArray(), element => element.ToString()));
						}

						if (IncludeEmojis)
						{
							if (emoticonsUnicodeBlock.Count < 1)
							{
								ConstructEmojiList();
							}

							int index = GetPositiveRandomInt(generator) % emoticonsUnicodeBlock.Count;
							generated.Add(emoticonsUnicodeBlock[index]);
							possibleChars.AddRange(emoticonsUnicodeBlock);
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

					GeneratedPassword = string.Join("", generated); //  new String( generated.ToArray());
					copyToClipboardCommand.RaiseCanExecuteChanged();
				}));
		}
	}


	private ICommand generatePronounceablePasswordCommand;
	public ICommand GeneratePronounceablePasswordCommand
	{
		get
		{
			return generatePronounceablePasswordCommand 
				?? (generatePronounceablePasswordCommand = new ActionCommand(() =>
				{
					List<string> commonWords = EmbedResourceLoader.ReadAsList(languageToCommonWords[0].Item2);

					string currentPronounceablePassword = "";

					using (var generator = RandomNumberGenerator.Create())
					{
						int bigIndex = GetPositiveRandomInt(generator);
						int smallIndex = bigIndex % commonWords.Count;

						string firstWord = commonWords[smallIndex];

						if (StartWithUpperCase)
						{
							firstWord = char.ToUpper(firstWord[0]) + firstWord.Substring(1);
						}

						bigIndex = GetPositiveRandomInt(generator);
						smallIndex = bigIndex % commonWords.Count;
						string secondWord = commonWords[smallIndex];

						currentPronounceablePassword = firstWord + secondWord;

						if (IncludeNumbers)
						{
							bigIndex = GetPositiveRandomInt(generator);
							smallIndex = bigIndex % 99;
							currentPronounceablePassword += smallIndex;
						}

						if (IncludeSpecialCharSimple)
						{
							int index = GetPositiveRandomInt(generator) % specialCharactersPronounceable.Count;
							currentPronounceablePassword += specialCharactersPronounceable[index];
						}		
					}

					GeneratedPronounceablePassword = currentPronounceablePassword;
					copyPronounceableToClipboardCommand.RaiseCanExecuteChanged();
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


	private ActionConditionalCommand copyPronounceableToClipboardCommand;
	public ActionConditionalCommand CopyPronounceableToClipboardCommand
	{
		get
		{
			return copyPronounceableToClipboardCommand 
				?? (copyPronounceableToClipboardCommand = new ActionConditionalCommand(() =>
				{
					Clipboard.SetText(this.generatedPronounceablePassword);
				}, () => this.generatedPronounceablePassword.Length > 1));
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
					this.callOnClose?.Invoke();
				}));
		}
	}

	private ICommand useCommand;
	public ICommand UseCommand
	{
		get
		{
			return useCommand 
				?? (useCommand = new ActionCommand(() =>
				{
					this.passwordCallback?.Invoke(this.generatedPassword);
					this.callOnClose?.Invoke();
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

	private static void ConstructEmojiList()
	{
		int startValue = 0x1F600;
		emoticonsUnicodeBlock.Add(Char.ConvertFromUtf32(startValue));
		for (int i = 0; i < 80; i++)
		{
			startValue++;
			emoticonsUnicodeBlock.Add(Char.ConvertFromUtf32(startValue));
		}
	}
}