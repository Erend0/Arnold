using NEA.Models;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NEA.Pages.TabbedPage.CustomPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class CreateRoutinePage : ContentPage
    {

        public ObservableCollection<Exercise> Exercises { get; set; }
        public CreateRoutinePage()
        {
            InitializeComponent();
            Exercises = new ObservableCollection<Exercise>();
            ListofExercises.ItemsSource = Exercises;


        }

        private void AddExercise_Pressed(object sender, EventArgs e)
        {
            //// checks if exercisename or sets or reps is null
            //if (ExerciseName.Text == null || Sets.Text == null || Reps.Text == null)
            //{
            //    DisplayAlert("Error", "Please fill in all fields", "OK");
            //}
            //else
            //{
            //    Exercises.Add(new Exercise { ExerciseName = ExerciseName.Text, Sets = Convert.ToInt32(Sets.Text), Reps = Convert.ToInt32(Reps.Text) });
            //}

        }
    }
}