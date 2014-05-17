namespace wphTweaks.Regedit
{
    using HomebrewHelperWP;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Threading;
    using System.Windows;

    public class RegistryViewModel : INotifyPropertyChanged
    {

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            if (App.Current != null && App.Current.RootVisual != null)
            {
                Dispatcher = App.Current.RootVisual.Dispatcher;
            }

            this.RefreshRegistry();

            this.IsDataLoaded = true;
        }
        public bool IsDataLoaded
        {
            get;
            private set;
        }
        private bool isWorking;

        public RegistryValueInfo CurrentValueEditing { get; set; }
        private string currentFilter;

        public string CurrentFilter
        {
            get
            {
                return this.currentFilter;
            }
            set
            {
                if (this.currentFilter != value)
                {
                    this.currentFilter = value;
                    this.NotifyPropertyChanged("CurrentFilter");
                }
            }
        }

        private bool isFiltering;

        public bool IsFiltering
        {
            get
            {
                return this.isFiltering;
            }
            set
            {
                var old = this.isFiltering;
                if (old != value)
                {
                    this.isFiltering = value;
                    if (!value)
                    {
                        CurrentFilter = null;
                    }

                    this.NotifyPropertyChanged("IsFiltering");
                }
            }
        }
        public RegistryViewModel()
        {
            RegistryEntries = new ObservableCollection<RegistryEntryViewModel>();
            CurrentRegistryKey = "";
            currentRegistryKeyName = "HKLM";
        }

        void mainViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentFilter")
            {
                this.RefreshRegistry();
            }
        }
        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<RegistryEntryViewModel> RegistryEntries { get; private set; }

        public string CurrentRegistryKeyName
        {
            get
            {
                return this.currentRegistryKeyName;
            }

            private set
            {
                this.currentRegistryKeyName = value;
                this.NotifyPropertyChanged("CurrentRegistryKeyName");
            }
        }

        public string CurrentRegistryKey
        {
            get
            {
                return this.currentRegistryKey;
            }

            set
            {
                currentRegistryKey = value;
                CurrentRegistryKeyName = string.IsNullOrEmpty(currentRegistryKey) ? "" : currentRegistryKey;

                try
                {
                    RefreshRegistry();
                }
                finally
                {
                    this.NotifyPropertyChanged("CurrentRegistryKey");
                }
            }
        }

        object workItem = new object();

        public void RefreshRegistry()
        {
            //if (!Registry.IsAvailable)
            //{
            //    RegistryEntries.Clear();
            //    RegistryEntries.Add(new RegistryEntryViewModel(new RegistryValue("\\" + AppResources.RegistryNotAvailable)));
            //    return;
            //}

            if (isWorking)
            {
                stopWorking = true;
            }

            try
            {
                lock (workItem)
                {
                    if (isWorking)
                    {
                        throw new InvalidOperationException("Should not happen!");
                    }


                    isWorking = true;
                    RegistryEntries.Clear();
                    var filter = CurrentFilter;
                    if (IsFiltering && !string.IsNullOrEmpty(filter))
                    {
                        // Searching
                        ThreadPool.QueueUserWorkItem(state => SearchRegistry(CurrentRegistryKey, filter));
                    }
                    else
                    {
                        try
                        {
                            foreach (var subKey in Registry.GetSubKeyNames(RegistryHive.HKLM,this.CurrentRegistryKey))
                            {
                                RegistryEntries.Add(new RegistryEntryViewModel(CurrentRegistryKey,subKey));
                            }
                            foreach (var valueName in Registry.GetValues(RegistryHive.HKLM, this.CurrentRegistryKey))
                            {
                                RegistryEntries.Add(new RegistryEntryViewModel(CurrentRegistryKey, valueName));
                            }
                        }
                        finally
                        {
                            isWorking = false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Ignoring Exception: {0}", e);
                // Ignore
            }
            finally
            {
                this.NotifyPropertyChanged("IsWorking");
            }
        }

        private void SearchRegistryPrivate(string registryKey, string filter)
        {
            try
            {
                foreach (var valueName in Registry.GetValues(RegistryHive.HKLM, registryKey))
                {
                    if (valueName.Name.Contains(filter))
                    {
                        RegistryValueInfo name = valueName;
                        Dispatcher.BeginInvoke(() => RegistryEntries.Add(new RegistryEntryViewModel(CurrentRegistryKey, name)));
                    }

                    if (stopWorking)
                    {
                        isWorking = false;
                        return;
                    }
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Debug.WriteLine("Ignoring UnauthorizedAccessException: {0}", e);
                // Ignore
            }
            
            try
            {
                foreach (var subKey in Registry.GetSubKeyNames(RegistryHive.HKLM,registryKey))
                {
                    if (subKey.Contains(filter))
                    {
                        Dispatcher.BeginInvoke(() => RegistryEntries.Add(new RegistryEntryViewModel(CurrentRegistryKey, subKey)));
                    }

                    this.SearchRegistryPrivate(subKey, filter);

                    if (stopWorking)
                    {
                        isWorking = false;
                        return;
                    }
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Debug.WriteLine("Ignoring UnauthorizedAccessException: {0}", e);
                // Ignore
            }
        }
        

        private void SearchRegistry(string registryKey, string filter)
        {
            lock (workItem)
            {
                isWorking = true;
                try
                {
                    this.SearchRegistryPrivate(registryKey, filter);
                }
                finally
                {
                    IsWorking = false;
                }
            }
        }

        private string currentRegistryKeyName;

        private string currentRegistryKey;

        private bool stopWorking;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsWorking
        {
            get
            {
                return isWorking;
            }
            set
            {
                isWorking = value;
                Dispatcher.BeginInvoke(() => this.NotifyPropertyChanged("IsWorking"));
            }
        }

        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public string Header
        {
            get
            {
                return "registry";
            }
        }

        public System.Windows.Threading.Dispatcher Dispatcher { get; set; }
    }
}