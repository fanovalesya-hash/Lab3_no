using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Lab3_no
{
    public class DiscountToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float floatDiscount && floatDiscount > 20)
            {
                return (Brush)new BrushConverter().ConvertFromString("#C8A2C8");
            }
            if (value is double doubleDiscount && doubleDiscount > 20)
            {
                return (Brush)new BrushConverter().ConvertFromString("#C8A2C8");
            }

            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
