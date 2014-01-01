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
    public class Tweak
    {
        public enum tweakType { dword = 0, str }
        public enum controlType { toggle = 0, slider, selector }
        public string title;

        public string key;

        public tweakType keyType;

        public string defaultString;
        public int defaultInt;

        public controlType type;
        public bool rebootNeeded;

        //slider
        public int minValue;
        public int maxValue;

        //toggle
        public int onValue = 1;
        public int offValue = 0;
        public string strOnValue;
        public string strOffValue;
        public string description;


        //selector
        public List<SelectorTweak> options;

        public RegistryHive getHive()
        {
            string firstFour = key.Substring(0, 4);
            switch (firstFour)
            {
                case "HKLM":
                    return RegistryHive.HKLM;
                case "HKCU":
                    return RegistryHive.HKCU;
            }
            return RegistryHive.HKLM;
        }
        public string getValueName()
        {
            return key.Substring(key.LastIndexOf("\\") + 1);
        }
        public string getKeyName()
        {
            return key.Substring(5, key.LastIndexOf("\\") - 5);
        }
    }

    public class SelectorTweak
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public int IntValue { get; set; }
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
