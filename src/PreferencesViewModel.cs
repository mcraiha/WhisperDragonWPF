using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.ObjectModel;
using WhisperDragonWPF;

public class PreferencesViewModel : INotifyPropertyChanged
{
	public string PreferencesLocation { get; set; }

	public ObservableCollection<string> ShowModes { get; }

	private string selectedTitleShowMode;

	public string SelectedTitleShowMode
    {
        get
        {
            return this.selectedTitleShowMode;
        }
        set
        {
            if (this.selectedTitleShowMode != value)
            {
                this.selectedTitleShowMode = value;
                OnPropertyChanged(nameof(SelectedTitleShowMode));
            }
        }
    }

	private readonly SettingsData settings;
	private readonly Action<SettingsData> saveAction;
	private readonly Action closeAction;

	public event PropertyChangedEventHandler PropertyChanged;

	public PreferencesViewModel(SettingsData settingsData, string fileLocation, Action<SettingsData> saveAction, Action closeAction)
	{
		this.settings = settingsData;
		this.PreferencesLocation = fileLocation;
		this.saveAction = saveAction;
		this.closeAction = closeAction;

		this.ShowModes = new ObservableCollection<string>();
		foreach (ShowMode showMode in (ShowMode[]) Enum.GetValues(typeof(ShowMode)))
		{
			this.ShowModes.Add(showMode.ToString());
		}
		
		this.SelectedTitleShowMode = settingsData.TitleShowMode.ToString();
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
					this.saveAction(this.settings);
					this.closeAction();
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
					this.closeAction();
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