using PregnancyCountdown.Models;

namespace PregnancyCountdown.Services
{
    public interface ISettingsProvider
    {
        Settings GetSettings();

        void SetSettings(Settings settings);
    }
}
