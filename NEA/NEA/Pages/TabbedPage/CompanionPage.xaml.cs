using System;
using System.Collections.Generic;
using NEA.Models.ListViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NEA.Pages.TabbedPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompanionPage : ContentPage
    {
        private int index = 0;
        private int Addedsets = 0;
        private List<ExerciseData> Exercises;

        public CompanionPage(List<ExerciseData> exercises)
        {
            InitializeComponent();
            Exercises = exercises;
            PopulateSetLayout(exercises[index]);
        }
        private void PopulateSetLayout(ExerciseData exercise)
        {
            Addedsets = 0;
            Sets.Children.Clear();
            ExerciseName.Text = exercise.ExerciseName;
            for (int i = 0; i < exercise.Sets; i++)
            {
                // add new entry for reps, and weight with a checkbox for each set in each row of grid
                Sets.Children.Add(new Entry
                {
                    Placeholder= exercise.Reps.ToString() + " Reps"
                },0,i);
                Sets.Children.Add(new Entry
                {
                    Placeholder = "Weigth",

                },1,i);
                Sets.Children.Add(new CheckBox
                {
                    

                },2,i);
            }
        }
        private void Exercise_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            index++;
            PopulateSetLayout(Exercises[index]);

        }
        private void Skip_Clicked(object sender, EventArgs e)
        {
            index++;
            if(index == 1)
            {
                Back.IsVisible = true;
            }
            PopulateSetLayout(Exercises[index]);
            // Checks if index has reached the end of list
            if(index == Exercises.Count - 1)
            {
                // This is done to prevent an out of bounds error due to indexing
                Skip.IsVisible = false;
                Complete.IsVisible = true;
            }

            

        }
        private void Back_Clicked(object sender, EventArgs e)
        {
            index--;
            // Initially the back button is not visible to not go out of bounds for the index of the list this makes it visible
            if (index == 0)
            {
                Back.IsVisible = false;
            }
            // Skip is made invisible at the last exercises, if the user goes back this makes it visible again
            if(index == Exercises.Count - 2)
            {
                Skip.IsVisible = true;
                Complete.IsVisible = false;
            }
            PopulateSetLayout(Exercises[index]);
        }
        private void Quit_Clicked(object sender, EventArgs e)
        {

        }
        private void Pause_Clicked(object sender, EventArgs e)
        {

        }
        private void AddSet_Clicked(object sender, EventArgs e)
        {
            int row = Exercises[index].Sets + Addedsets + 1;
            Sets.Children.Add(new Entry
            {
                Placeholder = Exercises[index].Reps.ToString() + " Reps"
            },0,row);
            Sets.Children.Add(new Entry
            {
                Placeholder = "Weight"
            },1,row);
            Sets.Children.Add(new CheckBox
            {
            },2,row);
            Addedsets++;
        }
        private void Complete_Clicked(object sender, EventArgs e)
        {

        }

        // Instructions:
        // When skip is clicked the exercise is stored in a data structure
        // When Exercise_CheckChange is clicked all the sets and reps are assumed to be done and stored in a data structure
        // Data structure also stores the addded sets 
        // When complete exercise is clicked save everything and update the db and display a success message
        // quit function and pause function store the exercise
    }
}