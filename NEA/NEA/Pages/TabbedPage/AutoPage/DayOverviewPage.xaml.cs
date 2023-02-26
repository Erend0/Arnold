using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using NEA.Data;
using NEA.Tasks;
using NEA.Models;
using NEA.Models.ListViewModels;

namespace NEA.Pages.TabbedPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DayOverviewPage : ContentPage
    {
        
        protected ObservableCollection<ExerciseData> Exercises { get; set; }
        private ScheduleRepository _ScheduleRepo = new ScheduleRepository();
        private int UserID { get; set; }
        private int TimeTaken {get; set; }
        private string DayName { get; set; }
        private int Type { get; set; }

        public DayOverviewPage(int userID,string dayname,int type)
        {
            Type = type;
            UserID = userID;
            DayName = dayname;
            InitializeComponent();


            if (type == 1)
            {
                var button = new Button();
                button.Text = "Delete custom day";
                button.Clicked += Button_Clicked;
                Stack.Children.Add(button); 
            }
            Exercises = new ObservableCollection<ExerciseData>();
            ExerciseList.ItemsSource = Exercises;
            DayName = dayname;
            UserID = userID;
            
            var exerciserepo = new ExerciseRepository();
            var userdatarepo = new UserDataRepository();
            var machinerepo = new MachineRepository();

            List<Schedule> exercises = _ScheduleRepo.GetSchedule(UserID, DayName,type);
            string useraim = userdatarepo.GetUserData(UserID)[0];
            int resttime = 60;
            if (useraim == "Muscle Strength")
            {
                resttime = 90;
            }
            else if (useraim == "Endurance")
            {
                resttime = 45;
            }
            foreach (Schedule exercise in exercises)
            {
                string machinename = machinerepo.GetMachineName(exerciserepo.GetExercise(exercise.ExerciseID).MachineID);
                Exercises.Add(new ExerciseData { ExerciseName = exerciserepo.GetExercise(exercise.ExerciseID).ExerciseName, Sets = exercise.Sets, Reps = exercise.Reps, MachineName=machinename});
                TimeTaken += exercise.Sets * (exercise.Reps * 5 + resttime);
            }
            Time_Taken.Text = "Estimate time taken: " + (TimeTaken/60).ToString() + " minutes";
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            _ScheduleRepo.DeleteDay(UserID, DayName);
            App.Current.MainPage = new NavigationPage(new HomePage());

        }

        private void StartDay_Clicked(object sender, System.EventArgs e)
        {
            List<ExerciseData> collectionaslist = new List<ExerciseData>();
            foreach (ExerciseData exercise in Exercises)
            {
                collectionaslist.Add(exercise);
            }
            Navigation.PushAsync(new CompanionPage(collectionaslist, false, UserID, Type, DayName));
        }

        private void Regenerate_Clicked(object sender, EventArgs e)
        {
            var schedulerepo = new ScheduleRepository();
            schedulerepo.DeleteDay(UserID, DayName);
            Workout regeneratedday = new Workout(DayName);
            DisplayAlert("Regenerated", "Day has been succesfully regenerated", "Ok");
            Navigation.PopAsync();
        }
    }
}