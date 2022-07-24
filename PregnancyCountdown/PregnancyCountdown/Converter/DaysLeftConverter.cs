using System;
using System.Globalization;
using Xamarin.Forms;

namespace PregnancyCountdown.Converter
{
    public class DaysLeftConverter : IValueConverter
    {
        public static int Convert(DateTime dueDate) => (int)Math.Max(Math.Ceiling((dueDate - DateTime.Now).TotalDays), 0);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dueDate)
                return Convert(dueDate);
            return default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int days && targetType == typeof(DateTime))
                return DateTime.Now.AddDays(days);
            else if (int.TryParse(value as string, out days))
                return DateTime.Now.AddDays(days);
            return default;
        }
    }
}
