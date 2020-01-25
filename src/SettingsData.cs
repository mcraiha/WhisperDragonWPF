
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
        public ShowMode TitleShowMode { get; set; } = ShowMode.ShowFull;
        
        public SettingsData()
        {

        }
    }
}