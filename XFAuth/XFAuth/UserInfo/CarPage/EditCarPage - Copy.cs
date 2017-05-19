using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Media;
using Xamarin.Forms;

namespace XFAuth.UserInfo.CarPage
{
    class EditCarPage:ContentPage
    {
        //taking and picking pictures: https://channel9.msdn.com/Blogs/MVP-VisualStudio-Dev/XamarinForms-taking-pictures-from-the-camera-and-from-disk-using-the-Media-plugin
        Grid GDmain;
        Button BTNtakePhoto, BTNselectPhoto;
        Image IMGmain;
        public EditCarPage()
        {
            {
                GDmain = new Grid();
                GDmain.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                GDmain.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                GDmain.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                {
                    BTNtakePhoto = new Button();
                    BTNtakePhoto.Clicked += BTNtakePhoto_Clicked;
                    GDmain.Children.Add(BTNtakePhoto, 0, 0);
                }
                {
                    BTNselectPhoto = new Button();
                    BTNselectPhoto.Clicked += BTNselectPhoto_Clicked;
                    GDmain.Children.Add(BTNselectPhoto, 0, 1);
                }
                {
                    IMGmain = new Image();
                    GDmain.Children.Add(IMGmain, 0, 2);
                }
                this.Content = GDmain;
            }
        }

        private async void BTNselectPhoto_Clicked(object sender, EventArgs e)
        {
            if(!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Oops", "It seems your device doesn't support for picking a photo", "OK");
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync();
            if(file==null)
            {
                await DisplayAlert("Oops", "file is null", "OK");
            }
            else
            {
                IMGmain.Source = ImageSource.FromStream(() => file.GetStream());
                await DisplayAlert("", "Done", "OK");
            }
        }

        private async void BTNtakePhoto_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable)
            {
                await DisplayAlert("Well...", "I can't find any camera available on this device", "OK");
                return;
            }
            if (!CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Oops", "It seems your camera doesn't support for taking a photo", "OK");
                return;
            }
            var fileName = $"{DateTime.Now.Ticks}.jgp";
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory="XFAuth",
                SaveToAlbum = true,
                Name = fileName
            });
            if (file == null)
            {
                await DisplayAlert("Oops", "file is null", "OK");
            }
            else
            {
                IMGmain.Source = ImageSource.FromStream(() => file.GetStream());
                await DisplayAlert("Photo took", $"Saved as {fileName}", "OK");
            }
        }
    }
}
