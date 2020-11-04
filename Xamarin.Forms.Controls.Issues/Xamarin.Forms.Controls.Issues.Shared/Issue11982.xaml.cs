using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

#if UITEST
using Xamarin.UITest;
using NUnit.Framework;
using Xamarin.Forms.Core.UITests;
#endif

namespace Xamarin.Forms.Controls.Issues
{
#if UITEST
	[Category(UITestCategories.Shape)]
#endif
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 11982,
		"[Bug] [Android] SwipeView used in a CollectionView fires the SelectionChangedCommand unintentionally",
		PlatformAffected.Android)]
	public partial class Issue11982 : TestContentPage
	{
		public Issue11982()
		{
#if APP
			InitializeComponent();

            SelectionChangedCommand = new Command<Issue11982Model>((item) =>
            {
                if (item == null)
                    return;

                Issue11982CollectionView1.SelectedItem = null;
                Issue11982CollectionView2.SelectedItem = null;

                DisplayAlert("Info", item.Name, "Ok");
            });

            Items1 = new ObservableCollection<Issue11982Model>
            {
                new Issue11982Model { Name = "First" },
                new Issue11982Model { Name = "Second" },
                new Issue11982Model { Name = "Third" },
            };

            Items2 = new ObservableCollection<Issue11982Model>
            {
                new Issue11982Model { Name = "First" },
                new Issue11982Model { Name = "Second" },
                new Issue11982Model { Name = "Third" },
            };

            BindingContext = this;
#endif
        }

		public ObservableCollection<Issue11982Model> Items1 { get; private set; }

        public ObservableCollection<Issue11982Model> Items2 { get; private set; }

        public ICommand SelectionChangedCommand { get; private set; }

        protected override void Init()
		{

		}
    }

    public class Issue11982Model : INotifyPropertyChanged
    {
        string _name;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public ICommand TestCommand { get; private set; }

        public Issue11982Model()
        {
            TestCommand = new Command(() =>
            {
                Name += "- PRESSED";
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;

            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}