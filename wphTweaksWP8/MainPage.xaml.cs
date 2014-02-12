using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using HomebrewHelperWP;
using Microsoft.Phone.Tasks;

namespace wphTweaks
{
    public partial class MainPage : PhoneApplicationPage
    {
        bool cooldown = true;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            // Add the quick toggles defined in Tweaks
            foreach (TweakCategory cat in Tweaks.Tweaks.Categories)
            {
                if (cat.Tweaks.Count > 0)
                {
                    addCategory(cat.Title);
                    foreach (Tweak t in cat.Tweaks)
                    {
                        if (!Versions.IsOSVersion(t.RequiredOSVersion))
                        {
                            continue;
                        }
                        if (t is ToggleTweak)
                        {
                            var tweak = (ToggleTweak)t;
                            ToggleSwitch control = new ToggleSwitch();
                            control.Tag = tweak;
                            control.Header = tweak.Title;
                            if (tweak.Description != "")
                                control.Content = tweak.Description;

                            control.FontSize = 22;

                            //get valuelolo
                            if (tweak.KeyType == TweakType.DWORD)
                            {
                                uint val = Registry.ReadDWORD(tweak.Hive, tweak.KeyName, tweak.ValueName);
                                control.IsChecked = (val == (int)tweak.OnValue);
#if DEBUG
                                System.Diagnostics.Debug.WriteLine(tweak.Title + " = " + val);
#endif
                            }
                            else if (tweak.KeyType == TweakType.String)
                            {
                                string val = Registry.ReadString(tweak.Hive, tweak.KeyName, tweak.ValueName);
                                control.IsChecked = (val == (string)tweak.OnValue);
#if DEBUG
                                System.Diagnostics.Debug.WriteLine(tweak.Title + " = " + val);
#endif
                            }

                            control.Checked += new EventHandler<RoutedEventArgs>(control_Checked);
                            control.Unchecked += new EventHandler<RoutedEventArgs>(control_Checked);

                            controlsPanel.Children.Add(control);
                        }
                        else if (t is SelectorTweak)
                        {
                            var tweak = (SelectorTweak)t;
                            ListPicker lp = new ListPicker();
                            lp.Tag = tweak;
                            lp.Header = tweak.Title;
                            lp.ItemsSource = tweak.Options;
                            lp.SetValue(ListPicker.ItemCountThresholdProperty, 10);
                            if (tweak.KeyType == TweakType.DWORD)
                            {
                                uint val = Registry.ReadDWORD(tweak.Hive, tweak.KeyName, tweak.ValueName);
                                foreach (var opt in tweak.Options)
                                {
                                    if ((int)opt.Value == val)
                                    {
                                        lp.SelectedItem = opt;
                                        break;
                                    }
                                }
#if DEBUG
                                System.Diagnostics.Debug.WriteLine(tweak.Title + " = " + val);
#endif
                            }
                            else if (tweak.KeyType == TweakType.String)
                            {
                                string val = Registry.ReadString(tweak.Hive, tweak.KeyName, tweak.ValueName);
                                foreach (var opt in tweak.Options)
                                {
                                    if ((string)opt.Value == val)
                                    {
                                        lp.SelectedItem = opt;
                                        break;
                                    }
                                }
#if DEBUG
                                System.Diagnostics.Debug.WriteLine(tweak.Title + " = " + val);
#endif
                            }
                            lp.SelectionChanged += new SelectionChangedEventHandler(lp_SelectionChanged);
                            lp.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(lp_Tap);
                            controlsPanel.Children.Add(lp);
                        }
                        else if (t is SliderTweak)
                        {
                            var tweak = (SliderTweak)t;
                            uint val = Registry.ReadDWORD(tweak.Hive, tweak.KeyName, tweak.ValueName);
                            if (val == 2147942487)
                            {
                                val = (uint)tweak.DefaultValue;
                            }
                            else if (val < tweak.MinValue)
                            {
                                val = (uint)tweak.MinValue;
                            }
                            else if (val > tweak.MaxValue)
                            {
                                val = (uint)tweak.MaxValue;
                            }

                            StackPanel sliderStack = new StackPanel();
                            Grid vertStack = new Grid();

                            TextBlock tb2 = new TextBlock();
                            tb2.FontSize = (double)Application.Current.Resources["PhoneFontSizeNormal"];
                            tb2.Padding = new Thickness(10, 0, 0, 0);
                            tb2.Text = tweak.Title;
                            tb2.FontFamily = (FontFamily)Application.Current.Resources["PhoneFontFamilyNormal"];
                            tb2.Foreground = (Brush)Application.Current.Resources["PhoneSubtleBrush"];
                            vertStack.Children.Add(tb2);

                            TextBlock tb = new TextBlock();
                            tb.FontSize = 18;
                            tb.Text = val.ToString();
                            tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                            tb.Margin = new Thickness(0, 0, 10, 0);
                            vertStack.Children.Add(tb);
                            sliderStack.Children.Add(vertStack);

                            Slider sl = new Slider();
                            sl.Tag = tweak;
                            sl.Name = tweak.Title;
                            sl.Minimum = tweak.MinValue;
                            sl.Maximum = tweak.MaxValue;
                            sl.SmallChange = 1;
                            sl.LargeChange = 2;

                            sl.Value = val;

                            sl.ValueChanged += sl_ValueChanged;
                            sliderStack.Children.Add(sl);
                            controlsPanel.Children.Add(sliderStack);

                        }
                    }
                }
            }

        }

        void sl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider sl = (Slider)sender;
            TextBlock tb = (TextBlock)((Grid)((StackPanel)sl.Parent).Children[0]).Children[1];
            uint oldVal = uint.Parse(tb.Text);
            uint newVal = (uint)e.NewValue;
            if (oldVal != newVal)
            {
                tb.Text = (newVal).ToString();
                SliderTweak tweak = (SliderTweak)sl.Tag;
                try
                {
                    Registry.CreateKey(tweak.Hive, tweak.KeyName);
                }
                catch
                {
                }

                Registry.WriteDWORD(tweak.Hive, tweak.KeyName, tweak.ValueName, newVal);
                uint newval = Registry.ReadDWORD(tweak.Hive, tweak.KeyName, tweak.ValueName);
                if (newVal != newval)
                {
                    GoogleAnalytics.EasyTracker.GetTracker().SendException("Tweak " + tweak.Title + " failed: " + (CSharp___DllImport.Win32ErrorCode)Registry.LastError, false);
                    MessageBox.Show("Tweak failed!\nPossible error code: " + (CSharp___DllImport.Win32ErrorCode)Registry.LastError);
                    return;
                }
                if (tweak.RebootNeeded)
                    rbneeded();
            }


        }

        void rearrangeSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Rearrange.xaml", UriKind.Relative));
        }

        void addCategory(string str)
        {
            TextBlock tb2 = new TextBlock();
            tb2.FontSize = 25;
            tb2.Text = str;
            controlsPanel.Children.Add(tb2);
        }

        void btn_Click(object sender, RoutedEventArgs e)
        {
            string googlePath = @"Software\Microsoft\Internet Explorer\SearchProviders\Google";
            string googleUrl = "http://www.google.com/search?q={searchTerms}";
            //HKCU\Software\Microsoft\Internet Explorer\SearchProviders\Google\URL
            //http://www.google.com/search?hl=en&q={searchTerms}&meta=
            try
            {
                Registry.CreateKey(RegistryHive.HKCU, googlePath);
                if (Registry.LastError != 0)
                {
                    MessageBox.Show("Failed: " + (CSharp___DllImport.Win32ErrorCode)Registry.LastError);

                }
            }
            catch
            {
            }
            try
            {
                Registry.WriteString(RegistryHive.HKCU, googlePath, "URL", googleUrl);
                string newval = Registry.ReadString(RegistryHive.HKCU, googlePath, "URL");
                if (googleUrl != newval)
                {
                    GoogleAnalytics.EasyTracker.GetTracker().SendException("Add google failed: " + (CSharp___DllImport.Win32ErrorCode)Registry.LastError, false);
                    MessageBox.Show("Tweak failed!\nPossible error code: " + (CSharp___DllImport.Win32ErrorCode)Registry.LastError);
                    return;
                }
                var lp = new ListPicker();
                lp.Items.Add("Bing (default)");
                lp.Items.Add("Google.com");
                lp.Items.Add("Google app");
                CustomMessageBox cmb = new CustomMessageBox()
                {
                    Title = "Question",
                    Content = lp,
                    IsLeftButtonEnabled = false,
                    IsRightButtonEnabled = true,
                    Caption = "Choose search key action",
                    Message = "Choose what the search key does. This setting might not work in regions where Bing works!",
                    RightButtonContent = "done"
                };
                cmb.Dismissed += new EventHandler<DismissedEventArgs>((dsender, de) =>
                {
                    GoogleAnalytics.EasyTracker.GetTracker().SendEvent("tools", "clicked", "Add Google", lp.SelectedIndex);
                    if (lp.SelectedIndex == 1)
                    {
                        Registry.WriteString(RegistryHive.HKCU, googlePath, "AppUri", googleUrl);
                    }
                    else if (lp.SelectedIndex == 2)
                    {
                        Registry.WriteString(RegistryHive.HKCU, googlePath, "AppUri", "app://220bfbf2-ee02-496c-a656-651a6c0c6518/_default");
                    }
                    else
                    {
                        Registry.WriteString(RegistryHive.HKCU, googlePath, "AppUri", "");
                        Registry.RemoveValue(RegistryHive.HKCU, googlePath, "AppUri");
                    }
                    if (Registry.LastError != 0)
                    {
                        MessageBox.Show("Failed: " + (CSharp___DllImport.Win32ErrorCode)Registry.LastError);

                    }
                    NativeToastLauncher.Launcher.LaunchToast(Guid.Parse("{a85aaecb-e288-4b19-a8e0-fca5d0f2a444}"), "app://5B04B775-356B-4AA0-AAF8-6491FFEA5663/_default", "WPH Tweaks", "Tap to open IE settings", NativeToastLauncher.ToastType.Default);
                });
                cmb.Show();
            }
            catch
            {
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //Disclaimer
            bool disclaimer = false;
            System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.TryGetValue<bool>("disclaimer2", out disclaimer);
            if (!disclaimer)
            {
                NavigationService.Navigate(new Uri("/DisclaimerPage.xaml", UriKind.Relative));
            }

            LayoutRoot.Visibility = System.Windows.Visibility.Visible;
        }

        void picker_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ((ListPicker)sender).Open();
        }

        void lp_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ListPicker lp = (ListPicker)sender;
            lp.Open();
        }

        void lp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!cooldown)
            {
                ListPicker ctrl = (ListPicker)sender;
                SelectorTweak tweak = (SelectorTweak)ctrl.Tag;
                GoogleAnalytics.EasyTracker.GetTracker().SendEvent("tweaks", "selected", tweak.Title, ctrl.SelectedIndex);
                try
                {
                    Registry.CreateKey(tweak.Hive, tweak.KeyName);
                }
                catch
                {
                }
                if (tweak.KeyType == TweakType.String)
                {
                    string val = (string)((SelectorTweakItem)ctrl.SelectedItem).Value;
                    Registry.WriteString(tweak.Hive, tweak.KeyName, tweak.ValueName, val);
                    string newval = Registry.ReadString(tweak.Hive, tweak.KeyName, tweak.ValueName);
#if DEBUG
                    System.Diagnostics.Debug.WriteLine(val + " " + newval);
#endif
                    if (val != newval)
                    {
                        GoogleAnalytics.EasyTracker.GetTracker().SendException("Tweak " + tweak.Title + " failed: " + (CSharp___DllImport.Win32ErrorCode)Registry.LastError, false);
                        MessageBox.Show("Tweak failed!\nPossible error code: " + (CSharp___DllImport.Win32ErrorCode)Registry.LastError);
                        return;
                    }
                }
                else
                {
                    int val = (int)((SelectorTweakItem)ctrl.SelectedItem).Value;
                    Registry.WriteDWORD(tweak.Hive, tweak.KeyName, tweak.ValueName, (uint)val);
                    uint newval = Registry.ReadDWORD(tweak.Hive, tweak.KeyName, tweak.ValueName);
                    if (val != newval)
                    {
                        GoogleAnalytics.EasyTracker.GetTracker().SendException("Tweak " + tweak.Title + " failed: " + (CSharp___DllImport.Win32ErrorCode)Registry.LastError, false);
                        MessageBox.Show("Tweak failed!\nPossible error code: " + (CSharp___DllImport.Win32ErrorCode)Registry.LastError);
                        return;
                    }
                    if (tweak.RebootNeeded)
                        rbneeded();
                }
            }
        }

        void control_Checked(object sender, RoutedEventArgs e)
        {
            if (!cooldown)
            {
                ToggleSwitch ctrl = (ToggleSwitch)sender;
                ToggleTweak tweak = (ToggleTweak)ctrl.Tag;
                GoogleAnalytics.EasyTracker.GetTracker().SendEvent("tweaks", "checked", tweak.Title, ctrl.IsChecked.Value ? 1 : 0);
                try
                {
                    Registry.CreateKey(tweak.Hive, tweak.KeyName);
                }
                catch
                {
                }
                if (tweak.KeyType == TweakType.DWORD)
                {
                    int val = (int)(ctrl.IsChecked.Value ? tweak.OnValue : tweak.OffValue);
                    Registry.WriteDWORD(tweak.Hive, tweak.KeyName, tweak.ValueName, (uint)val);
                    uint newval = Registry.ReadDWORD(tweak.Hive, tweak.KeyName, tweak.ValueName);
                    if (val != newval)
                    {
                        GoogleAnalytics.EasyTracker.GetTracker().SendException("Tweak " + tweak.Title + " failed: " + (CSharp___DllImport.Win32ErrorCode)Registry.LastError, false);
                        MessageBox.Show("Tweak possibly failed!\nPossible error code: " + (CSharp___DllImport.Win32ErrorCode)Registry.LastError);
                        return;
                    }
                }
                else
                {
                    string val = (string)(ctrl.IsChecked.Value ? tweak.OnValue : tweak.OffValue);
                    Registry.WriteString(tweak.Hive, tweak.KeyName, tweak.ValueName, val);
                    string newval = Registry.ReadString(tweak.Hive, tweak.KeyName, tweak.ValueName);
#if DEBUG
                    System.Diagnostics.Debug.WriteLine(val + " " + newval);
#endif
                    if (val != newval)
                    {
                        GoogleAnalytics.EasyTracker.GetTracker().SendException("Tweak " + tweak.Title + " failed: " + (CSharp___DllImport.Win32ErrorCode)Registry.LastError, false);
                        MessageBox.Show("Tweak failed!\nPossible error code: " + (CSharp___DllImport.Win32ErrorCode)Registry.LastError);
                        return;
                    }
                }
                if (tweak.RebootNeeded)
                {
                    rbneeded();
                }
            }
        }

        private void controlsPanel_Loaded(object sender, RoutedEventArgs e)
        {
            cooldown = false;
        }

        private void btnUnlockMarketplace_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/mostore.xaml", UriKind.Relative));
        }

        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to restart the phone?", "Confirmation", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                GoogleAnalytics.EasyTracker.GetTracker().SendEvent("tools", "click", "Reboot", 0);
                Reboot();
            }
        }

        private void btnBrandedUpdates_Click(object sender, RoutedEventArgs e)
        {
            //var old = "";
            String[] obvals = { "MOName", "OemName", "MobileOperator" };
            foreach (String val in obvals)
            {
                //old = WP7RootToolsSDK.Registry.GetStringValue(WP7RootToolsSDK.RegistryHyve.LocalMachine, @"System\Platform\DeviceTargetingInfo", val);
                //WP7RootToolsSDK.Registry.SetStringValue(WP7RootToolsSDK.RegistryHyve.LocalMachine, @"System\Platform\DeviceTargetingInfo", val, old + "_blocked");
            }

        }

        private void btnEnableBrandedUpdates_Click(object sender, RoutedEventArgs e)
        {
            //var old = "";
            String[] obvals = { "MOName", "OemName", "MobileOperator" };

            foreach (String val in obvals)
            {
                //old = WP7RootToolsSDK.Registry.GetStringValue(WP7RootToolsSDK.RegistryHyve.LocalMachine, @"System\Platform\DeviceTargetingInfo", val);
                //WP7RootToolsSDK.Registry.SetStringValue(WP7RootToolsSDK.RegistryHyve.LocalMachine, @"System\Platform\DeviceTargetingInfo", val, old.Replace("_blocked", ""));
            }

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //WP7RootToolsSDK.FileSystem.CopyFile(@"\Applications\Install\abc1e9fe-b4ab-402c-ab21-11e97e3fde3a\Install\SplashScreenImage.jpg", @"\Windows\mologo.bmp");
        }

        private void btnRestoreCarrierLogo_Click(object sender, RoutedEventArgs e)
        {
            //WP7RootToolsSDK.FileSystem.DeleteFile(@"\Windows\mologo.bmp");
        }
        public static void Reboot()
        {
            if (!HomebrewHelperWP.Tools.SystemTools.Reboot())
            {
                MessageBox.Show("Automatic rebooting is not available on this device. Please reboot manually using the power button.");
            }
            else
            {
                MessageBox.Show("Windows Phone should reboot shortly. Please wait and don't do anything to prevent data loss.");
            }
        }

        public static void rbneeded()
        {
            if (MessageBox.Show("Reboot needed for this change to take effect. Reboot now?", "Reboot Needed", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Reboot();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/pivotSplash.xaml", UriKind.Relative));
        }

        private void btnCustomizeKeyboard_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Keyboard.xaml", UriKind.Relative));
        }

        private void InternationalSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/InternationalSettings.xaml", UriKind.Relative));
        }

        private void SoundEditorButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SoundEditor.xaml", UriKind.Relative));
        }

        private void DataConnectionStringsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/DataConnectionStringsEditor.xaml", UriKind.Relative));
        }

        private void wphLink_Click(object sender, RoutedEventArgs e)
        {
            new WebBrowserTask()
            {
                Uri = new Uri("http://windowsphonehacker.com/?utm_source=wph-tweaks&utm_medium=link&utm_campaign=wph-tweaks")
            }.Show();
        }

        private void CameraAppsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/CameraApps.xaml", UriKind.Relative));
        }

    }
}