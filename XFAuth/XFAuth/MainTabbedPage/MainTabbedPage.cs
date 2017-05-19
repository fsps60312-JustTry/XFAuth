﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XFAuth.MainTabbed
{
    public partial class MainTabbedPage:TabbedPage
    {
        public MainTabbedPage()
        {
            this.Title = "Initializing...";
            NavigationPage.SetHasNavigationBar(this, false);
            this.Title = "Tabbed Page";
            this.ItemsSource = new TabPageProperties[] {
                    new TabPageProperties ("Explore", new TabPages.ExplorePage()),
                    new TabPageProperties ("Saved", new TabPages.SavedPage()),
                    new TabPageProperties ("Trips", new TabPages.TripsPage()),
                    new TabPageProperties ("Inbox", new TabPages.InboxPage()),
                    new TabPageProperties ("Profile", new TabPages.ProfilePage())
                };

            this.ItemTemplate = new DataTemplate(() =>
            {
                return new TabPage();
            });
            DoConstructionActions();
        }
        private async void DoConstructionActions()
        {
            if (AppData.AppData.userFacebookProfile == null)
            {
                await Navigation.PushModalAsync(new MainPage());
            }
        }

        // Data type:
        class TabPageProperties
        {
            public TabPageProperties(string name, View content)
            {
                this.Name = name;
                this.Content = content;
            }

            public string Name { private set; get; }

            public View Content { private set; get; }

            public override string ToString()
            {
                return Name;
            }
        }

        // Format page
        class TabPage : ContentPage
        {
            public TabPage()
            {
                // This binding is necessary to label the tabs in
                // the TabbedPage.
                this.SetBinding(ContentPage.TitleProperty, "Name");
                this.SetBinding(ContentPage.ContentProperty, "Content");
            }
        }

    }
}
