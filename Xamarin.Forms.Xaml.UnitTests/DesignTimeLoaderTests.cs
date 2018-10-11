using NUnit.Framework;
using System.Linq;
using System;
using System.Collections.Generic;

using Xamarin.Forms.Core.UnitTests;

namespace Xamarin.Forms.Xaml.UnitTests
{
	[TestFixture]
	public class DesignTimeLoaderTests : BaseTestFixture
	{
		[Test]
		public void ContenPageWithMissingClass()
		{
			var xaml = @"
				<ContentPage xmlns=""http://xamarin.com/schemas/2014/forms""
				      x:Class=""Xamarin.Forms.Xaml.UnitTests.CustomView""
				/>";

			// This shoudl be a ContentPage
			Assert.IsInstanceOf<ContentPage> (XamlLoader.Create(xaml, true), "#1");
		}

		[Test]
		public void ViewWithMissingClass()
		{
			var xaml = @"
				<View xmlns=""http://xamarin.com/schemas/2014/forms""
				      x:Class=""Xamarin.Forms.Xaml.UnitTests.CustomView""
				/>";

			// This should be a View
			Assert.IsInstanceOf<View> (XamlLoader.Create (xaml, true), "#1");
		}

		[Test]
		public void ContenPageWithMissingType()
		{
			var xaml = @"
				<ContentPage xmlns=""http://xamarin.com/schemas/2014/forms""
					xmlns:local=""clr-namespace:MissingNamespace;assembly=MissingAssembly"">
					<ContentPage.Content>
						<local:MyCustomButton />
					</ContentPage.Content>
				</ContentPage>";

			// We should have a ContentPage with a 'View' of some sort as the content because
			// we do not know what the real object is.
			var result = (ContentPage) XamlLoader.Create(xaml, true);
			Assert.IsInstanceOf<View> (result.Content, "#1");
		}
	}
}
