using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using NEA.Data;
using NEA.Models;
using System.Runtime.CompilerServices;
using NEA.Pages;
using NEA.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace NEA.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserDataPage : ContentPage
    {
        public UserDataPage()
        {
            InitializeComponent();
        }
        private void Button_Pressed(object sender, EventArgs e)
        {
            if (Aim.SelectedIndex == -1)
            {
                DisplayAlert("Error", "Please select an aim", "OK");
            }
            else
            {
                // gets days slider value
                int days = (int)Days.Value;
                // gets time slider value 
                int time = (int)Time.Value;
                // gets aim value
                string aim = Aim.SelectedItem.ToString();

                // inserts user data into userdata repositoru 
                var userrepo = new UserRepository();
                int userID = userrepo.GetLoggedInUser().UserID;
                
                var userdataRepo = new UserDataRepository();
                userdataRepo.InsertUserData(userID,time,days,aim);

                // The calls which will generate the workout is called
                // The all variable means all of the days will be generated 
                Workout workout = new Workout("all");
                App.Current.MainPage = new NavigationPage(new HomePage());


            }
        }
   
      
    }
}