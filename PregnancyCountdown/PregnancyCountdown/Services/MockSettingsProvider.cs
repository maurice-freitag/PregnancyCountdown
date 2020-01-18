using System;
using PregnancyCountdown.Models;

namespace PregnancyCountdown.Services
{
    public class MockSettingsProvider : ISettingsProvider
    {
        private static Settings settings = new Settings { DueDate = DateTime.Now.AddDays(40) };

        public Settings GetSettings() => settings;

#pragma warning disable S2696 // Instance members should not write to "static" fields
        public void SetSettings(Settings settings) =>
                MockSettingsProvider.settings = settings;
#pragma warning restore S2696 // Instance members should not write to "static" fields
    }
}
