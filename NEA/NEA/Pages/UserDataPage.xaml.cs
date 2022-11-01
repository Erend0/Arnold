using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using NEA.Data;
using NEA.Models;
using System.Runtime.CompilerServices;
using NEA.Pages;


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
            // gets days slider value
            int days = (int)Days.Value;
            // gets time slider value 
            int time = (int)Time.Value;
            // gets aim value
            string aim = Aim.SelectedItem.ToString();

            if (Aim.SelectedIndex == -1)
            {
                DisplayAlert("Error", "Please select an aim", "OK");
            }
            else
            {
                // gets user id of current logged in user 
                var userrepo = new UserRepository();
                int CurrentUserID = userrepo.GetLoggedInUserId();
                // inserts new user into the userdata table
                var userdataRepo = new UserDataRepository();
                userdataRepo.InsertUserData(CurrentUserID, days, time, aim);
                // changes the current mainpage to HomePage.xaml



                // creates workout -----------------------------------------
                
                App.Current.MainPage = new NavigationPage(new HomePage());
            }
        }
      
    }
}