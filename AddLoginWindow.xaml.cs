using System;
using System.Collections.Generic;
using System.Windows;

namespace WhisperDragonWPF
{
	/// <summary>
	/// Interaction logic for AddLoginWindow.xaml
	/// </summary>
	public partial class AddLoginWindow : Window
	{
		public AddLoginWindow(List<string> keyIdentifiers, Action<LoginSimplified, string /* Key identifier */> addLogin)
		{
			InitializeComponent();
			DataContext = new AddLoginViewModel(keyIdentifiers, AddClose, CancelClose, addLogin, passwordBox);
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