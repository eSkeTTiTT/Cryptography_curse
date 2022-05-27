using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Cryptography_curse.Infrastructure.Converters.Base
{
    public abstract class MultiConverterBase<TMultiConverter> : MarkupExtension, IMultiValueConverter
        where TMultiConverter : class, new()
    {
        private static TMultiConverter _valueProvide;

        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        public virtual object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _valueProvide ??= new TMultiConverter();
        }
    }
}
