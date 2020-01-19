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
using CSCommonSecrets;

namespace WhisperDragonWPF
{
	/// <summary>
	/// Interaction logic for CreateCommonSecretsWindow.xaml
	/// </summary>
	public partial class CreateCommonSecretsWindow : Window
	{
		private Action<KeyDerivationFunctionEntry, string> positive;

		public CreateCommonSecretsWindow(Action<KeyDerivationFunctionEntry, string> onPositive)
		{
			InitializeComponent();
			this.positive = onPositive;
			DataContext = new CreateCommonSecretsViewModel(OkClose, CancelClose, passwordBox1, passwordBox2);
		}

		private void CancelClose()
		{
			this.Close();
		}

		private void OkClose(KeyDerivationFunctionEntry kdfe, string password)
		{
			this.Close();
			this.positive(kdfe, password);
		}

		#region Validators


		#endregion // Validators

	}
}