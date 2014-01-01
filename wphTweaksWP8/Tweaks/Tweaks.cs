using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Info;

namespace wphTweaks.Tweaks
{
    public class Tweaks
    {
        public static List<TweakCategory> Categories = new List<TweakCategory>();
        static Tweaks()
        {
            Categories.Add(new SystemTweaks());
            Categories.Add(new CustomizationTweaks());

            if (DeviceStatus.DeviceManufacturer.ToLower().Equals("samsung"))
            {
                Categories.Add(new SamsungTweaks());
            }
        }
    }
}
