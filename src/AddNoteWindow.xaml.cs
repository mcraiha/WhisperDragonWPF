using System;
using System.Collections.Generic;
using System.Windows;

namespace WhisperDragonWPF
{
	/// <summary>
	/// Interaction logic for AddNoteWindow.xaml
	/// </summary>
	public partial class AddNoteWindow : Window
	{
		public AddNoteWindow(List<string> keyIdentifiers, Action<NoteSimplified, string /* Key identifier */> addNote)
		{
			InitializeComponent();
			DataContext = new AddNoteViewModel(keyIdentifiers, AddClose, CancelClose, addNote);
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