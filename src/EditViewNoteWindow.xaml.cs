using System;
using System.Collections.Generic;
using System.Windows;

namespace WhisperDragonWPF
{
	/// <summary>
	/// Interaction logic for EditViewNoteWindow.xaml
	/// </summary>
	public partial class EditViewNoteWindow : Window
	{
		public EditViewNoteWindow(NoteSimplified current, List<string> keyIdentifiers, Action<NoteSimplified, bool /* Was Security Modified */, string /* Key identifier */> editNote)
		{
			InitializeComponent();
			DataContext = new EditViewNoteViewModel(current, keyIdentifiers, EditClose, CancelClose, editNote);
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