using System;
using System.Linq;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using static Android.Graphics.Drawables.GradientDrawable;
using AView = Android.Views.View;

namespace Xamarin.Forms.Platform.Android
{
	public static class BrushExtensions
	{
		public static void SetGradient(this AView view, Brush brush)
		{
			GradientStopCollection gradients = null;

			if (brush is SolidColorBrush solidColorBrush)
			{
				var backgroundColor = solidColorBrush.Color;

				if (backgroundColor.IsDefault)
					view.SetBackground(null);
				else
					view.SetBackgroundColor(backgroundColor.ToAndroid());

				return;
			}

			if (brush is LinearGradientBrush linearGradientBrush)
				gradients = linearGradientBrush.GradientStops;

			if (brush is RadialGradientBrush radialGradientBrush)
				gradients = radialGradientBrush.GradientStops;

			if (gradients == null || !gradients.Any())
				return;

			view.SetPaintGradient(brush);
		}

		public static void SetGradient(this Paint paint, Brush brush, int height, int width)
		{
			if(brush is SolidColorBrush solidColorBrush)
			{
				var backgroundColor = solidColorBrush.Color;
				paint.Color = backgroundColor.ToAndroid();
			}

			if (brush is LinearGradientBrush linearGradientBrush)
			{
				var p1 = linearGradientBrush.StartPoint;
				var x1 = (float)p1.X;
				var y1 = (float)p1.Y;

				var p2 = linearGradientBrush.EndPoint;
				var x2 = (float)p2.X;
				var y2 = (float)p2.Y;

				var orderedStops = linearGradientBrush.GradientStops.Distinct().OrderBy(x => x.Offset).ToList();
				var colors = orderedStops.Select(x => x.Color.ToAndroid().ToArgb()).ToArray();
				var positions = orderedStops.Select(x => x.Offset).ToArray();

				if (colors.Length < 2)
					return;
				
				var linearGradientShader = new LinearGradient(
					width * x1,
					height * y1,
					width * x2,
					height * y2,
					colors,
					positions,
					Shader.TileMode.Clamp);

				paint.SetShader(linearGradientShader);
			}

			if (brush is RadialGradientBrush radialGradientBrush)
			{
				var center = radialGradientBrush.GradientOrigin;
				float centerX = (float)center.X;
				float centerY = (float)center.Y;
				float radiusX = (float)radialGradientBrush.RadiusX;
				float radiusY = (float)radialGradientBrush.RadiusY;

				var orderedStops = radialGradientBrush.GradientStops.Distinct().OrderBy(b => b.Offset).ToList();
				var colors = orderedStops.Select(s => s.Color.ToAndroid().ToArgb()).ToArray();
				var positions = orderedStops.Select(c => c.Offset).ToArray();

				if (colors.Length < 2)
					return;

				var radialGradientShader = new RadialGradient(					
					width * centerX,
					height * centerY,
					(width * radiusX + height * radiusY) / 2,
					colors,
					positions,
					Shader.TileMode.Clamp);

				paint.SetShader(radialGradientShader);
			}
		}

		public static void SetGradient(this GradientDrawable gradientDrawable, Brush brush, int height, int width)
		{
			if (brush == null && brush.IsEmpty)
				return;

			if (brush is SolidColorBrush solidColorBrush)
			{
				Color bgColor = solidColorBrush.Color;
				gradientDrawable.SetColor(bgColor.IsDefault ? Color.White.ToAndroid() : bgColor.ToAndroid());
			}

			if (brush is LinearGradientBrush linearGradientBrush)
			{
				var p1 = linearGradientBrush.StartPoint;
				var x1 = (float)p1.X;
				var y1 = (float)p1.Y;

				var p2 = linearGradientBrush.EndPoint;
				var x2 = (float)p2.X;
				var y2 = (float)p2.Y;

				const double Rad2Deg = 180.0 / Math.PI;
				var angle = Math.Atan2(y2 - y1, x2 - x1) * Rad2Deg;

				var orderedStops = linearGradientBrush.GradientStops.Distinct().OrderBy(x => x.Offset).ToList();
				var colors = orderedStops.Select(x => x.Color.ToAndroid().ToArgb()).ToArray();
				var positions = orderedStops.Select(x => x.Offset).ToArray();

				if (colors.Length < 2)
					return;

				gradientDrawable.SetGradientType(GradientType.LinearGradient);
				gradientDrawable.SetColors(colors);
				gradientDrawable.SetGradientOrientation(angle);
			}

			if (brush is RadialGradientBrush radialGradientBrush)
			{
				var center = radialGradientBrush.GradientOrigin;
				float centerX = (float)center.X;
				float centerY = (float)center.Y;
				float radiusX = (float)radialGradientBrush.RadiusX / 2;
				float radiusY = (float)radialGradientBrush.RadiusY / 2;

				var orderedStops = radialGradientBrush.GradientStops.Distinct().OrderBy(b => b.Offset).ToList();
				var colors = orderedStops.Select(s => s.Color.ToAndroid().ToArgb()).ToArray();
				var positions = orderedStops.Select(c => c.Offset).ToArray();

				if (colors.Length < 2)
					return;

				gradientDrawable.SetGradientType(GradientType.RadialGradient);
				gradientDrawable.SetGradientCenter(centerX, centerY);
				gradientDrawable.SetGradientRadius((width * radiusX + height * radiusY) / 2);
				gradientDrawable.SetColors(colors);
			}
		}

		internal static void SetPaintGradient(this AView view, Brush brush)
		{
			var gradientStrokeDrawable = new GradientStrokeDrawable
			{
				Shape = new RectShape()
			};
			gradientStrokeDrawable.SetGradient(brush);

			view.Background?.Dispose();
			view.Background = gradientStrokeDrawable;
		}

		internal static void SetGradientOrientation(this GradientDrawable drawable, double angle)
		{
			var orientation = 
				angle >= 0 && angle < 45 ? Orientation.LeftRight :
				angle < 90 ? Orientation.BlTr :
				angle < 135 ? Orientation.BottomTop :
				angle < 180 ? Orientation.BrTl :
				angle < 225 ? Orientation.RightLeft :
				angle < 270 ? Orientation.TrBl :
				angle < 315 ? Orientation.TopBottom : Orientation.TlBr;

			drawable.SetOrientation(orientation);
		}
	}
}