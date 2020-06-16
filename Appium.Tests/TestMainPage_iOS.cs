using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;

namespace Appium.Tests
{
    [TestFixture]
    public class TestMainPage_iOS: TestMainPage<IOSDriver<IOSElement>, IOSElement>
    {
        public TestMainPage_iOS(): base("MainPageTests")
        {
        }

        protected override IOSDriver<IOSElement> GetDriver()
        {
            return new IOSDriver<IOSElement>(driverUri, appiumOptions);
        }

        protected override void InitAppiumOptions(AppiumOptions appiumOptions)
        {
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, "iPhone 8 Plus");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, "iOS");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, "13.5");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.Udid, "E45890F2-E52A-410C-803C-72E1E833B42C");
            appiumOptions.AddAdditionalCapability(IOSMobileCapabilityType.BundleId, "com.xamarin.quickui.controlgallery");
        }
    }
}
