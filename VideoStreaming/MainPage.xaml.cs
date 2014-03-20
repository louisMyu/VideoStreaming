using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using VideoStreaming.Resources;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.IO;
using System.Text;

namespace VideoStreaming
{
    public partial class MainPage : PhoneApplicationPage
    {
        List<string> urlList = new List<string>();
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }
        private void GetStream()
        {
            WebRequest request = WebRequest.Create("https://api.twitch.tv/kraken/streams?game=Magic:+The+Gathering");
            request.BeginGetResponse(GetResponse, request);
        }

        private void GetResponse(IAsyncResult result)
        {
            String responseString = "";
            WebRequest request = (WebRequest)result.AsyncState;
            try
            {
                WebResponse response = request.EndGetResponse(result);
                using (System.IO.Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    responseString = reader.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                Console.WriteLine(e.ToString());
            }
            JObject json = JObject.Parse(responseString);
            JArray array = (JArray)json["streams"];
            for (int i = 0; i < array.Count; i++)
            {
                string temp = (String)array[i]["channel"]["url"];
                urlList.Add(temp);
            }
        }

        private void Button_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            GetStream();
        }
        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}