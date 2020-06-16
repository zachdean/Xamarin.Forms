using Foundation;
using ObjCRuntime;
using System;
using System.Collections.Generic;
using UIKit;

namespace Xamarin.Forms.Platform.iOS
{
	internal class PageContainer : UIView, IUIAccessibilityContainer
	{
		readonly IAccessibilityElementsController _parent;
		List<NSObject> _accessibilityElements = null;
		bool _disposed;

		public PageContainer(IAccessibilityElementsController parent)
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			_parent = parent;
		}

		public PageContainer()
		{
			//IsAccessibilityElement = false;
		}

		public override bool IsAccessibilityElement
		{
			get => _thing == null;
			set => base.IsAccessibilityElement = value;
		}

		List<NSObject> AccessibilityElements
		{
			get
			{
				// lazy-loading this list so that the expensive call to GetAccessibilityElements only happens when VoiceOver is on.
				if (_accessibilityElements == null || _accessibilityElements.Count == 0)
				{
					Console.WriteLine($"Build Elements: {this}");
					_accessibilityElements = _parent.GetAccessibilityElements();
				}

				return _accessibilityElements;
			}
		}

		public void ClearAccessibilityElements()
		{
			_accessibilityElements = null;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && !_disposed)
			{
				ClearAccessibilityElements();
				_disposed = true;
			}
			base.Dispose(disposing);
		}

		/*[Export("accessibilityElementCount")]
		[Internals.Preserve(Conditional = true)]
		nint AccessibilityElementCount()
		{
			if (AccessibilityElements == null || AccessibilityElements.Count == 0)
				return 0;

			// Note: this will only be called when VoiceOver is enabled
			return AccessibilityElements.Count;
		}

		[Export("accessibilityElementAtIndex:")]
		[Internals.Preserve(Conditional = true)]
		NSObject GetAccessibilityElementAt(nint index)
		{
			if (AccessibilityElements == null || AccessibilityElements.Count == 0)
				return NSNull.Null;


			Console.WriteLine($"GetAccessibilityElementAt: {index}");
			// Note: this will only be called when VoiceOver is enabled
			return AccessibilityElements[(int)index];
		}


		[Export("indexOfAccessibilityElement:")]
		[Internals.Preserve(Conditional = true)]
		int GetIndexOfAccessibilityElement(NSObject element)
		{
			if (AccessibilityElements == null || AccessibilityElements.Count == 0)
				return int.MaxValue;

			// Note: this will only be called when VoiceOver is enabled
			return AccessibilityElements.IndexOf(element);
		}*/

		NSArray _thing;
		[Internals.Preserve(Conditional = true)]
		public virtual NSObject accessibilityElements2
		{
			[Export("accessibilityElements", ArgumentSemantic.Copy)]
			get
			{
				if (_thing == null && _thing?.Count == 0)
				{
					var elements = _parent.GetAccessibilityElements();

					if (elements != null)
						_thing = NSArray.FromObjects(elements.ToArray());
				}

				return _thing;
			}
		}

		/*[Export("accessibilityElements:")]
		[Internals.Preserve(Conditional = true)]
		NSObject accessibilityElements(NSObject element)
		{
			return NSArray.FromObjects(AccessibilityElements);
		}*/
	}
}