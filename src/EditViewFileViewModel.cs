using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.ComponentModel;
using WhisperDragonWPF;
using CSCommonSecrets;

/// <summary>
/// Edit or view existing note
/// </summary>
public class EditViewFileViewModel : INotifyPropertyChanged
{
	// User should NOT be able to edit these
	private int zeroBasedIndexNumber;
	private bool wasOriginallySecret = true;

	private FileEntry currentFileEntry = null;
	private FileEntrySecret currentFileEntrySecret = null;
	private byte[] derivedPassword = null;

	// User editable values
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

	public string Filename { get; set; } = "";
	public string FileSize { get; set; } = "";

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

	private readonly Action<int /* zeroBasedIndexNumber */, bool /* isNowSecure */, bool /* Was Security Modified */, string /* Key identifier */> editFile;

	public EditViewFileViewModel(FileEntry current, int zeroBasedIndexNumber, List<string> keyIds, Action positiveAction, Action negativeAction, Action<int /* zeroBasedIndexNumber */, bool /* isNowSecure */, bool /* Was Security Modified */, string /* Key identifier */> edit)
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
		this.editFile = edit;

		this.currentFileEntry = current;
		this.zeroBasedIndexNumber = zeroBasedIndexNumber;
		this.wasOriginallySecret = false;
		this.IsSecret = false;

		this.Filename = currentFileEntry.GetFilename();
	}

	public EditViewFileViewModel(FileEntrySecret current, byte[] derivedPassword, int zeroBasedIndexNumber, List<string> keyIds, Action positiveAction, Action negativeAction, Action<int /* zeroBasedIndexNumber */, bool /* isNowSecure */, bool /* Was Security Modified */, string /* Key identifier */> edit)
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
		this.editFile = edit;

		this.currentFileEntrySecret = current;
		this.derivedPassword = derivedPassword;
		this.zeroBasedIndexNumber = zeroBasedIndexNumber;
		this.wasOriginallySecret = true;
		this.IsSecret = true;

		this.Filename = currentFileEntrySecret.GetFilename(this.derivedPassword);
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

	private ICommand editNoteCommand;
	public ICommand EditNoteCommand
	{
		get
		{
			return editNoteCommand 
				?? (editNoteCommand = new ActionCommand(() =>
				{
					this.editFile(this.zeroBasedIndexNumber, this.wasOriginallySecret, this.IsSecret != this.wasOriginallySecret, this.selectedKeyIdentifier);
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