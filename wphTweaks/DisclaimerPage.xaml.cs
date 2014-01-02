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
using Microsoft.Phone.Info;

namespace wphTweaks
{
    public partial class DisclaimerPage : PhoneApplicationPage
    {
        DateTime navigateTime;

        public DisclaimerPage()
        {
            InitializeComponent();
#if WP8
            textBlock6.Text = "Powered by GoodDayToDie's native access libraries";
#endif
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Remove("disclaimer2");
                System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Add("disclaimer2", true);
                System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Save();
            }
            catch
            {
            }
            if (e.NewValue > 7)
            {
                slider1.IsEnabled = false;
                GoogleAnalytics.EasyTracker.GetTracker().SendEvent("device", DeviceStatus.DeviceManufacturer + " " + DeviceStatus.DeviceName, "OS: " + Environment.OSVersion.Version + ", FW: " + DeviceStatus.DeviceFirmwareVersion + ", HW: " + DeviceStatus.DeviceHardwareVersion, 0);
                GoogleAnalytics.EasyTracker.GetTracker().SendTiming(DateTime.Now.Subtract(navigateTime), "Disclaimer", "ReadTime", "Disclaimer Read Time");
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
                else
                {
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                }
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationService.RemoveBackEntry();
            navigateTime = DateTime.Now;
        }
    }
}