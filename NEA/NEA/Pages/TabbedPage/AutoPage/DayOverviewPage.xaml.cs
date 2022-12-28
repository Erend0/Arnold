using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NEA.Data;
using NEA.Models.ListViewModels;
using System.Collections.ObjectModel;
using System;
using NEA.Tasks;

namespace NEA.Pages.TabbedPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DayOverviewPage : ContentPage
    {
        
        protected ObservableCollection<ExerciseData> Exercises { get; set; }
        private int UserID { get; set; }
        private int TimeTaken {get; set; }
        private string DayName { get; set; }

        public DayOverviewPage(int userID,string dayname)
        {
            InitializeComponent();
            Exercises = new ObservableCollection<ExerciseData>();
            ExerciseList.ItemsSource = Exercises;
            DayName = dayname;
            UserID = userID;

            var scheduleRepo = new ScheduleRepository();
            var exerciserepo = new ExerciseRepository();
            var userdatarepo = new UserDataRepository();
            
            int[] exercises = scheduleRepo.GetSchedule(UserID, DayName,0);
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
            foreach (int exercise in exercises)
            {
                Exercises.Add(new ExerciseData { ExerciseName = exerciserepo.GetExercise(exercise).ExerciseName, Sets = exerciserepo.GetExercise(exercise).Sets, Reps = exerciserepo.GetExercise(exercise).Reps });
                int[] exercisedata = exerciserepo.GetExerciseData(exercise);
                TimeTaken += exercisedata[0] * (exercisedata[1] * 5 + resttime);

            }
            Time_Taken.Text = "Estimate time taken: " + (TimeTaken/60).ToString() + " minutes";
        }

        private void StartDay_Clicked(object sender, System.EventArgs e)
        {

        }

        private void Regenerate_Clicked(object sender, EventArgs e)
        {
            var schedulerepo = new ScheduleRepository();
            schedulerepo.DeleteDay(UserID, DayName);
            Workout regeneratedday = new Workout(DayName);
            DisplayAlert("Regenerated", "Day has been succesfully regenerated", "Ok");
            App.Current.MainPage = new NavigationPage(new HomePage());
        }
    }
}