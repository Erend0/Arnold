using System;
using NEA.Data;
using NEA.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NEA
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrackPage : ContentPage
    {
        public int UserID { get; set; }
        public TrackPage()
        {
            InitializeComponent();
            PopulateData();
        }
        public void PopulateData()
        {
            var userrepo = new UserRepository();
            UserID = userrepo.GetLoggedInUser().UserID;
            var userdatarepo = new UserDataRepository();
            int[] data = userdatarepo.GetWorkoutData(UserID);
            TotalSets.Text =Convert.ToString(data[0]) + " Sets";
            TotalReps.Text =Convert.ToString(data[1]) + " Reps";
            TotalVolume.Text = Convert.ToString(data[2]) +" Kg";
            TotalTime.Text = Convert.ToString(data[3]/60) + " Minutes";
            NumberofWorkouts.Text = Convert.ToString(data[4]) + " Workouts";
            
        }

        private void LogOut_Clicked(object sender, EventArgs e)
        {
            // logout user
            var userrepo = new UserRepository();
            userrepo.LogoutUser();
            Application.Current.MainPage = new LoginPage();

        }

        private void Settings_Clicked(object sender, EventArgs e)
        {

        }

        private void RemoveLatest_Clicked(object sender, EventArgs e)
        {

        }

        private void AlterData_Clicked(object sender, EventArgs e)
        {

        }
    }

   
}