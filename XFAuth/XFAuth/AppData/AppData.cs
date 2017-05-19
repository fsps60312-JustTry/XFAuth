using System;
using System.Collections.Generic;
using System.Text;
using Plugin.DeviceInfo;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;

namespace XFAuth.AppData
{
    static class AppData
    {
        public enum DataType { FacebookId, CarInfo };
        //public static string tmp;
        public static string AppId { get { return "629463373922090"; } }
        public static string AccountKey { get { return "D01CYiIDETKq5s1CfnCEhuTymPqLT12h7V/wdFKr3fc+fY7DvhRejJsStn5+egQg+QYXZRMGENEQlWZ4IZU9yw=="; } }
        public static string AccountName { get { return "xfauth"; } }
        public static string deviceId { get { return CrossDeviceInfo.Current.Id.ToLower(); } }
        public static FacebookProfile userFacebookProfile;
        public static List<CarInfo> cars = new List<CarInfo>();
        public static async Task Upload(DataType dataType)
        {
            string s = null;
            switch (dataType)
            {
                case DataType.FacebookId:
                    s = JsonConvert.SerializeObject(userFacebookProfile);
                    break;
                case DataType.CarInfo:
                    s = JsonConvert.SerializeObject(cars);
                    break;
                default:
                    string msg = $"Upload DataType \"{dataType}\" haven't been implemented!";
                    Debug.WriteLine(msg);
                    await App.Current.MainPage.DisplayAlert("Error", msg, "OK");
                    return;
            }
            Trace.Assert(s != null);
            var blockBlob = await GetBlockBlobAsync(dataType.ToString().ToLower());
                //container.GetBlockBlobReference(dataType.ToString().ToLower());
            await blockBlob.UploadTextAsync(s);
        }
        public static async Task Download(DataType dataType)
        {
            var blockBlob = await GetBlockBlobAsync(dataType.ToString().ToLower());
            if (!await blockBlob.ExistsAsync())
            {
                await App.Current.MainPage.DisplayAlert("", $"You haven't save any \"{dataType}\" data on cloud!", "OK");
                return;
            }
            string s = await blockBlob.DownloadTextAsync();
            switch (dataType)
            {
                case DataType.FacebookId:
                    userFacebookProfile = JsonConvert.DeserializeObject<FacebookProfile>(s);
                    break;
                case DataType.CarInfo:
                    cars = JsonConvert.DeserializeObject<List<CarInfo>>(s);
                    break;
                default:
                    string msg = $"Download DataType \"{dataType}\" haven't been implemented!";
                    Debug.WriteLine(msg);
                    await App.Current.MainPage.DisplayAlert("Error", msg, "OK");
                    return;
            }
        }
        private static CloudBlobContainer container
        {
            get
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    "DefaultEndpointsProtocol=https;" +
                    $"AccountName={AppData.AccountName};" +
                    $"AccountKey={AppData.AccountKey};" +
                    "EndpointSuffix=core.windows.net"
                );
                var client = storageAccount.CreateCloudBlobClient();
                return client.GetContainerReference(deviceId);
            }
        }
        public static async Task<CloudBlockBlob> GetBlockBlobAsync(string name)
        {
            var container = AppData.container;
            await container.CreateIfNotExistsAsync();
            return container.GetBlockBlobReference(name);
        }
    }
}