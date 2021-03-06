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
using System.Collections.ObjectModel;
#if WP8
using HomebrewHelperWP;
#endif

namespace wphTweaks
{
    public partial class Rearrange : PhoneApplicationPage
    {
        class settingid
        {
            public string title;

            public string id;
            public int order;

            public override string ToString()
            {
                return title;
            }
        }
        ObservableCollection<settingid> settings = new ObservableCollection<settingid>();
        ObservableCollection<string> str = new ObservableCollection<string>();
        public Rearrange()
        {
            InitializeComponent();
#if WP7
            var key = WP7RootToolsSDK.Registry.GetKey(WP7RootToolsSDK.RegistryHive.LocalMachine, @"\Software\Microsoft\Settings\{69DAA7D1-09EA-4eae-A67E-56E4B0B4CA5B}\SecureItems");
            foreach (WP7RootToolsSDK.RegistryValue id in key.GetSubItems())
            {
                string title = WP7RootToolsSDK.Registry.GetStringValue(WP7RootToolsSDK.RegistryHive.LocalMachine, @"\Software\Microsoft\Settings\" + id.ValueName, "Title");
                settings.Add(new settingid() { id = id.ValueName, title = title, order = Convert.ToInt16(id.Value) });
            }
#else
            foreach (var val in Registry.GetSubKeyNames(RegistryHive.HKLM, @"Software\Microsoft\Settings\{69DAA7D1-09EA-4eae-A67E-56E4B0B4CA5B}\SecureItems"))
            {
                settings.Add(new settingid()
                {
                    id = val,
                    title = Registry.ReadString(RegistryHive.HKLM, @"Software\Microsoft\Settings\" + val, "Title"),
                    order = Convert.ToInt16(Registry.ReadDWORD(RegistryHive.HKLM, @"Software\Microsoft\Settings\{69DAA7D1-09EA-4eae-A67E-56E4B0B4CA5B}\SecureItems\", val))
                });
            }
#endif
            settings = new ObservableCollection<settingid>(settings.OrderBy(item => item.order));


            reorderListBox.DataContext = this.settings;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("rearrangeSettings", "save", "Rearrange settings", 0);
            int i = 0;
            foreach (settingid id in settings)
            {
#if WP7
                WP7RootToolsSDK.Registry.SetDWordValue(WP7RootToolsSDK.RegistryHive.LocalMachine, @"\Software\Microsoft\Settings\{69DAA7D1-09EA-4eae-A67E-56E4B0B4CA5B}\SecureItems", id.id, (uint)i);
#else
                Registry.WriteDWORD(RegistryHive.HKLM, @"\Software\Microsoft\Settings\{69DAA7D1-09EA-4eae-A67E-56E4B0B4CA5B}\SecureItems", id.id, (uint)i);
#endif
                i++;
            }
        }
    }
}