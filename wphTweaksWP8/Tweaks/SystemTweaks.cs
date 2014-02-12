﻿using System;
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
                RequiredOSVersion = Versions.GDR3,
                Key = @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Theme\LargeScreen",
                RebootNeeded = true
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
                RequiredOSVersion = Versions.GDR3,
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


            //Not working?
            //t = new Tweak();
            //t.Title = "Allow disabling camera shutter sound";
            //t.OnValue = 0;
            //t.OffValue = 1;
            //t.Key = @"HKLM\SOFTWARE\Microsoft\EventSounds\Sounds\Camera\Locked";
            //Tweaks.Add(t);

        }

    }
}