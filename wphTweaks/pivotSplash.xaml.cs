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
#if WP8
using HomebrewHelperWP;
using System.Windows.Media.Imaging;
using ImageTools.IO.Bmp;
using ImageTools.IO;
using ImageTools;
using ImageTools.IO.Png;
using ImageTools.IO.Jpeg;
#endif

namespace wphTweaks
{
    public partial class pivotSplash : PhoneApplicationPage
    {
        public pivotSplash()
        {
            InitializeComponent();
#if WP8
            Decoders.AddDecoder<BmpDecoder>();
            Decoders.AddDecoder<PngDecoder>();
            Decoders.AddDecoder<JpegDecoder>();
#endif
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
#if WP8
                string defsplash = "";
                if (IsolatedStorageSettings.ApplicationSettings.TryGetValue<string>("defaultsplash", out defsplash))
                {
                    Registry.WriteString(RegistryHive.HKLM, @"SYSTEM\Shell\BootScreens", "WPBootScreenOverride", defsplash);
                }
                else
                {
                    Registry.WriteString(RegistryHive.HKLM, @"SYSTEM\Shell\BootScreens", "WPBootScreenOverride", "");
                    Registry.RemoveValue(RegistryHive.HKLM, @"SYSTEM\Shell\BootScreens", "WPBootScreenOverride");
                }
#else
                WP7RootToolsSDK.FileSystem.DeleteFile(@"\windows\mologo.bmp");
#endif
            }
            catch
            {
            }
        }

        private void WebBrowser_Navigating_1(object sender, NavigatingEventArgs e)
        {
            if (e.Uri.ToString().Contains("#downloadbg="))
            {
                var id = e.Uri.ToString().Substring(e.Uri.ToString().IndexOf("=") + 1);
                e.Cancel = true;

                using (var store = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication())
                {
                    WebClient c = new WebClient();
                    c.OpenReadAsync(new Uri("http://windowsphonehacker.com/splashes/bmp/" + id, UriKind.Absolute), id);
                    c.OpenReadCompleted += c_OpenReadCompleted;
                    MessageBox.Show("Downloading. This may take a bit.");
                }

            }
        }

        void c_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            string id = (string)e.UserState;
            try
            {
                if (!e.Cancelled)
                {
                    saveImage(e.Result, id.ToLower().EndsWith(".jpg"));
                    setBackground();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed!\n" + ex.Message);
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
            string defsplash = "";
            if (!IsolatedStorageSettings.ApplicationSettings.TryGetValue<string>("defaultsplash", out defsplash))
            {
                defsplash = Registry.ReadString(RegistryHive.HKLM, @"SYSTEM\Shell\BootScreens", "WPBootScreenOverride");
                if (defsplash.Length > 4)
                {
                    IsolatedStorageSettings.ApplicationSettings["defaultsplash"] = defsplash;
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }
            }
#endif
            bro.Navigate(new Uri("http://windowsphonehacker.com/splashes/get.php?resolution=" + Resolution, UriKind.Absolute));
        }
        private Size _resolutionSize;
        Size ResolutionSize
        {
            get
            {
                if (_resolutionSize != null)
                {
                    return _resolutionSize;
                }
#if WP8
                object size;
                if (Microsoft.Phone.Info.DeviceExtendedProperties.TryGetValue("PhysicalScreenResolution", out size))
                {
                    return _resolutionSize = (Size)size;
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

        string Resolution
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
            if (e.TaskResult == Microsoft.Phone.Tasks.TaskResult.OK)
            {
                saveImage(e.ChosenPhoto, true);
                setBackground();
            }
        }

        private void saveImage(Stream image, bool isJpeg = false)
        {
#if WP8
            using (var store = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var stream = store.OpenFile("mologo.bmp", System.IO.FileMode.Create))
                {
                    ExtendedImage ei = new ExtendedImage();
                    IImageEncoder encoder = new BmpEncoder();
                    if (isJpeg)
                    {
                        MessageBox.Show("We don't support JPG files yet!");
                        throw new UnsupportedImageFormatException();
                        var decoder = new JpegDecoder();
                        decoder.Decode(ei, stream);
                    }
                    else
                    {
                        var decoder = new BmpDecoder();
                        decoder.Decode(ei, stream);
                    }

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
#else
            MessageBox.Show("Feature not implmented yet!");
#endif
        }

        private void ViewCurrentImageButton_Click(object sender, RoutedEventArgs e)
        {
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
                MessageBox.Show("Failed: " + ex.Message);
            }
        }

        private void SplashImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SplashImage.Visibility = Visibility.Collapsed;
        }
    }
}