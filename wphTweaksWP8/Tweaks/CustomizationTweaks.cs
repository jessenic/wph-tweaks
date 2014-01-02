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
