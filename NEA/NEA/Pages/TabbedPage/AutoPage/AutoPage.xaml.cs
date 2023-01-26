using NEA.Data;
using System;
using NEA.Pages.TabbedPage;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NEA.Tasks;
using NEA.Models.ListViewModels;
using NEA.Models;
using System.Collections.Generic;

namespace NEA
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AutoPage : ContentPage
    {
        protected ObservableCollection<Day> Days { get; set; }
        ScheduleRepository _ScheduleRepo = new ScheduleRepository();
        int UserID { get; set; }
        int UserDays { get; set; }
        


        public AutoPage()
        {
           
            InitializeComponent();
            Days = new ObservableCollection<Day>();
            ListofDays.ItemsSource = Days;
            var userRepo = new UserRepository();
            UserID = userRepo.GetLoggedInUser().UserID;
            CheckResume();
            var userdataRepo = new UserDataRepository();
            UserDays = Convert.ToInt32(userdataRepo.GetUserData(UserID)[2]);
            Populatecollection();

            // The resume page database table is checked to see if there is any workout to continue
        }
        public void Populatecollection()
        {
            string[] daynames = _ScheduleRepo.GetDays(UserID, 0);
            foreach (string day in daynames)
            {
                if (day != null)
                {
                    Days.Add(new Day { DayName = day });
                    
                }
            }
            
        }
        private void CalculateTimeTaken(string dayname)
        {
            
        }
        private void ListofDays_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var day = e.Item as Day;
            Navigation.PushAsync(new DayOverviewPage(UserID, day.DayName,0));
        }

        private void Regenerate_Clicked(object sender, EventArgs e)
        {
            _ScheduleRepo.DeleteSchedule(UserID);
            DisplayAlert("Success", "All days have been regenerated", "Ok");
            Workout workout = new Workout("all");
        }
        private void CheckResume()
        {
            // Checks the database table to see if there is a workout to continue 
            // If so the button to enable the workout resuming function is enabled
            ResumeRepository resumerepo = new ResumeRepository();
            string daytoresume = resumerepo.CheckResume(UserID);
            if (daytoresume != null)
            {
                Resume.IsVisible = true;
            }
        }

        private void Resume_Clicked(object sender, EventArgs e)
        {
            ResumeRepository resumerepo = new ResumeRepository();
            MachineRepository machinerepo = new MachineRepository();
            ExerciseRepository exerciserepo = new ExerciseRepository();

            string daytoresume = resumerepo.CheckResume(UserID);
            int type = resumerepo.ReturnType(UserID);
            List<Schedule> exercises = _ScheduleRepo.GetSchedule(UserID, daytoresume,type);

            List<ExerciseData> exercises2 = new List<ExerciseData>();
            
            
            foreach (Schedule exercise in exercises)
            {
                string machinename = machinerepo.GetMachineName(exerciserepo.GetExercise(exercise.ExerciseID).MachineID);
                exercises2.Add(new ExerciseData { ExerciseName = exerciserepo.GetExercise(exercise.ExerciseID).ExerciseName, Sets = exercise.Sets, Reps = exercise.Reps, MachineName = machinename });
            }
            Navigation.PushAsync(new CompanionPage(exercises2,true, UserID,type,daytoresume));
        }
    }
}