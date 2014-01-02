using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wphTweaks
{
    public class Versions
    {
        public static readonly Version WP8 = new Version(8, 0);
        public static readonly Version WP78 = new Version(7, 10, 8858);
        public static readonly Version GDR2 = new Version(8, 0, 10327);
        public static readonly Version GDR3 = new Version(8, 0, 10492);

        public static bool IsOSVersion(Version version)
        {
            if (version == null)
            {
                return true;
            }
            return Environment.OSVersion.Version >= version;
        }


    }
}
