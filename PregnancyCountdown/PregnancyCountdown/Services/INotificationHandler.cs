namespace PregnancyCountdown.Services
{
    public interface INotificationHandler
    {
        void UpdateNotificationPreferences(bool enableNotifications);
    }
}
