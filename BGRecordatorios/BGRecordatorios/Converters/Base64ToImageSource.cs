﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.IO;
using Xamarin.Forms;

namespace BGRecordatorios.Converters
{
    public class Base64ToImageSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                byte[] bytes = System.Convert.FromBase64String(value.ToString());
                return ImageSource.FromStream(() => new MemoryStream(bytes));
            }
            return null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
