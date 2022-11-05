using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private void Login_Clicked(object sender, EventArgs e)
        {
            var userRepo = new UserRepository();

            // check if UserName with username and with UserPin userpin exists in the databaseif it does, then navigate to the MainPage if it doesn't, then display an error 
            if (userRepo.LoginUser(UserName.Text, UserPin.Text))
            {
                Application.Current.MainPage = new NavigationPage(new HomePage());
            }
            else
            {     
                DisplayAlert("Error", "The username or pin is incorrect", "OK");
            }




        }

        private void Register_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new RegisterPage();

        }
    }
}