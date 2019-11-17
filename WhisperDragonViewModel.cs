using System;
using System.Collections.Generic;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using WhisperDragonWPF;
using Microsoft.Win32;
using CSCommonSecrets;

public class WhisperDragonViewModel
{
	private ObservableCollection<LoginSimplified> logins = new ObservableCollection<LoginSimplified>();
	public ObservableCollection<LoginSimplified> Logins
	{
		get { return this.logins; }
	}

	public LoginSimplified SelectedLogin { get; set; }

	private TabControl tabSections;

	private CommonSecretsContainer csc = null;
	private string filePath = null;
	private readonly Dictionary<string, byte[]> derivedPasswords = new Dictionary<string, byte[]>();

	public WhisperDragonViewModel(TabControl sections)
	{
		for (int i = 0; i < 5; i++ )
		{
			logins.Add(new LoginSimplified() {
				IsSecure =  i % 2 == 0,
				indexNumber = i, 
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
					AddLoginWindow addLoginWindow = new AddLoginWindow(this.AddLoginToCollection);
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
					//CreateKeyDerivationFunctionWindow keyDerivationFunctionWindow = new CreateKeyDerivationFunctionWindow();
					//keyDerivationFunctionWindow.ShowDialog();
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
					if (openFileDialog.ShowDialog() == true)
					{

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
					SaveFileDialog saveFileDialog = new SaveFileDialog();
					if(saveFileDialog.ShowDialog() == true)
					{

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
					AddLoginWindow addLoginWindow = new AddLoginWindow(this.AddLoginToCollection);
					addLoginWindow.ShowDialog();
				}));
		}
	}

	private void AddLoginToCollection(LoginSimplified newLogin)
	{
		this.logins.Add(newLogin);
	}

	#region New, Open, Save, Close

	private void CreateNewCommonSecrets(KeyDerivationFunctionEntry kdfe, string password)
	{
		this.derivedPasswords.Clear();
		this.derivedPasswords[kdfe.GetKeyIdentifier()] = kdfe.GeneratePasswordBytes(password);

		this.csc = new CommonSecretsContainer(kdfe);

		LoginInformation demoLogin = new LoginInformation("Demo login", "https://localhost", "sample@email.com", "Dragon", "gwWTY#¤&%36");

		this.csc.AddLoginInformationSecret(password, demoLogin, kdfe.GetKeyIdentifier());
		
		this.logins.Clear();
		List<LoginSimplified> newLogins =LoginSimplified.TurnIntoUICompatible(csc.loginInformations, csc.loginInformationSecrets, this.derivedPasswords);

		foreach (LoginSimplified login in newLogins)
		{
			this.logins.Add(login);
		}
	}

	#endregion // New, Open, Save, Close
}