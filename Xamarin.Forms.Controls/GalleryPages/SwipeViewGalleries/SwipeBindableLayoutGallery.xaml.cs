using System.Collections.ObjectModel;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls.GalleryPages.SwipeViewGalleries
{
	[Preserve(AllMembers = true)]
	public partial class SwipeBindableLayoutGallery : ContentPage
	{
		public SwipeBindableLayoutGallery()
		{
			InitializeComponent();
			BindingContext = new SwipeViewGalleryViewModel();
		}
	}

	[Preserve(AllMembers = true)]
	public class Message
	{
		public string Title { get; set; }
		public string SubTitle { get; set; }
		public string Description { get; set; }
		public string Date { get; set; }
	}

	[Preserve(AllMembers = true)]
	public class SwipeViewGalleryViewModel : BindableObject
	{
		private ObservableCollection<Message> _messages;

		public SwipeViewGalleryViewModel()
		{
			Messages = new ObservableCollection<Message>();
			LoadMessages();
		}

		public ObservableCollection<Message> Messages
		{
			get { return _messages; }
			set
			{
				_messages = value;
				OnPropertyChanged();
			}
		}

		private void LoadMessages()
		{
			for (int i = 0; i < 100; i++)
			{
				Messages.Add(new Message { Title = "Lorem ipsum", SubTitle = "Lorem ipsum dolor sit amet", Date = "Yesterday", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." });
			}
		}
	}
}