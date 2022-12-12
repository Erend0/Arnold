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
        public DayOverviewPage(int UserID,string DayName)
        {
            InitializeComponent();
            
            var scheduleRepo = new ScheduleRepository();
            int[] exercises = scheduleRepo.GetSchedule(UserID, DayName);
            Exercises = new ObservableCollection<Exercise>();
            ExerciseList.ItemsSource = Exercises;
            var exerciserepo = new ExerciseRepository();
            foreach (int exercise in exercises)
            {
                Exercises.Add(exerciserepo.GetExercise(exercise));
            }
        }
    }
}