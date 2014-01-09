using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wphTweaks.Tweaks
{
    class CustomizationTweaks : TweakCategory
    {
        public CustomizationTweaks()
        {
            Title = "Customizations";

            Tweaks.Add(new SliderTweak()
            {
                Title = "Recent people in people hub",
                DefaultValue = 8,
                Key = @"HKLM\Software\Microsoft\Pim\RecentList\8\MaxSize",
                MinValue = 0,
                MaxValue = 50
            });

            var lockTimes = new List<SelectorTweakItem>();
            lockTimes.Add(new SelectorTweakItem()
            {
                Title = "30 seconds",
                Value = 30
            });
            lockTimes.Add(new SelectorTweakItem()
            {
                Title = "1 minute",
                Value = 60
            });
            lockTimes.Add(new SelectorTweakItem()
            {
                Title = "3 minutes",
                Value = 180
            });
            lockTimes.Add(new SelectorTweakItem()
            {
                Title = "5 minutes",
                Value = 300
            });
            lockTimes.Add(new SelectorTweakItem()
            {
                Title = "15 minutes",
                Value = 900
            });
            lockTimes.Add(new SelectorTweakItem()
            {
                Title = "30 minutes",
                Value = 1800
            });
            lockTimes.Add(new SelectorTweakItem()
            {
                Title = "Never",
                Value = 0
            });

            Tweaks.Add(new SelectorTweak()
            {
                Title = "Screen timeout in seconds",
                Key = @"HKLM\SOFTWARE\Microsoft\Shell\Timeouts\DCUserIdle",
                Options = lockTimes,
                RebootNeeded = true
            });

            Tweaks.Add(new SelectorTweak()
            {
                Title = "Screen timeout in seconds when charging",
                Key = @"HKLM\SOFTWARE\Microsoft\Shell\Timeouts\ACUserIdle",
                Options = lockTimes,
                RebootNeeded = true
            });

            Tweaks.Add(new ToggleTweak()
            {
                Title = "Limit volume on reboot",
                Key = @"HKLM\SOFTWARE\OEM\VolumeLimit\EnableVolumeLimit"
            });

            Tweaks.Add(new SliderTweak()
            {
                Title = "Volume Limit on reboot",
                Key = @"HKLM\SOFTWARE\OEM\VolumeLimit\VolumeLimit",
                MinValue = 0,
                MaxValue = 30,
                DefaultValue = 19
            });

            Tweaks.Add(new ToggleTweak()
            {
                Title = "Capacitive button vibration",
                OnValue = 1,
                OffValue = 0,
                Key = @"HKLM\System\Touch\Buttons\Vibrate",
                RebootNeeded = true
            });

            Tweaks.Add(new ToggleTweak()
            {
                Title = "Start key wakes up the screen",
                OffValue = 0,
                OnValue = 1,
                KeyType = TweakType.DWORD,
                Key = @"HKLM\SYSTEM\Keyboard\EnableStartOnIdle",
                RebootNeeded = true
            });
        }
    }
}
