using NEA.Data;
using NEA.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NEA
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            
                var userRepo = new UserRepository();
                var loggedInUser = userRepo.GetLoggedInUser();
                if (loggedInUser.Count > 0)
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
