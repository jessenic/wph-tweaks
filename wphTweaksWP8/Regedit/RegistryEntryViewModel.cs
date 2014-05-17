namespace wphTweaks.Regedit
{
    using HomebrewHelperWP;
    using System.ComponentModel;
    using System.Windows.Media.Imaging;

    public class RegistryEntryViewModel : INotifyPropertyChanged
    {
        private readonly RegistryValueInfo regValue;
        
        private readonly string subKey;

        private bool isSelected;

        private string elementName;

        public RegistryEntryViewModel(string path, string subKey)
        {
            this.subKey = subKey;
            var index = subKey.LastIndexOf('\\');
            string simpleName;
            if (index < 0)
            {
                simpleName = subKey;
            }
            else
            {
                simpleName = subKey.Substring(index + 1);
            }
            this.elementName = simpleName;
            this.FullPath = path == "" ? subKey : (path + "\\" + subKey);
        }

        public RegistryEntryViewModel(string path, RegistryValueInfo regValue)
        {
            this.regValue = regValue;
            this.FullPath = path;
            this.elementName = regValue.Name;
        }

        public BitmapImage Icon
        {
            get
            {
                if (subKey != null)
                {
                    return IconStore.IconFolder;
                }
                else
                {
                    return IconStore.IconFile;
                }
            }
        }

        public string ElementName
        {
            get
            {
                return this.elementName;
            }
            set
            {
                this.elementName = value;
            }
        }

        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                this.isSelected = value;
                this.NotifyPropertyChanged("IsSelected");
            }
        }

        public string Element
        {
            get
            {
                return subKey;
            }
        }

        public RegistryValueInfo Value
        {
            get
            {
                return regValue;
            }
        }

        public string FullPath
        {
            get;
            //{
            //    if (subKey != null)
            //    {
            //        return subKey;
            //    }
            //    else
            //    {
            //        return regValue.Name;
            //    }
            //}
            private set;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}