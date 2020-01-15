using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Camera
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InternetConnectionErrorPage : ContentPage
	{
		public InternetConnectionErrorPage ()
		{
			InitializeComponent ();



            image.Source = ImageSource.FromResource("Camera.Image.error.png");
            image.Aspect = Aspect.AspectFill;
        }
    }
}