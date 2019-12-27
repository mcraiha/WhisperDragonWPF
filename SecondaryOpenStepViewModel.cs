using System.Collections.Generic;
using System.Security.Cryptography;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.ComponentModel;
using WhisperDragonWPF;

/// <summary>
/// This window is shown when CommonSecrets container has password protected content
/// </summary>
public class SecondaryOpenStepViewModel : INotifyPropertyChanged
{
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

	// TODO: Add get secret from file method

	public event PropertyChangedEventHandler PropertyChanged;

	private readonly List<string> keyIds;
	private readonly Action<Dictionary<string, byte[]>> giveBackDerivatedPasswords;
	private readonly Action onNegativeClose;
	private readonly Action onPositiveClose;

	private readonly PasswordBox passwordBox;

	public SecondaryOpenStepViewModel(List<string> keyIdentifiers, Action<Dictionary<string, byte[]>> secondaryOpen, Action cancelCallback, Action openCallBack, PasswordBox pwBox)
	{
		this.keyIds = keyIdentifiers;
		this.giveBackDerivatedPasswords = secondaryOpen;

		this.onNegativeClose = cancelCallback;
		this.onPositiveClose = openCallBack;

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

	
	private ICommand openCommand;
	public ICommand OpenLoginCommand
	{
		get
		{
			return openCommand 
				?? (openCommand = new ActionCommand(() =>
				{
					
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