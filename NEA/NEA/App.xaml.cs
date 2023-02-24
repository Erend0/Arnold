using NEA.Data;
using NEA.Pages;
using NEA.Tasks;
using System;
using Xamarin.Forms;


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
                int UserID = userRepo.GetLoggedInUser().UserID;
                var resumerepo = new ResumeRepository();
                bool quitearly = resumerepo.HasQuitEarly(UserID);
                
                if (quitearly)
                {
                    MainPage = new UserDataPage();
                }
                else
                {
                    MainPage = new NavigationPage(new HomePage());
                }
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
