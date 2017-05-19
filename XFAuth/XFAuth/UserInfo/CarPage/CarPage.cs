﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.WindowsAzure.Storage;

namespace XFAuth.UserInfo.CarPage
{
    //class CarInfo
    //{
    //    public static List<CarInfo> Cars = new List<CarInfo>();
    //}
    class CarPage:ContentPage
    {
        StackLayout SLmain,SLcars;
        ActivityIndicator AImain;
        ToolbarItem TIadd, TIupdate;
        public CarPage()
        {
            InitializeViews();
            RegisterEvents();
            DoConstructionActions();
        }
        private void UpdateViews()
        {
            SLcars.Children.Clear();
            for(int carIndex=0;carIndex<AppData.AppData.cars.Count;carIndex++)
            {
                var car = AppData.AppData.cars[carIndex];
                int carIndexNow = carIndex;
                var btn = new Button();
                btn.Text = car.name;
                btn.Clicked += async delegate (object sender, EventArgs e)
                {
                    (sender as Button).IsEnabled = false;
                    await Navigation.PushAsync(new EditCarPage(carIndexNow));
                    (sender as Button).IsEnabled = true;
                };
                SLcars.Children.Add(btn);
            }
        }
        private async Task DownloadDataAsync()
        {
            AImain.IsRunning = AImain.IsVisible = true;
            await AppData.AppData.Download(AppData.AppData.DataType.CarInfo);
            AImain.IsRunning = AImain.IsVisible = false;
        }
        private async void DoConstructionActions()
        {
            await DownloadDataAsync();
            UpdateViews();
            this.Appearing += delegate
            {
                UpdateViews();
            };
        }
        private void RegisterEvents()
        {
            TIadd.Clicked += async delegate
              {
                  await Navigation.PushAsync(new EditCarPage());
              };
            TIupdate.Clicked += async delegate
              {
                  await DownloadDataAsync();
                  UpdateViews();
              };
        }
        private void InitializeViews()
        {
            {
                TIadd = new ToolbarItem();
                TIadd.Text = "Add";
                this.ToolbarItems.Add(TIadd);
            }
            {
                TIupdate = new ToolbarItem();
                TIupdate.Text = "Update";
                this.ToolbarItems.Add(TIupdate);
            }
            {
                SLmain = new StackLayout();
                {
                    AImain = new ActivityIndicator();
                    AImain.IsVisible = false;
                    SLmain.Children.Add(AImain);
                }
                {
                    SLcars = new StackLayout();
                    SLmain.Children.Add(SLcars);
                }
                this.Content = SLmain;
            }
        }
    }
}
