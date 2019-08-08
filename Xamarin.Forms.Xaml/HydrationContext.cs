using System;
using System.Collections.Generic;

namespace Xamarin.Forms.Xaml
{
	class HydrationContext
	{
		public Dictionary<INode, object> Values { get; } = new Dictionary<INode, object>();
		public Dictionary<INode, Func<object>> ValueCreators { get; } = new Dictionary<INode, Func<object>>();
		public Dictionary<IElementNode, Type> Types { get; } = new Dictionary<IElementNode, Type>();
		public HydrationContext ParentContext { get; set; }
		public Action<Exception> ExceptionHandler { get; set; }
		public object RootElement { get; set; }
	}
}