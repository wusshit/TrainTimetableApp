// BooleanToVisibilityConverter.cs
using System;
using System.Globalization;
using System.Windows;       // Required for Visibility
using System.Windows.Data;  // Required for IValueConverter

// **** Ensure this namespace is correct for your project ****
namespace TrainTimetableApp
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : IValueConverter
    {
        // Converts bool to Visibility
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = false;
            if (value is bool b)
            {
                boolValue = b;
            }

            // Parameter "Invert" flips the logic (true->Collapsed, false->Visible)
            bool invert = parameter as string == "Invert";
            if (invert)
            {
                boolValue = !boolValue;
            }

            // Use Collapsed so it doesn't take up layout space when hidden
            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        // Converts Visibility back to bool (not needed for one-way bindings)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Not implemented for one-way bindings
            throw new NotImplementedException();
        }
    }
}