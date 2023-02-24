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
        private ObservableCollection<Day> Days { get; set; }
        private ScheduleRepository _scheduleRepo;
        private UserRepository _userRepo;
        private UserDataRepository _userDataRepo;
        private ResumeRepository _resumeRepo;
        private MachineRepository _machineRepo;
        private ExerciseRepository _exerciseRepo;
        private int UserID { get; set; }
        private int UserDays { get; set; }

        public AutoPage()
        {
            InitializeComponent();
            Days = new ObservableCollection<Day>();
            ListofDays.ItemsSource = Days;
            _userRepo = new UserRepository();
            UserID = _userRepo.GetLoggedInUser().UserID;
            CheckResume();
            _userDataRepo = new UserDataRepository();
            UserDays = Convert.ToInt32(_userDataRepo.GetUserData(UserID)[2]);
            PopulateCollection();
        }

        private void PopulateCollection()
        {
            _scheduleRepo = new ScheduleRepository();
            string[] daynames = _scheduleRepo.GetDays(UserID, 0);

            foreach (string day in daynames)
            {
                if (day != null)
                {
                    Days.Add(new Day { DayName = day });
                }
            }
        }

        private void ListofDays_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var day = e.Item as Day;
            Navigation.PushAsync(new DayOverviewPage(UserID, day.DayName, 0));
        }

        private void Regenerate_Clicked(object sender, EventArgs e)
        {
            _scheduleRepo.DeleteSchedule(UserID);
            DisplayAlert("Success", "All days have been regenerated", "Ok");
            Workout workout = new Workout("all");
            Days.Clear();
            PopulateCollection();
        }

        private void CheckResume()
        {
            _resumeRepo = new ResumeRepository();
            string dayToResume = _resumeRepo.CheckResume(UserID);
            if (dayToResume != null)
            {
                Resume.IsVisible = true;
            }
        }

        private void Resume_Clicked(object sender, EventArgs e)
        {
            var _resumerepo = new ResumeRepository();
            var _machinerepo = new MachineRepository();
            var _exerciserepo = new ExerciseRepository();
            string dayToResume = _resumerepo.CheckResume(UserID);
            int workoutType = _resumerepo.ReturnType(UserID);
            List<Schedule> exercises = _scheduleRepo.GetSchedule(UserID, dayToResume, workoutType);

            List<ExerciseData> exerciseDataList = new List<ExerciseData>();

            foreach (var exercise in exercises)
            {
                string machineName = _machinerepo.GetMachineName(_exerciserepo.GetExercise(exercise.ExerciseID).MachineID);
                exerciseDataList.Add(new ExerciseData
                {
                    ExerciseName = _exerciserepo.GetExercise(exercise.ExerciseID).ExerciseName,
                    Sets = exercise.Sets,
                    Reps = exercise.Reps,
                    MachineName = machineName
                });
            }

            Navigation.PushAsync(new CompanionPage(exerciseDataList, true, UserID, workoutType, dayToResume));

        }
    }
}