
namespace WhisperDragonWPF
{
    public enum ShowMode
    {
        ShowFull = 0,
        ShowFirstFour,
        ShowFirst,
        HiddenCorrectLength,
        HiddenConstantLenght,
        HiddenRandomLength
    }

    /// <summary>
    /// All settings for WhisperDragonWPF
    /// </summary>
    public class SettingsData
    {
        // Logins
        public ShowMode LoginTitleShowMode { get; set; } = ShowMode.ShowFull;

        public ShowMode LoginUrlShowMode { get; set; } = ShowMode.ShowFull;

        public ShowMode LoginEmailShowMode { get; set; } = ShowMode.ShowFull;

        public ShowMode LoginUsernameShowMode { get; set; } = ShowMode.ShowFull;

        public ShowMode LoginPasswordShowMode { get; set; } = ShowMode.HiddenConstantLenght;

        public ShowMode LoginCategoryShowMode { get; set; } = ShowMode.ShowFull;


        // Notes
        public ShowMode NoteTitleShowMode { get; set; } = ShowMode.ShowFull;
        public ShowMode NoteTextShowMode { get; set; } = ShowMode.HiddenConstantLenght;


        // Files
        public ShowMode FileFilenameShowMode { get; set; } = ShowMode.ShowFull;
        public ShowMode FileFilesizeShowMode { get; set; } = ShowMode.ShowFull;
        public ShowMode FileFiletypeShowMode { get; set; } = ShowMode.ShowFull;
        
        public SettingsData()
        {

        }
    }
}