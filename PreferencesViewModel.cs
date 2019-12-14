using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.ComponentModel;
using WhisperDragonWPF;

public class PreferencesViewModel : INotifyPropertyChanged
{
	public string PreferencesLocation { get; set; }

	private Action saveCloseAction;
	private Action cancelCloseAction;

	public event PropertyChangedEventHandler PropertyChanged;

	public PreferencesViewModel(string fileLocation, Action saveAction, Action cancelAction)
	{
		PreferencesLocation = fileLocation;
		saveCloseAction = saveAction;
		cancelCloseAction = cancelAction;
	}

	#region Buttons

	
	private ICommand saveCommand;
	public ICommand SaveCommand
	{
		get
		{
			return saveCommand 
				?? (saveCommand = new ActionCommand(() =>
				{
					this.saveCloseAction();
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
					this.cancelCloseAction();
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