using System;
using PregnancyCountdown.ViewModels;

namespace PregnancyCountdown.Models
{
    public class Settings : BaseViewModel
    {
        private DateTime dueDate;
        public DateTime DueDate
        {
            get => dueDate;
            set => SetProperty(ref dueDate, value);
        }

        private string babyName;

        public string BabyName
        {
            get => babyName;
            set => SetProperty(ref babyName, value);
        }

        private bool enableNotifications;
        public bool EnableNotifications
        {
            get => enableNotifications;
            set => SetProperty(ref enableNotifications, value);
        }
    }
}