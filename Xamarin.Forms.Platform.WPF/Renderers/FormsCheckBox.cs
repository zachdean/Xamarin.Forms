using WBrush = System.Windows.Media.Brush;
using WPFCheckBox = System.Windows.Controls.CheckBox;
using WSolidColorBrush = System.Windows.Media.SolidColorBrush;
using System.Windows;

namespace Xamarin.Forms.Platform.WPF
{
	public class FormsCheckBox : WPFCheckBox
	{

		public static readonly DependencyProperty TintBrushProperty =
			DependencyProperty.Register(nameof(TintBrush), typeof(Brush), typeof(FormsCheckBox),
				new PropertyMetadata(default(Brush), OnTintBrushPropertyChanged));

		static void OnTintBrushPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var checkBox = (FormsCheckBox)d;

			if (e.NewValue is SolidColorBrush solidBrush && solidBrush.Color.A == 0)
			{
				checkBox.BorderBrush = Color.Black.ToBrush();
			}
			else if (e.NewValue is WSolidColorBrush b)
			{
				checkBox.BorderBrush = b;
			}
		}

		public FormsCheckBox()
		{
			BorderBrush = Color.Black.ToBrush();
		}

		public WBrush TintBrush
		{
			get { return (WBrush)GetValue(TintBrushProperty); }
			set { SetValue(TintBrushProperty, value); }
		}
	}
}
