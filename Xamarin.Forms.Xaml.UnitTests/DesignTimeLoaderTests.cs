using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Xamarin.Forms.Core.UnitTests;

namespace Xamarin.Forms.Xaml.UnitTests
{
	[TestFixture]
	public class DesignTimeLoaderTests
	{
		[SetUp]
		public void Setup()
		{
			Device.PlatformServices = new MockPlatformServices();
			Xamarin.Forms.Internals.Registrar.RegisterAll(new Type[0]);
		}

		[TearDown]
		public void TearDown()
		{
			Device.PlatformServices = null;
			XamlLoader.FallbackTypeResolver = null;
			XamlLoader.ValueCreatedCallback = null;
			Xamarin.Forms.Internals.ResourceLoader.ExceptionHandler = null;
		}

		[Test]
		public void ContentPageWithMissingClass()
		{
			var xaml = @"
				<ContentPage xmlns=""http://xamarin.com/schemas/2014/forms""
					xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
					x:Class=""Xamarin.Forms.Xaml.UnitTests.CustomView""
				/>";

			Assert.That(XamlLoader.Create(xaml, true), Is.TypeOf<ContentPage>());
		}

		[Test]
		public void ViewWithMissingClass()
		{
			var xaml = @"
				<ContentView xmlns=""http://xamarin.com/schemas/2014/forms""
					xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
					x:Class=""Xamarin.Forms.Xaml.UnitTests.CustomView""
				/>";

			Assert.That(XamlLoader.Create(xaml, true), Is.TypeOf<ContentView>());
		}

		[Test]
		public void ContentPageWithMissingType()
		{
			XamlLoader.FallbackTypeResolver = (p, type) => type ?? typeof(MockView);

			var xaml = @"
				<ContentPage xmlns=""http://xamarin.com/schemas/2014/forms""
					xmlns:local=""clr-namespace:MissingNamespace;assembly=MissingAssembly""
					xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
					<ContentPage.Content>
						<local:MyCustomButton />
					</ContentPage.Content>
				</ContentPage>";

			var page = (ContentPage) XamlLoader.Create(xaml, true);
			Assert.That(page.Content, Is.TypeOf<MockView>());
		}

		[Test]
		public void MissingTypeWithKnownProperty()
		{
			XamlLoader.FallbackTypeResolver = (p, type) => type ?? typeof(Button);
			XamlLoader.ValueCreatedCallback = (x, v) => {
				if (x.XmlTypeName == "MyCustomButton" && v is VisualElement ve) {
					ve._mergedStyle.ReRegisterImplicitStyles("MissingNamespace.MyCustomButton");
				}
			};

			var xaml = @"
				<ContentPage xmlns=""http://xamarin.com/schemas/2014/forms""
					xmlns:local=""clr-namespace:MissingNamespace;assembly=MissingAssembly""
					xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
					<ContentPage.Content>
						<local:MyCustomButton BackgroundColor=""Red"" />
					</ContentPage.Content>
				</ContentPage>";

			var page = (ContentPage)XamlLoader.Create(xaml, true);
			Assert.That(page.Content, Is.TypeOf<Button>());
			Assert.That(page.Content.BackgroundColor, Is.EqualTo(new Color(1,0,0)));
		}

		[Test]
		public void MissingTypeWithUnknownProperty()
		{
			XamlLoader.FallbackTypeResolver = (p, type) => type ?? typeof(Button);
			XamlLoader.ValueCreatedCallback = (x, v) => {
				if (x.XmlTypeName == "MyCustomButton" && v is VisualElement ve) {
					ve._mergedStyle.ReRegisterImplicitStyles("MissingNamespace.MyCustomButton");
				}
			};

			var xaml = @"
				<ContentPage xmlns=""http://xamarin.com/schemas/2014/forms""
					xmlns:local=""clr-namespace:MissingNamespace;assembly=MissingAssembly""
					xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
					<ContentPage.Content>
						<local:MyCustomButton MyColor=""Red"" />
					</ContentPage.Content>
				</ContentPage>";

			var page = (ContentPage)XamlLoader.Create(xaml, true);
			Assert.That(page.Content, Is.TypeOf<Button>());
		}

		[Test]
		public void ExplicitStyleAppliedToMissingType()
		{
			XamlLoader.FallbackTypeResolver = (p, type) => type ?? typeof(Button);
			XamlLoader.ValueCreatedCallback = (x, v) => {
				if (x.XmlTypeName == "MyCustomButton" && v is VisualElement ve) {
					ve._mergedStyle.ReRegisterImplicitStyles("MissingNamespace.MyCustomButton");
				}
			};

			var xaml = @"
				<ContentPage xmlns=""http://xamarin.com/schemas/2014/forms""
					xmlns:local=""clr-namespace:MissingNamespace;assembly=MissingAssembly""
					xmlns:x=""http://schemas.microsoft.com/winfx/2009/xaml"">
					<ContentPage.Resources>
						<Style x:Key=""LocalStyle"" TargetType=""local:MyCustomButton"">
							<Setter Property=""BackgroundColor"" Value=""Red"" />
						</Style>
					</ContentPage.Resources>
					<local:MyCustomButton Style=""{StaticResource LocalStyle}"" />
				</ContentPage>";

			var page = (ContentPage)XamlLoader.Create(xaml, true);
			Assert.That(page.Content, Is.TypeOf<Button>());
			Assert.That(page.Content.BackgroundColor, Is.EqualTo(Color.Red));
		}

		[Test][Ignore]
		public void ImplicitStyleAppliedToMissingType()
		{
			XamlLoader.FallbackTypeResolver = (p, type) => type ?? typeof(Button);
			XamlLoader.ValueCreatedCallback = (x, v) => {
				if (x.XmlTypeName == "MyCustomButton" && v is VisualElement ve) {
					ve._mergedStyle.ReRegisterImplicitStyles("MissingNamespace.MyCustomButton");
				}
			};

			var xaml = @"
				<ContentPage xmlns=""http://xamarin.com/schemas/2014/forms""
					xmlns:local=""clr-namespace:MissingNamespace;assembly=MissingAssembly"">
					<ContentPage.Resources>
						<Style TargetType=""local:MyCustomButton"">
							<Setter Property=""BackgroundColor"" Value=""Red"" />
						</Style>
					</ContentPage.Resources>
					<local:MyCustomButton />
				</ContentPage>";

			var page = (ContentPage)XamlLoader.Create(xaml, true);

			var myButton = (Button)page.Content;

			Assert.That(myButton.BackgroundColor, Is.EqualTo(Color.Red));
		}

		[Test]
		public void StyleTargetingRealTypeNotAppliedToMissingType()
		{
			XamlLoader.FallbackTypeResolver = (p, type) => type ?? typeof(Button);
			XamlLoader.ValueCreatedCallback = (x, v) => {
				if (x.XmlTypeName == "MyCustomButton" && v is VisualElement ve) {
					ve._mergedStyle.ReRegisterImplicitStyles("MissingNamespace.MyCustomButton");
				}
			};

			var xaml = @"
				<ContentPage xmlns=""http://xamarin.com/schemas/2014/forms""
					xmlns:local=""clr-namespace:MissingNamespace;assembly=MissingAssembly"">
					<ContentPage.Resources>
						<Style TargetType=""Button"">
							<Setter Property=""BackgroundColor"" Value=""Red"" />
						</Style>
					</ContentPage.Resources>
					<local:MyCustomButton />
				</ContentPage>";

			var page = (ContentPage)XamlLoader.Create(xaml, true);

			var myButton = (Button)page.Content;

			//Button Style shouldn't apply to MyCustomButton
			Assert.That(myButton.BackgroundColor, Is.Not.EqualTo(Color.Red));
		}

		[Test][Ignore]
		public void StyleTargetingMissingTypeNotAppliedToFallbackType()
		{
			XamlLoader.FallbackTypeResolver = (p, type) => type ?? typeof(Button);
			XamlLoader.ValueCreatedCallback = (x, v) => {
				if (x.XmlTypeName == "MyCustomButton" && v is VisualElement ve) {
					ve._mergedStyle.ReRegisterImplicitStyles("MissingNamespace.MyCustomButton");
				}
			};

			var xaml = @"
				<ContentPage xmlns=""http://xamarin.com/schemas/2014/forms""
					xmlns:local=""clr-namespace:MissingNamespace;assembly=MissingAssembly"">
					<ContentPage.Resources>
						<Style TargetType=""local:MyCustomButton"">
							<Setter Property=""BackgroundColor"" Value=""Red"" />
						</Style>
					</ContentPage.Resources>
					<Button />
				</ContentPage>";

			var page = (ContentPage)XamlLoader.Create(xaml, true);

			var myButton = (Button)page.Content;

			//MyCustomButton Style should not be applied
			Assert.That(myButton.BackgroundColor, Is.Not.EqualTo(Color.Red));
		}

		[Test]
		public void StyleAppliedToDerivedTypesAppliesToDerivedMissingType()
		{
			XamlLoader.FallbackTypeResolver = (p, type) => type ?? typeof(Button);
			XamlLoader.ValueCreatedCallback = (x, v) => {
				if (x.XmlTypeName == "MyCustomButton" && v is VisualElement ve) {
					ve._mergedStyle.ReRegisterImplicitStyles("MissingNamespace.MyCustomButton");
				}
			};

			var xaml = @"
				<ContentPage xmlns=""http://xamarin.com/schemas/2014/forms""
					xmlns:local=""clr-namespace:MissingNamespace;assembly=MissingAssembly"">
					<ContentPage.Resources>
						<Style TargetType=""Button"" ApplyToDerivedTypes=""True"">
							<Setter Property=""BackgroundColor"" Value=""Red"" />
						</Style>
					</ContentPage.Resources>
					<local:MyCustomButton />
				</ContentPage>";

			var page = (ContentPage)XamlLoader.Create(xaml, true);

			var myButton = (Button)page.Content;

			//Button Style should apply to MyCustomButton
			Assert.That(myButton.BackgroundColor, Is.EqualTo(Color.Red));
		}

		[Test]
		public void UnknownGenericType()
		{
			XamlLoader.FallbackTypeResolver = (p, type) => type ?? typeof(MockView);

			var xaml = @"
				<ContentPage xmlns=""http://xamarin.com/schemas/2014/forms""
					xmlns:local=""clr-namespace:MissingNamespace;assembly=MissingAssembly""
					xmlns:x=""http://schemas.microsoft.com/winfx/2009/xaml"">
					<local:MyCustomButton x:TypeArguments=""local:MyCustomType"" />
				 </ContentPage>";

			var page = (ContentPage)XamlLoader.Create(xaml, true);
			Assert.That(page.Content, Is.TypeOf<MockView>());
		}

		[Test]
		public void UnknownMarkupExtensionOnMissingType()
		{
			XamlLoader.FallbackTypeResolver = (p, type) => type ?? typeof(MockView);

			var xaml = @"
				<ContentPage xmlns=""http://xamarin.com/schemas/2014/forms""
					xmlns:local=""clr-namespace:MissingNamespace;assembly=MissingAssembly"">
					<local:MyCustomButton Bar=""{local:Foo}"" />
				</ContentPage>";

			var page = (ContentPage)XamlLoader.Create(xaml, true);
			Assert.That(page.Content, Is.TypeOf<MockView>());
		}

		[Test]
		public void UnknownMarkupExtensionKnownType()
		{
			XamlLoader.FallbackTypeResolver = (p, type) => type ?? typeof(MockView);

			var xaml = @"
				<ContentPage xmlns=""http://xamarin.com/schemas/2014/forms""
					xmlns:x=""http://schemas.microsoft.com/winfx/2009/xaml""
					xmlns:local=""clr-namespace:MissingNamespace;assembly=MissingAssembly"">
					<Button Text=""{local:Foo}"" />
				</ContentPage>";

			var page = (ContentPage)XamlLoader.Create(xaml, true);
			Assert.That(page.Content, Is.TypeOf<Button>());
		}

		[Test]
		public void StaticResourceKeyInApp()
		{
			var app = @"
				<Application xmlns=""http://xamarin.com/schemas/2014/forms""
					xmlns:x=""http://schemas.microsoft.com/winfx/2009/xaml"">
					<Application.Resources>
						<ResourceDictionary>
							<Style TargetType=""Button"" x:Key=""StyleInApp"">
								<Setter Property=""BackgroundColor"" Value=""HotPink"" />
							</Style>
						</ResourceDictionary>
					</Application.Resources>
				</Application>
			";
			Application.Current = (Application)XamlLoader.Create(app, true);

			var xaml = @"
				<ContentPage xmlns=""http://xamarin.com/schemas/2014/forms"">
					<Button Style=""{StaticResource StyleInApp}"" />
				</ContentPage>";

			var page = (ContentPage)XamlLoader.Create(xaml, true);
			Assert.That(page.Content, Is.TypeOf<Button>());
			Assert.That(page.Content.BackgroundColor, Is.EqualTo(Color.HotPink));
		}

		[Test]
		public void StaticResourceKeyNotFound()
		{
			var xaml = @"
				<ContentPage xmlns=""http://xamarin.com/schemas/2014/forms"">
					<Button Style=""{StaticResource MissingStyle}"" />
				</ContentPage>";

			var exceptions = new List<Exception>();
			Xamarin.Forms.Internals.ResourceLoader.ExceptionHandler = exceptions.Add;

			var page = (ContentPage)XamlLoader.Create(xaml, true);
			Assert.That(page.Content, Is.TypeOf<Button>());
			Assert.That(exceptions.Count, Is.EqualTo(2));
		}

		[Test]
		public void CssStyleAppliedToMissingType()
		{
			XamlLoader.FallbackTypeResolver = (p, type) => type ?? typeof(Button);
			XamlLoader.ValueCreatedCallback = (x, v) => {
				if (x.XmlTypeName == "MyCustomButton" && v is Element e) {
					e._cssFallbackTypeName = "MyCustomButton";
				}
				if (x.XmlTypeName == "MyCustomButton" && v is VisualElement ve) {
					ve._mergedStyle.ReRegisterImplicitStyles("MissingNamespace.MyCustomButton");
				}
			};

			var xaml = @"
				<ContentPage xmlns=""http://xamarin.com/schemas/2014/forms""
					xmlns:local=""clr-namespace:MissingNamespace;assembly=MissingAssembly"">
					<ContentPage.Resources>
						<StyleSheet>
							<![CDATA[
							MyCustomButton {
								background-color: blue;
							}
							]]>
						</StyleSheet>
					</ContentPage.Resources>
					<local:MyCustomButton />
				</ContentPage>";

			var page = (ContentPage)XamlLoader.Create(xaml, true);

			var myButton = (Button)page.Content;

			Assert.That(myButton.BackgroundColor, Is.EqualTo(Color.Blue));
		}

		[Test]
		public void CssStyleTargetingRealTypeNotAppliedToMissingType()
		{
			XamlLoader.FallbackTypeResolver = (p, type) => type ?? typeof(Button);
			XamlLoader.ValueCreatedCallback = (x, v) => {
				if (x.XmlTypeName == "MyCustomButton" && v is Element e) {
					e._cssFallbackTypeName = "MyCustomButton";
				}
				if (x.XmlTypeName == "MyCustomButton" && v is VisualElement ve) {
					ve._mergedStyle.ReRegisterImplicitStyles("MissingNamespace.MyCustomButton");
				}
			};

			var xaml = @"
				<ContentPage xmlns=""http://xamarin.com/schemas/2014/forms""
					xmlns:local=""clr-namespace:MissingNamespace;assembly=MissingAssembly"">
					<ContentPage.Resources>
						<StyleSheet>
							<![CDATA[
							button {
								background-color: red;
							}
							]]>
						</StyleSheet>
					</ContentPage.Resources>
					<StackLayout>
						<Button />
						<MyCustomButton />
					</StackLayout>
				</ContentPage>";

			var page = (ContentPage)XamlLoader.Create(xaml, true);

			var button = ((StackLayout)page.Content).Children[0];
			var myButton = ((StackLayout)page.Content).Children[1];

			Assert.That(button.BackgroundColor, Is.EqualTo(Color.Red));
			Assert.That(myButton.BackgroundColor, Is.Not.EqualTo(Color.Red));
		}

		[Test]
		public void CssStyleTargetingMissingTypeNotAppliedToFallbackType()
		{
			XamlLoader.FallbackTypeResolver = (p, type) => type ?? typeof(Button);
			XamlLoader.ValueCreatedCallback = (x, v) => {
				if (x.XmlTypeName == "MyCustomButton" && v is VisualElement ve) {
					ve._mergedStyle.ReRegisterImplicitStyles("MissingNamespace.MyCustomButton");
				}
			};
			var xaml = @"
				<ContentPage xmlns=""http://xamarin.com/schemas/2014/forms"">
					<ContentPage.Resources>
						<StyleSheet>
							<![CDATA[
							MyCustomButton {
								background-color: blue;
							}
							]]>
						</StyleSheet>
					</ContentPage.Resources>
					<Button />
				</ContentPage>";

			var page = (ContentPage)XamlLoader.Create(xaml, true);

			var myButton = (Button)page.Content;

			Assert.That(myButton.BackgroundColor, Is.Not.EqualTo(Color.Blue));
		}

		[Test]
		public void ShellTypeNotParsed()
		{
			XamlLoader.FallbackTypeResolver = (p, type) => type ?? typeof(ContentPage);

			var xaml = @"
			   <Shell xmlns = ""http://xamarin.com/schemas/2014/forms""
				   xmlns:x = ""http://schemas.microsoft.com/winfx/2009/xaml""
				   xmlns:d = ""http://xamarin.com/schemas/2014/forms/design""
				   xmlns:mc = ""http://schemas.openxmlformats.org/markup-compatibility/2006""
				   mc:Ignorable = ""d""
				   RouteHost = ""companyname.com""
				   RouteScheme = ""app""
				   FlyoutBehavior = ""Disabled"" >

					<Shell.Resources>
						<ResourceDictionary>
							<Color x:Key = ""NavigationPrimary"">#2196F3</Color>
							<Style x:Key = ""BaseStyle"" TargetType = ""Element"">
								<Setter Property = ""Shell.ShellBackgroundColor"" Value = ""{StaticResource NavigationPrimary}"" />
								<Setter Property = ""Shell.ShellForegroundColor"" Value = ""White"" />
								<Setter Property = ""Shell.ShellTitleColor"" Value = ""White"" />
								<Setter Property = ""Shell.ShellDisabledColor"" Value = ""#B4FFFFFF"" />
								<Setter Property = ""Shell.ShellUnselectedColor"" Value = ""#95FFFFFF"" />
								<Setter Property = ""Shell.ShellTabBarBackgroundColor"" Value = ""{StaticResource NavigationPrimary}"" />
								<Setter Property = ""Shell.ShellTabBarForegroundColor"" Value = ""White"" />
								<Setter Property = ""Shell.ShellTabBarUnselectedColor"" Value = ""#95FFFFFF"" />
								<Setter Property = ""Shell.ShellTabBarTitleColor"" Value = ""White"" />
							</Style>
							<Style TargetType = ""ShellItem"" BasedOn = ""{StaticResource BaseStyle}"" />
						</ResourceDictionary>
					</Shell.Resources>

					<ShellItem>
						<ShellSection Title = ""Browse"" Icon = ""tab_feed.png"" />
						<ShellSection Title = ""About"" Icon = ""tab_about.png"" />
					</ShellItem>
				</Shell>";


			Device.SetFlags(new List<string> { "Shell_Experimental" });
			var shell = (Shell)XamlLoader.Create(xaml, true);

			ShellSection section = shell?.Items[0]?.Items[0];
			Assert.That(section.Title, Is.EqualTo("Browse"));
		}
	}
}