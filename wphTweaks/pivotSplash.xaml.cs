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
                defsplash = Registry.ReadString(RegistryHive.HKLM, @"SYSTEM\Shell\BootScreens", "WPShutdownScreenOverride_Original");
                if (defsplash == null || defsplash.Length < 2 || defsplash == "none")
                {
                    Registry.WriteString(RegistryHive.HKLM, @"SYSTEM\Shell\BootScreens", "WPShutdownScreenOverride", "");
                    Registry.RemoveValue(RegistryHive.HKLM, @"SYSTEM\Shell\BootScreens", "WPShutdownScreenOverride");
                }
                else
                {
                    Registry.WriteString(RegistryHive.HKLM, @"SYSTEM\Shell\BootScreens", "WPShutdownScreenOverride", defsplash);
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

#if WP8
                    CustomMessageBox cmb = new CustomMessageBox()
                    {
                        IsLeftButtonEnabled = true,
                        IsRightButtonEnabled = true,
                        RightButtonContent = "Shutdown",
                        LeftButtonContent = "Startup",
                        Message = "Which boot screen you'd like to replace?"

                    };
                    cmb.Dismissed += new EventHandler<DismissedEventArgs>((dsender, de) =>
                    {
                        bool shutdown = false;
                        if (de.Result == CustomMessageBoxResult.RightButton)
                        {
                            shutdown = true;
                        }
                        saveImage(e.Result, shutdown);
                        setBackground(shutdown);
                    });
                    cmb.Show();
#else
                    saveImage(e.Result);
                    setBackground();
#endif
                }
            }
            catch (Exception ex)
            {
                GoogleAnalytics.EasyTracker.GetTracker().SendException("Splash Download Failed: " + ex.Message, false);
                MessageBox.Show("Download failed!\n" + ex.Message);
            }
        }


        void setBackground(bool shutdown = false)
        {
#if WP8
            string dir = @"C:\Data\Users\DefApps\AppData\{a85aaecb-e288-4b19-a8e0-fca5d0f2a444}\Local\";
            if (shutdown)
            {
                Registry.WriteString(RegistryHive.HKLM, @"SYSTEM\Shell\BootScreens", "WPShutdownScreenOverride", dir + "mologo_shutdown.bmp");
            }
            else
            {
                Registry.WriteString(RegistryHive.HKLM, @"SYSTEM\Shell\BootScreens", "WPBootScreenOverride", dir + "mologo.bmp");
            }
#else
            string dir = @"\Applications\Data\abc1e9fe-b4ab-402c-ab21-11e97e3fde3a\Data\IsolatedStore";
            WP7RootToolsSDK.FileSystem.CopyFile(dir + "\\mologo.bmp", @"\windows\mologo.bmp");
#endif
            MessageBox.Show("Splash successfully changed.");
        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
#if WP8
            string defsplash = Registry.ReadString(RegistryHive.HKLM, @"SYSTEM\Shell\BootScreens", "WPShutdownScreenOverride_Original");
            if (defsplash == null || defsplash.Length < 2)
            {
                defsplash = Registry.ReadString(RegistryHive.HKLM, @"SYSTEM\Shell\BootScreens", "WPShutdownScreenOverride");
                if (defsplash.Length < 2)
                {
                    defsplash = "none";
                }
                Registry.WriteString(RegistryHive.HKLM, @"SYSTEM\Shell\BootScreens", "WPShutdownScreenOverride_Original", defsplash);
            }
            defsplash = Registry.ReadString(RegistryHive.HKLM, @"SYSTEM\Shell\BootScreens", "WPBootScreenOverride_Original");
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

        public class ImageListItem
        {
            public string Title { get; set; }
            public ImageSource Thumbnail { get; set; }
            public Dictionary<string, string> Resolutions = new Dictionary<string, string>(); // resolution, uri
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
                    ImageListItem ili = null;
                    foreach (var img in ImageList.Items)
                    {
                        if (((ImageListItem)img).Title.ToLower().Equals(vars[0].ToLower()))
                        {
                            ili = ((ImageListItem)img);
                            break;
                        }
                    }
                    if (ili == null)
                    {
                        ili = new ImageListItem();
                        ili.Title = vars[0].Trim();
                        ili.Thumbnail = new BitmapImage(new Uri(vars[2]));
                        ili.Resolutions.Add(vars[1], vars[3]);
                        ImageList.Items.Add(ili);
                    }
                    else
                    {
                        ili.Resolutions.Add(vars[1], vars[3]);
                    }
                }
            }
            catch (Exception ex)
            {
                GoogleAnalytics.EasyTracker.GetTracker().SendException("Getting online gallery Failed: " + ex.Message, false);
                MessageBox.Show("Getting online gallery failed!\n" + ex.Message);
            }
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
#if WP8
                    Dispatcher.BeginInvoke(() =>
                    {
                        CustomMessageBox cmb = new CustomMessageBox()
                        {
                            IsLeftButtonEnabled = true,
                            IsRightButtonEnabled = true,
                            RightButtonContent = "Shutdown",
                            LeftButtonContent = "Startup",
                            Message = "Which boot screen you'd like to replace?"

                        };
                        cmb.Dismissed += new EventHandler<DismissedEventArgs>((dsender, de) =>
                        {
                            bool shutdown = false;
                            if (de.Result == CustomMessageBoxResult.RightButton)
                            {
                                shutdown = true;
                            }
                            saveImage(e.ChosenPhoto, shutdown);
                            setBackground(shutdown);
                        });
                        cmb.Show();
                    });
#else
                    saveImage(e.ChosenPhoto);
                    setBackground();
#endif
                }
                catch (Exception ex)
                {
                    GoogleAnalytics.EasyTracker.GetTracker().SendException("Splash change Failed: " + ex.Message, false);
                    MessageBox.Show("Splash change failed!\n" + ex.Message);
                }
            }
        }

        private void saveImage(Stream image, bool shutdown = false)
        {
            using (var store = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication())
            {
                string filename = "mologo.bmp";
                if (shutdown)
                {
                    filename = "mologo_shutdown.bmp";
                }
                using (var stream = store.OpenFile(filename, System.IO.FileMode.Create))
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
                CustomMessageBox cmb = new CustomMessageBox()
                {
                    IsLeftButtonEnabled = true,
                    IsRightButtonEnabled = true,
                    RightButtonContent = "Shutdown",
                    LeftButtonContent = "Startup",
                    Message = "Which boot screen you'd like to view?"

                };
                cmb.Dismissed += new EventHandler<DismissedEventArgs>((dsender, de) =>
                {
                    string screen = "Boot";
                    if (de.Result == CustomMessageBoxResult.RightButton)
                    {
                        screen = "Shutdown";
                    }

                    using (var stream = System.IO.File.OpenRead(Registry.ReadString(RegistryHive.HKLM, @"SYSTEM\Shell\BootScreens", "WP" + screen + "ScreenOverride")))
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
                    }
                });
                cmb.Show();
#else
                        MessageBox.Show("Feature not implemented yet!");
                }
#endif

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

        private void ImageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ImageList.SelectedItem == null)
                return;
            var ili = ImageList.SelectedItem as ImageListItem;
            ImageList.SelectedItem = null;
            var uri = ili.Resolutions.First().Value;
            if (!ili.Resolutions.Keys.Contains(Resolution))
            {
                if (ili.Resolutions.Keys.Count > 1)
                {
                    bool goodfound = false;
                    foreach (var reso in ili.Resolutions)
                    {
                        if (MessageBox.Show("This splash screen is not in your phone's native resolution.\nWould you like to use " + reso.Key + " instead?\nPress Cancel for other options: " + String.Join(", ", ili.Resolutions.Keys), "Warning!", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            goodfound = true;
                            uri = reso.Value;
                            break;
                        }
                    }
                    if (!goodfound)
                    {
                        return;
                    }
                }
                else
                {
                    if (MessageBox.Show("This splash screen is not in your phone's native resolution. " + ili.Resolutions.First().Key + " will be used instead, but it may not fill the whole screen!", "Warning!", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                }
            }
            downloadStartTime = DateTime.Now;
            WebClient c = new WebClient();
            c.OpenReadAsync(new Uri(uri, UriKind.Absolute), ili.Title);
            c.OpenReadCompleted += c_OpenReadCompleted;

            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("splashchanger", "click_" + ili.Title, "Download image " + ili.Title, 0);
            MessageBox.Show("Downloading. This may take a bit.");
        }
    }
}