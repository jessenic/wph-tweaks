﻿using System;
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
using Microsoft.Phone.Info;

namespace wphTweaks
{
    public partial class MainPage : PhoneApplicationPage
    {
        bool cooldown = true;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Check for root
            if (WP7RootToolsSDK.Environment.HasRootAccess() && !Mangopollo.Utils.IsWP8)
            {
                // Add the quick toggles defined in Tweaks
                foreach (Tweak tweak in Tweaks.tweaks)
                {
                    if (tweak.type == Tweak.controlType.toggle)
                    {
                        ToggleSwitch control = new ToggleSwitch();

                        control.Header = tweak.title;
                        if (tweak.description != "")
                            control.Content = tweak.description;

                        control.FontSize = 22;

                        //get valuelolo
                        if (tweak.keyType == Tweak.tweakType.dword)
                        {
                            var val = 0;
                            try
                            {
                                val = (int)WP7RootToolsSDK.Registry.GetDWordValue(tweak.getHive(), tweak.getKeyName(), tweak.getValueName());
                            }
                            catch
                            {

                            }
                            control.IsChecked = (val == tweak.onValue);
                        }
                        if (tweak.keyType == Tweak.tweakType.str)
                        {
                            var val = "";
                            try
                            {
                                val = WP7RootToolsSDK.Registry.GetStringValue(tweak.getHive(), tweak.getKeyName(), tweak.getValueName());
                            }
                            catch
                            {
                            }
                            control.IsChecked = (val == tweak.strOnValue);
                        }

                        control.Checked += new EventHandler<RoutedEventArgs>(control_Checked);
                        control.Unchecked += new EventHandler<RoutedEventArgs>(control_Checked);

                        controlsPanel.Children.Add(control);
                    }
                    if (tweak.type == Tweak.controlType.selector)
                    {
                        ListPicker lp = new ListPicker();
                        lp.Header = tweak.title;
                        lp.ItemsSource = tweak.options;
                        lp.SetValue(ListPicker.ItemCountThresholdProperty, 10);
                        lp.SelectionChanged += new SelectionChangedEventHandler(lp_SelectionChanged);
                        lp.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(lp_Tap);
                        controlsPanel.Children.Add(lp);
                    }
                    if (tweak.type == Tweak.controlType.title)
                    {
                        addCategory(tweak.title);
                    }
                }
            }
            else
            {
                LayoutRoot.Visibility = System.Windows.Visibility.Collapsed;
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
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("tools", "clicked", "Add Google", 0);
            //HKCU\Software\Microsoft\Internet Explorer\SearchProviders\Google\URL
            //http://www.google.com/search?hl=en&q={searchTerms}&meta=
            try
            {
                WP7RootToolsSDK.Registry.CreateKey(WP7RootToolsSDK.RegistryHive.CurrentUser, @"Software\Microsoft\Internet Explorer\SearchProviders\Google");
            }
            catch
            {
            }
            try
            {
                WP7RootToolsSDK.Registry.SetStringValue(WP7RootToolsSDK.RegistryHive.CurrentUser, @"Software\Microsoft\Internet Explorer\SearchProviders\Google", "URL", "http://www.google.com/search?hl=en&q={searchTerms}&meta=");
            }
            catch
            {
            }

            CSharp___DllImport.Phone.AppLauncher.LaunchBuiltInApplication(CSharp___DllImport.Phone.AppLauncher.Apps.Internet7Settings, "_default");
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (Mangopollo.Utils.IsWP8)
            {
                GoogleAnalytics.EasyTracker.GetTracker().SendEvent("wrongdevice", DeviceStatus.DeviceManufacturer + " " + DeviceStatus.DeviceName, "OS: " + Environment.OSVersion.Version + ", FW: " + DeviceStatus.DeviceFirmwareVersion + ", HW: " + DeviceStatus.DeviceHardwareVersion, 0);
                MessageBox.Show("This app is for Windows Phone 7 and you seem to have Windows Phone 8. Please downlad the Windows Phone 8 version instead.");
                if (NavigationService.CanGoBack)
                {
                    while (NavigationService.RemoveBackEntry() != null)
                    {
                        NavigationService.RemoveBackEntry();
                    }
                }
                return;
            }
            //Disclaimer
            bool disclaimer = false;
            System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.TryGetValue<bool>("disclaimer2", out disclaimer);
            if (!disclaimer)
            {
                NavigationService.Navigate(new Uri("/DisclaimerPage.xaml", UriKind.Relative));
            }

            if (!WP7RootToolsSDK.Environment.HasRootAccess())
            {
                NavigationService.Navigate(new Uri("/NeedsRoot.xaml", UriKind.Relative));
            }

            LayoutRoot.Visibility = System.Windows.Visibility.Visible;

            UpdateChecker.CheckUpdatesAsync();
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

                foreach (Tweak tweak in Tweaks.tweaks)
                {
                    if (tweak.title == (string)ctrl.Header)
                    {
                        GoogleAnalytics.EasyTracker.GetTracker().SendEvent("tweaks", "selected", tweak.title, ctrl.SelectedIndex);
                        if (tweak.keyType == Tweak.tweakType.str)
                        {
                            string val = ((SelectorTweak)ctrl.SelectedItem).Value;
                            WP7RootToolsSDK.Registry.SetStringValue(tweak.getHive(), tweak.getKeyName(), tweak.getValueName(), val);
                            System.Diagnostics.Debug.WriteLine(val);
                        }
                        else
                        {
                            try
                            {
                                WP7RootToolsSDK.Registry.CreateKey(tweak.getHive(), tweak.getKeyName());
                            }
                            catch
                            {
                            }
                            int val = ((SelectorTweak)ctrl.SelectedItem).IntValue;
                            WP7RootToolsSDK.Registry.SetDWordValue(tweak.getHive(), tweak.getKeyName(), tweak.getValueName(), (uint)val);
                            if (tweak.rebootNeeded)
                                rbneeded();
                        }
                    }
                }
            }
        }

        void control_Checked(object sender, RoutedEventArgs e)
        {
            if (!cooldown)
            {
                ToggleSwitch ctrl = (ToggleSwitch)sender;

                foreach (Tweak tweak in Tweaks.tweaks)
                {
                    if (tweak.title == (string)ctrl.Header)
                    {
                        GoogleAnalytics.EasyTracker.GetTracker().SendEvent("tweaks", "checked", tweak.title, ctrl.IsChecked.Value ? 1 : 0);
                        if (tweak.keyType == Tweak.tweakType.dword)
                        {
                            try
                            {
                                WP7RootToolsSDK.Registry.CreateKey(tweak.getHive(), tweak.getKeyName());
                            }
                            catch
                            {
                            }
                            int val = (ctrl.IsChecked.Value ? tweak.onValue : tweak.offValue);
                            WP7RootToolsSDK.Registry.SetDWordValue(tweak.getHive(), tweak.getKeyName(), tweak.getValueName(), (uint)val);
                        }
                        else
                        {
                            string val = (ctrl.IsChecked.Value ? tweak.strOnValue : tweak.strOffValue);
                            WP7RootToolsSDK.Registry.SetStringValue(tweak.getHive(), tweak.getKeyName(), tweak.getValueName(), val);
                        }
                        if (tweak.rebootNeeded)
                        {
                            rbneeded();
                        }
                    }
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
                CSharp___DllImport.Phone.OS.Shutdown(CSharp___DllImport.EWX.EWX_REBOOT);
            }
        }

        private void btnBrandedUpdates_Click(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("tools", "click", "Branded Updates", 0);
            var old = "";
            String[] obvals = { "MOName", "OemName", "MobileOperator" };
            foreach (String val in obvals)
            {
                old = WP7RootToolsSDK.Registry.GetStringValue(WP7RootToolsSDK.RegistryHive.LocalMachine, @"System\Platform\DeviceTargetingInfo", val);
                WP7RootToolsSDK.Registry.SetStringValue(WP7RootToolsSDK.RegistryHive.LocalMachine, @"System\Platform\DeviceTargetingInfo", val, old + "_blocked");
            }

        }

        private void btnEnableBrandedUpdates_Click(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("tools", "click", "Enable Branded Updates", 0);
            var old = "";
            String[] obvals = { "MOName", "OemName", "MobileOperator" };

            foreach (String val in obvals)
            {
                old = WP7RootToolsSDK.Registry.GetStringValue(WP7RootToolsSDK.RegistryHive.LocalMachine, @"System\Platform\DeviceTargetingInfo", val);
                WP7RootToolsSDK.Registry.SetStringValue(WP7RootToolsSDK.RegistryHive.LocalMachine, @"System\Platform\DeviceTargetingInfo", val, old.Replace("_blocked", ""));
            }

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("tools", "click", "Disable carrier logo", 0);
            WP7RootToolsSDK.FileSystem.CopyFile(@"\Applications\Install\abc1e9fe-b4ab-402c-ab21-11e97e3fde3a\Install\SplashScreenImage.jpg", @"\Windows\mologo.bmp");
        }

        private void btnRestoreCarrierLogo_Click(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("tools", "click", "Restore carrier logo", 0);
            WP7RootToolsSDK.FileSystem.DeleteFile(@"\Windows\mologo.bmp");
        }

        public static void rbneeded()
        {
            if (MessageBox.Show("Restarted needed for this change to take effect. Restart now?", "Restart Needed", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                CSharp___DllImport.Phone.OS.Shutdown(CSharp___DllImport.EWX.EWX_REBOOT);
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

    }
}