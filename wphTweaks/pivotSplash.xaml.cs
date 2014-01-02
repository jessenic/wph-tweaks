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
using System.IO;
using System.IO.IsolatedStorage;
using ImageTools.IO.Bmp;
using ImageTools.IO;
using ImageTools;
using System.Windows.Media.Imaging;
#if WP8
using HomebrewHelperWP;
#endif

namespace wphTweaks
{
    public partial class pivotSplash : PhoneApplicationPage
    {
        public pivotSplash()
        {
            InitializeComponent();
            Decoders.AddDecoder<BmpDecoder>();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("splashchanger", "click", "Restore Splash", 0);
            try
            {
#if WP8
                string defsplash = Registry.ReadString(RegistryHive.HKLM, @"SYSTEM\Shell\BootScreens", "WPBootScreenOverride_Original");
                if (defsplash == null || defsplash.Length < 2 || defsplash == "none")
                {
                    Registry.WriteString(RegistryHive.HKLM, @"SYSTEM\Shell\BootScreens", "WPBootScreenOverride", "");
                    Registry.RemoveValue(RegistryHive.HKLM, @"SYSTEM\Shell\BootScreens", "WPBootScreenOverride");
                }
                else
                {
                    Registry.WriteString(RegistryHive.HKLM, @"SYSTEM\Shell\BootScreens", "WPBootScreenOverride", defsplash);
                }
#else
                WP7RootToolsSDK.FileSystem.DeleteFile(@"\windows\mologo.bmp");
#endif
            }
            catch
            {
            }
        }

        DateTime downloadStartTime;


        void c_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            string id = (string)e.UserState;
            try
            {
                if (!e.Cancelled)
                {
                    if (e.Error != null)
                    {
                        throw e.Error;
                    }
                    GoogleAnalytics.EasyTracker.GetTracker().SendTiming(DateTime.Now.Subtract(downloadStartTime), "splashchanger", "download_" + id, "Splash " + id + " Download Time");

                    saveImage(e.Result);
                    setBackground();
                }
            }
            catch (Exception ex)
            {
                GoogleAnalytics.EasyTracker.GetTracker().SendException("Splash Download Failed: " + ex.Message, false);
                MessageBox.Show("Download failed!\n" + ex.Message);
            }
        }


        void setBackground()
        {
#if WP8
            string dir = @"C:\Data\Users\DefApps\AppData\{a85aaecb-e288-4b19-a8e0-fca5d0f2a444}\Local\";
            Registry.WriteString(RegistryHive.HKLM, @"SYSTEM\Shell\BootScreens", "WPBootScreenOverride", dir + "mologo.bmp");
#else
            string dir = @"\Applications\Data\abc1e9fe-b4ab-402c-ab21-11e97e3fde3a\Data\IsolatedStore";
            WP7RootToolsSDK.FileSystem.CopyFile(dir + "\\mologo.bmp", @"\windows\mologo.bmp");
#endif
            MessageBox.Show("Splash successfully changed.");
        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
#if WP8
            string defsplash = Registry.ReadString(RegistryHive.HKLM, @"SYSTEM\Shell\BootScreens", "WPBootScreenOverride_Original");
            if (defsplash == null || defsplash.Length < 2)
            {
                defsplash = Registry.ReadString(RegistryHive.HKLM, @"SYSTEM\Shell\BootScreens", "WPBootScreenOverride");
                if (defsplash.Length < 2)
                {
                    defsplash = "none";
                }
                Registry.WriteString(RegistryHive.HKLM, @"SYSTEM\Shell\BootScreens", "WPBootScreenOverride_Original", defsplash);
            }
#endif
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += wc_DownloadStringCompleted;
            wc.DownloadStringAsync(new Uri("http://jessenic.github.io/wph-tweaks/splashes.txt"));
        }

        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    throw e.Error;
                }
                foreach (string line in e.Result.Split('\n'))
                {
                    //Format: id|resolution|thumb|image|
                    string[] vars = line.Split('|');
                    ImageList.Items.Add(new TextBlock() { Text = vars[0] + " " + vars[1] });
                    var img = new Image();
                    img.Tap += img_Tap;
                    var bmi = new BitmapImage(new Uri(vars[2]));
                    img.Tag = vars[3];
                    img.Name = "image_" + vars[0];
                    img.Source = bmi;

                    ImageList.Items.Add(img);
                }
            }
            catch (Exception ex)
            {
                GoogleAnalytics.EasyTracker.GetTracker().SendException("Getting online gallery Failed: " + ex.Message, false);
                MessageBox.Show("Getting online gallery failed!\n" + ex.Message);
            }
        }

        void img_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Image img = (Image)sender;
            downloadStartTime = DateTime.Now;
            WebClient c = new WebClient();
            c.OpenReadAsync(new Uri((string)img.Tag, UriKind.Absolute), img.Name);
            c.OpenReadCompleted += c_OpenReadCompleted;

            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("splashchanger", "click_" + img.Name, "Download image " + img.Name, 0);
            MessageBox.Show("Downloading. This may take a bit.");
        }
        private Size _resolutionSize;
        public Size ResolutionSize
        {
            get
            {
                if (_resolutionSize.Height > 0 && _resolutionSize.Width > 0)
                {
                    return _resolutionSize;
                }
#if WP8
                object size;
                if (Microsoft.Phone.Info.DeviceExtendedProperties.TryGetValue("PhysicalScreenResolution", out size))
                {
                    _resolutionSize = (Size)size;
                    if (_resolutionSize.Height > 0 && _resolutionSize.Width > 0)
                    {
                        return _resolutionSize;
                    }
                }
                switch (App.Current.Host.Content.ScaleFactor)
                {
                    case 160:
                        return _resolutionSize = new Size(768, 1280);
                    case 150:
                        return _resolutionSize = new Size(720, 1280);
                    default:
                        return _resolutionSize = new Size(480, 800);
                }
#else
                return _resolutionSize = new Size(480, 800);
#endif
            }
        }

        public string Resolution
        {
            get
            {
                return (int)ResolutionSize.Width + "x" + (int)ResolutionSize.Height;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Tasks.PhotoChooserTask t = new Microsoft.Phone.Tasks.PhotoChooserTask();
            t.PixelHeight = (int)ResolutionSize.Height;
            t.PixelWidth = (int)ResolutionSize.Width;
            t.ShowCamera = true;
            t.Completed += t_Completed;
            t.Show();
        }

        void t_Completed(object sender, Microsoft.Phone.Tasks.PhotoResult e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("splashchanger", "photolib", "Splash from photolib: " + e.TaskResult.ToString(), (int)e.TaskResult);
            if (e.TaskResult == Microsoft.Phone.Tasks.TaskResult.OK)
            {
                try
                {
                    saveImage(e.ChosenPhoto);
                    setBackground();
                }
                catch (Exception ex)
                {
                    GoogleAnalytics.EasyTracker.GetTracker().SendException("Splash change Failed: " + ex.Message, false);
                    MessageBox.Show("Splash change failed!\n" + ex.Message);
                }
            }
        }

        private void saveImage(Stream image)
        {
            using (var store = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var stream = store.OpenFile("mologo.bmp", System.IO.FileMode.Create))
                {
                    var img = new Image();
                    img.Width = ResolutionSize.Width;
                    img.Height = ResolutionSize.Height;
                    var bi = new BitmapImage();
                    bi.SetSource(image);

                    img.Source = bi;
                    var img1 = img.ToImage();
                    IImageEncoder encoder = new BmpEncoder();
                    ExtendedImage ei = new ExtendedImage(img1);

                    if (ei.PixelHeight != (int)ResolutionSize.Height || ei.PixelWidth != (int)ResolutionSize.Width)
                    {
                        byte[] pixels = new byte[((int)ResolutionSize.Height) * ((int)ResolutionSize.Width)];
                        int i = 0;
                        foreach (byte b in pixels)
                        {
                            if (ei.Pixels.Length < i)
                            {
                                pixels[i] = b;
                            }
                            else
                            {
                                pixels[i] = 0;
                            }
                            i++;
                        }
                        ei.SetPixels((int)ResolutionSize.Width, (int)ResolutionSize.Height, ei.Pixels);
                    }
                    encoder.Encode(ei, stream);
                }
            }
        }

        private void ViewCurrentImageButton_Click(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("splashchanger", "click", "View Current Image", 0);
            try
            {
#if WP8
                using (var stream = System.IO.File.OpenRead(Registry.ReadString(RegistryHive.HKLM, @"SYSTEM\Shell\BootScreens", "WPBootScreenOverride")))
                {
#else

                using (var store = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!store.FileExists("mologo.bmp"))
                    {

                        string dir = @"\Applications\Data\abc1e9fe-b4ab-402c-ab21-11e97e3fde3a\Data\IsolatedStore";
                        WP7RootToolsSDK.FileSystem.CopyFile(@"\windows\mologo.bmp", dir + "\\mologo.bmp");
                    }
                    using (var stream = store.OpenFile("mologo.bmp", System.IO.FileMode.Open))
                    {
#endif
#if WP8
                    var iso = new ExtendedImage();
                    var dec = new ImageTools.IO.Bmp.BmpDecoder();
                    dec.Decode(iso, stream);
                    SplashImage.Source = iso.ToBitmap();
                    SplashImage.Visibility = Visibility.Visible;
#else
                        MessageBox.Show("Feature not implemented yet!");
#endif
                }
#if !WP8
            }
#endif
            }
            catch (Exception ex)
            {
                GoogleAnalytics.EasyTracker.GetTracker().SendException("View Current Failed: " + ex.Message, false);
                MessageBox.Show("Failed: " + ex.Message);
            }
        }

        private void SplashImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SplashImage.Visibility = Visibility.Collapsed;
        }
    }
}