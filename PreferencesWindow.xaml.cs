using System.Windows;

namespace WhisperDragonWPF
{
	/// <summary>
	/// Interaction logic for PreferencesWindow.xaml
	/// </summary>
	public partial class PreferencesWindow : Window
	{
		public PreferencesWindow(string location)
		{
			InitializeComponent();
			DataContext = new PreferencesViewModel(location, this.SaveClose, this.CancelClose);
		}

		private void SaveClose()
		{
			this.Close();
		}

		private void CancelClose()
		{
			this.Close();
		}


		#region Validators


		#endregion // Validators

	}
}