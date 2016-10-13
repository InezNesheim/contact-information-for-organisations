using System;
using System.Globalization;
using System.Windows.Data;

namespace AltinnDesktopTool.Utils.Converter
{
    public class RadioButtonCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType.IsAssignableFrom(typeof(bool)) && targetType.IsAssignableFrom(typeof(string)))
                throw new ArgumentException("RadioButtonCheckedConverter can only convert to boolean or string.");

            if (targetType == typeof(string))
                return value.ToString();

            return string.Compare(value.ToString(), (string)parameter, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType.IsAssignableFrom(typeof(bool)) && targetType.IsAssignableFrom(typeof(string)))
                throw new ArgumentException("RadioButtonCheckedConverter can only convert back value from a string or a boolean.");

            if (!targetType.IsEnum)
                throw new ArgumentException("RadioButtonCheckedConverter can only convert value to an Enum Type.");

            var s = value as string;
            if (s != null)
            {
                return Enum.Parse(targetType, s, true);
            }

            // We have a boolean, as for binding to a checkbox. we use parameter
            if ((bool)value)
                return Enum.Parse(targetType, (string)parameter, true);

            return Binding.DoNothing;
        }
    }
}
