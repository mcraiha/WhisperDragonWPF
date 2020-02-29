using System;
using System.Collections.Generic;
using System.Windows;

namespace WhisperDragonWPF
{
	/// <summary>
	/// Interaction logic for AddFileWindow.xaml
	/// </summary>
	public partial class AddFileWindow : Window
	{
		public AddFileWindow(List<string> keyIdentifiers, Action<string /* Filename */, byte[] /* byte Array */, bool /* is secret */, string /* Key identifier */> addFile)
		{
			InitializeComponent();
			DataContext = new AddFileViewModel(keyIdentifiers, AddClose, CancelClose, addFile);
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