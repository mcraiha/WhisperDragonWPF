using System;
using System.Collections.Generic;
using System.Windows;

namespace WhisperDragonWPF
{
	/// <summary>
	/// Interaction logic for EditViewLoginWindow.xaml
	/// </summary>
	public partial class EditViewLoginWindow : Window
	{
		public EditViewLoginWindow(LoginSimplified current, List<string> keyIdentifiers, Action<LoginSimplified, bool /* Was Security Modified */, string /* Key identifier */> editLogin)
		{
			InitializeComponent();
			DataContext = new EditViewLoginViewModel(current, keyIdentifiers, EditClose, CancelClose, editLogin, passwordBox);
		}

		private void EditClose()
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