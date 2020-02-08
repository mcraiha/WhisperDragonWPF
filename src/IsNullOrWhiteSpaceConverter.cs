using System;
using System.Windows.Data;
using System.Globalization;

namespace WhisperDragonWPF
{
	/*
		<Window.Resources>
			<local:IsNullOrWhiteSpaceConverter x:Key="isnullorwhitespaceconverter" />
		</Window.Resources>
	*/

	public class IsNullOrWhiteSpaceConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value.GetType() == typeof(string))
			{
				var input = (string)value;
      			return string.IsNullOrWhiteSpace(input);
			}

			throw new InvalidOperationException("IsNullOrWhiteSpaceConverter can only convert string value types");
		}
	
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}