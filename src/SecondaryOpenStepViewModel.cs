using System.Collections.Generic;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.ComponentModel;
using CSCommonSecrets;

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

	private readonly List<KeyDerivationFunctionEntry> keyDerivationFunctionEntries;
	private readonly Action<Dictionary<string, byte[]>> giveBackDerivatedPasswords;
	private readonly Action onNegativeClose;
	private readonly Action onPositiveClose;

	private readonly PasswordBox passwordBox;

	public SecondaryOpenStepViewModel(List<KeyDerivationFunctionEntry> kdfes, Action<Dictionary<string, byte[]>> finalizeOpenWithDerivedPasswords, Action cancelCallback, Action openCallBack, PasswordBox pwBox)
	{
		this.keyDerivationFunctionEntries = kdfes;
		this.giveBackDerivatedPasswords = finalizeOpenWithDerivedPasswords;

		this.onNegativeClose = cancelCallback;
		this.onPositiveClose = openCallBack;

		this.passwordBox = pwBox;
	}

	private Dictionary<string, byte[]> GenerateDerivedPasswordsFromFields()
	{
		if (this.keyDerivationFunctionEntries.Count == 1)
		{
			string tempPass = this.visiblePassword ? this.Password : passwordBox.Password;
			return new Dictionary<string, byte[]>() { { this.keyDerivationFunctionEntries[0].GetKeyIdentifier(), this.keyDerivationFunctionEntries[0].GeneratePasswordBytes(tempPass) } };
		}

		return null;
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
	public ICommand OpenCommand
	{
		get
		{
			return openCommand 
				?? (openCommand = new ActionCommand(() =>
				{
					this.giveBackDerivatedPasswords(this.GenerateDerivedPasswordsFromFields());
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