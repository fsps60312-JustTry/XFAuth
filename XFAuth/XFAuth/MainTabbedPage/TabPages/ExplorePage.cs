﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;


namespace XFAuth.MainTabbed.TabPages
{
    class ExplorePage:RelativeLayout
    {
        Map MAP;
        Grid GDmain;
        Button BTNfilter,BTNbroadcast;
        public ExplorePage()
        {
            InitializeViews();
            RegisterEvents();
        }
        async Task PushNotification()
        {
            switch (Device.RuntimePlatform)
            {
                case "Android":
                    {
                        await Application.Current.MainPage.DisplayAlert("", $"Working on pushing notification on {Device.RuntimePlatform}", "OK");
                    }
                    break;
                default:
                    {
                        await Application.Current.MainPage.DisplayAlert("", $"Push notification on {Device.RuntimePlatform} is currently not supported!", "OK");
                    }
                    break;
            }
        }
        void RegisterEvents()
        {
            BTNbroadcast.Clicked += async delegate
            {
                if (await Application.Current.MainPage.DisplayAlert("確認", "向周圍的車主發送通知？", "OK", "Cancel"))
                {
                    await PushNotification();
                }
            };
        }
        void InitializeViews()
        {
            {
                MAP = new Map(new MapSpan(new Position(25.0195948, 121.5418243), 0.01, 0.01));
                this.Children.Add(MAP,
                    Constraint.RelativeToParent((parent) => { return 0; }),
                    Constraint.RelativeToParent((parent) => { return 0; }),
                    Constraint.RelativeToParent((parent) => { return parent.Width; }),
                    Constraint.RelativeToParent((parent) => { return parent.Height; }));
            }
            {
                GDmain = new Grid();
                GDmain.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.0, GridUnitType.Star) });
                GDmain.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.0, GridUnitType.Auto) });
                GDmain.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.0, GridUnitType.Auto) });
                GDmain.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.0, GridUnitType.Star) });
                GDmain.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10.0, GridUnitType.Star) });
                GDmain.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1.0, GridUnitType.Auto) });
                GDmain.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1.0, GridUnitType.Star) });
                {
                    BTNfilter = new Button();
                    BTNfilter.Text = "Filter";
                    BTNfilter.Opacity = 0.5;
                    GDmain.Children.Add(BTNfilter, 1, 1);
                }
                {
                    BTNbroadcast = new Button();
                    BTNbroadcast.Text = "Broadcast";
                    BTNbroadcast.Opacity = 0.5;
                    GDmain.Children.Add(BTNbroadcast, 2, 1);
                }
                this.Children.Add(GDmain,
                    Constraint.RelativeToParent((parent) => { return 0; }),
                    Constraint.RelativeToParent((parent) => { return 0; }),
                    Constraint.RelativeToParent((parent) => { return parent.Width; }),
                    Constraint.RelativeToParent((parent) => { return parent.Height; }));
            }
        }
    }
}
