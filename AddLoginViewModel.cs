using System.Collections.Generic;
using System.Security.Cryptography;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.ComponentModel;
using WhisperDragonWPF;

public class AddLoginViewModel : INotifyPropertyChanged
{
	public bool IsSecret { get; set; } = true;

	public string Title { get; set; } = "";

	public string URL { get; set; } = "";

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

	public event PropertyChangedEventHandler PropertyChanged;

	private readonly Action onPositiveClose;

	private readonly Action onNegativeClose;

	private readonly Action<LoginSimplified> addLogin;

	private readonly PasswordBox passwordBox;

	public AddLoginViewModel(Action positiveAction, Action negativeAction, Action<LoginSimplified> add, PasswordBox pwBox)
	{
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

	#endregion

	
	#region Buttons

	
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
						Username = this.Username,
						Password = this.Password,
						Category = this.Category,
						Tags = this.Tags,
						//CreationTime = DateTime.UtcNow,

					 });
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