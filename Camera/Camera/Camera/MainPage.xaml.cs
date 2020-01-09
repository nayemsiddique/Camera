using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Camera
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
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


            PermissionStatus strogeStatus = await Plugin.Permissions.CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);


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

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
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
                if (status == null)
                {
                    await DisplayAlert("File Access Error", "Need File Access", "Ok");
                    return;
                }
            }



            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions { });

            if (file == null) return;

            myImage.Source = ImageSource.FromStream(() => file.GetStream());

        }

    }
}
