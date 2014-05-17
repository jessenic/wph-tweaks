namespace wphTweaks.Regedit
{
    using HomebrewHelperWP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

    public class RegistryEntryEditViewModel : INotifyPropertyChanged
    {
        private static RegistryType[] values = new RegistryType[]
            {
                RegistryType.Binary, RegistryType.String, RegistryType.Integer, RegistryType.MultiString
            };

        public RegistryEntryEditViewModel(RegistryValueInfo currentValueEditing)
        {
            if (currentValueEditing.Name != "")
            {
                ValueName = currentValueEditing.Name;
                //string fullPath = currentValueEditing.FullPath;
                KeyPath = currentValueEditing.Name; // fullPath.Substring(0, fullPath.Length - ValueName.Length);

                try
                {
                    try
                    {
                        //Value = currentValueEditing.Value.ToString();
                    }
                    catch (NotSupportedException)
                    {
                        // This key-type is not supported 
                    }
                    ValueType = currentValueEditing.Type;
                }
                catch (KeyNotFoundException)
                {
                    // Key not found
                }
                catch (UnauthorizedAccessException)
                {
                    // no access
                }
                catch (ArgumentException e)
                {
                    // On root notes...
                }
            }
        }

        private string keyPath;

        public string KeyPath
        {
            get
            {
                return this.keyPath;
            }
            set
            {
                this.keyPath = value;
                this.NotifyPropertyChanged("KeyPath");
            }
        }

        private string valueName;

        public IEnumerable<RegistryType> Values
        {
            get
            {
                return values;
            }
        }

        public string ValueName
        {
            get
            {
                return this.valueName;
            }
            set
            {
                this.valueName = value;
                this.NotifyPropertyChanged("ValueName");
            }
        }

        private RegistryType valueType;

        public RegistryType ValueType
        {
            get
            {
                return this.valueType;
            }
            set
            {
                this.valueType = value;
                this.NotifyPropertyChanged("ValueType");
            }
        }

        private string value;

        public string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
                this.NotifyPropertyChanged("Value");
            }
        }

        public object ValueObject
        {
            set
            {
                var b = value as byte[];
                if (b != null)
                {
                    Value = Convert.ToBase64String(b);
                    return;
                }

                var ms = value as string[];
                if (ms != null)
                {
                    Value = string.Join("\n", ms);
                    return;
                }

                Value = value.ToString();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}