using Camera.Model;
using Imgur.API;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace Camera
{
    public partial class MainPage : ContentPage
    {
        private MediaFile file;
        public static string CLIENT_ID = "d8064c8c67c32c2";
        public static string CLIENT_SECRET = "fb668ff35c0bf5e4325414ea92cfe7fd966d4fc6";

        //file=null;
        public MainPage()
        {
            InitializeComponent();
            Application.Current.Properties["TOKEN_ACCESS"] = "c50d2bd20d0c4a21b9beaeadd219626037eb21fe";
            Application.Current.Properties["REFRESH_TOKEN"] = "9c9f52fa2e9595a1f1f8d14b92615b4960b958f8";
            Application.Current.Properties["TOKEN_TYPE"] = "bearer";
            Application.Current.Properties["ACCOUNT_ID"] = "68145441";
            Application.Current.Properties["IMGUR_USER_ACCOUNT"] = "nayemasif";
            Application.Current.Properties["EXPIRES_IN"] = "315360000";



        }

        private async void TakePhotoAction(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            PermissionStatus cameraStatus = await Plugin.Permissions.CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);


            if (cameraStatus != PermissionStatus.Granted)
            {
                var permission = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                var status = permission[Permission.Camera];
            }


            PermissionStatus strogeStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);


            if (strogeStatus != PermissionStatus.Granted)
            {
                var permission = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                var status = permission[Permission.Storage];
            }


            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
                //Location=""


            });

            //await DisplayAlert("Path", file.Path, "ok");

            if (file == null)
                return;

            myImage.Source = ImageSource.FromStream(() => file.GetStream());
            myImage.IsVisible = true;

        }

        private async void ChoosePhotoAction(object sender, EventArgs e)
        {

            await CrossMedia.Current.Initialize();

            PermissionStatus strogeStatus = await Plugin.Permissions.CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);


            if (strogeStatus != PermissionStatus.Granted)
            {
                var permission = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                var status = permission[Permission.Storage];
                if (status == PermissionStatus.Denied)
                {
                    await DisplayAlert("File Access Error", "Need File Access", "Ok");
                    return;
                }
            }



            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                return;
            }

            file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions { });

            if (file == null) return;

            myImage.Source = ImageSource.FromStream(() => file.GetStream());
            myImage.IsVisible = true;

        }




        private async void UploadButtonAction(object sender, EventArgs e)
        {
            if (file == null)
            {
                await DisplayAlert("NO File", "No File Selected", "ok");
                return;
            }
            bool ans = await DisplayAlert("Upload", "Are you really want to upload?", "Yes", "No");

            if (ans)
            {

                if (CheckConnection.isConnected)
                {

                    activityIndicator.IsRunning = true;
                    activityIndicator.IsEnabled = true;

                    //await DisplayAlert("NETWORK", "Internet ok", "ok");
                    status.Text = "";

                    myImage.IsVisible = false;


                    try
                    {

                        var token = TokenSingleton.GetToken();

                        //await DisplayAlert("Expaire At", token.ExpiresAt + "", "OK");

                        var client = new ImgurClient(CLIENT_ID, CLIENT_SECRET, token);
                        //await DisplayAlert("Client Id", client.ClientId, "ok");


                        //await DisplayAlert("Client Id", CLIENT_ID, "ok");
                        //await DisplayAlert("Client Sec", CLIENT_SECRET, "ok");
                        //await DisplayAlert("Token",token.ExpiresAt+"","ok");

                        var endpoint = new ImageEndpoint(client);



                        var response = await endpoint.UploadImageStreamAsync(file.GetStream());
                        activityIndicator.IsEnabled = false;
                        activityIndicator.IsRunning = false;

                        bool isOpen = await DisplayAlert("Upload Successful", "Want To See The Image?", "YES", "NO");
                        if (isOpen)
                        {
                            await Browser.OpenAsync(response.Link, BrowserLaunchMode.External);
                        }


                    }
                    catch (ImgurException ex)
                    {
                        Debug.Write("Error uploading the image to Imgur");
                        activityIndicator.IsEnabled = false;
                        activityIndicator.IsRunning = false;
                        await DisplayAlert("Error", "Error uploading the image to Imgur", "ok");
                        Debug.Write(ex.Message);


                    }
                }
                else
                {
                    status.Text = "No Internet";
                    status.TextColor = Color.Red;
                    //await DisplayAlert("NETWORK", "NO Internet", "ok");

                }
            }

            else
            {
                file = null;

                return;
            }
        }


        private void ScanButtonAction(object sender, EventArgs e)
        {
            myImage.IsVisible = false;
            file = null;
            var scannerPage = new ZXingScannerPage();
            Navigation.PushAsync(scannerPage);
            scannerPage.OnScanResult += result =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {

                    await Navigation.PopAsync();

                    status.Text = result.Text;
                });
            };
        }


    }
}

//https://api.imgur.com/oauth2/authorize?client_id=YOUR_CLIENT_ID&response_type=REQUESTED_RESPONSE_TYPE&state=APPLICATION_STATE
