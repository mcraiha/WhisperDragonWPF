using System;
using System.Collections.Generic;
using System.Windows;
using CSCommonSecrets;

namespace WhisperDragonWPF
{
	/// <summary>
	/// Interaction logic for EditViewFileWindow.xaml
	/// </summary>
	public partial class EditViewFileWindow : Window
	{
		public EditViewFileWindow(FileEntry current, int zeroBasedIndexNumber, List<string> keyIdentifiers, Action<int /* zeroBasedIndexNumber */, bool /* isNowSecure */, bool /* Was Security Modified */, string /* Key identifier */> editFile)
		{
			InitializeComponent();
			DataContext = new EditViewFileViewModel(current, zeroBasedIndexNumber, keyIdentifiers, EditClose, CancelClose, editFile);
		}

		public EditViewFileWindow(FileEntrySecret current, byte[] derivedPassword, int zeroBasedIndexNumber, List<string> keyIdentifiers, Action<int /* zeroBasedIndexNumber */, bool /* isNowSecure */, bool /* Was Security Modified */, string /* Key identifier */> editFile)
		{
			InitializeComponent();
			DataContext = new EditViewFileViewModel(current, derivedPassword, zeroBasedIndexNumber, keyIdentifiers, EditClose, CancelClose, editFile);
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