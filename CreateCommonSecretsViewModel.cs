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

public class CreateCommonSecretsViewModel : INotifyPropertyChanged
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
				this.Password = passwordBox1.Password;
			}
			else
			{
				passwordBox1.Password = this.Password;
			}

			OnPropertyChanged(nameof(PasswordTextVisibility));
			OnPropertyChanged(nameof(PasswordBoxVisibility));
			OnPropertyChanged(nameof(Password));
		} 
	}

	public string Password { get; set; } = "";

	public string PasswordEntropy { get; set; } = "";

	public event PropertyChangedEventHandler PropertyChanged;

	private readonly Action callOnPositive;
	private readonly Action callOnNegative;

	private readonly PasswordBox passwordBox1;
	private readonly PasswordBox passwordBox2;

	public CreateCommonSecretsViewModel(Action positiveAction, Action negativeAction, PasswordBox pwBox1, PasswordBox pwBox2)
	{
		this.callOnPositive = positiveAction;
		this.callOnNegative = negativeAction;
		this.passwordBox1 = pwBox1;
		this.passwordBox2 = pwBox2;

		this.passwordBox1.PasswordChanged += Password1Changed;
		this.passwordBox2.PasswordChanged += Password2Changed;
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

	private ICommand createCommand;
	public ICommand CreateCommand
	{
		get
		{
			return createCommand 
				?? (createCommand = new ActionCommand(() =>
				{
					this.callOnPositive();
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
					this.callOnNegative();
				}));
		}
	}

	#endregion // Buttons

	#region Values changed

	private void Password1Changed(Object sender, RoutedEventArgs args)
	{
		this.UpdatePasswordEntropy(passwordBox1.Password);
	}

	private void Password2Changed(Object sender, RoutedEventArgs args)
	{
		
	}

	private void UpdatePasswordEntropy(string pw)
	{
		this.PasswordEntropy = EntropyCalcs.CalcutePasswordEntropy(pw).ToString();
		OnPropertyChanged(nameof(PasswordEntropy));
	}

	#endregion // Values changed

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