using System;
using System.Collections.Generic;
using System.Windows.Input;
using NUnit.Framework;
using Xamarin.Forms;
using Xamarin.Forms.Core.UnitTests;

namespace Xamarin.Forms.Xaml.UnitTests
{
	public class E2C_Command : ICommand
	{
		event EventHandler ICommand.CanExecuteChanged
		{
			add
			{
				throw new NotImplementedException();
			}

			remove
			{
				throw new NotImplementedException();
			}
		}

		public bool CanExecute(object parameter) => true;
		public void Execute(object parameter) => Assert.Pass();
	}

	public class E2C_Command3 : ICommand
	{
		event EventHandler ICommand.CanExecuteChanged
		{
			add
			{
				throw new NotImplementedException();
			}

			remove
			{
				throw new NotImplementedException();
			}
		}

		public bool CanExecute(object parameter) => parameter is "Foo";
		public void Execute(object parameter)
		{
			if (parameter is "Foo")
				Assert.Pass();
			Assert.Fail();
		}
	}

	public partial class EventToCommand : ContentPage
	{
		public class VM
		{
			public ICommand Happened { get; } = new Command(o => {
				if (o is "Bar")
					Assert.Pass();
				Assert.Fail();
			});
			public string Param { get; } = "Bar";
		}

		public EventToCommand() => InitializeComponent();
		public EventToCommand(bool useCompiledXaml)
		{
			//this stub will be replaced at compile time
		}

		public event EventHandler Happened;
		public event EventHandler Happened2;
		public event EventHandler Happened3;

		public void SendHappening() => Happened?.Invoke(this, EventArgs.Empty);
		public void SendHappening2() => Happened2?.Invoke(this, EventArgs.Empty);
		public void SendHappening3() => Happened3?.Invoke(this, EventArgs.Empty);

		[TestFixture]
		public class Tests
		{
			[SetUp]
			public void Setup()
			{
				Device.PlatformServices = new MockPlatformServices();
				Xamarin.Forms.Internals.Registrar.RegisterAll(new Type[0]);
			}

			[TearDown] public void TearDown() => Device.PlatformServices = null;

			[Test]
			public void EventBindingToCommand([Values (false, true)]bool useCompiledXaml)
			{
				var layout = new EventToCommand(useCompiledXaml) {
					BindingContext = new VM()
				};
				layout.SendHappening();

				Assert.Fail();
			}

			[Test]
			public void EventValueToCommand([Values(false, true)]bool useCompiledXaml)
			{
				var layout = new EventToCommand(useCompiledXaml);
				layout.SendHappening2();

				Assert.Fail();
			}

			[Test]
			public void EventStaticResourceToCommand([Values(false, true)]bool useCompiledXaml)
			{
				var layout = new EventToCommand(useCompiledXaml);
				layout.SendHappening3();

				Assert.Fail();
			}

			[Test]
			public void Compiles([Values(true)]bool useCompiledXaml)
			{
				if (!useCompiledXaml)
					return;
				MockCompiler.Compile(typeof(EventToCommand));
			}
		}
	}
}
