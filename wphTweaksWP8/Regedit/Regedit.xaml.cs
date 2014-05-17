using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using HomebrewHelperWP;
using System.Windows.Input;

namespace wphTweaks.Regedit
{
    public partial class Regedit : PhoneApplicationPage
    {
        public static RegistryViewModel regVM = new RegistryViewModel();
        public Regedit()
        {
            InitializeComponent();
            this.DataContext = regVM;
            if (!regVM.IsDataLoaded)
            {
                regVM.LoadData();
            }
        }
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (regVM.CurrentRegistryKey != "")
            {
                e.Cancel = true;
                RegistryGoUpClick(null, null);
            }
        }


        private void GoToRegistryClick(object sender, EventArgs e)
        {
            var registry = regVM;

            //var result = InputBoxHelper.ShowInputBox(
            //    AppResources.MainpageGotoRegistryTitle, AppResources.MainpageGotoRegistry, registry.CurrentRegistryKey.FullPath);
            //if (result == null)
            {
                return;
            }

            // registry.CurrentRegistryKey = new RegistryKey(result);
        }



        private void GoToRegistryEditPage(RegistryEntryViewModel elem)
        {
            var value = elem.Value;
            regVM.CurrentValueEditing = value;
            var cmb = new CustomMessageBox()
            {
                Title = "Edit " + value.Name,
                Tag = elem,
                Message = "Type: " + value.Type.ToString(),
                IsLeftButtonEnabled = true,
                LeftButtonContent = "save",
                IsRightButtonEnabled = true,
                RightButtonContent = "cancel"
            };
            var sp = new StackPanel();
            sp.Children.Add(new TextBlock() { Text = "Value:" });
            switch (value.Type)
            {
                case RegistryType.String:
                    {
                        var valbox = new TextBox() { Text = Registry.ReadString(RegistryHive.HKLM, elem.FullPath, elem.ElementName), Name = "ValueBox" };
                        sp.Children.Add(valbox);
                    } break;
                case RegistryType.Integer:
                    {
                        var valbox = new TextBox()
                        {
                            Text = Registry.ReadDWORD(RegistryHive.HKLM, elem.FullPath, elem.ElementName).ToString(),
                            Name = "ValueBox",
                            InputScope = new InputScope()
                                {
                                    Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } }
                                }
                        };
                        sp.Children.Add(valbox);
                    } break;

            }
            cmb.Content = sp;
            cmb.Show();
            cmb.Dismissed += cmb_Dismissed;
        }

        void cmb_Dismissed(object sender, DismissedEventArgs e)
        {
            if (e.Result == CustomMessageBoxResult.LeftButton)
            {
                var cmb = sender as CustomMessageBox;
                var elem = cmb.Tag as RegistryEntryViewModel;
                var sp = cmb.Content as StackPanel;
                var textbox = (from child in sp.Children
                               where child is TextBox && ((TextBox)child).Name.Equals("ValueBox", StringComparison.OrdinalIgnoreCase)
                               select (TextBox)child).FirstOrDefault();
                if (textbox != null)
                {
                    switch (elem.Value.Type)
                    {
                        case RegistryType.String:
                            Registry.WriteString(RegistryHive.HKLM, elem.FullPath, elem.ElementName, textbox.Text);
                            break;
                        case RegistryType.Integer:
                            Registry.WriteDWORD(RegistryHive.HKLM, elem.FullPath, elem.ElementName, uint.Parse(textbox.Text));
                            break;
                    }
                }
            }
        }


        private void RegistryGoUpClick(object sender, EventArgs e)
        {
            var parent = "";
            if (regVM.CurrentRegistryKey.Contains("\\"))
            {
                parent = regVM.CurrentRegistryKey.Substring(0, regVM.CurrentRegistryKey.LastIndexOf(@"\"));
            }
            if (parent != null)
            {
                regVM.CurrentRegistryKey = parent;
            }
        }

        private void EditRegistryManualClick(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet");
            //this.GoToRegistryEditPage(regVM.CurrentRegistryKey + "\\");
        }

        private void RegistrySearchClick(object sender, EventArgs e)
        {
            regVM.IsFiltering = !regVM.IsFiltering;
        }

        private void Grid_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var panel = sender as FrameworkElement;
            var elem = panel.DataContext as RegistryEntryViewModel;
            if (elem.Element != null)
            {
                regVM.CurrentRegistryKey = elem.FullPath;
            }
            else
            {
                this.GoToRegistryEditPage(elem);
            }
        }

        private void Grid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //if (fix.ShouldIgnoreTab(sender, e)) { return; }
            var panel = sender as FrameworkElement;
            var elem = panel.DataContext as RegistryEntryViewModel;
            elem.IsSelected = !elem.IsSelected;
        }

        private void PathBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                regVM.CurrentRegistryKey = PathBox.Text;
                e.Handled = true;
            }
        }

        private void PathBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FocusManager.GetFocusedElement() != PathBox)
            {
                PathBox.Select(PathBox.Text.Length, 0);
            }
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void RenameClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void RefreshClick(object sender, EventArgs e)
        {
            regVM.RefreshRegistry();
        }

        private void NewKeyClick(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void NewValueClick(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }
    }
}