using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Versioning;
using NEA.Models.ListViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NEA.Pages.TabbedPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompanionPage : ContentPage
    {
        private int index = 0;
        Dictionary<Tuple<int, int>, int[]> EntryData = new Dictionary<Tuple<int, int>, int[]>();
        Dictionary<int, int> AddedSets = new Dictionary<int, int>();
        private List<ExerciseData> Exercises;
        
        public CompanionPage(List<ExerciseData> exercises)
        {
            InitializeComponent();
            Exercises = exercises;
            PopulateSetLayout(exercises[index]);
        }
        private void PopulateSetLayout(ExerciseData exercise)
        {
            if (!AddedSets.ContainsKey(index))
            {
                AddedSets.Add(index, 0);
            }
            Sets.Children.Clear();
            ExerciseName.Text = exercise.ExerciseName;
            for (int i = 0; i < exercise.Sets; i++)
            {
                // add new entry for reps, and weight with a checkbox for each set in each row of grid
                Sets.Children.Add(new Entry
                {
                    Placeholder = exercise.Reps.ToString() + " Reps",
                    Keyboard = Keyboard.Numeric
                }, 0, i);
                Sets.Children.Add(new Entry
                {
                    Placeholder = "Weigth",
                    Keyboard = Keyboard.Numeric
                }, 1, i);;
                Sets.Children.Add(new CheckBox
                {
                }, 2, i);
            }
        }
        private void Exercise_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            index++;
            PopulateSetLayout(Exercises[index]);
        }
        private void Skip_Clicked(object sender, EventArgs e)
        {
            bool missing = false;
            for (int i = 0; i < Sets.Children.Count; i++)
            {
                if (Sets.Children[i] is Entry)
                {
                    if (string.IsNullOrEmpty(((Entry)Sets.Children[i]).Text))
                    {
                        missing = true;
                    }
                }
            }
            if (!missing)
            {
                index++;
                if (index == 1)
                {
                    Back.IsVisible = true;
                }
                PopulateSetLayout(Exercises[index]);
                if (index == Exercises.Count - 1)
                {
                    Skip.IsVisible = false;
                    Complete.IsVisible = true;
                }
                for (int i = 0; i < Sets.Children.Count; i++)
                {
                    if (Sets.Children[i] is Entry)
                    {
                        /////////////////////////////////////////
                        //////////////////////////////////////////
                        ////
                        ///////////////////////////////////////
                        ///////////////////////////////////////
                        /// 
                        /// 
                        ///
                        ///
                        ///
                        ///
                        ///
                        ///

                    }



                }
            



        }
            else
            {
                DisplayAlert("Missing Data", "Please fill in all the data", "OK");
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
            if (index == Exercises.Count - 2)
            {
                Skip.IsVisible = true;
                Complete.IsVisible = false;
            }
            PopulateSetLayout(Exercises[index]);
            // add the extra set using the dictionary 
            for (int i = 0; i < Exercises[index].Sets + AddedSets[index]; i++)
            {
                Sets.Children.Add(new Entry
                {
                    Text = EntryData[new Tuple<int, int>(index, i)][0].ToString()

                }, 0, i);
                Sets.Children.Add(new Entry
                {
                    Text = EntryData[new Tuple<int, int>(index, i)][1].ToString()
                }, 1, i);
                Sets.Children.Add(new CheckBox
                {
                }, 2, i);
            }
            
        }
        private void Quit_Clicked(object sender, EventArgs e)
        {

        }
        private void Pause_Clicked(object sender, EventArgs e)
        {

        }
        private void AddSet_Clicked(object sender, EventArgs e)
        {
            // get the added sets for the current exercise in the added sets dictionary 
            int addedSets = AddedSets[index];
            int row = Exercises[index].Sets + addedSets + 1;
            Sets.Children.Add(new Entry
            {
                Placeholder = Exercises[index].Reps.ToString() + " Reps",
                Keyboard = Keyboard.Numeric
            }, 0, row);
            Sets.Children.Add(new Entry
            {
                Placeholder = "Weight",
                Keyboard= Keyboard.Numeric
                
            }, 1, row);
            Sets.Children.Add(new CheckBox
            {
            }, 2, row);
            AddedSets[index] = addedSets + 1;
        }
        private void Complete_Clicked(object sender, EventArgs e)
        {

        }
    }
}