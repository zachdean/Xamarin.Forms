using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Xamarin.Forms
{
	public class DropGestureRecognizer : GestureRecognizer
	{
		public static readonly BindableProperty AllowDropProperty = BindableProperty.Create(nameof(AllowDrop), typeof(bool), typeof(DropGestureRecognizer), true);

		public static readonly BindableProperty DragOverCommandProperty = BindableProperty.Create(nameof(DragOverCommand), typeof(ICommand), typeof(DropGestureRecognizer), null);

		public static readonly BindableProperty DragOverCommandParameterProperty = BindableProperty.Create(nameof(DragOverCommandParameter), typeof(object), typeof(DropGestureRecognizer), null);
		
		public static readonly BindableProperty DragLeaveCommandProperty = BindableProperty.Create(nameof(DragLeaveCommand), typeof(ICommand), typeof(DropGestureRecognizer), null);

		public static readonly BindableProperty DragLeaveCommandParameterProperty = BindableProperty.Create(nameof(DragLeaveCommandParameter), typeof(object), typeof(DropGestureRecognizer), null);

		public static readonly BindableProperty DropCommandProperty = BindableProperty.Create(nameof(DropCommand), typeof(ICommand), typeof(DragGestureRecognizer), null);

		public static readonly BindableProperty DropCommandParameterProperty = BindableProperty.Create(nameof(DropCommandParameter), typeof(object), typeof(DropGestureRecognizer), null);
		
		VisualStateGroup _dropStates;
		VisualElement _parent;

		public DropGestureRecognizer()
		{
			ExperimentalFlags.VerifyFlagEnabled(nameof(DropGestureRecognizer), ExperimentalFlags.DragAndDropExperimental);

			_dropStates = new VisualStateGroup()
			{
				Name = "DropStates"
			};

			_dropStates.States.Add(new VisualState() { Name = "DragNormal" });
			_dropStates.States.Add(new VisualState() { Name = "DragOver" });

		}

		public event EventHandler<DragEventArgs> DragLeave;
		public event EventHandler<DragEventArgs> DragOver;
		public event EventHandler<DropEventArgs> Drop;

		public bool AllowDrop
		{
			get { return (bool)GetValue(AllowDropProperty); }
			set { SetValue(AllowDropProperty, value); }
		}

		public ICommand DragOverCommand
		{
			get { return (ICommand)GetValue(DragOverCommandProperty); }
			set { SetValue(DragOverCommandProperty, value); }
		}

		public object DragOverCommandParameter
		{
			get { return (object)GetValue(DragOverCommandParameterProperty); }
			set { SetValue(DragOverCommandParameterProperty, value); }
		}
		public ICommand DragLeaveCommand
		{
			get { return (ICommand)GetValue(DragLeaveCommandProperty); }
			set { SetValue(DragLeaveCommandProperty, value); }
		}

		public object DragLeaveCommandParameter
		{
			get { return (object)GetValue(DragLeaveCommandParameterProperty); }
			set { SetValue(DragLeaveCommandParameterProperty, value); }
		}

		public ICommand DropCommand
		{
			get { return (ICommand)GetValue(DropCommandProperty); }
			set { SetValue(DropCommandProperty, value); }
		}

		public object DropCommandParameter
		{
			get { return (object)GetValue(DropCommandParameterProperty); }
			set { SetValue(DropCommandParameterProperty, value); }
		}

		protected override void OnParentSet()
		{
			base.OnParentSet();

			if (_parent != null)
				DragGestureRecognizer.RemoveVisualStateGroups(_parent);

			if (!(Parent is VisualElement ve))
				return;

			_parent = ve;
			DragGestureRecognizer.RemoveVisualStateGroups(_parent);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public void SendDragOver(DragEventArgs args)
		{
			DragOverCommand?.Execute(DragOverCommandParameter);
			DragOver?.Invoke(this, args);

			VisualStateManager.GoToState(_parent, "DragOver");
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public void SendDragLeave(DragEventArgs args)
		{
			DragLeaveCommand?.Execute(DragLeaveCommandParameter);
			DragLeave?.Invoke(this, args);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public async Task SendDrop(DropEventArgs args, VisualElement element)
		{
			if (!AllowDrop)
				return;

			DropCommand?.Execute(DropCommandParameter);
			Drop?.Invoke(this, args);

			if(!args.Handled)
			{
				var dataView = args.Data;
				var internalProperties = dataView.PropertiesInternal;
				VisualElement dragSource = null;
				ImageSource sourceTarget = await dataView.GetImageAsync();
				string text = await dataView.GetTextAsync();

				// TODO: Shane Generalize the retrieval of "values" from elements to provide the text for
				if (internalProperties.ContainsKey("DragSource"))
				{
					dragSource = (VisualElement)internalProperties["DragSource"];
					if (sourceTarget == null && dragSource is IImageElement imageElement)
						sourceTarget = imageElement.Source;

					if (String.IsNullOrWhiteSpace(text))
					{
						text = dragSource.GetStringValue();
					}
				}

				if (Parent is IImageElement && sourceTarget == null)
					sourceTarget = text;

				if (Parent is Image image)
					image.Source = sourceTarget;
				else if (Parent is ImageButton ib)
					ib.Source = sourceTarget;
				else if (Parent is Button b)
					b.ImageSource = sourceTarget;
				
				Parent?.TrySetValue(text);
			}
		}
	}
}