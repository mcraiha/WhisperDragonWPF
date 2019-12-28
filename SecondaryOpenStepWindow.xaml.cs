using System;
using System.Collections.Generic;
using System.Windows;
using CSCommonSecrets;

namespace WhisperDragonWPF
{
	/// <summary>
	/// Interaction logic for SecondaryOpenStepWindow.xaml
	/// </summary>
	public partial class SecondaryOpenStepWindow : Window
	{
		public SecondaryOpenStepWindow(List<KeyDerivationFunctionEntry> keyDerivationFunctionEntries, Action<Dictionary<string, byte[]>> finalizeOpenWithDerivedPasswords)
		{
			InitializeComponent();
			DataContext = new SecondaryOpenStepViewModel(keyDerivationFunctionEntries, finalizeOpenWithDerivedPasswords, CancelClose, OpenClose, passwordBox);
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