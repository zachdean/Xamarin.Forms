namespace System.Maui.Platform
{
	public partial class StepperHandler
	{
		public static PropertyMapper<IStepper> StepperMapper = new PropertyMapper<IStepper>(ViewHandler.ViewMapper)
		{
			[nameof(IStepper.Minimum)] = MapPropertyMinimum,
			[nameof(IStepper.Maximum)] = MapPropertyMaximum,
			[nameof(IStepper.Increment)] = MapPropertyIncrement,
			[nameof(IStepper.Value)] = MapPropertyValue
#if __ANDROID__ || NETCOREAPP
			,[nameof(IStepper.IsEnabled)] = MapPropertyIsEnabled
#endif
		};

		public StepperHandler() : base(StepperMapper)
		{

		}

		public StepperHandler(PropertyMapper mapper) : base(mapper ?? StepperMapper)
		{

		}
	}
}