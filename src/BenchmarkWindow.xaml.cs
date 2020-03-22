using System.Windows;

namespace WhisperDragonWPF
{
	/// <summary>
	/// Interaction logic for BenchmarkWindow.xaml
	/// </summary>
	public partial class BenchmarkWindow : Window
	{
		public BenchmarkWindow()
		{
			InitializeComponent();
			DataContext = new BenchmarkViewModel(OkClose);
		}

		private void OkClose()
		{
			this.Close();
		}

	}
}