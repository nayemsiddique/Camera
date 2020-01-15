using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Camera
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();



            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            //Application.Current.Properties["TOKEN_ACCESS"] = "fae99ecaadc48c63f4211d74e1d184120cc34247";
            //Application.Current.Properties["REFRESH_TOKEN"] = "55c6e47daa5f6171fd4c48eb138aac11182d1e04";
            //Application.Current.Properties["TOKEN_TYPE"] = "bearer";
            //Application.Current.Properties["ACCOUNT_ID"] = "68145441";
            //Application.Current.Properties["IMGUR_USER_ACCOUNT"] = "nayemasif";
            //Application.Current.Properties["EXPIRES_IN"] = "315360000";
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
