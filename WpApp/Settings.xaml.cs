using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace WpApp
{
    public sealed partial class Settings : ContentDialog
    {
        public string Username { get; set; }

        public Settings()
        {
            this.InitializeComponent();
            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values.Any(m => m.Key.Equals("username")))
                this.name.Text = Windows.Storage.ApplicationData.Current.LocalSettings.Values.Single(m => m.Key.Equals("username")).Value as string;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if(Windows.Storage.ApplicationData.Current.LocalSettings.Values.Any(m => m.Key.Equals("username")))
                Windows.Storage.ApplicationData.Current.LocalSettings.Values.Remove("username");
            Windows.Storage.ApplicationData.Current.LocalSettings.Values.Add(new KeyValuePair<string, object>("username", this.name.Text));
            this.Username = this.name.Text;

    }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
