using NEA.Data;
using NEA.Models;
using NEA.Pages;
using System;
using System.Linq;
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
                // Once the pin and username functions are both satisfied
                // The user is added to the database
                // The homepage is changed to the details page
                storeUserDetails();
                Application.Current.MainPage = new NavigationPage(new UserDataPage()); ;

            }
        }
        
        private bool checkPin()
        {
            // Statement below is required to check string details
            string checkpin = ((Entry)UserPin).Text;
            // Error : Pin is too short
            if ( checkpin == null ||checkpin.Length < 4)
            {
                // check if checping is  null

                DisplayAlert("Error", "The Pin is too short: It should be 4 digits", "OK");
            }
            // Error : Pin is too long
            else if (checkpin.Length > 4)
            {
                DisplayAlert("Error", "The Pin is too long: It should be 4 digits", "OK");
            }
            // Error : Pin contains non numeric values
            else if (checkpin.All(char.IsDigit) == false)
            {
                DisplayAlert("Error", "The Pin contains non numeric values", "OK");
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
            bool NameDuplicateError = doesExist();
            // check if it is larger than 0 characters 
            if ( checkUserName == null|| checkUserName.Length < 0)
            {
                DisplayAlert("Error", "The Username is too short: It should be 1 or more characters", "OK");
            }
            // check if the username already exists
            else if (NameDuplicateError == true)
            {
                DisplayAlert("Error", "The Username already exists", "OK");
            }
            // checks if the username is too long
            else if (checkUserName.Length > 6)
            {
                DisplayAlert("Error", "The Username is too long: It should be 6 or less characters", "OK");
            }
            // Accept : Correct Username format
            else
            {
                return true;
            }

            return false;
        }
        
        // This method is used to check if a username already exists in the database
        private bool doesExist()
        {
            string NameToCheck = ((Entry)UserName).Text;
            // The userrepository function GetUsernames is called
            // Each name is checked against the name to check
            var userrepo = new UserRepository();
            var usernames = userrepo.GetUsernames();

            foreach (var user in usernames)
            {
                if (user.UserName == NameToCheck)
                {
                    return true;
                }
            }
            return false;

        }

        // This function stores the user details in the database, and sets the HasLoggedIn to true / 1
        private void storeUserDetails()
        {
            string userName = ((Entry)UserName).Text;
            int userPin = int.Parse(((Entry)UserPin).Text);
            // create new user with UserName as userName and UserPin as userpin
            var user = new User
            {
                UserName = userName,
                UserPin = userPin,
                HasLoggedIn = 1,
            };
            // add user to database
            var userRepo = new UserRepository();
            userRepo.AddNewUser(user);
        }
        
        // This function is triggered by the button in the xaml code, and is used to promt the user back to the login page
        private void Login_Pressed(object sender, EventArgs e)
        {
            // change mainpage to login page
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}