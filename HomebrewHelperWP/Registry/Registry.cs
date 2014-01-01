using Registry;
using RPCComponent;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomebrewHelperWP
{
    public static class Registry
    {
        public static uint LastError { get; private set; }
        public static bool HasError
        {
            get
            {
                return LastError != 0;
            }
        }
        public static string ReadString(RegistryHive hive, string path, string value)
        {
            string ret = string.Empty;
            try
            {
                if (!NativeRegistry.ReadString((global::Registry.RegistryHive)hive, path, value, out ret))
                {
                    uint error = NativeRegistry.GetError();
                    //ret = SammyReadString(hive, path, value, out error);
                    LastError = error;
                    //if (error != 0)
                    //{
                    //throw new Exception("Read failed: " + error);
                    //}
                }
                else
                {
                    LastError = 0;
                }
            }
            catch (Exception ex)
            {
                LastError = (uint)ex.HResult;
            }
            return ret;
        }
        public static uint ReadDWORD(RegistryHive hive, string path, string value)
        {
            uint ret = 0;
            try
            {
                if (!NativeRegistry.ReadDWORD((global::Registry.RegistryHive)hive, path, value, out ret))
                {
                    uint error = NativeRegistry.GetError();
                    if (SammyHelper.UseSammy)
                    {
                        ret = SammyHelper.SammyReadDWORD(hive, path, value, out error);
                    }
                    LastError = error;
                    //if (error != 0)
                    //{
                    //throw new Exception("Read failed: " + error);
                    //}
                }
                else
                {
                    LastError = 0;
                }
            }
            catch (Exception ex)
            {
                LastError = (uint)ex.HResult;
            }
            return ret;
        }
        public static void WriteString(RegistryHive hive, string path, string value, string data)
        {
            try
            {
                if (!NativeRegistry.WriteString((global::Registry.RegistryHive)hive, path, value, data))
                {
                    LastError = NativeRegistry.GetError();
                    if (SammyHelper.UseSammy)
                    {
                        uint retval;
                        LastError = SammyHelper.SammyWriteString(hive, path, value, data, out retval);
                    }
                    //if (error != 0)
                    //{
                    //    throw new Exception("Write failed: " + error);
                    //}
                }
                else
                {
                    LastError = 0;
                }
            }
            catch (Exception ex)
            {
                LastError = (uint)ex.HResult;
            }
        }
        public static void WriteDWORD(RegistryHive hive, string path, string value, uint data)
        {
            try
            {
                if (!NativeRegistry.WriteDWORD((global::Registry.RegistryHive)hive, path, value, data))
                {
                    LastError = NativeRegistry.GetError();
                    if (SammyHelper.UseSammy)
                    {
                        uint retval;
                        LastError = SammyHelper.SammyWriteDWORD(hive, path, value, data, out retval);
                    }
                    //if (error != 0)
                    //{
                    //    throw new Exception("Write failed: " + error);
                    //}
                }
                else
                {
                    LastError = 0;
                }
            }
            catch (Exception ex)
            {
                LastError = (uint)ex.HResult;
            }
        }

        public static string[] GetSubKeyNames(RegistryHive hive, string path)
        {
            string[] ret = new string[] { };
            if (!NativeRegistry.GetSubKeyNames((global::Registry.RegistryHive)hive, path, out ret))
            {
                LastError = NativeRegistry.GetError();
            }
            else
            {
                LastError = 0;
            }
            return ret;
        }

        public static void RemoveValue(RegistryHive hive, string path, string value)
        {
            LastError = 0;
            if (!NativeRegistry.DeleteValue((global::Registry.RegistryHive)hive, path, value))
            {
                LastError = NativeRegistry.GetError();
            }
        }


        public static void CreateKey(RegistryHive hive, string path)
        {
            if (!NativeRegistry.CreateKey((global::Registry.RegistryHive)hive, path))
            {
                LastError = NativeRegistry.GetError();
            }
            else
            {
                LastError = 0;
            }
        }
    }
    public enum RegistryHive
    {
        HKCR = -2147483648,
        HKCU = -2147483647,
        HKLM = -2147483646,
        HKU = -2147483645,
        HKPD = -2147483644,
        HKCC = -2147483643,
    }
}
