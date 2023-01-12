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
            AddContent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            // logout user
            var userrepo = new UserRepository();
            userrepo.LogoutUser();
            Application.Current.MainPage = new LoginPage();

        }
        private void AddContent()
        {
            var userdatarepo = new UserDataRepository();
            
            
        }

        private void Settings_Clicked(object sender, EventArgs e)
        {

        }

        private void RemoveLatest_Clicked(object sender, EventArgs e)
        {

        }

        private void AlterData_Clicked(object sender, EventArgs e)
        {

        }
    }

   
}