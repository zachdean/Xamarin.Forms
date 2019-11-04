using System;
using System.ComponentModel;
using System.Linq;
using Android.Graphics;
using Android.Graphics.Drawables;
using ACanvas = Android.Graphics.Canvas;
using AColor = Android.Graphics.Color;

namespace Xamarin.Forms.Platform.Android
{
	public class BackgroundDrawable : Drawable
	{
		readonly VisualElement _element;
		Bitmap _normalBitmap;
		bool _isDisposed;

		public override int Opacity
		{
			get { return 0; }
		}

		public BackgroundDrawable(VisualElement element)
		{
			_element = element;
			_element.PropertyChanged += PancakeViewOnPropertyChanged;
		}

		public override void Draw(ACanvas canvas)
		{
			int width = Bounds.Width();
			int height = Bounds.Height();

			if (width <= 0 || height <= 0)
			{
				DisposeBitmap();
				return;
			}

			try
			{
				if (_normalBitmap == null || _normalBitmap.Height != height || _normalBitmap.Width != width)
				{
					DisposeBitmap();
					_normalBitmap = CreateBitmap(width, height);
				}
			}
			catch (ObjectDisposedException)
			{
				_normalBitmap = CreateBitmap(width, height);
			}

			using (var paint = new Paint())
			{
				canvas.DrawBitmap(_normalBitmap, 0, 0, paint);
			}
		}

		private void DisposeBitmap()
		{
			if (_normalBitmap != null)
			{
				_normalBitmap.Dispose();
				_normalBitmap = null;
			}
		}

		public override void SetAlpha(int alpha)
		{
		}

		public override void SetColorFilter(ColorFilter colorFilter)
		{
		}

		protected override bool OnStateChange(int[] state)
		{
			return false;
		}

		Bitmap CreateBitmap(int width, int height)
		{
			Bitmap bitmap;

			using (Bitmap.Config config = Bitmap.Config.Argb8888)
			{
				bitmap = Bitmap.CreateBitmap(width, height, config);
			}

			using (var canvas = new ACanvas(bitmap))
			{
				DrawCanvas(canvas, width, height);
			}

			return bitmap;
		}

		void DrawBackground(ACanvas canvas, int width, int height)
		{
			using (var paint = new Paint { AntiAlias = true })
			using (Path.Direction direction = Path.Direction.Cw)
			using (Paint.Style style = Paint.Style.Fill)
			{
				var path = new Path();
				path.AddRect(new RectF(0, 0, width, height), Path.Direction.Ccw);
				path.Close();

				if (_element.Background != null)
				{
					if(_element.Background is SolidColorBrush solidColorBrush)
					{
						AColor color = solidColorBrush.Color.ToAndroid();
						paint.SetStyle(style);
						paint.Color = color;
					}

					if (_element.Background is LinearGradientBrush linearGradientBrush)
					{
						var p1 = linearGradientBrush.StartPoint;
						var p2 = linearGradientBrush.EndPoint;
						var xDiff = p2.X - p1.X;
						var yDiff = p2.Y - p1.Y;
						var angle = Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI;

						var a = width * Math.Pow(Math.Sin(2 * Math.PI * ((angle + 0.75) / 2)), 2);
						var b = height * Math.Pow(Math.Sin(2 * Math.PI * ((angle + 0.0) / 2)), 2);
						var c = width * Math.Pow(Math.Sin(2 * Math.PI * ((angle + 0.25) / 2)), 2);
						var d = height * Math.Pow(Math.Sin(2 * Math.PI * ((angle + 0.5) / 2)), 2);

						var orderedStops = linearGradientBrush.GradientStops.OrderBy(x => x.Offset).ToList();
						var colors = orderedStops.Select(x => x.Color.ToAndroid().ToArgb()).ToArray();
						var locations = orderedStops.Select(x => x.Offset).ToArray();

						var shader = new LinearGradient(width - (float)a, (float)b, width - (float)c, (float)d, colors, locations, Shader.TileMode.Clamp);
						paint.SetShader(shader);
					}

					if(_element.Background is RadialGradientBrush radialGradientBrush)
					{
						var center = radialGradientBrush.GradientOrigin;
						float x = (float)center.X;
						float y = (float)center.Y;
						float radius = (float)radialGradientBrush.RadiusX;

						var orderedStops = radialGradientBrush.GradientStops.OrderBy(b => b.Offset).ToList();
						var colors = orderedStops.Select(s => s.Color.ToAndroid().ToArgb()).ToArray();
						var locations = orderedStops.Select(c => c.Offset).ToArray();

						var shader = new RadialGradient(x, y, radius, colors, locations, Shader.TileMode.Clamp);
						paint.SetShader(shader);
					}
				}
				else
				{
					AColor color = _element.BackgroundColor.ToAndroid();
					paint.SetStyle(style);
					paint.Color = color;
				}

				canvas.DrawPath(path, paint);
			}
		}

		void PancakeViewOnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == VisualElement.BackgroundProperty.PropertyName)
			{
				if (_normalBitmap == null)
					return;

				using (var canvas = new ACanvas(_normalBitmap))
				{
					int width = Bounds.Width();
					int height = Bounds.Height();
					canvas.DrawColor(AColor.Black, PorterDuff.Mode.Clear);
					DrawCanvas(canvas, width, height);
				}

				InvalidateSelf();
			}
		}

		void DrawCanvas(ACanvas canvas, int width, int height)
		{
			DrawBackground(canvas, width, height);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && !_isDisposed)
			{
				DisposeBitmap();

				if (_element != null)
				{
					_element.PropertyChanged -= PancakeViewOnPropertyChanged;
				}

				_isDisposed = true;
			}

			base.Dispose(disposing);
		}
	}
}