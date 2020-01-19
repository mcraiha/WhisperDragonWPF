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
	/// Interaction logic for CreateKeyDerivationFunctionWindow.xaml
	/// </summary>
	public partial class CreateKeyDerivationFunctionWindow : Window
	{
		public CreateKeyDerivationFunctionWindow()
		{
			InitializeComponent();
			DataContext = new CreateKeyDerivationFunctionViewModel();
		}

		private void OkClose()
		{
			this.Close();
		}

		#region Validators


		#endregion // Validators

	}
}