
using Imgur.API;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models.Impl;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Camera
{
    public partial class MainPage : ContentPage
    {
        private MediaFile file;

        //file=null;
        public MainPage()
        {
            InitializeComponent();
            Application.Current.Properties["TOKEN_ACCESS"] = "fae99ecaadc48c63f4211d74e1d184120cc34247";
            Application.Current.Properties["REFRESH_TOKEN"] = "55c6e47daa5f6171fd4c48eb138aac11182d1e04";
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

        }

        private async void ChoosePhotoAction(object sender, EventArgs e)
        {

            await CrossMedia.Current.Initialize();

            PermissionStatus strogeStatus = await Plugin.Permissions.CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);


            if (strogeStatus != PermissionStatus.Granted)
            {
                var permission = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                var status = permission[Permission.Storage];
                if (status== PermissionStatus.Denied)
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

        }

        public OAuth2Token CreateToken()
        {
            var TOKEN_ACCESS = Application.Current.Properties["TOKEN_ACCESS"].ToString();
            var REFRESH_TOKEN = Application.Current.Properties["REFRESH_TOKEN"].ToString();
            var TOKEN_TYPE = Application.Current.Properties["TOKEN_TYPE"].ToString();

            var ACCOUNT_ID = Application.Current.Properties["ACCOUNT_ID"].ToString();
            var IMGUR_USER_ACCOUNT = Application.Current.Properties["IMGUR_USER_ACCOUNT"].ToString();

            var EXPIRES_IN = Application.Current.Properties["EXPIRES_IN"].ToString();

            var token = new OAuth2Token(TOKEN_ACCESS, REFRESH_TOKEN, TOKEN_TYPE, ACCOUNT_ID, IMGUR_USER_ACCOUNT, int.Parse(EXPIRES_IN));
            //DisplayAlert("");
            return token;
        }

        
        //public Task<IOAuth2Token> RefreshToken()
        //{
        //    var client = new ImgurClient(CLIENT_ID, CLIENT_SECRET);
        //    var endpoint = new OAuth2Endpoint(client);
        //    var token = endpoint.GetTokenByRefreshTokenAsync(REFRESH_TOKEN);
        //    return token;
        //}

        private async void UploadButtonAction(object sender, EventArgs e)
        {
            if (file == null)
            {
                return;
            }
            bool ans = await DisplayAlert("Upload", "Are you really want to upload?", "Yes", "No");
     
            if (ans) { 

                    var CLIENT_ID = "d8064c8c67c32c2";
                    var CLIENT_SECRET = "fb668ff35c0bf5e4325414ea92cfe7fd966d4fc6";
                    try
                    {
                        var client = new ImgurClient(CLIENT_ID, CLIENT_SECRET, CreateToken());
                        var endpoint = new ImageEndpoint(client);



                        var response = await endpoint.UploadImageStreamAsync(file.GetStream());
                      bool isOpen= await DisplayAlert("Upload Success", "Want To See The Image?", "YES","NO");
                    if (isOpen)
                    {
                       await Browser.OpenAsync(response.Link, BrowserLaunchMode.External);
                    }


                    }
                    catch (ImgurException ex)
                    {
                        Debug.Write("Error uploading the image to Imgur");
                        Debug.Write(ex.Message);
                    }

                }
          else  return;
        }

    }
}

//https://api.imgur.com/oauth2/authorize?client_id=YOUR_CLIENT_ID&response_type=REQUESTED_RESPONSE_TYPE&state=APPLICATION_STATE
