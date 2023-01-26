using System;
using NEA.Data;
using NEA.Models.OtherModels;
using NEA.Pages;
using NEA.Pages.TabbedPage;
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
            GetLastWorkoutData();
            // Checks if there was a latest workout
            // To determine if the workout delete button should be offered
            var workoutrepo = new WorkoutRepository();
            WorkoutTracker[] latestworkout = workoutrepo.GetAll();
            if (latestworkout.Length != 0)
            {
                Remove.IsVisible = true;
            }
            else
            {
                Remove.IsVisible = false;
            }
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
            if (data[4] == 1)
            {
                NumberofWorkouts.Text = Convert.ToString(data[4]) + " Workout";
            }
            else
            {
                NumberofWorkouts.Text = Convert.ToString(data[4]) + " Workouts";
            }
        }
        private void GetLastWorkoutData()
        {

            var workoutrepo = new WorkoutRepository();
            var resumerepo = new ResumeRepository();
            var userdatarepo = new UserDataRepository();


            WorkoutTracker[] latestworkout = workoutrepo.GetAll();
            int sets = 0;
            int reps = 0;
            int volume = 0;
            int time = resumerepo.GetTime(UserID);
            // Get all the sets which have a record for the weight and reps
            // When the workout was marked as complete, and the data of total sets was updated
            // Only the data with a weight and reps was added to the database
            // So the sets which have a weight and reps are the sets which were completed
            foreach (WorkoutTracker log in latestworkout)
            {
                volume += log.Weight * log.Reps;
                reps += log.Reps;
                if (log.Reps != 0 && log.Weight != 0)
                {
                    sets++;
                }
            }
            // Update the data in the UI
            Sets.Text = Convert.ToString(sets) + " Sets";
            Reps.Text = Convert.ToString(reps) + " Reps";
            Volume.Text = Convert.ToString(volume) + " Kg";
            Time.Text = Convert.ToString(time / 60) + " Minutes";
        }

        private void LogOut_Clicked(object sender, EventArgs e)
        {
            // logout user
            var userrepo = new UserRepository();
            userrepo.LogoutUser();
            var WorkoutRepo = new WorkoutRepository();
            WorkoutRepo.DeleteAll();
            Application.Current.MainPage = new LoginPage();

        }

        private void Settings_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPage());
        }

        // Using the resumerepo and workoutrepo, the latest workout is removed from the database
        private void RemoveLatest_Clicked(object sender, EventArgs e)
        {
            // set the database repos
            var workoutrepo = new WorkoutRepository();
            var resumerepo = new ResumeRepository();
            var userdatarepo = new UserDataRepository();

            // get from ui
            int sets = Convert.ToInt32(Sets.Text.Split(' ')[0]);
            int reps = Convert.ToInt32(Reps.Text.Split(' ')[0]);
            int volume = Convert.ToInt32(Volume.Text.Split(' ')[0]);
            int time = Convert.ToInt32(Time.Text.Split(' ')[0]) * 60;
            
            // remove the latest workout
            resumerepo.DeleteAll();
            workoutrepo.DeleteAll();
            userdatarepo.RemoveWorkoutData(UserID, sets, reps, volume, time);

            Remove.IsVisible = false;
            PopulateData();
            GetLastWorkoutData();
        }

        private void AlterData_Clicked(object sender, EventArgs e)
        {

        }
    }

   
}