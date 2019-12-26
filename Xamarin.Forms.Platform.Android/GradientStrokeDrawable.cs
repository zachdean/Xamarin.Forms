using System.Linq;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using AColor = Android.Graphics.Color;

namespace Xamarin.Forms.Platform.Android
{
	public class GradientStrokeDrawable : PaintDrawable
	{
		Paint _strokePaint;

		public GradientStrokeDrawable()
		{
			_strokePaint = new Paint
			{
				Dither = true,
				AntiAlias = true
			};
			_strokePaint.SetStyle(Paint.Style.Stroke);
		}

		public void SetColor(AColor backgroundColor)
		{
			_strokePaint.Color = backgroundColor;
		}

		public void SetStroke(int strokeWidth, AColor strokeColor)
		{
			_strokePaint.StrokeWidth = strokeWidth;
			_strokePaint.Color = strokeColor;
			InvalidateSelf();
		}

		public void SetGradient(Brush brush)
		{
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

				var linearGradientShader = new LinearGradientShader(colors, positions, x1, y1, x2, y2);
				SetShaderFactory(new GradientShaderFactory(linearGradientShader));
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

				var radialGradientShader = new RadialGradientShader(colors, positions, centerX, centerY, radiusX, radiusY);
				SetShaderFactory(new GradientShaderFactory(radialGradientShader));
			}
		}

		protected override void OnDraw(Shape shape, Canvas canvas, Paint paint)
		{
			base.OnDraw(shape, canvas, paint);

			shape.Draw(canvas, _strokePaint);
		}

		public abstract class IGradientShader
		{
			public int[] Colors { get; set; }
			public float[] Positions { get; set; }
		}

		public class LinearGradientShader : IGradientShader
		{
			public LinearGradientShader()
			{

			}

			public LinearGradientShader(int[] colors, float[] positions, float x1, float y1, float x2, float y2)
			{
				Colors = colors;
				Positions = positions;
				X1 = x1;
				Y1 = x1;
				X2 = x2;
				Y2 = y2;
			}

			public float X1 { get; set; }
			public float Y1 { get; set; }
			public float X2 { get; set; }
			public float Y2 { get; set; }
		}

		public class RadialGradientShader : IGradientShader
		{
			public RadialGradientShader()
			{

			}

			public RadialGradientShader(int[] colors, float[] positions, float centerX, float centerY, float radiusX, float radiusY)
			{
				Colors = colors;
				Positions = positions;
				CenterX = centerX;
				CenterY = centerY;
				RadiusX = radiusX;
				RadiusY = radiusY;
			}

			public float CenterX { get; set; }
			public float CenterY { get; set; }
			public float RadiusX { get; set; }
			public float RadiusY { get; set; }
		}

		internal class GradientShaderFactory : ShaderFactory
		{
			readonly IGradientShader _gradientShader;

			public GradientShaderFactory(IGradientShader gradientShader)
			{
				_gradientShader = gradientShader;
			}

			public override Shader Resize(int width, int height)
			{
				if (width == 0 && height == 0)
					return null;

				if (_gradientShader is LinearGradientShader linearGradientShader)
				{
					if (linearGradientShader.Colors.Length < 2)
						return null;

					return new LinearGradient(
						width * linearGradientShader.X1,
						height * linearGradientShader.Y1,
						width * linearGradientShader.X2,
						height * linearGradientShader.Y2,
						linearGradientShader.Colors,
						linearGradientShader.Positions,
						Shader.TileMode.Clamp);
				}

				if (_gradientShader is RadialGradientShader radialGradientShader)
				{
					if (radialGradientShader.Colors.Length < 2)
						return null;

					return new RadialGradient(
						width * radialGradientShader.CenterX,
						height * radialGradientShader.CenterY,
						(width * radialGradientShader.RadiusX + height * radialGradientShader.RadiusY) / 2,
						radialGradientShader.Colors,
						radialGradientShader.Positions,
						Shader.TileMode.Clamp);
				}
				return null;
			}
		}
	}
}