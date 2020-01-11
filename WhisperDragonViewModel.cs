using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using WhisperDragonWPF;
using Microsoft.Win32;
using CSCommonSecrets;

public enum CloseType
{
	Close = 0,
	SaveAndClose,
	SaveAs,
	Cancel
}

public class WhisperDragonViewModel : INotifyPropertyChanged
{
	public const string appName = "WhisperDragon WPF";

	public const string untitledTempName = "Untitled";

	public string MainTitle { get; set; } = appName;

	public bool IsSaveEnabled 
	{ 
		get { return csc != null; }
	}

	private ObservableCollection<LoginSimplified> logins = new ObservableCollection<LoginSimplified>();
	public ObservableCollection<LoginSimplified> Logins
	{
		get { return this.logins; }
	}

	public LoginSimplified SelectedLogin { get; set; }

	private TabControl tabSections;

	private Window mainWindow;

	public event PropertyChangedEventHandler PropertyChanged;

	/// <summary>
	/// Our current common secrets container reference
	/// </summary>
	private CommonSecretsContainer csc = null;

	/// <summary>
	/// What save format should be used
	/// </summary>
	private DeserializationFormat saveFormat = DeserializationFormat.None;

	/// <summary>
	/// Path to current file (might be null)
	/// </summary>
	private string filePath = null;

	/// <summary>
	/// Is CommonSecretsContainer modified
	/// </summary>
	private bool isModified = false;

	/// <summary>
	/// Because we do not want to store actual passwords in memory, keep collection of derived ones (TODO: encrypt at some point)
	/// </summary>
	/// <typeparam name="string">Key Identifier</typeparam>
	/// <typeparam name="byte[]">Derived password as bytes</typeparam>
	private readonly Dictionary<string, byte[]> derivedPasswords = new Dictionary<string, byte[]>();

	

	public WhisperDragonViewModel(TabControl sections, Window window)
	{
		// TODO: remove this dummy init
		for (int i = 0; i < 5; i++ )
		{
			logins.Add(new LoginSimplified() {
				IsSecure =  i % 2 == 0,
				zeroBasedIndexNumber = i, 
				Title = Path.GetRandomFileName(), 
				Username = Path.GetRandomFileName(), 
				URL = $"https://{Path.GetRandomFileName()}",
				Email =  $"{Path.GetRandomFileName()}@{Path.GetRandomFileName()}",
				});
		}

		this.tabSections = sections;
		this.mainWindow = window;
	}


	#region Select tabs

	private ICommand selectFirstTabCommand;
	public ICommand SelectFirstTab
	{
		get
		{
			return selectFirstTabCommand 
				?? (selectFirstTabCommand = new ActionCommand(() =>
				{
					//MessageBox.Show("SomeCommand");
					this.tabSections.SelectedIndex = 0;
				}));
		}
	}

	private ICommand selectSecondTabCommand;
	public ICommand SelectSecondTab
	{
		get
		{
			return selectSecondTabCommand 
				?? (selectSecondTabCommand = new ActionCommand(() =>
				{
					//MessageBox.Show("SomeCommand");
					this.tabSections.SelectedIndex = 1;
				}));
		}
	}

	private ICommand selectThirdTabCommand;
	public ICommand SelectThirdTab
	{
		get
		{
			return selectThirdTabCommand 
				?? (selectThirdTabCommand = new ActionCommand(() =>
				{
					//MessageBox.Show("SomeCommand");
					this.tabSections.SelectedIndex = 2;
				}));
		}
	}

	#endregion // Select tabs


	#region Context menu items

	private ICommand openURLViaMenu;
	public ICommand OpenURLViaMenu
	{
		get
		{
			return openURLViaMenu
				?? (openURLViaMenu = new ActionCommand(() =>
				{
					
				}));
		}
	}

	private ICommand copyURLViaMenu;
	public ICommand CopyURLViaMenu
	{
		get
		{
			return copyURLViaMenu
				?? (copyURLViaMenu = new ActionCommand(() =>
				{
					if (this.SelectedLogin != null)
					{
						Clipboard.SetText(this.SelectedLogin.URL);
					}
				}));
		}
	}

	private ICommand copyUsernameViaMenu;
	public ICommand CopyUsernameViaMenu
	{
		get
		{
			return copyUsernameViaMenu
				?? (copyUsernameViaMenu = new ActionCommand(() =>
				{
					if (this.SelectedLogin != null)
					{
						Clipboard.SetText(this.SelectedLogin.Username);
					}
				}));
		}
	}

	private ICommand copyPasswordViaMenu;
	public ICommand CopyPasswordViaMenu
	{
		get
		{
			return copyPasswordViaMenu
				?? (copyPasswordViaMenu = new ActionCommand(() =>
				{
					if (this.SelectedLogin != null)
					{
						Clipboard.SetText(this.SelectedLogin.Password);
					}
				}));
		}
	}

	private ICommand addLoginViaMenu;
	public ICommand AddLoginViaMenu
	{
		get
		{
			return addLoginViaMenu
				?? (addLoginViaMenu = new ActionCommand(() =>
				{
					AddLoginWindow addLoginWindow = new AddLoginWindow(this.derivedPasswords.Keys.ToList(), this.AddLoginToCollection);
					addLoginWindow.ShowDialog();
				}));
		}
	}

	private ICommand editViewLoginViaMenu;
	public ICommand EditViewLoginViaMenu
	{
		get
		{
			return editViewLoginViaMenu
				?? (editViewLoginViaMenu = new ActionCommand(() =>
				{
					EditViewLoginWindow editViewLoginWindow = new EditViewLoginWindow(this.SelectedLogin, this.derivedPasswords.Keys.ToList(), this.EditLoginInCollection);
					editViewLoginWindow.ShowDialog();
				}));
		}
	}

	private ICommand duplicateLoginViaMenu;
	public ICommand DuplicateLoginViaMenu
	{
		get
		{
			return duplicateLoginViaMenu
				?? (duplicateLoginViaMenu = new ActionCommand(() =>
				{
					
				}));
		}
	}

	private ICommand deleteLoginViaMenu;
	public ICommand DeleteLoginViaMenu
	{
		get
		{
			return deleteLoginViaMenu
				?? (deleteLoginViaMenu = new ActionCommand(() =>
				{
					if (this.SelectedLogin != null)
					{
						logins.Remove(this.SelectedLogin);
					}
				}));
		}
	}

	

	#endregion // Context menu items


	#region Tools

	private ICommand createNewCommonSecretsContainerViaMenu;

	public ICommand CreateNewCommonSecretsContainerViaMenu
	{
		get
		{
			return createNewCommonSecretsContainerViaMenu 
				?? (createNewCommonSecretsContainerViaMenu = new ActionCommand(() =>
				{
					CreateCommonSecretsWindow createCommonSecretsWindow = new CreateCommonSecretsWindow(this.CreateNewCommonSecrets);
					createCommonSecretsWindow.ShowDialog();
				}));
		}
	}

	private ICommand openCommonSecretsContainerViaMenu;

	public ICommand OpenCommonSecretsContainerViaMenu
	{
		get
		{
			return openCommonSecretsContainerViaMenu 
				?? (openCommonSecretsContainerViaMenu = new ActionCommand(() =>
				{
					OpenFileDialog openFileDialog = new OpenFileDialog();
					openFileDialog.Filter = "CommonSecrets JSON (*.commonsecrets.json)|*.commonsecrets.json|CommonSecrets XML (*.commonsecrets.xml)|*.commonsecrets.xml|All files (*.*)|*.*";
					if (openFileDialog.ShowDialog() == true)
					{
						// First check what is the file format
						byte[] allBytes = File.ReadAllBytes(openFileDialog.FileName);
						DeserializationFormat fileFormat = DeserializationFormat.None;
						foreach (var kvp in DeserializationDefinitions.deserializers)
						{
							if (kvp.Value.isThisFormat(allBytes))
							{
								fileFormat = kvp.Key;
								break;
							}
						}

						if (fileFormat == DeserializationFormat.None || fileFormat == DeserializationFormat.Unknown)
						{
							MessageBox.Show($"Cannot identify format of file: {openFileDialog.FileName}", "Error");
							return;
						}

						// Try to deserialize
						CommonSecretsContainer tempContainer = DeserializationDefinitions.deserializers[fileFormat].deserialize(allBytes);

						// If contains secrets, then show secondary open step (which basically asks for passwords)
						SecondaryOpenStepWindow secondaryOpenStepWindow = new SecondaryOpenStepWindow(tempContainer.keyDerivationFunctionEntries,
																				(Dictionary<string,byte[]> derivedPasswords) => this.FinalOpenStepWithSecrets(openFileDialog.FileName, fileFormat, tempContainer, derivedPasswords));
						secondaryOpenStepWindow.ShowDialog();
					}
				}));
		}
	}

	/// <summary>
	/// Final open step when file contains secrets
	/// </summary>
	/// <param name="filename">Filename</param>
	/// <param name="fileFormat">File format</param>
	/// <param name="tempContainer">Temp container</param>
	/// <param name="newDerivedPasswords">New derived passwords</param>
	private void FinalOpenStepWithSecrets(string filename, DeserializationFormat fileFormat, CommonSecretsContainer tempContainer, Dictionary<string, byte[]> newDerivedPasswords)
	{
		// Check that every entry can be decoded with given passwords
		bool success = true;
		foreach (LoginInformationSecret loginInformationSecret in tempContainer.loginInformationSecrets)
		{
			if (!loginInformationSecret.CanBeDecryptedWithDerivedPassword(newDerivedPasswords[loginInformationSecret.GetKeyIdentifier()]))
			{
				MessageBox.Show($"Cannot decrypt login information secret which uses key identifier: {loginInformationSecret.GetKeyIdentifier()}", "Decryption error");
				success = false;
				break;
			}
		}

		if (!success)
		{
			return;
		}

		foreach (NoteSecret noteSecret in tempContainer.noteSecrets)
		{
			if (!noteSecret.CanBeDecryptedWithDerivedPassword(newDerivedPasswords[noteSecret.GetKeyIdentifier()]))
			{
				MessageBox.Show($"Cannot decrypt note secret which uses key identifier: {noteSecret.GetKeyIdentifier()}", "Decryption error");
				success = false;
				break;
			}
		}

		if (!success)
		{
			return;
		}

		foreach (FileEntrySecret fileEntrySecret in tempContainer.fileSecrets)
		{
			if (!fileEntrySecret.CanBeDecryptedWithDerivedPassword(newDerivedPasswords[fileEntrySecret.GetKeyIdentifier()]))
			{
				MessageBox.Show($"Cannot decrypt file secret which uses key identifier: {fileEntrySecret.GetKeyIdentifier()}", "Decryption error");
				success = false;
				break;
			}
		}

		if (!success)
		{
			return;
		}

		// SUCCESS POINT
		this.derivedPasswords.Clear();
		foreach (var kvp in newDerivedPasswords)
		{
			this.derivedPasswords.Add(kvp.Key, kvp.Value);
		}

		this.csc = tempContainer;
		this.isModified = false;
		this.filePath = filename;

		// Select the save format based on format of file opened (and assuming we know how to save it)
		if (DeserializationDefinitions.deserializers[fileFormat].savingSupported)
		{
			this.saveFormat = fileFormat;
		}
		else
		{
			this.saveFormat = DeserializationFormat.None;
		}
		
		this.UpdateMainTitle(filename);

		// Enable save features
		OnPropertyChanged(nameof(this.IsSaveEnabled));

		this.GenerateLoginSimplifiedsFromCommonSecrets();
	}

	private ICommand saveCommonSecretsContainerViaMenu;

	public ICommand SaveCommonSecretsContainerViaMenu
	{
		get
		{
			return saveCommonSecretsContainerViaMenu 
				?? (saveCommonSecretsContainerViaMenu = new ActionCommand(() =>
				{
					this.ActualSaveCommonSecretsContainer();
				}));
		}
	}

	public bool ActualSaveCommonSecretsContainer()
	{
		// Check that we know what save format should be used
		if (this.saveFormat == DeserializationFormat.None)
		{
			return this.ActualSaveAsCommonSecretsContainer();
		}

		// Check that there is a least one secret when saving (otherwise there is no way to verify passwords when opening)
		if (!this.CommonSecretsContainerHasAtLeastOneSecret())
		{
			MessageBox.Show("There must be at least one secret! Otherwise no password verification can be done for file.", "Error");
			return false;
		}

		// Check that writing to file is still possible
		bool isWritePossible = false;
		using (var fs = File.OpenWrite(this.filePath))
		{
			isWritePossible = fs.CanWrite;
		}

		if (!isWritePossible)
		{
			MessageBox.Show($"File {this.filePath} is not writeable!", "Error");
			return false;
		}

		try
		{
			// Use serializer selected earlier
			byte[] bytesToWrite = SerializationDefinitions.serializers[this.saveFormat](this.csc);
			File.WriteAllBytes(this.filePath, bytesToWrite);
			this.isModified = false;
			this.UpdateMainTitle(this.filePath);
			return true;
		}
		catch (Exception e)
		{
			MessageBox.Show($"Error happened while saving: {e}", "Error");
		}

		return false;
	}

	private ICommand saveAsCommonSecretsContainerViaMenu;

	public ICommand SaveAsCommonSecretsContainerViaMenu
	{
		get
		{
			return saveAsCommonSecretsContainerViaMenu 
				?? (saveAsCommonSecretsContainerViaMenu = new ActionCommand(() =>
				{
					this.ActualSaveAsCommonSecretsContainer();
				}));
		}
	}

	public bool ActualSaveAsCommonSecretsContainer()
	{
		// Check that there is a least one secret when saving (otherwise there is no way to verify passwords when opening)
		if (!this.CommonSecretsContainerHasAtLeastOneSecret())
		{
			MessageBox.Show("There must be at least one secret! Otherwise no password verification can be done for file.", "Error");
			return false;
		}

		SaveFileDialog saveFileDialog = new SaveFileDialog();
		saveFileDialog.Filter = "CommonSecrets JSON (*.commonsecrets.json)|*.commonsecrets.json|CommonSecrets XML (*.commonsecrets.xml)|*.commonsecrets.xml";
		saveFileDialog.Title = "Save a CommonSecrets file";
		if (saveFileDialog.ShowDialog() == true && !string.IsNullOrEmpty(saveFileDialog.FileName))
		{
			try
			{
				// Assume JSON for now
				byte[] jsonBytes = SerializationDefinitions.serializers[DeserializationFormat.Json](this.csc);
				File.WriteAllBytes(saveFileDialog.FileName, jsonBytes);
				this.filePath = saveFileDialog.FileName;
				this.saveFormat = DeserializationFormat.Json;
				this.isModified = false;
				this.UpdateMainTitle(this.filePath);
				return true;
			}
			catch (Exception e)
			{
				MessageBox.Show($"Error happened while saving: {e}", "Error");
			}
		}

		return false;
	}

	private bool CommonSecretsContainerHasAtLeastOneSecret()
	{
		return this.csc != null && (this.csc.loginInformationSecrets.Count > 0 || this.csc.noteSecrets.Count > 0 || this.csc.fileSecrets.Count > 0);
	}

	private ICommand generatePasswordViaMenu;
	public ICommand GeneratePasswordViaMenu
	{
		get
		{
			return generatePasswordViaMenu 
				?? (generatePasswordViaMenu = new ActionCommand(() =>
				{
					CreatePasswordWindow passwordWindow = new CreatePasswordWindow();
					passwordWindow.ShowDialog();
				}));
		}
	}

	private ICommand preferencesViaMenu;
	public ICommand PreferencesViaMenu
	{
		get
		{
			return preferencesViaMenu 
				?? (preferencesViaMenu = new ActionCommand(() =>
				{
					PreferencesWindow preferencesWindow = new PreferencesWindow("c:\\temp\\something.json");
					preferencesWindow.ShowDialog();
				}));
		}
	}

	#endregion // Tools

	#region Help

	private ICommand showAboutViaMenu;

	public ICommand ShowAboutViaMenu
	{
		get
		{
			return showAboutViaMenu
				?? (showAboutViaMenu = new ActionCommand(() =>
				{
					AboutWindow aboutWindow = new AboutWindow();
					aboutWindow.ShowDialog();
				}));
		}
	}

	#endregion // Help

	private ICommand addLoginViaButton;
	public ICommand AddLoginViaButton
	{
		get
		{
			return addLoginViaButton 
				?? (addLoginViaButton = new ActionCommand(() =>
				{
					AddLoginWindow addLoginWindow = new AddLoginWindow(this.derivedPasswords.Keys.ToList(), this.AddLoginToCollection);
					addLoginWindow.ShowDialog();
				}));
		}
	}

	private void AddLoginToCollection(LoginSimplified newLogin, string keyIdentifier)
	{
		LoginInformation loginToAdd = new LoginInformation(newLogin.Title, newLogin.URL, newLogin.Email, newLogin.Username, newLogin.Password, 
															newLogin.Notes, newLogin.Icon, newLogin.Category, newLogin.Tags);
		if (newLogin.IsSecure)
		{
			this.csc.AddLoginInformationSecret(this.derivedPasswords[keyIdentifier], loginToAdd, keyIdentifier);
		}
		else
		{
			this.csc.loginInformations.Add(loginToAdd);
		}

		// Adding a login information modifies the structure
		this.isModified = true;
		this.UpdateMainTitle(this.filePath != null ? this.filePath : untitledTempName);

		this.GenerateLoginSimplifiedsFromCommonSecrets();
	}

	private void EditLoginInCollection(LoginSimplified editedLogin, string keyIdentifier)
	{

	}

	#region New, Open, Save, Close

	private void CreateNewCommonSecrets(KeyDerivationFunctionEntry kdfe, string password)
	{
		this.derivedPasswords.Clear();
		this.derivedPasswords[kdfe.GetKeyIdentifier()] = kdfe.GeneratePasswordBytes(password);

		this.csc = new CommonSecretsContainer(kdfe);

		LoginInformation demoLogin = new LoginInformation("Demo login", "https://localhost", "sample@email.com", "Dragon", "gwWTY#¤&%36", "This login will expire in 2030", new byte[] {}, "Samples", "Samples\tDemo");

		(bool success, string possibleError) = this.csc.AddLoginInformationSecret(password, demoLogin, kdfe.GetKeyIdentifier());

		if (!success)
		{
			MessageBox.Show($"Error when adding demo secret: {possibleError}", "Error");
			return;
		}
		
		this.GenerateLoginSimplifiedsFromCommonSecrets();
		
		this.isModified = true;
		this.UpdateMainTitle(untitledTempName);
		OnPropertyChanged(nameof(this.IsSaveEnabled));
	}

	#endregion // New, Open, Save, Close

	#region Exit

	private ICommand tryToExitViaMenu;
	public ICommand TryToExitViaMenu
	{
		get
		{
			return tryToExitViaMenu 
				?? (tryToExitViaMenu = new ActionCommand(() =>
				{
					this.mainWindow.Close();
				}));
		}
	}

    public CloseType CanExecuteClosing()
    {
		if (this.isModified)
		{
			string file = this.filePath != null ? this.filePath : untitledTempName;
			MessageBoxResult result = MessageBox.Show($"Do you want to save your changes to {file} ?",
					appName,
					MessageBoxButton.YesNoCancel);

			if (this.filePath == null && result == MessageBoxResult.Yes)
			{
				return CloseType.SaveAs;
			}
			else if (result == MessageBoxResult.Yes)
			{
				return CloseType.SaveAndClose;
			}
			else if (result == MessageBoxResult.No)
			{
				return CloseType.Close;
			}
			else
			{
				return CloseType.Cancel;
			}
		}

		return CloseType.Close;
    }

	#endregion // Exit

	#region Title generation

	private void UpdateMainTitle(string fileName)
	{
		string possibleAsterisk = this.isModified ? "*" : "";
		this.MainTitle = $"{possibleAsterisk}{fileName} - {appName}";
		OnPropertyChanged(nameof(this.MainTitle));
	}

	#endregion // Title generation

	#region Common

	private void GenerateLoginSimplifiedsFromCommonSecrets()
	{
		this.logins.Clear();
		List<LoginSimplified> newLogins = LoginSimplified.TurnIntoUICompatible(this.csc.loginInformations, this.csc.loginInformationSecrets, this.derivedPasswords);

		foreach (LoginSimplified login in newLogins)
		{
			this.logins.Add(login);
		}
	}

	#endregion // Common

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