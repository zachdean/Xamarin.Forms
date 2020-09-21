using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Text;
using ReactiveUI;
using Xamarin.Forms;
using Xamarin.Platform;

namespace Sample.ReactiveControl
{
	public class ReactiveView : ReactiveObject, IObservable<string>, IView
	{
		Subject<string> propertyChanged;
		private Rectangle _frame;
		private Color _backgroundCOlor;

		public ReactiveView()
		{
			propertyChanged = new Subject<string>();

			propertyChanged.Subscribe(result =>
			{
				Handler?.UpdateValue(result);
			});
		}

		protected void OnPropertyChanged([CallerMemberName] string memberName = "") =>
			propertyChanged.OnNext(memberName);

		public bool IsEnabled => throw new NotImplementedException();

		public Color BackgroundColor
		{
			get
			{
				return _backgroundCOlor;
			}
			set
			{
				_backgroundCOlor = value;
				OnPropertyChanged();
			}
		}

		public Rectangle Frame
		{
			get
			{
				return _frame;
			}
			set
			{
				_frame = value;
				OnPropertyChanged();
			}
		}

		public IViewHandler Handler { get; set; }

		public IFrameworkElement Parent => throw new NotImplementedException();

		public SizeRequest DesiredSize => throw new NotImplementedException();

		public bool IsMeasureValid => throw new NotImplementedException();

		public bool IsArrangeValid => throw new NotImplementedException();

		public void Arrange(Rectangle bounds)
		{
			Frame = bounds;
			Handler.SetFrame(Frame);
		}

		public void InvalidateArrange()
		{
		}

		public void InvalidateMeasure()
		{
		}

		public SizeRequest Measure(double widthConstraint, double heightConstraint)
		{
			return Handler.GetDesiredSize(widthConstraint, heightConstraint);
		}

		public IDisposable Subscribe(IObserver<string> observer)
		{
			return propertyChanged.Subscribe(observer);
		}
	}
}
