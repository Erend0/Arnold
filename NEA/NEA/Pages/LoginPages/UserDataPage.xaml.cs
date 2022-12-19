using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NEA.Data;
using NEA.Tasks;
using System.Collections.Generic;
using System.Linq;
using NEA.Models;

namespace NEA.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    
    public partial class UserDataPage : ContentPage
    {
        private int UserID { get; set; }
        public UserDataPage()
        {
            InitializeComponent();
        }
        private void Submit_Pressed(object sender, EventArgs e)
        {
            if (Aim.SelectedIndex == -1)
            {
                DisplayAlert("Error", "Please select an aim", "OK");
            }
            else
            {
                int days = (int)Days.Value;
                // gets time stepper value (converted to seconds)
                int time = (int)Time.Value*60;
                // gets aim value
                string aim = Aim.SelectedItem.ToString();

                // The User's data is inserted into the database
                var userRepo = new UserRepository();
                UserID = userRepo.GetLoggedInUser().UserID;
                var userdataRepo = new UserDataRepository();
                userdataRepo.InsertUserData(UserID,time,days,aim);

                
                // The class which will generate the workout is instantiated
                // The "all" parameter means all of the days will be generated
                Workout workout = new Workout("all");
                List<List<int>> CreatedWorkout = workout.generatedworkout;
               
                // The database is updated with the generated workout
                DBUpdate(CreatedWorkout,days);
                
                
                // The current page is changed to the main tabbed page
                App.Current.MainPage = new NavigationPage(new HomePage());
            }
        }
        private void DBUpdate(List<List<int>> Workout, int Days)
        {
            string[] DayNames = new string[4];
            if (Days == 3)
            {
                DayNames[0] = ("Chest,Tricep,Legs");
                DayNames[1] = ("Back,Biceps,Shoulders");
                DayNames[2] = ("Biceps,legs,chest");
            }
            if (Days == 4 || Days == 5)
            {
                DayNames[0] = ("Chest,Triceps");
                DayNames[1] = ("Back,Biceps");
                DayNames[2] = ("Shoulders");
                DayNames[3] = ("Legs");
            }
            if(Days == 5)
            {
                DayNames[4] = ("Cardio");
            }
            var scheduleRepo = new ScheduleRepository();
            int dayindex = 0;
            foreach (List<int> day in Workout)
            {
                foreach (int exercise in day)
                {
                    // create new instance of schedule
                    var schedule = new Schedule
                    {
                        UserID = UserID,
                        DayName = DayNames[dayindex],
                        ExerciseID = exercise,
                    };
                    scheduleRepo.CreateSchedule(schedule);
                }
                dayindex += 1;
            }
        }
    }
}