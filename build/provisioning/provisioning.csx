var channel = Env("CHANNEL") ?? "Stable";

if (IsMac)
{
  Item (XreItem.Xcode_11_0_0_beta_7).XcodeSelect ();
}
Console.WriteLine(channel);
XamarinChannel(channel);