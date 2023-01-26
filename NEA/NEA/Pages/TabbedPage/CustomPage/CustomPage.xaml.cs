using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NEA.Pages.TabbedPage.CustomPage;
using NEA.Data;
using NEA.Models.ListViewModels;
using NEA.Models;
using NEA.Pages.TabbedPage;
using static SQLite.SQLite3;
using System.Collections.Generic;

namespace NEA
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomPage : ContentPage
    {
        protected ObservableCollection<Day> Days { get; set; }
        private int UserID { get; set; }
        ScheduleRepository _ScheduleRepo = new ScheduleRepository();
        public CustomPage()
        {
            InitializeComponent();
            Days = new ObservableCollection<Day>();
            ListofCustomDays.ItemsSource = Days;
            var schedulerepo = new ScheduleRepository();

            var userrepo = new UserRepository();
            UserID = userrepo.GetLoggedInUser().UserID;
            string[] daynames = schedulerepo.GetDays(UserID, 1);
            foreach(string day in daynames)
            {
                if(day != null)
                {
                    Days.Add(new Day { DayName = day });
                }
            }

            CheckResume();
        }
        private void AddRoutine_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NavigationPage(new ExerciseSearchPage()));
        }

        private void ListofCustomDays_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var day = e.Item as Day;
            Navigation.PushAsync(new DayOverviewPage(UserID, day.DayName, 1));

        }

        private void CheckResume()
        {
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
            List<Schedule> exercises = _ScheduleRepo.GetSchedule(UserID, daytoresume, type);

            List<ExerciseData> exercises2 = new List<ExerciseData>();


            foreach (Schedule exercise in exercises)
            {
                string machinename = machinerepo.GetMachineName(exerciserepo.GetExercise(exercise.ExerciseID).MachineID);
                exercises2.Add(new ExerciseData { ExerciseName = exerciserepo.GetExercise(exercise.ExerciseID).ExerciseName, Sets = exercise.Sets, Reps = exercise.Reps, MachineName = machinename });
            }
            Navigation.PushAsync(new CompanionPage(exercises2, true, UserID, type, daytoresume));

        }
    }
}