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
            
            
            
            // check if app is running for the first time 
            if (Properties.ContainsKey("FirstTime"))
            {
                
                // check if any user is logged in database
                var userRepo = new UserRepository();
                
                var loggedInUser = userRepo.GetLoggedInUser();
                if (loggedInUser.Count > 0)
                {
                 // if user is logged in, go to main page
                 MainPage = new NavigationPage(new HomePage());
                   
                }
                else
                {
                    // if no user is logged in, go to login page
                    MainPage = new RegisterPage();
                }
            }
            else
            {

                // if the app is not running for the first time 
                // check if any user has logged in before 
                var userRepo = new UserRepository();
                var loggedInUser = userRepo.GetLoggedInUser();
                if (loggedInUser.Count > 0)
                {
                    
                    
                    
                   
                    
                    
                    // if user is logged in, go to main page
                    MainPage = new NavigationPage(new HomePage());
                }
                else
                {
                    // if no user is logged in, go to login page
                    MainPage = new RegisterPage();
                }
               
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
