using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Phone.Management.Deployment;
using System.Windows.Media;
using Windows.ApplicationModel;
using System.IO.IsolatedStorage;
using System.IO;
using HomebrewHelperWP;
using CSharp___DllImport;

namespace wphTweaks
{
    public partial class CameraApps : PhoneApplicationPage
    {
        private bool loaded = false;
        public CameraApps()
        {
            InitializeComponent();
            //[HKEY_LOCAL_MACHINE\lumiasoft\Microsoft\Photos\OEM\DefaultLens\{73e1801b-916e-4db5-87ba-65fbc8dfd8fc}]
            //@ = "App Name"
            loaded = false;
            var apps = InstallationManager.FindPackages();
            foreach (var app in apps.OrderBy(a => a.Id.Name))
            {
                MainListBox.Items.Add(app);
                string str = Registry.ReadString(RegistryHive.HKLM, @"SOFTWARE\Microsoft\Photos\OEM\DefaultLens\" + app.Id.ProductId, "");
                if (str != "")
                {
                    MainListBox.SelectedItems.Add(app);
                }
            }
            loaded = true;
        }

        private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (loaded)
            {
                if (e.AddedItems != null)
                {
                    foreach (var item in e.AddedItems)
                    {
                        Registry.CreateKey(RegistryHive.HKLM, @"SOFTWARE\Microsoft\Photos\OEM\DefaultLens\" + ((Package)item).Id.ProductId);
                        MessageBox.Show(((Win32ErrorCode)Registry.LastError).ToString());
                        Registry.WriteString(RegistryHive.HKLM, @"SOFTWARE\Microsoft\Photos\OEM\DefaultLens\" + ((Package)item).Id.ProductId, "", ((Package)item).Id.Name);
                        MessageBox.Show(((Win32ErrorCode)Registry.LastError).ToString());
                    }
                }
                if (e.RemovedItems != null)
                {
                    foreach (var item in e.RemovedItems)
                    {
                        Registry.RemoveKey(RegistryHive.HKLM, @"SOFTWARE\Microsoft\Photos\OEM\DefaultLens\" + ((Package)item).Id.ProductId, true);
                    }
                }
            }
        }
    }
}