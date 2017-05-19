﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;
using Xamarin.Forms;
using System.Diagnostics;

namespace XFAuth.UserInfo.CarPage
{
    class EditCarPage:ContentPage
    {
        //taking and picking pictures: https://channel9.msdn.com/Blogs/MVP-VisualStudio-Dev/XamarinForms-taking-pictures-from-the-camera-and-from-disk-using-the-Media-plugin
        ScrollView SVmain;
        StackLayout SLmain;
        ActivityIndicator AImain;
        Image IMGcar;
        TableView TVmain;
        EntryCell ECname,ECage;
        ToolbarItem TIdone;
        ulong carPhotoId;
        int carIndex;
        private async Task<bool> AddCarAndExit()
        {
            int age;
            if(!int.TryParse(ECage.Text,out age))
            {
                await DisplayAlert("", $"欄位\"{ECage.Label}\"的格式不正確", "OK");
                return false;
            }
            var carInfo = new AppData.CarInfo
            {
                pictureId = carPhotoId,
                name = ECname.Text,
                type = AppData.CarInfo.CarType.Car,
                age = age
            };
            if (carIndex == -1)// add
            {
                AppData.AppData.cars.Add(carInfo);
            }
            else// edit
            {
                AppData.AppData.cars.RemoveAt(carIndex);
                AppData.AppData.cars.Insert(carIndex, carInfo);
            }
            await AppData.AppData.Upload(AppData.AppData.DataType.CarInfo);
            return true;
        }
        public EditCarPage(int carIndex=-1)
        {
            this.carIndex = carIndex;
            InitializeViews();
            RegisterEvents();
            LoadData();
        }
        private async Task GetAPhotoForCar()
        {
            var file = await GetAPhotoAsync();
            if (file == null)
            {
                await DisplayAlert("", "Canceled", "OK");
                return;
            }
            else
            {
                IMGcar.IsVisible = false;
                AImain.IsRunning = AImain.IsVisible = true;
                carPhotoId = await AppData.PhotoManager.UploadPhotoAsync(file.GetStream());
                await UpdateCarPhotoAsync();
                AImain.IsRunning = AImain.IsVisible = false;
                IMGcar.IsVisible = true;
            }
        }
        private async void LoadData() { await LoadDataAsync(); }
        private async Task LoadDataAsync()
        {
            if (carIndex == -1) return;
            var carInfo = AppData.AppData.cars[carIndex];
            carPhotoId = carInfo.pictureId;
            ECname.Text = carInfo.name;
            ECage.Text = $"{carInfo.age}";
            await UpdateCarPhotoAsync();
        }
        private async Task UpdateCarPhotoAsync()
        {
            var photoStream = await AppData.PhotoManager.DownloadPhotoAsync(carPhotoId);
            IMGcar.Source = ImageSource.FromStream(() => photoStream);
        }
        private void RegisterEvents()
        {
            TIdone.Clicked +=async delegate (object sender, EventArgs e)
            {
                AImain.IsRunning = AImain.IsEnabled = true;
                if (await AddCarAndExit())
                {
                    await Navigation.PopAsync();
                }
                AImain.IsRunning = AImain.IsEnabled = false;
            };
            IMGcar.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    IMGcar.Opacity = 0.6;
                    await IMGcar.FadeTo(1);
                    await GetAPhotoForCar();
                }),
                NumberOfTapsRequired = 1
            });
        }
        private void InitializeViews()
        {
            {
                TIdone = new ToolbarItem();
                TIdone.Text = "Done";
                this.ToolbarItems.Add(TIdone);
            }
            {
                SVmain = new ScrollView();
                {
                    SLmain = new StackLayout();
                    {
                        AImain = new ActivityIndicator();
                        AImain.IsRunning = AImain.IsVisible = false;
                        SLmain.Children.Add(AImain);
                    }
                    {
                        IMGcar = new Image();
                        IMGcar.Source = "Icon.png";
                        SLmain.Children.Add(IMGcar);
                    }
                    {
                        TVmain = new TableView(new TableRoot());
                        TVmain.Root.Add(new TableSection("Info"));
                        {
                            ECname = new EntryCell();
                            ECname.Label = "Name";
                            TVmain.Root[0].Add(ECname);
                        }
                        {
                            ECage = new EntryCell();
                            ECage.Label = "車齡(年)";
                            TVmain.Root[0].Add(ECage);
                        }
                        SLmain.Children.Add(TVmain);
                    }
                    SVmain.Content = SLmain;
                }
                this.Content = SVmain;
            }
        }
        private async Task<Plugin.Media.Abstractions.MediaFile> GetAPhotoAsync()
        {
            var result = await DisplayActionSheet("", "Cancel", null, new string[] { "Take a photo", "Choose a photo" });
            switch (result)
            {
                case "Take a photo":
                    {
                        await CrossMedia.Current.Initialize();
                        if (!CrossMedia.Current.IsCameraAvailable)
                        {
                            await DisplayAlert("Well...", "I can't find any camera available on this device", "OK");
                            return null;
                        }
                        if (!CrossMedia.Current.IsTakePhotoSupported)
                        {
                            await DisplayAlert("Oops", "It seems your camera doesn't support for taking a photo", "OK");
                            return null;
                        }
                        return await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                        {
                            SaveToAlbum = false
                        });
                    }
                case "Choose a photo":
                    {
                        if (!CrossMedia.Current.IsPickPhotoSupported)
                        {
                            await DisplayAlert("Oops", "It seems your device doesn't support for picking a photo", "OK");
                            return null;
                        }
                        return await CrossMedia.Current.PickPhotoAsync();
                    }
                case "Cancel":
                case null:
                    return null;
                default:
                    {
                        await DisplayAlert("Unknown option", $"{result}", "OK");
                        return null;
                    }
            }
        }
    }
}
