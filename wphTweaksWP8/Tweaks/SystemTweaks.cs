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
            Tweak t;

            t = new Tweak();
            t.title = "'Never' timeout option";
            t.onValue = 0;
            t.offValue = 1;
            t.key = @"HKLM\SOFTWARE\Microsoft\Settings\Lock\DisableNever";
            Tweaks.Add(t);

            t = new Tweak();
            t.title = "Allow disabling camera shutter sound";
            t.onValue = 0;
            t.offValue = 1;
            t.key = @"HKLM\SOFTWARE\Microsoft\EventSounds\Sounds\Camera\Locked";
            Tweaks.Add(t);

            t = new Tweak();
            t.title = "Large screen mode (GDR3)";
            t.onValue = 1;
            t.offValue = 0;
            t.rebootNeeded = true;
            t.key = @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Theme\LargeScreen";
            Tweaks.Add(t);

            t = new Tweak();
            t.title = "Big screen for start screen (GDR3)";
            t.onValue = 2;
            t.offValue = 0;
            t.key = @"HKLM\Software\Microsoft\Shell\OEM\Start\ScreenSize";
            t.rebootNeeded = true;
            Tweaks.Add(t);

            t = new Tweak();
            t.title = "Save maps to SD card";
            t.onValue = 1;
            t.offValue = 0;
            t.key = @"HKLM\System\Maps\Storage\UseExternalStorage";
            Tweaks.Add(t);

            t = new Tweak();
            t.title = "Disable FM Radio";
            t.onValue = 1;
            t.offValue = 0;
            t.key = @"HKLM\Software\Microsoft\FMRadio\OEM\NotPresent";
            Tweaks.Add(t);

            t = new Tweak();
            t.title = "Enable Data Sense";
            t.onValue = 1;
            t.offValue = 0;
            t.key = @"HKLM\Software\Microsoft\Data Sense\DSEnabled";
            Tweaks.Add(t);

            t = new Tweak();
            t.title = "Disable Start Menu Letters";
            t.onValue = 500;
            t.offValue = 45;
            t.key = @"HKLM\Software\Microsoft\Shell\Start\GroupingThreshold";
            t.rebootNeeded = true;
            Tweaks.Add(t);

        }

    }
}
