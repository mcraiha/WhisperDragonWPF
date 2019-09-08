using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WhisperDragonWPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class CreatePasswordWindow : Window
	{
		public CreatePasswordWindow()
		{
			InitializeComponent();
			DataContext = new CreatePasswordViewModel(this.OkClose);
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