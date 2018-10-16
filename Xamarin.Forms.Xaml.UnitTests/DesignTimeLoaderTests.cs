using NUnit.Framework;

using Xamarin.Forms.Core.UnitTests;

namespace Xamarin.Forms.Xaml.UnitTests
{
	[TestFixture]
	public class DesignTimeLoaderTests : BaseTestFixture
	{
		[Test]
		public void ContentPageWithMissingClass()
		{
			var xaml = @"
				<ContentPage xmlns=""http://xamarin.com/schemas/2014/forms"" xmlns:x=""http://schemas.microsoft.com/winfx/2009/xaml""
				      x:Class=""Xamarin.Forms.Xaml.UnitTests.CustomView""
				/>";

			// This should be a ContentPage
			Assert.IsInstanceOf<ContentPage> ((ContentPage)XamlLoader.Create(xaml, true), "#1");
		}

		[Test]
		public void ViewWithMissingClass()
		{
			var xaml = @"
				<View xmlns=""http://xamarin.com/schemas/2014/forms"" xmlns:x=""http://schemas.microsoft.com/winfx/2009/xaml""
				      x:Class=""Xamarin.Forms.Xaml.UnitTests.CustomView""
				/>";

			// This should be a View
			Assert.IsInstanceOf<View> ((ContentPage)XamlLoader.Create(xaml, true), "#1");
		}

		[Test]
		public void ContentPageWithMissingType()
		{
			var xaml = @"
				<ContentPage xmlns=""http://xamarin.com/schemas/2014/forms""
					xmlns:local=""clr-namespace:MissingNamespace;assembly=MissingAssembly"">
					<local:MyCustomButton />
				</ContentPage>";

			// We should have a ContentPage with a 'View' of some sort as the content because
			// we do not know what the real object is.
			var result = (ContentPage)XamlLoader.Create(xaml, true);
			Assert.IsInstanceOf<View> (result.Content, "#1");
		}

		[Test]
		public void ExplicitStyleAppliedToMissingType()
		{
			var xaml = @"
				<ContentPage xmlns=""http://xamarin.com/schemas/2014/forms""
					xmlns:local=""clr-namespace:MissingNamespace;assembly=MissingAssembly""
					xmlns:x=""http://schemas.microsoft.com/winfx/2009/xaml"">
					<ContentPage.Resources>
						<Style x:Key=""Test"" TargetType=""local:MyCustomButton"">
							<Setter Property=""BackgroundColor"" Value=""Red""></Setter>
						</Style>
					</ContentPage.Resources>
					<local:MyCustomButton Style=""{StaticResource Test}"" />
				</ContentPage>";

			// We should have a ContentPage with a 'View' of some sort as the content because
			// we do not know what the real object is. It's background color should be red.
			var result = (ContentPage)XamlLoader.Create(xaml, true);

			View content = result.Content;
			Assert.IsNotNull(content);
			Assert.Equals(content.BackgroundColor, new Color(1, 0, 0));
		}

		[Test]
		public void ImplicitStyleAppliedToMissingType()
		{
			var xaml = @"
				<ContentPage xmlns=""http://xamarin.com/schemas/2014/forms""
					xmlns:local=""clr-namespace:MissingNamespace;assembly=MissingAssembly"">
					<ContentPage.Resources>
						<Style TargetType=""local:MyCustomButton"">
							<Setter Property=""BackgroundColor"" Value=""Red""></Setter>
						</Style>
					</ContentPage.Resources>
					<local:MyCustomButton />
				</ContentPage>";

			// We should have a ContentPage with a 'View' of some sort as the content because
			// we do not know what the real object is. It's background color should be red.
			var result = (ContentPage)XamlLoader.Create(xaml, true);

			View content = result.Content;
			Assert.IsNotNull(content);
			Assert.Equals(content.BackgroundColor, new Color(1, 0, 0));
		}

		[Test]
		public void MissingTypeWithUnknownProperty()
		{
			var xaml = @"
				<ContentPage xmlns=""http://xamarin.com/schemas/2014/forms""
					xmlns:x=""http://schemas.microsoft.com/winfx/2009/xaml""
					xmlns:local=""clr-namespace:MissingNamespace;assembly=MissingAssembly"">

					<local:MyCustomButton MyText=""Hello"" />

				</ContentPage>";

			// We should have a ContentPage with a 'View' of some sort as the content because
			// we do not know what the real object is. The unknown property should be ignored.
			var result = (ContentPage)XamlLoader.Create(xaml, true);
			Assert.IsInstanceOf<View>(result.Content, "#1");
		}
	}
}
