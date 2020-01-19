using System.Collections.Generic;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.ComponentModel;
using WhisperDragonWPF;

public class AboutViewModel
{
	public string Description { get; set; } = "WhisperDragonWPF is CommonSecrets compatible password/secrets manager for WPF.";

	private readonly Action closeWindow;

	public AboutViewModel(Action callToClose)
	{
		this.closeWindow = callToClose;
	}
	
	#region Buttons

	private ICommand okCommand;
	public ICommand OkCommand
	{
		get
		{
			return okCommand 
				?? (okCommand = new ActionCommand(() =>
				{
					this.closeWindow();
				}));
		}
	}

	#endregion // Buttons
}