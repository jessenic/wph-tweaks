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
            Tweak t = new Tweak();

            t = new Tweak();
            t.title = "Volume Control";
            t.key = @"HKLM\SOFTWARE\Microsoft\Settings\Volume\MaxSystemUIVolume";
            t.type = Tweak.controlType.selector;
            t.options = new List<SelectorTweak>();
            t.options.Add(new SelectorTweak() { Title = "0 - 10", IntValue = 10 });
            t.options.Add(new SelectorTweak() { Title = "0 - 15", IntValue = 15 });
            t.options.Add(new SelectorTweak() { Title = "0 - 20", IntValue = 20 });
            t.options.Add(new SelectorTweak() { Title = "0 - 30 (Default)", IntValue = 30 });
            t.options.Add(new SelectorTweak() { Title = "0 - 40", IntValue = 40 });
            t.options.Add(new SelectorTweak() { Title = "0 - 50", IntValue = 50 });
            t.rebootNeeded = true;
            Tweaks.Add(t);

            t = new Tweak();
            t.title = "Limit volume on reboot";
            t.key = @"HKLM\SOFTWARE\OEM\VolumeLimit\EnableVolumeLimit";
            t.type = Tweak.controlType.toggle;
            t.onValue = 1;
            t.offValue = 0;
            Tweaks.Add(t);

            t = new Tweak();
            t.title = "Volume Limit on reboot";
            t.key = @"HKLM\SOFTWARE\OEM\VolumeLimit\VolumeLimit";
            t.type = Tweak.controlType.slider;
            t.minValue = 0;
            t.maxValue = 30;
            t.defaultInt = 19;
            Tweaks.Add(t);

            t = new Tweak();
            t.title = "Capacitive button vibration";
            t.onValue = 1;
            t.offValue = 0;
            t.key = @"HKLM\System\Touch\Buttons\Vibrate";
            t.rebootNeeded = true;
            Tweaks.Add(t);

            t = new Tweak();
            t.title = "Start key wakes up the screen";
            t.offValue = 0;
            t.onValue = 1;
            t.keyType = Tweak.tweakType.dword;
            t.key = @"HKLM\SYSTEM\Keyboard\EnableStartOnIdle";
            t.rebootNeeded = true;
            Tweaks.Add(t);
        }
    }
}
