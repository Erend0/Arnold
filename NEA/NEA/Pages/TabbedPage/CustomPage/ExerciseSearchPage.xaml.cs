using NEA.Data;
using NEA.Models;
using NEA.Models.ListViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NEA.Pages.TabbedPage.CustomPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class ExerciseSearchPage : ContentPage
    {
        protected ObservableCollection<ExerciseData> ListOfAllExercises { get; set; }
        private List<ExerciseData> SelectedExercises = new List<ExerciseData>();
        private ExerciseRepository _ExerciseRepo = new ExerciseRepository();
        private MuscleTargetedRepository _MuscleTargetedRepo = new MuscleTargetedRepository();
        private MuscleRepository  _MuscleRepo = new MuscleRepository();
        private MachineRepository _MachineRepo = new MachineRepository();
        

        public ExerciseSearchPage()
        {

            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            ListOfAllExercises = new ObservableCollection<ExerciseData>();
            ListofExercises.ItemsSource = ListOfAllExercises;
            List<Exercise> exercisesfound = _ExerciseRepo.GetAllExercises();
            FillCollection(exercisesfound);
        }
        private void FillCollection(List<Exercise> exercisesfound)
        {
            List<ExerciseData> copyOfSelectedExercises = new List<ExerciseData>(SelectedExercises);

            foreach (Exercise exercise in exercisesfound)
            {
                ExerciseData currentexercise = new ExerciseData();
                currentexercise.ExerciseName = exercise.ExerciseName;
                currentexercise.MachineName = _MachineRepo.GetMachineName(exercise.MachineID);
                string[] musclenames = _MuscleRepo.GetMuscleName(_MuscleTargetedRepo.GetMuscleID(exercise.ExerciseID));
                currentexercise.MajorMuscle = musclenames[0];
                currentexercise.MinorMuscle = musclenames[1];
                currentexercise.Sets = exercise.Sets;
                currentexercise.Reps = exercise.Reps;

                bool isSelected = false;
                foreach (ExerciseData selectedexercise in copyOfSelectedExercises)
                {
                    if (selectedexercise.ExerciseName == currentexercise.ExerciseName)
                    {
                        currentexercise.IsChecked = true;
                        SelectedExercises.Remove(selectedexercise);
                        isSelected = true;
                        break;
                    }
                }

                if (!isSelected && currentexercise.IsChecked)
                {
                    SelectedExercises.Add(currentexercise);
                }

                ListOfAllExercises.Add(currentexercise);
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListOfAllExercises.Clear();
            List<Exercise> exercisesfound = _ExerciseRepo.SearchExercises(SearchText.Text);
            FillCollection(exercisesfound);
        }

        
        private void Continue_Pressed(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new AddToSchedulePage(SelectedExercises));
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var checkbox = (CheckBox)sender;
            var exercise = (ExerciseData)checkbox.BindingContext;

            if (checkbox.IsChecked)
            {
              SelectedExercises.Add(exercise);
            }
            else
            {
                SelectedExercises.Remove(exercise);
            }
        }

    }
}
