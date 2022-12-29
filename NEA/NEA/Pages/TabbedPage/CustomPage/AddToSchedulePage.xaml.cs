using NEA.Data;
using NEA.Models;
using NEA.Models.ListViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace NEA.Pages.TabbedPage.CustomPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddToSchedulePage : ContentPage
    {
        private ObservableCollection<ExerciseData> AddedExercises { get; set; }
        private ScheduleRepository _ScheduleRepo = new ScheduleRepository();
        private int UserID { get; set; }
        public AddToSchedulePage(List<ExerciseData> Exercises)
        {
            InitializeComponent();

            var userepo = new UserRepository();
            UserID = userepo.GetLoggedInUser().UserID;
            AddedExercises = new ObservableCollection<ExerciseData>();
            foreach (var exercise in Exercises)
            {
                AddedExercises.Add(exercise);
            }
            ListofExercises.ItemsSource = AddedExercises;
        }
        private void Sets_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            var exercise = (ExerciseData)entry.BindingContext;
            exercise.Sets = Convert.ToInt32(entry.Text);
        }
        private void Reps_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            var exercise = (ExerciseData)entry.BindingContext;
            exercise.Reps = Convert.ToInt32(entry.Text);

        }
        // The .xaml code cannot access async methods, hence this method acts as a bridge to the actual save method
        private void Save_Pressed(object sender, EventArgs e)
        {
            Save_PressedAsync(sender, e);
        }
        // The method below is async due to the await feature for the display alert
        // All exercises are checked to see if they have sets and reps inputted, if not then the user is alerted and an action is taken
        private async Task Save_PressedAsync(object sender, System.EventArgs e)
        {
            // This flag is used to see if the database should be updated yet
            bool flag = true;
            foreach(ExerciseData exercise in AddedExercises)
            {
                if (exercise.Reps == 0 || exercise.Sets == 0)
                {
                    bool answer = await DisplayAlert("Error", "A value for sets and reps for some exercises are not inputted, press YES to use the default sets and reps for these", "Yes", "No");
                    if (answer == true)
                    {
                        AddSetsandReps();
                    }
                    else
                    {
                        flag = false;
                    }
                    break;
                }
            }
            if(DayName.Text == null )
            {
                DisplayAlert("Error", "Day name cannot be blank", "OK");
                flag = false;
            }
            if(_ScheduleRepo.CheckDayName(UserID,DayName.Text)){
                DisplayAlert("Error", "This day name already exists, please pick a new one", "OK");
                flag = false;
            }
            if (flag)
            {
                DBUpdate(DayName.Text);
                App.Current.MainPage = new NavigationPage(new HomePage());
            }
        }

        protected void Cancel_Pressed(object sender, System.EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new HomePage());
        }

        private void AddSetsandReps()
        {
            var exerciserepo = new ExerciseRepository();
            foreach (ExerciseData exercise in AddedExercises)
            {
                int[] setsandreps = exerciserepo.GetSetsandReps(exercise.ExerciseName);
                exercise.Sets = setsandreps[0];
                exercise.Reps = setsandreps[1];
            }
        }
        private void DBUpdate(string dayname)
        {
            var schedulerepo = new ScheduleRepository();
            var exerciserepo = new ExerciseRepository();
            foreach (ExerciseData exercise in AddedExercises)
            {
                var schedule = new Schedule
                {
                    UserID = UserID,
                    ExerciseID = exerciserepo.SearchExercises(exercise.ExerciseName)[0].ExerciseID,
                    Sets = exercise.Sets,
                    Reps = exercise.Reps,
                    DayName = dayname,
                    Type = 1,
                };
                schedulerepo.CreateSchedule(schedule);
                
            }
        }
    }

}

