using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

#if UITEST
using Xamarin.Forms.Core.UITests;
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Xamarin.Forms.Controls.Issues
{
#if UITEST
	[Category(UITestCategories.ManualReview)]
#endif
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 6267, "Setting a picker BackgroundColor through XAML binding or programmatically in code behind crashes app", PlatformAffected.Android)]
	public class Issue6267 : TestContentPage // or TestMasterDetailPage, etc ...
	{
		Picker _picker;

		
		protected override void Init()
		{
			_picker = new Picker();
			_picker.SetBinding(Picker.ItemsSourceProperty,nameof(MainPageViewModel.ColorsObservableCollection));
			_picker.SetBinding(Picker.BackgroundColorProperty, nameof(MainPageViewModel.BackGroundColor));
			_picker.SetBinding(Picker.SelectedItemProperty, nameof(MainPageViewModel.Colors));
			_picker.Title = "Select A Color For This Subscription";
			_picker.TitleColor = Color.Black;
			_picker.TextColor = Color.White;
			_picker.ItemDisplayBinding = new Binding(nameof(MainPageViewModel.ColorName));
			_picker.SelectedIndexChanged += _picker_SelectedIndexChanged;
			Content = new StackLayout { Children = { _picker }, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center, Padding = new Thickness(15)};

			BindingContext = new MainPageViewModel();
		}

		private void _picker_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			_picker.BackgroundColor = Color.Red;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			//_picker.BackgroundColor = Color.Red;
		}

		[Preserve(AllMembers = true)]
		public class MainPageViewModel : INotifyPropertyChanged
		{
			private readonly List<Colors> _subColors = new List<Colors>();

			public ObservableCollection<Colors> ColorsObservableCollection { get; set; }

			private Color _backGroundColor;

			public Color BackGroundColor
			{
				get => _backGroundColor;
				set
				{
					_backGroundColor = value;
					OnPropertyChanged("BackGroundColor");
				}
			}

			private string _colorName;

			public string ColorName
			{
				get => _colorName;
				set
				{
					_colorName = value;

					OnPropertyChanged("ColorName");
				}
			}

			private Colors _colors;

			public Colors Colors
			{
				get => _colors;
				set
				{
					_colors = value;

					if (_colors != null)
					{
						BackGroundColor = _colors.Color;
						ColorName = _colors.ColorName;
					}

					OnPropertyChanged("Colors");
				}
			}
			public event PropertyChangedEventHandler PropertyChanged;

			public void OnPropertyChanged(string propertyName)
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}

			public MainPageViewModel()
			{
				ColorsObservableCollection = new ObservableCollection<Colors>();
				BuildColorsList();
			}

			public void BuildColorsList()
			{
				ColorsObservableCollection?.Clear();

				//Build colors observable collection
				if (ColorsObservableCollection != null && ColorsObservableCollection.Count <= 0)
				{
					_subColors.Clear();

					_subColors.Add(new Colors
					{
						BackGroundColor = "Color.DodgerBlue",
						ColorName = "DodgerBlue",
						Color = Color.DodgerBlue
					});

					_subColors.Add(new Colors
					{
						BackGroundColor = "Color.Green",
						ColorName = "Green",
						Color = Color.Green
					});

					_subColors.Add(new Colors
					{
						BackGroundColor = "Color.Orange",
						ColorName = "Orange",
						Color = Color.Orange
					});

					_subColors.Add(new Colors
					{
						BackGroundColor = "Color.Brown",
						ColorName = "Brown",
						Color = Color.Brown
					});

					_subColors.Add(new Colors
					{
						BackGroundColor = "Color.Maroon",
						ColorName = "Maroon",
						Color = Color.Maroon
					});

					_subColors.Add(new Colors
					{
						BackGroundColor = "Color.DarkSlateGray",
						ColorName = "Dark Slate Gray",
						Color = Color.DarkSlateGray
					});

					foreach (var subColor in _subColors)
					{
						ColorsObservableCollection?.Add(subColor);
					}
				}
			}
		}

		public class Colors : INotifyPropertyChanged
		{
			public List<Colors> SubColors = new List<Colors>();

			private string _backGroundColor;

			public string BackGroundColor
			{
				get => _backGroundColor;
				set
				{
					_backGroundColor = value;

					if (string.IsNullOrEmpty(value))
						_backGroundColor = "Color.DodgerBlue";

					OnPropertyChanged("BackGroundColor");
				}
			}

			private string _colorName;

			public string ColorName
			{
				get => _colorName;
				set
				{
					_colorName = value;

					if (string.IsNullOrEmpty(value))
						_colorName = "DodgerBlue";

					OnPropertyChanged("ColorName");
				}
			}

			private Color _color;

			public Color Color
			{
				get => _color;
				set
				{
					_color = value;

					if (value != null)
						_color = Color.DodgerBlue;

					OnPropertyChanged("Color");
				}
			}

			public event PropertyChangedEventHandler PropertyChanged;

			public void OnPropertyChanged(string propertyName)
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}

#if UITEST
		[Test]
		public void Issue1Test() 
		{
			// Delete this and all other UITEST sections if there is no way to automate the test. Otherwise, be sure to rename the test and update the Category attribute on the class. Note that you can add multiple categories.
			RunningApp.Screenshot ("I am at Issue 1");
			RunningApp.WaitForElement (q => q.Marked ("IssuePageLabel"));
			RunningApp.Screenshot ("I see the Label");
		}
#endif
	}
}