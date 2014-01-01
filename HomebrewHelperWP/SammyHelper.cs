using RPCComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomebrewHelperWP
{
    internal static class SammyHelper
    {

        internal static uint SammyWriteString(RegistryHive hive, string path, string value, string data, out uint retval)
        {
            InitSammy();
            return CRPCComponent.Registry_SetString((uint)hive, path, value, data, out retval);
        }

        internal static uint SammyWriteDWORD(RegistryHive hive, string path, string value, uint data, out uint retval)
        {
            InitSammy();
            return CRPCComponent.Registry_SetDWORD((uint)hive, path, value, data, out retval);
        }

        internal static string SammyReadString(RegistryHive hive, string path, string value, out uint error)
        {
            InitSammy();
            return CRPCComponent.Registry_GetString((uint)hive, path, value, out error);
        }

        internal static uint SammyReadDWORD(RegistryHive hive, string path, string value, out uint error)
        {
            InitSammy();
            return CRPCComponent.Registry_GetDWORD((uint)hive, path, value, out error);
        }

        internal static bool SammyInited = false;
        internal static bool UseSammy = true;
        internal static void InitSammy()
        {
            if (!SammyInited)
            {
                try
                {
                    CRPCComponent.Initialize();
                    SammyInited = true;
                }
                catch
                {
                    UseSammy = false;
                }
            }
        }

        internal static uint SammyReboot()
        {
            InitSammy();
            uint ret;
            CRPCComponent.System_Reboot(out ret);
            return ret;
        }
    }
}
