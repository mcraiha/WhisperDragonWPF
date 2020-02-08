using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.ComponentModel;
using WhisperDragonWPF;

public class AddLoginViewModel : INotifyPropertyChanged
{
	private bool isSecret = true;
	public bool IsSecret 
	{ 
		get
		{
			return this.isSecret;
		} 
		set
		{
			this.isSecret = value;
			OnPropertyChanged(nameof(KeyIdentifierVisibility));
		}
	}

	public string Title { get; set; } = "";

	public string URL { get; set; } = "";

	public string Email { get; set; } = "";

	public string Username { get; set; } = "";

	private bool visiblePassword = false;
	public bool VisiblePassword 
	{ 
		get
		{
			return this.visiblePassword;
		}
		
		set
		{
			this.visiblePassword = value;
			
			if (this.visiblePassword)
			{
				this.Password = passwordBox.Password;
			}
			else
			{
				passwordBox.Password = this.Password;
			}

			OnPropertyChanged(nameof(PasswordTextVisibility));
			OnPropertyChanged(nameof(PasswordBoxVisibility));
			OnPropertyChanged(nameof(Password));
		} 
	}

	public string Password { get; set; } = "";

	public string Notes { get; set; } = "";

	// TODO: Add icon here when supported

	public string Category { get; set; } = "";

	public string Tags { get; set; } = "";

	public ObservableCollection<string> KeyIdentifiers { get; }
	public string selectedKeyIdentifier;

	public string SelectedKeyIdentifier
    {
        get
        {
            return this.selectedKeyIdentifier;
        }
        set
        {
            if (this.selectedKeyIdentifier != value)
            {
                this.selectedKeyIdentifier = value;
                OnPropertyChanged(nameof(SelectedKeyIdentifier));
            }
        }
    }

	public event PropertyChangedEventHandler PropertyChanged;

	private readonly Action onPositiveClose;

	private readonly Action onNegativeClose;

	private readonly Action<LoginSimplified, string /* Key identifier */> addLogin;

	private readonly PasswordBox passwordBox;

	public AddLoginViewModel(List<string> keyIds, Action positiveAction, Action negativeAction, Action<LoginSimplified, string /* Key identifier */> add, PasswordBox pwBox)
	{
		this.KeyIdentifiers = new ObservableCollection<string>();
		foreach (string keyIdentifier in keyIds)
		{
			this.KeyIdentifiers.Add(keyIdentifier);
		}

		if (keyIds.Count > 0)
		{
			this.SelectedKeyIdentifier = keyIds[0];
		}
		
		this.onPositiveClose = positiveAction;
		this.onNegativeClose = negativeAction;
		this.addLogin = add;
		this.passwordBox = pwBox;
	}


	#region Visibilities

	public Visibility PasswordTextVisibility
	{ 
		get
		{
			return this.visiblePassword ? Visibility.Visible : Visibility.Collapsed;
		} 
		set
		{

		}
	}

	public Visibility PasswordBoxVisibility
	{ 
		get
		{
			return !this.visiblePassword ? Visibility.Visible : Visibility.Collapsed;
		} 
		set
		{

		}
	}

	public Visibility KeyIdentifierVisibility
	{ 
		get
		{
			return this.IsSecret ? Visibility.Visible : Visibility.Collapsed;
		} 
		set
		{

		}
	}

	#endregion

	
	#region Buttons

	private ICommand copyPasswordCommand;
	public ICommand CopyPasswordCommand
	{
		get
		{
			return copyPasswordCommand 
				?? (copyPasswordCommand = new ActionCommand(() =>
				{
					if (this.visiblePassword)
					{
						Clipboard.SetText(this.Password);
					}
					else
					{
						Clipboard.SetText(passwordBox.Password);
					}
				}));
		}
	}

	private ICommand generatePasswordCommand;
	public ICommand GeneratePasswordCommand
	{
		get
		{
			return generatePasswordCommand 
				?? (generatePasswordCommand = new ActionCommand(() =>
				{
					CreatePasswordWindow passwordWindow = new CreatePasswordWindow(this.UpdatePassword);
					passwordWindow.ShowDialog();
				}));
		}
	}

	private void UpdatePassword(string newPassword)
	{
		if (this.visiblePassword)
		{
			this.Password = newPassword;
			OnPropertyChanged(nameof(Password));
		}
		else
		{
			passwordBox.Password = newPassword;
		}
	}
	
	private ICommand addLoginCommand;
	public ICommand AddLoginCommand
	{
		get
		{
			return addLoginCommand 
				?? (addLoginCommand = new ActionCommand(() =>
				{
					this.addLogin(new LoginSimplified() {
						Title = this.Title,
						URL = this.URL,
						Email = this.Email,
						Username = this.Username,
						Password = this.visiblePassword ? this.Password : passwordBox.Password,
						Notes = this.Notes,
						Icon = new byte[] { 0 },
						Category = this.Category,
						Tags = this.Tags,
						IsSecure = this.IsSecret,
						//CreationTime = DateTime.UtcNow,

					 }, this.selectedKeyIdentifier);
					this.onPositiveClose();
				}));
		}
	}

	private ICommand cancelCommand;
	public ICommand CancelCommand
	{
		get
		{
			return cancelCommand 
				?? (cancelCommand = new ActionCommand(() =>
				{
					this.onNegativeClose();
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

	#region Debug/testing tools

#if DEBUG
	private static readonly string[] titles = new string[] {"Some site", "Another site", "Not my site"};
	private static readonly string[] urls = new string[] {"https://somesite.com", "https://anothersite.com", "https://notmysite.com"};
	private static readonly string[] usernames = new string[] {"bluedragon154", "megadragon", "topplayerINtown"};

	private static readonly Random rng = new Random(Seed: 1337);

#endif // DEBUG

	private ICommand debugFill;
	public ICommand DebugFill
	{
		get
		{
			return debugFill 
				?? (debugFill = new ActionCommand(() =>
				{
					#if DEBUG
					
					this.Title = titles[rng.Next(titles.Length)];
					OnPropertyChanged(nameof(Title));

					this.URL = urls[rng.Next(urls.Length)];
					OnPropertyChanged(nameof(URL));

					this.Email = "superdragon@dragonhome.com";
					OnPropertyChanged(nameof(Email));

					this.Username = usernames[rng.Next(usernames.Length)];
					OnPropertyChanged(nameof(Username));

					if (this.VisiblePassword)
					{
						this.Password = System.IO.Path.GetRandomFileName();
					}
					else
					{
						passwordBox.Password = System.IO.Path.GetRandomFileName();
					}
					OnPropertyChanged(nameof(PasswordTextVisibility));
					OnPropertyChanged(nameof(PasswordBoxVisibility));
					OnPropertyChanged(nameof(Password));

					this.Notes = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi pretium arcu nec sagittis tempor.";
					OnPropertyChanged(nameof(Notes));

					this.Category = "Test generated";
					OnPropertyChanged(nameof(Category));

					this.Tags = "Test";
					OnPropertyChanged(nameof(Tags));

					#endif // DEBUG
				}));
		}
	}

	#endregion // Debug/testing tools
}