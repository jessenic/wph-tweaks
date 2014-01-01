using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomebrewHelperWP.Tools
{
    public class SystemTools
    {
        public static bool Reboot()
        {
            if (SammyHelper.UseSammy)
            {
                SammyHelper.SammyReboot();
                return true;
            }
            else
            {
                return false; //Fail
            }
        }
    }
}
