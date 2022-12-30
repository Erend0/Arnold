using NEA.Data;
using Xamarin.Forms;
using System;

namespace NEA
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            var userRepo = new UserRepository();
            bool hasloggedin = userRepo.CheckLoggedInUser();
            // checks if there is a user logged in
            if (hasloggedin)
            {
                // if a user has logged in before, go to main page
                MainPage = new NavigationPage(new HomePage());
            }
            else
            {
                // if no user is logged in, go to login page
                MainPage = new RegisterPage();
            }
        } 
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
