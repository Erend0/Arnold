using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NEA.Data;
using NEA.Tasks;
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
            if (Aim.SelectedIndex == -1)
            {
                DisplayAlert("Error", "Please select an aim", "OK");
            }
            else
            {
                // gets days stepper value from the bindings
                int days = (int)Days.Value;
                // gets time stepper value (converted to seconds)
                int time = (int)Time.Value*60;
                // gets aim value
                string aim = Aim.SelectedItem.ToString();

                // Inserts user data into the database using the userdata repository
                var userrepo = new UserRepository();
                int userID = userrepo.GetLoggedInUser().UserID;
                var userdataRepo = new UserDataRepository();
                userdataRepo.InsertUserData(userID,time,days,aim);

                // The class which will generate the workout is instantiated
                // The "all" parameter means all of the days will be generated
                Workout workout = new Workout("all");
                // The current page is changed to the main tabbed page
                App.Current.MainPage = new NavigationPage(new HomePage());
            }
        }
    }
}