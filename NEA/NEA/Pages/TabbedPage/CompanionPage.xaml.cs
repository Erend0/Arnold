using System;
using System.Collections.Generic;
using NEA.Models;
using NEA.Models.ListViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NEA.Data;
using NEA.Models.OtherModels;

namespace NEA.Pages.TabbedPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompanionPage : ContentPage
    {
        private int index = 0;
        Dictionary<Tuple<int, int>, int[]> EntryData = new Dictionary<Tuple<int, int>, int[]>();
        Dictionary<int, int> AddedSets = new Dictionary<int, int>();
        public List<ExerciseData> Exercises;
        WorkoutRepository WorkoutRepo = new WorkoutRepository();

        public CompanionPage(List<ExerciseData> exercises)
        {
            WorkoutRepo.DeleteAll();
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
            for (int i = 0; i < exercise.Sets + AddedSets[index]; i++)
            {
                Entry Reps = new Entry {
                    Placeholder = exercise.Reps.ToString() + " Reps",
                    Keyboard = Keyboard.Numeric
                };
                Reps.TextChanged += RepsEntry_TextChanged;
                Sets.Children.Add(Reps,0,i);

                Entry Weight = new Entry
                {
                    Placeholder = "Weigth",
                    Keyboard = Keyboard.Numeric,
                };
                Weight.TextChanged += WeightEntry_TextChanged;
                Sets.Children.Add(Weight, 1, i);
            }
            
        }
        private void RepsEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int row = Grid.GetRow((Entry)sender);
                int value = int.Parse(e.NewTextValue);
                if (EntryData.ContainsKey(new Tuple<int, int>(index, row)))
                {

                    EntryData[new Tuple<int, int>(index, row)][0] = value;
                }
                else
                {
                    EntryData.Add(new Tuple<int, int>(index, row), new int[] { value, 0 });
                }
            }
            catch (Exception)
            {
                if (((Entry)sender).Text.Length > 0)
                {
                    ((Entry)sender).Text = ((Entry)sender).Text.Remove(((Entry)sender).Text.Length - 1);
                }
                else
                {
                    ((Entry)sender).Text = "";
                }
            }
        }
        private void WeightEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                // check if the new input is a number if not remove 1 character from the text if possible
                int row = Grid.GetRow((Entry)sender);
                int value = int.Parse(e.NewTextValue);
                if (EntryData.ContainsKey(new Tuple<int, int>(index, row)))
                {
                    EntryData[new Tuple<int, int>(index, row)][1] = value;
                }
                else
                {
                    EntryData.Add(new Tuple<int, int>(index, row), new int[] { 0, value});
                }
            }
            catch (Exception)
            {
                if (((Entry)sender).Text.Length > 0)
                {
                    ((Entry)sender).Text = ((Entry)sender).Text.Remove(((Entry)sender).Text.Length - 1);
                }
                else
                {
                    ((Entry)sender).Text = "";
                }
            }
            
        }
        private void Skip_Clicked(object sender, EventArgs e)
        {
            UpdateDB();
            if (index < Exercises.Count - 1)
            {
                index++;
                PopulateSetLayout(Exercises[index]);
                FillEntries();
                Back.IsVisible = true;
            }
            if (index == Exercises.Count - 1)
            {
                Complete.IsVisible = true;
                Skip.IsVisible = false;

            }

        }
        private void FillEntries()
        {
            WorkoutTracker[] Logs = WorkoutRepo.GetLogs(index);
          
            // for each item in the log if the sets and reps arent zero then fill the entry
            for (int i = 0; i < Exercises[index].Sets + AddedSets[index]; i++)
            {
                // if logs has an index 
                if (Logs.Length > i)
                {
                    if (Logs[i].Reps != 0)
                    {
                        ((Entry)Sets.Children[i * 2]).Text = Logs[i].Reps.ToString();
                    }
                    if (Logs[i].Weight != 0)
                    {
                        ((Entry)Sets.Children[i * 2 + 1]).Text = Logs[i].Weight.ToString();
                    }
                }
            }
        }
        private void Back_Clicked(object sender, EventArgs e)
        {
            if (index == Exercises.Count - 1)
            {
                UpdateDB();
            }
            index--;
            // Initially the back button is not visible to not go out of bounds for the index of the list this makes it visible
            if (index == 0)
            {
                Back.IsVisible = false;
            }
            // Skip is made invisible at the last exercises, if the user goes back this makes it visible again            if (index == Exercises.Count - 2)
            {
                Skip.IsVisible = true;
                Complete.IsVisible = false;
            }
            PopulateSetLayout(Exercises[index]);
            FillEntries();
        }
        private void Quit_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Quit", "Workout has been saved to resume later", "Ok");
            Navigation.PopAsync();
            
        }
        private void Pause_Clicked(object sender, EventArgs e)
        {

        }
        // This method is used for adding an extra set ( with two entries for weight and reps)
        // The number of added sets are stored in the AddedSets array to be used if the back button is used
        private void AddSet_Clicked(object sender, EventArgs e)
        {
         
            int addedSets = AddedSets[index];
            // The row to add the entries is determined by finding the total number of sets the exercise is meant to have
            int row = Exercises[index].Sets + addedSets;

            // The two entries are added to the grid in opposite columns in the same row
            Entry Reps = new Entry
            {
                Placeholder = Exercises[index].Reps.ToString() + " Reps",
                Keyboard = Keyboard.Numeric
            };
            Reps.TextChanged += RepsEntry_TextChanged;
            Sets.Children.Add(Reps, 0, row);

            Entry Weight = new Entry
            {
                Placeholder = "Weight",
                Keyboard = Keyboard.Numeric
            };
            Weight.TextChanged += WeightEntry_TextChanged;
            Sets.Children.Add(Weight, 1, row);

            
            AddedSets[index] = addedSets + 1;
        }
        // The complete button is made visible in the skip_clicked function when the last exercise is on display 
        // This function checks to see if any of the entries are empty
        // If they are the user is given the option to disregard those sets, or go back and fix them
        private async void Complete_Clicked(object sender, EventArgs e)
        {
            // Each exercise is checked to see if they have an entry for each set, and if they do if each entry has been recorded for the exercise 
            int traverse = 0;
            bool incomplete = false;
            foreach (ExerciseData exercise in Exercises)
            {
                for(int i = 0; i < exercise.Sets + AddedSets[traverse];i++)
                {
                    // check if entry data has a record for each set
                    // and none of the weight or reps is 0 
                    if (!EntryData.ContainsKey(new Tuple<int, int>(traverse, i)) || EntryData[new Tuple<int, int>(traverse, i)][0] == 0 || EntryData[new Tuple<int, int>(traverse, i)][1] == 0)
                    {
                        incomplete = true;
                        break;
                    }
                }
            } 
            if(incomplete == true)
            {
                bool answer = await DisplayAlert("Incomplete", "Some sets have not been recorded, would you like to disregard them?", "Yes", "No");
                if (answer == true)
                {
                   // workout is completed without changing the sets or reps  
                }
                else
                {
                    // the user is given the option to go back and fix the sets 
                }

            }
        }
        private void UpdateDB()
        {
            WorkoutRepo.Update(EntryData, index);

        }
    }
}