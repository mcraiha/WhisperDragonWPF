using System;
using System.Collections.Generic;
using System.Windows;

namespace WhisperDragonWPF
{
	/// <summary>
	/// Interaction logic for SecondaryOpenStepWindow.xaml
	/// </summary>
	public partial class SecondaryOpenStepWindow : Window
	{
		public SecondaryOpenStepWindow(List<string> keyIdentifiers, Action<Dictionary<string, byte[]>> secondaryOpen)
		{
			InitializeComponent();
			DataContext = new SecondaryOpenStepViewModel(keyIdentifiers, secondaryOpen, CancelClose, OpenClose, passwordBox);
		}

		private void OpenClose()
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