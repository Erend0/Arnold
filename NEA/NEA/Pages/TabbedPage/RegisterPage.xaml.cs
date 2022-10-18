﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NEA
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }


        private void Register_Clicked(object sender, EventArgs e)
        {
            // The Pin and Username checking functions are run until they are both returning true
            bool hasPin = checkPin();
            bool hasName = checkName();
            if (hasPin && hasName)
            {
                // Once the pin and username functions are both satisfied, the data is stored and the current screen is changed to the schedule page
                storeUserDetails();
                apppage();

            }
        }

        private bool checkPin()
        {
            // Statement below is required to check string details
            string checkpin = ((Entry)UserPin).Text;
            // Error : Pin is too short
            if (checkpin.Length < 4)
            {
                PassError.Text = "Error - The Pin is too short: It should be 4 digits";
            }
            // Error : Pin is too long
            else if (checkpin.Length > 4)
            {
                PassError.Text = "Error - The Pin is too long: It should be 4 digits";
            }
            // Error : Pin contains non numeric values
            else if (checkpin.All(char.IsDigit) == false)
            {
                PassError.Text = "Error - The Pin contains non numeric values";
            }
            // Accept : Correct Pin format
            else
            {
                return true;

            }
            return false;
        }
        private bool checkName()
        {
            // The username variable in the xaml entry is taken and stored as a variable called checkUserName
            string checkUserName = ((Entry)UserName).Text;
            // Used in the elif statement
            bool NameDuplicateError = doesExist(checkUserName);
            // check if it is larger than 0 characters 
            if (checkUserName.Length < 0)
            {
                UserNameError.Text = "The Username must be at least one characters";
            }
            // check if the username already exists
            else if(NameDuplicateError == true){
                UserName.Text = "This Username Already Exists, Please pick a new one";
            }
            else if(checkUserName.Length > 6)
            {
                UserNameError.Text = "The Username must be shorted than 6 characters";
            }
            // Accept : Correct Username format
            else
            {
                return true;
            }

            return false;
        }
        private bool doesExist(string NameToCheck)
        {
            return false;
        }

        //MUST COMPLETE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        private void storeUserDetails()
        {

            // stores the users details in the format of a textfile
        }



        private void apppage()
        {
            // Application.Current.MainPage = new NavigationPage(new DetailsPage());

            //await Navigation.PushAsync(new NavigationPage(new DetailsPage()));
        }
    }
}