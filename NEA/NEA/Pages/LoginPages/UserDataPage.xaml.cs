using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NEA.Data;
using NEA.Tasks;
using System.Collections.Generic;
using System.Linq;
using NEA.Models;

namespace NEA.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    
    public partial class UserDataPage : ContentPage
    {
        private int UserID { get; set; }
        public UserDataPage()
        {
            InitializeComponent();
        }
        private void Submit_Pressed(object sender, EventArgs e)
        {
            if (Aim.SelectedIndex == -1)
            {
                DisplayAlert("Error", "Please select an aim", "OK");
            }
            else
            {
                int days = (int)Days.Value;
                // gets time stepper value (converted to seconds)
                int time = (int)Time.Value*60;
                // gets aim value
                string aim = Aim.SelectedItem.ToString();

                // The User's data is inserted into the database
                var userRepo = new UserRepository();
                UserID = userRepo.GetLoggedInUser().UserID;
                var userdataRepo = new UserDataRepository();
                userdataRepo.InsertUserData(UserID,time,days,aim);

                
                // The class which will generate the workout is instantiated
                // The "all" parameter means all of the days will be generated
                Workout workout = new Workout("all");
             
                
                
                // The current page is changed to the main tabbed page
                App.Current.MainPage = new NavigationPage(new HomePage());
            }
        }
        
    }
}