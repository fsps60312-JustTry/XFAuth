using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XFAuth.UserInfo
{
    class ReportUserPage:ContentPage
    {
        public ReportUserPage()
        {
            this.Title = "Report User";
            DoConstructionActions();
        }
        private async void DoConstructionActions()
        {
            await DisplayAlert("User reported", "", "OK");
            await Navigation.PopAsync();
        }
    }
}
