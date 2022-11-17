using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NEA.Data;
namespace NEA.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        // This method is triggered by clicking the login button in the xaml interface
        private void Login_Clicked(object sender, EventArgs e)
        {
            var userRepo = new UserRepository();
            // The LoginUser function in the database functions is called and the username and pin are passed to it
            // If the details are valid the user is logged in and the homepage is changed to the autopage
            // If not the user is prompted with an error message, and allowed to retry 
            if (userRepo.LoginUser(UserName.Text, UserPin.Text))
            {
                Application.Current.MainPage = new NavigationPage(new HomePage());
            }
            else
            {     
                DisplayAlert("Error", "The username or pin is incorrect", "OK");
            }
        }
        // This method is triggered by clicking the register button in the xaml interface
        // The current homepage is switched to the register page
        private void Register_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new RegisterPage();
        }
    }
}