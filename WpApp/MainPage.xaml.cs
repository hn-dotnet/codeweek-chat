using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WpApp.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace WpApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DispatcherTimer timer = new DispatcherTimer();
        public string Username { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {

            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values.Any(m => m.Key.Equals("username")))
                this.Username = Windows.Storage.ApplicationData.Current.LocalSettings.Values.Single(m => m.Key.Equals("username")).Value as string;
            else
            {
                var dialog = new Settings();
                await dialog.ShowAsync();
                this.Username = dialog.Username;
            }

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += Timer_Tick;

            timer.Start();

            this.lbMessages.ItemsSource = await LoadMessages();

        }

        private async void Timer_Tick(object sender, object e)
        {

            var task = await LoadMessages();

            this.lbMessages.ItemsSource = null;
            this.lbMessages.Items.Clear();
            this.lbMessages.ItemsSource = task.OrderBy(m=>m.Date);

            lbMessages.ScrollIntoView(task.OrderBy(m => m.Date).Last());
        }




        private async Task<List<MessageModel>> LoadMessages()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://hn-dn-cw-chat.azurewebsites.net/api/values");

                // New code:
                HttpResponseMessage response = await client.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<MessageModel>>( await response.Content.ReadAsStringAsync());
                }

                throw new Exception("Fehler beim laden der Nachrichten");
            }
        }

        private async void SendMessage(MessageModel msg)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://hn-dn-cw-chat.azurewebsites.net/api/values");

                var msgStr = JsonConvert.SerializeObject(msg);
                requestMessage.Content = new StringContent(msgStr);
                requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");

                // New code:
                HttpResponseMessage response = await client.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                }
            }
        }

        private void btnSend_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbMessage.Text))
                return;

            SendMessage(new MessageModel()
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                Message = tbMessage.Text,
                Sender = this.Username
            });

            tbMessage.Text = string.Empty;
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Settings();
            await dialog.ShowAsync();
            if(!string.IsNullOrEmpty(dialog.Username))
                this.Username = dialog.Username;
        }
    }
}