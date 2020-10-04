// ---------------------------------------
// Print code sample PDFRasterizer.NET 
//
// Copyright TallComponents BV 2002-2014
// ---------------------------------------

namespace ScaledPrintingWPF
{
    using System;
    using System.Windows.Data;

    /// <summary>
    /// This is used in all radio buttons on the WPF form to covert an enumerator into a boolean 
    /// that indicates whether the button is selected or not (depending on the property in the view-model)
    /// And, of course, vise versa
    /// </summary>
    public class EnumToBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value of a radio button 
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Equals(parameter);
        }

        /// <summary>
        /// Converts a value of a radio button back.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Equals(true) ? parameter : Binding.DoNothing;
        }
    }
}