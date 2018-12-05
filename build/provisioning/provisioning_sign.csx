if (!IsMac)
  return;

AppleCodesignIdentity(
    "iPhone Developer: Xamarin QA (JP4JS5NR3R)",
    "http://dl.internalx.com/qa/code-signing-entitlements/pb_dev_iphone.p12");

AppleCodesignProfile("http://dl.internalx.com/qa/code-signing-entitlements/Wildcard_iOS_Development_Profile.mobileprovision");
