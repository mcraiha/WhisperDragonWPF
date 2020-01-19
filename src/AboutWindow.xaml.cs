using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace WhisperDragonWPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class AboutWindow : Window
	{
		public AboutWindow()
		{
			InitializeComponent();
			DataContext = new AboutViewModel(OkClose);
		}

		private void OpenInBrowser(object sender, RequestNavigateEventArgs e)
		{
			var ps = new ProcessStartInfo(e.Uri.AbsoluteUri)
			{ 
				UseShellExecute = true, 
				Verb = "open" 
			};
			Process.Start(ps);

			e.Handled = true;
		}

		private void OkClose()
		{
			this.Close();
		}

		#region Validators


		#endregion // Validators

	}
}