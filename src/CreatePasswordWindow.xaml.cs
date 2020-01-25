using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace WhisperDragonWPF
{
	/// <summary>
	/// Interaction logic for CreatePasswordWindow.xaml
	/// </summary>
	public partial class CreatePasswordWindow : Window
	{
		public CreatePasswordWindow(Action<string> passwordCallback)
		{
			InitializeComponent();
			DataContext = new CreatePasswordViewModel(this.OkClose, passwordCallback);
		}

		private void OkClose()
		{
			this.Close();
		}

		#region Validators

		private void PasswordLengthValidationTextBox(object sender, TextCompositionEventArgs e)
		{
			// Only numbers allowed
			foreach (char c in e.Text)
			{
				if (!char.IsDigit(c))
				{
					e.Handled = true;
					return;
				}
			}

			e.Handled = false;
		}

		#endregion // Validators

	}
}