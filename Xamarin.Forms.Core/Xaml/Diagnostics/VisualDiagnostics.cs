// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Xamarin.Forms.Xaml.Diagnostics
{
	public class VisualDiagnostics
	{
		static ConditionalWeakTable<object, XamlSourceInfo> sourceInfos = new ConditionalWeakTable<object, XamlSourceInfo>();

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void RegisterSourceInfo(object target, Uri uri, int lineNumber, int linePosition)
		{
			if (target != null && DebuggerHelper.DebuggerIsAttached && !sourceInfos.TryGetValue(target, out _))
				sourceInfos.Add(target, new XamlSourceInfo(uri, lineNumber, linePosition));
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static void SendVisualTreeChanged(object parent, object child)
		{
			if (DebuggerHelper.DebuggerIsAttached)
				VisualTreeChanged?.Invoke(parent, new VisualTreeChangeEventArgs(parent, child, FindChildIndex(parent as Element, child as Element), parent != null ? VisualTreeChangeType.Add : VisualTreeChangeType.Remove));
		}

		internal static int FindChildIndex(Element parent, Element child)
		{
			try
			{
				// If parent is null, then the child element is being removed.
				// Return -1 so the API consumer can re-init the parent nodes.
				if (parent == null)
					return -1;

				// If child is null, then it's not an element
				// LogicialChildren is a collection of elements
				// so something was passed in that should not have been.
				// Return -1 so the API consumer can re-init the parent nodes.
				if (child == null)
					return -1;

				return parent.LogicalChildren.IndexOf(child);
			}
			catch (Exception ex)
			{
				// If we somehow fail, log the error and return -1
				// The API consumer can at least re-init the parent nodes
				// to try again
				System.Diagnostics.Debug.WriteLine(ex);
				return -1;
			}
		}

		public static event EventHandler<VisualTreeChangeEventArgs> VisualTreeChanged;
		public static XamlSourceInfo GetXamlSourceInfo(object obj) => sourceInfos.TryGetValue(obj, out var sourceinfo) ? sourceinfo : null;
	}

	public enum VisualTreeChangeType
	{
		Add = 0,
		Remove = 1
	}
}
