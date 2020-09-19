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

		public ReactiveView()
		{
			propertyChanged = new Subject<string>();

			propertyChanged.Subscribe(result =>
			{
				Handler.UpdateValue(result);
			});
		}

		protected void OnPropertyChanged([CallerMemberName] string memberName = "") =>
			propertyChanged.OnNext(memberName);

		public bool IsEnabled => throw new NotImplementedException();

		public Color BackgroundColor => throw new NotImplementedException();

		public Rectangle Frame => throw new NotImplementedException();

		public IViewHandler Handler { get; set; }

		public IFrameworkElement Parent => throw new NotImplementedException();

		public SizeRequest DesiredSize => throw new NotImplementedException();

		public bool IsMeasureValid => throw new NotImplementedException();

		public bool IsArrangeValid => throw new NotImplementedException();

		public void Arrange(Rectangle bounds)
		{
			throw new NotImplementedException();
		}

		public void InvalidateArrange()
		{
			throw new NotImplementedException();
		}

		public void InvalidateMeasure()
		{
			throw new NotImplementedException();
		}

		public SizeRequest Measure(double widthConstraint, double heightConstraint)
		{
			throw new NotImplementedException();
		}

		public IDisposable Subscribe(IObserver<string> observer)
		{
			throw new NotImplementedException();
		}
	}
}
