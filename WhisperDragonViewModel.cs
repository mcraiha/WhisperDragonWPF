using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using WhisperDragonWPF;
using Microsoft.Win32;
using CSCommonSecrets;

public class WhisperDragonViewModel : INotifyPropertyChanged
{
	public const string appName = "WhisperDragon WPF";
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

	public event PropertyChangedEventHandler PropertyChanged;

	/// <summary>
	/// Our current common secrets container reference
	/// </summary>
	private CommonSecretsContainer csc = null;

	// TODO: Add save format structure

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

	// Settings for JSON serialization
	private static readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions
	{
		WriteIndented = true
	};

	public WhisperDragonViewModel(TabControl sections)
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

						// Try to deserialize

						// If contains secrets, then show secondary open step (which basically asks for passwords)
						SecondaryOpenStepWindow secondaryOpenStepWindow = new SecondaryOpenStepWindow(null, null);
						secondaryOpenStepWindow.ShowDialog();
					}
				}));
		}
	}

	private ICommand saveCommonSecretsContainerViaMenu;

	public ICommand SaveCommonSecretsContainerViaMenu
	{
		get
		{
			return saveCommonSecretsContainerViaMenu 
				?? (saveCommonSecretsContainerViaMenu = new ActionCommand(() =>
				{
					// Check that writing to file is still possible
				}));
		}
	}

	private ICommand saveAsCommonSecretsContainerViaMenu;

	public ICommand SaveAsCommonSecretsContainerViaMenu
	{
		get
		{
			return saveAsCommonSecretsContainerViaMenu 
				?? (saveAsCommonSecretsContainerViaMenu = new ActionCommand(() =>
				{
					// TODO: Check that there is a least one secret when saving (otherwise there is no way to verify passwords when opening)

					SaveFileDialog saveFileDialog = new SaveFileDialog();
					saveFileDialog.Filter = "CommonSecrets JSON (*.commonsecrets.json)|*.commonsecrets.json|CommonSecrets XML (*.commonsecrets.xml)|*.commonsecrets.xml";
					saveFileDialog.Title = "Save a CommonSecrets file";
					if (saveFileDialog.ShowDialog() == true && !string.IsNullOrEmpty(saveFileDialog.FileName))
					{
						try
						{
							// Assume JSON for now
							string json = JsonSerializer.Serialize(this.csc, serializerOptions);
							File.WriteAllText(saveFileDialog.FileName, json);
							this.filePath = saveFileDialog.FileName;
							this.UpdateMainTitle(this.filePath);
						}
						catch (Exception e)
						{
							MessageBox.Show($"Error happened while saving: {e}", "Error");
						}
					}
				}));
		}
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

		this.GenerateLoginSimplifiedsFromCommonSecrets();
	}

	#region New, Open, Save, Close

	private void CreateNewCommonSecrets(KeyDerivationFunctionEntry kdfe, string password)
	{
		this.derivedPasswords.Clear();
		this.derivedPasswords[kdfe.GetKeyIdentifier()] = kdfe.GeneratePasswordBytes(password);

		this.csc = new CommonSecretsContainer(kdfe);

		LoginInformation demoLogin = new LoginInformation("Demo login", "https://localhost", "sample@email.com", "Dragon", "gwWTY#¤&%36");

		(bool success, string possibleError) = this.csc.AddLoginInformationSecret(password, demoLogin, kdfe.GetKeyIdentifier());

		if (!success)
		{
			MessageBox.Show($"Error when adding demo secret: {possibleError}", "Error");
			return;
		}
		
		this.GenerateLoginSimplifiedsFromCommonSecrets();
		
		this.isModified = true;
		this.UpdateMainTitle("Untitled");
		OnPropertyChanged(nameof(this.IsSaveEnabled));
	}

	#endregion // New, Open, Save, Close

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