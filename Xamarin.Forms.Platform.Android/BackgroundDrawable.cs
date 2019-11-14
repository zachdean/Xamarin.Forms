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

		public BackgroundDrawable(VisualElement element)
		{
			_element = element;
			_element.PropertyChanged += OnViewOnPropertyChanged;
		}

		public override bool IsStateful
		{
			get { return false; }
		}

		public override int Opacity
		{
			get { return 0; }
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

		void DisposeBitmap()
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
						var x1 = p1.X;
						var y1 = p1.Y;

						var p2 = linearGradientBrush.EndPoint;
						var x2 = p2.X;
						var y2 = p2.Y;

						var radians = Math.Atan2(y2 - y1, x2 - x1);
						var angle = radians * (180 / Math.PI);

						var a = width * Math.Pow(Math.Sin(2 * Math.PI * ((angle + 0.00) / 2)), 2);
						var b = height * Math.Pow(Math.Sin(2 * Math.PI * ((angle + 0.75) / 2)), 2);
						var c = width * Math.Pow(Math.Sin(2 * Math.PI * ((angle + 0.5) / 2)), 2);
						var d = height * Math.Pow(Math.Sin(2 * Math.PI * ((angle + 0.25) / 2)), 2);

						var orderedStops = linearGradientBrush.GradientStops.Distinct().OrderBy(x => x.Offset).ToList();
						var colors = orderedStops.Select(x => x.Color.ToAndroid().ToArgb()).ToArray();
						var locations = orderedStops.Select(x => x.Offset).ToArray();

						var shader = new LinearGradient(width - (float)a, (float)b, width - (float)c, (float)d, colors, locations, Shader.TileMode.Clamp);
						paint.SetShader(shader);
					}

					if(_element.Background is RadialGradientBrush radialGradientBrush)
					{
						var center = radialGradientBrush.GradientOrigin;
						float centerX = width * (float)center.X;
						float centerY = height * (float)center.Y;
						float radius = (width * (float)radialGradientBrush.RadiusX +
							height * (float)radialGradientBrush.RadiusY) / 2;

						var orderedStops = radialGradientBrush.GradientStops.Distinct().OrderBy(b => b.Offset).ToList();
						var colors = orderedStops.Select(s => s.Color.ToAndroid().ToArgb()).ToArray();
						var locations = orderedStops.Select(c => c.Offset).ToArray();

						var shader = new RadialGradient(centerX, centerY, radius, colors, locations, Shader.TileMode.Clamp);
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

		void OnViewOnPropertyChanged(object sender, PropertyChangedEventArgs e)
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
					_element.PropertyChanged -= OnViewOnPropertyChanged;
				}

				_isDisposed = true;
			}

			base.Dispose(disposing);
		}
	}
}