using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wphTweaks.Tweaks
{
    public class CellCoreTweaks : TweakCategory
    {
        public CellCoreTweaks()
        {
            Title = "Cellular Tweaks";


            Tweaks.Add(new ToggleTweak()
            {
                Title = "Hide highest speed selection",
                OnValue = 1,
                OffValue = 0,
                Key = @"HKLM\Software\Microsoft\Cellular\MVSettings\IMSISpecific\Default\CellUX\HideHighestSpeed"
            });
            Tweaks.Add(new ToggleTweak()
            {
                Title = "Hide 2G from highest speed selection",
                OnValue = 1,
                OffValue = 0,
                Key = @"HKLM\Software\Microsoft\Cellular\MVSettings\IMSISpecific\Default\CellUX\HideHighestSpeed"
            });
            Tweaks.Add(new ToggleTweak()
            {
                Title = "Hide 4G from highest speed selection",
                OnValue = 1,
                OffValue = 0,
                Key = @"HKLM\Software\Microsoft\Cellular\MVSettings\IMSISpecific\Default\CellUX\HideHighestSpeed"
            });

            Tweaks.Add(new ToggleTweak()
            {
                Title = "Hide mode selection",
                OnValue = 1,
                OffValue = 0,
                Key = @"HKLM\Software\Microsoft\Cellular\MVSettings\IMSISpecific\Default\CellUX\HideModeSelection"
            });

            Tweaks.Add(new ToggleTweak()
            {
                Title = "LTE Enabled",
                OnValue = 1,
                OffValue = 0,
                Key = @"HKLM\Software\Microsoft\Cellular\MVSettings\IMSISpecific\Default\General\LTEEnabled"
            });

            Tweaks.Add(new ToggleTweak()
            {
                Title = "LTE Forced",
                OnValue = 1,
                OffValue = 0,
                Key = @"HKLM\Software\Microsoft\Cellular\MVSettings\IMSISpecific\Default\General\LTEForced"
            });
        }
    }
}
