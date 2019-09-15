
namespace WhisperDragonWPF
{
	public sealed class LoginSimplified
	{
		// Non visible
		public int indexNumber { get; set; }

		// Visible elements

		public bool IsSecure { get; set; }
		public string Title { get; set; }
		public string URL { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Category { get; set; }
		public string Tags { get; set; }
		public string CreationTime { get; set; }
		public string ModificationTime { get; set; }
	}
}