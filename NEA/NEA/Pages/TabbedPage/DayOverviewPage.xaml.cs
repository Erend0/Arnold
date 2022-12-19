using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NEA.Data;
using NEA.Models;
using System.Collections.ObjectModel;


namespace NEA.Pages.TabbedPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DayOverviewPage : ContentPage
    {
        public ObservableCollection<Exercise> Exercises { get; set; }
        private int TimeTaken {get; set; }
        public DayOverviewPage(int UserID,string DayName)
        {
            InitializeComponent();
            Exercises = new ObservableCollection<Exercise>();
            ExerciseList.ItemsSource = Exercises;

            var scheduleRepo = new ScheduleRepository();
            var exerciserepo = new ExerciseRepository();
            var userdatarepo = new UserDataRepository();
            
            int[] exercises = scheduleRepo.GetSchedule(UserID, DayName);
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
                Exercises.Add(exerciserepo.GetExercise(exercise));
                int[] exercisedata = exerciserepo.GetExerciseData(exercise);
                TimeTaken += exercisedata[0] * (exercisedata[1] * 5 + resttime);

            }
            Time_Taken.Text = "Estimate time taken: " + TimeTaken.ToString();
        }

        private void StartDay_Clicked(object sender, System.EventArgs e)
        {

        }
    }
}