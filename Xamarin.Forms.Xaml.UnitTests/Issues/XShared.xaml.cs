using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Xamarin.Forms;
using Xamarin.Forms.Core.UnitTests;

namespace Xamarin.Forms.Xaml.UnitTests
{
	[XamlCompilation(XamlCompilationOptions.Skip)]
	public partial class XShared : ContentPage
	{
		public XShared() => InitializeComponent();
		public XShared(bool useCompiledXaml)
		{
			//this stub will be replaced at compile time
		}

		[TestFixture]
		class Tests
		{
			[SetUp] public void Setup() => Device.PlatformServices = new MockPlatformServices();
			[TearDown] public void TearDown() => Device.PlatformServices = null;

			[Test]
			public async void XSharedIsSupportedOnResources([Values(false, true)]bool useCompiledXaml)
			{
				var layout = new XShared(useCompiledXaml);
				layout.Resources.TryGetValue("shared", out var shared);
				Assert.That(layout.Resources.TryGetValue("shared", out var test), Is.True, "shared");
				Assert.That(test, Is.SameAs(shared)); //shared values are shared

				layout.Resources.TryGetValue("notshared", out var notshared);
				Assert.That(layout.Resources.TryGetValue("notshared", out test), Is.True, "notshared");
				Assert.That(test, Is.Not.SameAs(notshared)); //notshared values are... not

				layout.sl.Resources.TryGetValue("slnotshared", out var slnotshared);
				Assert.That(layout.sl.Resources.TryGetValue("slnotshared", out test), Is.True, "slnotshared");
				Assert.That(test, Is.Not.SameAs(slnotshared));

				layout.l0.Resources.TryGetValue("l0notshared", out var l0notshared);
				Assert.That(layout.l0.Resources.TryGetValue("l0notshared", out test), Is.True, "l0notshared");
				Assert.That(test, Is.Not.SameAs(l0notshared));

				layout.l1.Resources.TryGetValue("l1notshared", out var l1notshared);
				Assert.That(layout.l1.Resources.TryGetValue("l1notshared", out test), Is.True, "l1notshared");
				Assert.That(test, Is.Not.SameAs(l1notshared));

				var wr = new WeakReference<object>(notshared);
				notshared = null;
				GC.Collect();
				await Task.Delay(500);
				GC.Collect();
				Assert.That(wr.TryGetTarget(out _), Is.False);

			}
		}
	}
}
