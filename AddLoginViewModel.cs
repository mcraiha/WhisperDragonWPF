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
			// Update to cause onpropertychange
			Password = password;
		} 
	}

	private string password = "";
	public string Password 
	{ 
		get
		{
			if (VisiblePassword)
			{
				return this.password;
			}
			
			return string.Create(this.password.Length, '*', (chars, buf) => {
																		for (int i = 0; i < chars.Length; i++) chars[i] = buf;
					});
		}
		set         
		{
			this.password = value;
			OnPropertyChanged(nameof(Password));
		}
	
	}

	public string Notes { get; set; } = "";

	public string Category { get; set; } = "";

	public string Tags { get; set; } = "";

	public event PropertyChangedEventHandler PropertyChanged;

	private Action onPositiveClose;

	private Action onNegativeClose;

	public AddLoginViewModel(Action positiveAction, Action negativeAction)
	{
		this.onPositiveClose = positiveAction;
		this.onNegativeClose = negativeAction;
	}

	
	#region Buttons

	
	private ICommand addLoginCommand;
	public ICommand AddLoginCommand
	{
		get
		{
			return addLoginCommand 
				?? (addLoginCommand = new ActionCommand(() =>
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