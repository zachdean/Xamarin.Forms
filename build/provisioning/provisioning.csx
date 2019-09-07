var channel = "release-8.2-xcode11";

Console.WriteLine(channel);
XamarinChannel(channel);
if (IsMac)
{
  Item (XreItem.Xcode_11_0_0_beta_7).XcodeSelect ();
}