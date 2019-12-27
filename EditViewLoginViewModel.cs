using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.ComponentModel;
using WhisperDragonWPF;

/// <summary>
/// Edit or view existing login information
/// </summary>
public class EditViewLoginViewModel : INotifyPropertyChanged
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

	private readonly Action<LoginSimplified, string /* Key identifier */> editLogin;

	private readonly PasswordBox passwordBox;

	public EditViewLoginViewModel(LoginSimplified current, List<string> keyIds, Action positiveAction, Action negativeAction, Action<LoginSimplified, string /* Key identifier */> edit, PasswordBox pwBox)
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
		this.editLogin = edit;
		this.passwordBox = pwBox;

		this.Title = current.Title;
		this.URL = current.URL;
		this.Email = current.Email;
		this.Username = current.Username;
		this.Password = current.Password;
		this.passwordBox.Password = current.Password;
		this.Notes = current.Notes;
		// TODO: Icon here once it is supported
		this.Category = current.Category;
		this.Tags = current.Tags;
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

	
	private ICommand editLoginCommand;
	public ICommand EditLoginCommand
	{
		get
		{
			return editLoginCommand 
				?? (editLoginCommand = new ActionCommand(() =>
				{
					this.editLogin(new LoginSimplified() {
						Title = this.Title,
						URL = this.URL,
						Email = this.Email,
						Username = this.Username,
						Password = this.visiblePassword ? this.Password : passwordBox.Password,
						Notes = this.Notes,
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
}