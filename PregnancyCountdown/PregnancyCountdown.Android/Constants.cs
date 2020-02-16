namespace PregnancyCountdown.Droid
{
    public static class Constants
    {
        public const string FullAccessConnectionString = @"Endpoint=sb://pregnancycountdownnotificationhubnamespace.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=+0E8z5H9Gt3TSaJ+JNqE9GB5wIU825U3sWr+iEfoK5c=";
        public const string ListenConnectionString = @"Endpoint=sb://pregnancycountdownnotificationhubnamespace.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=Okq45WwRbxQLuTffCoUruw8ta1IKKxHnYy48pmFXRMo=";
        public const string NotificationHubName = "PregnancyCountdownNotificationHub";
        public const string RegisterDeviceUri = @"https://pregnancycountdownnotificationserver.azurewebsites.net/api/RegisterDevice?code=l4n/bj5Ody75LIpYm3agSrIumOisQMZdVsskQgMlCFgoFPWbIZMjRQ==";
        public const string UnregisterDeviceUri = @"https://pregnancycountdownnotificationserver.azurewebsites.net/api/UnregisterDevice?code=kZbiyjJop3IxuHKa0mff/DlRUXdLiuVz429r9oLVYsgVIxKkzyDOcg==";
    }
}