using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls.Issues
{
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 10387, "[Bug] Incorrect Binding when Setting SwipeView Items from StaticResource", PlatformAffected.All)]
	public partial class Issue10387 : TestContentPage
	{
		public Issue10387()
		{
#if APP
			InitializeComponent();
			BindingContext = new Issue10387ViewModel();
#endif
		}

		protected override void Init()
		{

		}
	}

	[Preserve(AllMembers = true)]
	public class Issue10387Model
	{
		public string Name { get; set; }
		public int Age { get; set; }

		public Issue10387Model(string name, int age)
		{
			Name = name;
			Age = age;
		}
	}

	public class Issue10387ViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public ICommand DeleteCommand => new Command<Person>(DeletePerson);
		public ObservableCollection<Issue10387Model> People { get; private set; } = new ObservableCollection<Issue10387Model>();

		public Issue10387ViewModel()
		{
			People.Add(new Issue10387Model("Steve", 21));
			People.Add(new Issue10387Model("John", 42));
			People.Add(new Issue10387Model("Tom", 29));
			People.Add(new Issue10387Model("Lucas", 29));
			People.Add(new Issue10387Model("Jane", 30));
		}

		void DeletePerson(Person obj)
		{
			Application.Current.MainPage.DisplayAlert("DeleteCommand", obj?.Name, "Ok");
			OnPropertyChanged("DeletePerson");
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}