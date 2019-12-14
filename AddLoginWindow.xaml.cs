using System;
using System.Windows;

namespace WhisperDragonWPF
{
	/// <summary>
	/// Interaction logic for AddLoginWindow.xaml
	/// </summary>
	public partial class AddLoginWindow : Window
	{
		public AddLoginWindow(Action<LoginSimplified> addLogin)
		{
			InitializeComponent();
			DataContext = new AddLoginViewModel(AddClose, CancelClose, addLogin, passwordBox);
		}

		private void AddClose()
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