using System;
using System.Collections.Specialized;
using AppKit;

namespace Xamarin.Platform.Handlers
{
	public partial class PickerHandler : AbstractViewHandler<IPicker, NSPopUpButton>
	{
		protected override NSPopUpButton CreateNativeView()
		{
			return new NSPopUpButton();
		}

		protected override void ConnectHandler(NSPopUpButton nativeView)
		{
			nativeView.Activated += OnComboBoxSelectionChanged;

			if (VirtualView != null)
				((INotifyCollectionChanged)VirtualView.Items).CollectionChanged += OnCollectionChanged;

			base.ConnectHandler(nativeView);
		}

		protected override void DisconnectHandler(NSPopUpButton nativeView)
		{
			nativeView.Activated += OnComboBoxSelectionChanged;

			if (VirtualView != null)
				((INotifyCollectionChanged)VirtualView.Items).CollectionChanged += OnCollectionChanged;

			base.DisconnectHandler(nativeView);
		}

		void OnComboBoxSelectionChanged(object sender, EventArgs e)
		{
			if (VirtualView == null || TypedNativeView == null)
				return;

			VirtualView.SelectedIndex = (int)TypedNativeView.IndexOfSelectedItem;
		}

		void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (VirtualView == null || TypedNativeView == null)
				return;

			TypedNativeView.UpdatePicker(VirtualView);
		}
	}
}