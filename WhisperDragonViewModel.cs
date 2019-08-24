using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using WhisperDragonWPF;

public class WhisperDragonViewModel
{
	private TabControl tabSections;

	public WhisperDragonViewModel(TabControl sections)
	{
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


	#region Tools

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

	private ICommand addLoginViaButton;
	public ICommand AddLoginViaButton
	{
		get
		{
			return addLoginViaButton 
				?? (addLoginViaButton = new ActionCommand(() =>
				{
					AddLoginWindow addLoginWindow = new AddLoginWindow();
					addLoginWindow.ShowDialog();
				}));
		}
	}

	#endregion // Tools
}