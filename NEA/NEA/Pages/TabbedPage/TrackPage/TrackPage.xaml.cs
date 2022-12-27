using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NEA.Data;
using NEA.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NEA
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrackPage : ContentPage
    {
        public TrackPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            // logout user
            var userrepo = new UserRepository();
            userrepo.LogoutUser();
            Application.Current.MainPage = new LoginPage();

        }
        
    }

   
}