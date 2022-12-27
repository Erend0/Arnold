using NEA.Data;
using NEA.Models;
using NEA.Models.ListViewModels;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NEA.Pages.TabbedPage.CustomPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExerciseSearchPage : ContentPage
    {
        public ObservableCollection<ExerciseFound>  ListOfAllExercises { get; set; }
        public ExerciseSearchPage()
        {

            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            ListOfAllExercises = new ObservableCollection<ExerciseFound>();
            ListofExercises.ItemsSource = ListOfAllExercises;
            populatecollection();
        }
        private void populatecollection()
        {
            var exerciserepo = new ExerciseRepository();
            var machinerepo  = new MachineRepository();
            var muscletargetedrepo = new MuscleTargetedRepository();
            var musclerepo = new MuscleRepository();
            
            Exercise[] exercisesfound = exerciserepo.GetAllExercises();
            foreach(Exercise exercise in exercisesfound)
            {
                ExerciseFound currentexercise = new ExerciseFound();
                currentexercise.ExerciseName = exercise.ExerciseName;
                currentexercise.RecomendedSets = exercise.Sets;
                currentexercise.RecomendedReps = exercise.Reps;
                currentexercise.MachineName = machinerepo.GetMachineName(exercise.MachineID);
                string[] musclenames = musclerepo.GetMuscleName(muscletargetedrepo.GetMuscleID(exercise.ExerciseID));
                currentexercise.MajorMuscle = musclenames[0];
                currentexercise.MinorMuscle = musclenames[1];

                ListOfAllExercises.Add(currentexercise);
            } 
        }
    }
}