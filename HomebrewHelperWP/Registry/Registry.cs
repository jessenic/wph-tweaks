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
            uint error = 0;
            string ret = string.Empty;
            try
            {
                if (!NativeRegistry.ReadString((global::Registry.RegistryHive)hive, path, value, out ret))
                {
                    error = NativeRegistry.GetError();
                }
            }
            catch (Exception ex)
            {
                error = (uint)ex.HResult;
            }

            if (error != 0 && SammyHelper.UseSammy)
            {
                try
                {
                    ret = SammyHelper.SammyReadString(hive, path, value, out error);
                    if (ret.ToLower().StartsWith("error in"))
                    {
                        ret = string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    error = (uint)ex.HResult;
                }
            }
            LastError = error;
            return ret;
        }
        public static uint ReadDWORD(RegistryHive hive, string path, string value)
        {
            uint ret = 0;
            uint error = 0;
            try
            {
                if (!NativeRegistry.ReadDWORD((global::Registry.RegistryHive)hive, path, value, out ret))
                {
                    error = NativeRegistry.GetError();
                }
            }
            catch (Exception ex)
            {
                error = (uint)ex.HResult;
            }
            if (error != 0 && SammyHelper.UseSammy)
            {
                try
                {
                    error = SammyHelper.SammyReadDWORD(hive, path, value, out ret);
                }
                catch (Exception ex)
                {
                    error = (uint)ex.HResult;
                }
            }
            LastError = error;
            return ret;
        }
        public static void WriteString(RegistryHive hive, string path, string value, string data)
        {
            uint error = 0;
            try
            {
                if (!NativeRegistry.WriteString((global::Registry.RegistryHive)hive, path, value, data))
                {
                    error = NativeRegistry.GetError();
                }
            }
            catch (Exception ex)
            {
                error = (uint)ex.HResult;
            }
            if (error != 0 && SammyHelper.UseSammy)
            {
                try
                {
                    uint retval;
                    error = SammyHelper.SammyWriteString(hive, path, value, data, out retval);
                }
                catch (Exception ex)
                {
                    error = (uint)ex.HResult;
                }
            }
            LastError = error;
        }
        public static void WriteDWORD(RegistryHive hive, string path, string value, uint data)
        {
            uint error = 0;
            try
            {
                if (!NativeRegistry.WriteDWORD((global::Registry.RegistryHive)hive, path, value, data))
                {
                    error = NativeRegistry.GetError();
                }
            }
            catch (Exception ex)
            {
                error = (uint)ex.HResult;
            }

            if (error != 0 && SammyHelper.UseSammy)
            {
                try
                {
                    uint retval;
                    error = SammyHelper.SammyWriteDWORD(hive, path, value, data, out retval);
                }
                catch (Exception ex)
                {
                    error = (uint)ex.HResult;
                }
            }
            LastError = error;
        }

        public static string[] GetSubKeyNames(RegistryHive hive, string path)
        {
            string[] ret = new string[] { };
            uint error = 0;
            try
            {
                if (!NativeRegistry.GetSubKeyNames((global::Registry.RegistryHive)hive, path, out ret))
                {
                    error = NativeRegistry.GetError();
                }
            }
            catch (Exception ex)
            {
                error = (uint)ex.HResult;
            }
            LastError = error;
            if (ret == null)
            {
                return new string[] { };
            }
            return ret;
        }

        public static RegistryValueInfo[] GetValues(RegistryHive hive, string path)
        {
            ValueInfo[] ret = new ValueInfo[] { };
            uint error = 0;
            try
            {
                if (!NativeRegistry.GetValues((global::Registry.RegistryHive)hive, path, out ret))
                {
                    error = NativeRegistry.GetError();
                }
            }
            catch (Exception ex)
            {
                error = (uint)ex.HResult;
            }
            LastError = error;
            if (ret == null)
            {
                return new RegistryValueInfo[] { };
            }
            RegistryValueInfo[] ret2 = new RegistryValueInfo[ret.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                ret2[i].Length = ret[i].Length;
                ret2[i].Name = ret[i].Name;
                ret2[i].Type = (RegistryType)ret[i].Type;
            }
            return ret2;
        }


        public static void RemoveValue(RegistryHive hive, string path, string value)
        {
            uint error = 0;
            try
            {
                if (!NativeRegistry.DeleteValue((global::Registry.RegistryHive)hive, path, value))
                {
                    error = NativeRegistry.GetError();
                }
            }
            catch (Exception ex)
            {
                error = (uint)ex.HResult;
            }

            LastError = error;
        }

        public static void RemoveKey(RegistryHive hive, string path, bool recursive = true)
        {
            uint error = 0;
            try
            {
                if (!NativeRegistry.DeleteKey((global::Registry.RegistryHive)hive, path, recursive))
                {
                    error = NativeRegistry.GetError();
                }
            }
            catch (Exception ex)
            {
                error = (uint)ex.HResult;
            }

            LastError = error;
        }


        public static void CreateKey(RegistryHive hive, string path)
        {
            uint error = 0;
            try
            {
                if (!NativeRegistry.CreateKey((global::Registry.RegistryHive)hive, path))
                {
                    error = NativeRegistry.GetError();
                }
            }
            catch (Exception ex)
            {
                error = (uint)ex.HResult;
            }

            LastError = error;
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

    public struct RegistryValueInfo
    {
        public uint Length;
        public string Name;
        public RegistryType Type;
    }
    public enum RegistryType
    {
        None = 0,
        String = 1,
        VariableString = 2,
        Binary = 3,
        Integer = 4,
        IntegerBigEndian = 5,
        SymbolicLink = 6,
        MultiString = 7,
        ResourceList = 8,
        HardwareResourceLIst = 9,
        ResourceRequirement = 10,
        Long = 11,
    }
}
