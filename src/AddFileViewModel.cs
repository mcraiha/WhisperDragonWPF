using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.ComponentModel;
using WhisperDragonWPF;
using Microsoft.Win32;

/// <summary>
/// Add file view model
/// </summary>
public class AddFileViewModel : INotifyPropertyChanged
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

	public string FullFilePath { get; set; } = "";


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

	private readonly Action<string /* Filename */, byte[] /* byte Array */, bool /* is secret */, string /* Key identifier */> addFile;

	public AddFileViewModel(List<string> keyIds, Action positiveAction, Action negativeAction, Action<string /* Filename */, byte[] /* byte Array */, bool /* is secret */, string /* Key identifier */> add)
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
		this.addFile = add;
	}

	#region Visibilities

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

	#endregion // Visibilities


	#region Buttons

	private ICommand selectFileCommand;

	public ICommand SelectFileCommand
	{
		get
		{
			return selectFileCommand 
				?? (selectFileCommand = new ActionCommand(() =>
				{
					OpenFileDialog openFileDialog = new OpenFileDialog();
					if (openFileDialog.ShowDialog() == true)
					{
						this.FullFilePath = openFileDialog.FileName;
						OnPropertyChanged(nameof(FullFilePath));
					}
				}));
		}
	}

	private ICommand addFileCommand;
	public ICommand AddFileCommand
	{
		get
		{
			return addFileCommand 
				?? (addFileCommand = new ActionCommand(() =>
				{
					this.addFile(Path.GetFileName(FullFilePath), File.ReadAllBytes(FullFilePath), this.IsSecret, this.selectedKeyIdentifier);
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