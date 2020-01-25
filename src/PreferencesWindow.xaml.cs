using System;
using System.Windows;

namespace WhisperDragonWPF
{
	/// <summary>
	/// Interaction logic for PreferencesWindow.xaml
	/// </summary>
	public partial class PreferencesWindow : Window
	{
		private readonly Action<SettingsData> callOnSave;

		public PreferencesWindow(SettingsData settingsData, string location, Action<SettingsData> callOnSave)
		{
			InitializeComponent();
			this.callOnSave = callOnSave;
			DataContext = new PreferencesViewModel(settingsData, location, this.callOnSave, this.CloseCall);
		}

		private void CloseCall()
		{
			this.Close();
		}

		#region Validators


		#endregion // Validators

	}
}