
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
        public ShowMode TitleShowMode { get; set; } = ShowMode.ShowFull;

        public ShowMode UrlShowMode { get; set; } = ShowMode.ShowFull;

        public ShowMode EmailShowMode { get; set; } = ShowMode.ShowFull;

        public ShowMode UsernameShowMode { get; set; } = ShowMode.ShowFull;

        public ShowMode PasswordShowMode { get; set; } = ShowMode.HiddenConstantLenght;

        public ShowMode CategoryShowMode { get; set; } = ShowMode.ShowFull;


        // Notes
        public ShowMode NoteTitleShowMode { get; set; } = ShowMode.ShowFull;
        public ShowMode NoteTextShowMode { get; set; } = ShowMode.HiddenConstantLenght;
        
        public SettingsData()
        {

        }
    }
}