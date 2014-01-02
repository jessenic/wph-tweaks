using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using HomebrewHelperWP;

namespace wphTweaks
{
    public enum TweakType { DWORD, String }

    public class ToggleTweak : Tweak
    {
        public object OnValue = 1;
        public object OffValue = 0;
        public string Description = string.Empty;
    }

    public class SliderTweak : Tweak
    {
        public int MinValue;
        public int MaxValue;
        public int DefaultValue;
    }

    public class SelectorTweak : Tweak
    {
        public List<SelectorTweakItem> Options;
    }

    public class Tweak
    {
        public string Title;

        public string Key;

        public TweakType KeyType;

        public bool RebootNeeded;

        public Version RequiredOSVersion;

        public RegistryHive Hive
        {
            get
            {
                switch (Key.Substring(0, 4).ToUpper())
                {
                    case "HKLM":
                        return RegistryHive.HKLM;
                    case "HKCU":
                        return RegistryHive.HKCU;
                }
                return RegistryHive.HKLM;
            }
        }
        public string ValueName
        {
            get
            {
                return Key.Substring(Key.LastIndexOf("\\") + 1);
            }
        }
        public string KeyName
        {
            get
            {
                return Key.Substring(5, Key.LastIndexOf("\\") - 5);
            }
        }
    }

    public class SelectorTweakItem
    {
        public string Title { get; set; }
        public object Value { get; set; }
        public override string ToString()
        {
            return Title;
        }

    }

    public class TweakCategory
    {
        public Collection<Tweak> Tweaks = new Collection<Tweak>();
        public string Title = "";
    }
}
