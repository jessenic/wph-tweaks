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
            //Categories.Add(new CellCoreTweaks()); //These fail, need more caps?
            switch (DeviceStatus.DeviceManufacturer.ToUpper())
            {
                case "SAMSUNG":
                    Categories.Add(new SamsungTweaks());
                    break;
                case "NOKIA":
                    Categories.Add(new NokiaTweaks());
                    break;
            }
        }
    }
}
