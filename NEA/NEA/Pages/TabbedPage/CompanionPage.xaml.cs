using System;
using System.Collections.Generic;
using NEA.Models;
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
        public List<ExerciseData> Exercises;
        
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
                Console.WriteLine(value);
                Console.WriteLine(row);
                if (EntryData.ContainsKey(new Tuple<int, int>(index, row)))
                {
                    EntryData[new Tuple<int, int>(index, row)][0] = value;
                }
                else
                {
                    EntryData.Add(new Tuple<int, int>(index, row), new int[] { value, 0, 0 });
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
                Console.WriteLine(value);
                Console.WriteLine(row);
                if (EntryData.ContainsKey(new Tuple<int, int>(index, row)))
                {
                    EntryData[new Tuple<int, int>(index, row)][1] = value;
                }
                else
                {
                    EntryData.Add(new Tuple<int, int>(index, row), new int[] { 0, value, 0 });
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
            if (index < Exercises.Count - 1)
            {
                index++;
                PopulateSetLayout(Exercises[index]);
                FillEntries();
                Back.IsVisible = true;
            }
            else
            {
                Complete.IsVisible = true;
            }

        }
        private void FillEntries()
        {
            for (int i = 0; i < Exercises[index].Sets + AddedSets[index]; i++)
            {
                if (EntryData.ContainsKey(new Tuple<int, int>(index, i)))
                {
                    ((Entry)Sets.Children[i * 2]).Text = EntryData[new Tuple<int, int>(index, i)][0].ToString();
                    ((Entry)Sets.Children[i * 2 + 1]).Text = EntryData[new Tuple<int, int>(index, i)][1].ToString();
                }
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
            FillEntries();
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
            int row = Exercises[index].Sets + addedSets;

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
        private void Complete_Clicked(object sender, EventArgs e)
        {

        }
        private void UpdateDB()
        {

        }
    }
}