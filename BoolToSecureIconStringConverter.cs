using System;
using System.Windows.Data;
using System.Globalization;

namespace WhisperDragonWPF
{
	public class BoolToSecureIconStringConverter : IValueConverter
	{
		private static readonly string secureIcon = "🔐";
		private static readonly string unsecureIcon = "❌";

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value.GetType() == typeof(bool))
			{
				if ((bool)value)
				{
					return secureIcon;
				}
				else
				{
					return unsecureIcon;
				}
			}

			throw new InvalidOperationException("BoolToSecereIconStringConverter can only convert bool value types");
		}
	
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}