using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wphTweaks.Tweaks
{
    public class SystemTweaks : TweakCategory
    {
        public SystemTweaks()
        {
            Title = "System Settings";

            Tweaks.Add(new ToggleTweak()
            {
                Title = "'Never' screen timeout option",
                OnValue = 0,
                OffValue = 1,
                Key = @"HKLM\SOFTWARE\Microsoft\Settings\Lock\DisableNever"
            });

            Tweaks.Add(new ToggleTweak()
            {
                Title = "Smaller text in system apps",
                MinOSVersion = Versions.GDR3,
                MaxOSVersion = Versions.WP81,
                Key = @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Theme\LargeScreen",
                RebootNeeded = true
            });

            Tweaks.Add(new ToggleTweak()
            {
                Title = "5 quick toggles in Action Center",
                MinOSVersion = Versions.WP81,
                Key = @"HKLM\SOFTWARE\Microsoft\Settings\QuickSettingElements\Pinned\AllowedQuickSettingsElements",
                RebootNeeded = true,
                OnValue = 5,
                OffValue = 4
            });

            var startscreensizes = new List<SelectorTweakItem>();
            startscreensizes.Add(new SelectorTweakItem()
            {
                Title = "Small (2 medium tiles)",
                Value = 0
            });
            startscreensizes.Add(new SelectorTweakItem()
            {
                Title = "Medium (3 medium tiles)",
                Value = 1
            });
            startscreensizes.Add(new SelectorTweakItem()
            {
                Title = "Big (3 medium tiles)",
                Value = 2
            });
            Tweaks.Add(new SelectorTweak()
            {
                Title = "Start screen size",
                MinOSVersion = Versions.GDR3,
                Key = @"HKLM\Software\Microsoft\Shell\OEM\Start\ScreenSize",
                RebootNeeded = true,
                Options = startscreensizes
            });

            Tweaks.Add(new ToggleTweak()
            {
                Title = "Save maps to SD card",
                OnValue = 1,
                OffValue = 0,
                Key = @"HKLM\System\Maps\Storage\UseExternalStorage"
            });

            Tweaks.Add(new ToggleTweak()
            {
                Title = "Disable FM Radio option",
                OnValue = 1,
                OffValue = 0,
                Key = @"HKLM\Software\Microsoft\FMRadio\OEM\NotPresent"
            });

            Tweaks.Add(new ToggleTweak()
            {
                Title = "Enable Data Sense WiFI hotspots",
                OnValue = 1,
                OffValue = 0,
                Key = @"HKLM\Software\Microsoft\Data Sense\DSEnabled"
            });

            Tweaks.Add(new ToggleTweak()
            {
                Title = "Disable Start Menu Letters",
                OnValue = 500,
                OffValue = 45,
                Key = @"HKLM\Software\Microsoft\Shell\Start\GroupingThreshold",
                RebootNeeded = true
            });

            Tweaks.Add(new ToggleTweak()
            {
                Title = "Allow disabling camera shutter sound",
                OnValue = 1,
                OffValue = 0,
                Key = @"HKLM\Software\Microsoft\Photos\OEM\ShutterSoundUnlocked"
            });

            Tweaks.Add(new ToggleTweak()
            {
                Title = "Enable WiFi static IP",
                OnValue = 1,
                OffValue = 0,
                Key = @"HKLM\SYSTEM\ControlSet001\services\WiFiConnSvc\Parameters\Config\EnableStaticIP"
            });

            Tweaks.Add(new ToggleTweak()
            {
                Title = "Disable USB Control Panel",
                MinOSVersion = Versions.WP81,
                Key = @"HKLM\SOFTWARE\Microsoft\Shell\USB\DisableUSBControlPanel"
            });

            Tweaks.Add(new ToggleTweak()
            {
                Title = "Notify on weak charger",
                MinOSVersion = Versions.WP81,
                Key = @"HKLM\SOFTWARE\Microsoft\Shell\USB\NotifyOnWeakCharger"
            });
        }

    }
}
