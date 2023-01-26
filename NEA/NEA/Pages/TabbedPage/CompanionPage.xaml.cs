using System;
using System.Collections.Generic;
using NEA.Models.ListViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NEA.Data;
using NEA.Models.OtherModels;
using System.Diagnostics;

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
        ResumeRepository ResumeRepo = new ResumeRepository();
        private int UserID { get; set; }
        private int Type { get; set; }
        private string DayName { get; set; }
        private Stopwatch Stopwatch { get; set; }
      

        public CompanionPage(List<ExerciseData> exercises, bool resume,int userID, int type, string dayname)
        {
            InitializeComponent();
            Stopwatch = new Stopwatch();
            Stopwatch.Start();

            Exercises = exercises;

            UserID = userID;
            Type = type;
            DayName = dayname;
            NavigationPage.SetHasBackButton(this, false);
            if (!resume)
            {
                WorkoutRepo.DeleteAll();
                ResumeRepo.DeleteAll();
            }
            // The manually added sets for each exercise is recorded in the dictionary 
            // In order to enable adding them to the UI
            if (resume)
            {
                for (int i = 0; i < exercises.Count; i++)
                {
                    var check = ResumeRepo.GetAddedSets(i);
                    if (check != 0)
                    {
                        AddedSets.Add(i, check);
                    }
                }
            }
            // PopulateSetLayout method adds all the entries to the UI
            PopulateSetLayout(exercises[index]);
            // All of the previous data for sets and reps are added to the UI
            if (resume)
            {
                FillEntries();
            }
        }
        private void PopulateSetLayout(ExerciseData exercise)
        {
            if (!AddedSets.ContainsKey(index))
            {
                AddedSets.Add(index, 0);
            }
            
            Sets.Children.Clear();
            ExerciseName.Text = exercise.ExerciseName;

            // For each set of the exercise, two entries are created
            // One for recording reps and one for recording weight
            // A change in text in the entry is recorded in the EntryData dictionary through handler methods
            
            for (int i = 0; i < exercise.Sets + AddedSets[index]; i++)
            {
                Entry Reps = new Entry {
                    Placeholder = exercise.Reps.ToString() + " Reps",
                    // Keyboard is made numeric to restrict potential user error
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
            // Exception handling is performed to prevent the program from crashing from unexpected userdata
            // If the user inputs a non numeric value the last character is removed from the text, i.e. pressed the delete button
            // The dictionary EntryData is updated with the changes
            
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
            /// Determines if the exercise is the last one or not
            if (index < Exercises.Count - 1)
            {
                index++;
                PopulateSetLayout(Exercises[index]);
                FillEntries();
                Back.IsVisible = true;
            }
            // Depending on the page the user is at the complete, and skip buttons visibility is controlled
            // If the user is at the last exercise the complete button is shown
            if (index == Exercises.Count - 1)
            {
                Complete.IsVisible = true;
                Skip.IsVisible = false;
            }
            ResumeRepo.UpdateTime(UserID, Stopwatch.Elapsed.Seconds);
        }

        private void FillEntries()
        {
            // All the data for the entries for a given exercise is retrieved
            WorkoutTracker[] Logs = WorkoutRepo.GetLogs(index);
            // If there is a record of data for the weight or reps the ui is updated with them
            foreach (WorkoutTracker log in Logs)
            {
                if (log.Reps != 0)
                {
                    ((Entry)Sets.Children[log.Set * 2]).Text = log.Reps.ToString();
                }
                if (log.Weight != 0)
                {
                    ((Entry)Sets.Children[log.Set * 2 + 1]).Text = log.Weight.ToString();
                }
            }
        }
        private void Back_Clicked(object sender, EventArgs e)
        {
            
            UpdateDB();
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
            ResumeRepo.UpdateTime(UserID, Stopwatch.Elapsed.Seconds);
        }
        private async void Quit_Clicked(object sender, EventArgs e)
        {
            bool delete =  await DisplayAlert("Quit", "Are you sure you want to quit? all your progress will be lost", "Yes", "No");
            // check if yes or no is clicked
            if (delete)
            {
                WorkoutRepo.DeleteAll();
                ResumeRepo.RemoveRecord(UserID);
                ResumeRepo.DeleteAll();
                Stopwatch.Stop();
                ResumeRepo.ResetTime(UserID);
                Application.Current.MainPage = new NavigationPage(new HomePage());
            }
        }
        private void Pause_Clicked(object sender, EventArgs e)
        {
            ResumeRepo.ChangeDayToResume(UserID, DayName, Type);
            UpdateDB();
            ResumeRepo.UpdateTime(UserID, Stopwatch.Elapsed.Seconds);
            Stopwatch.Stop();
            Application.Current.MainPage = new NavigationPage(new HomePage());
    
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
            UpdateDB();
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
                    RecordProgress();
                    ResumeRepo.DeleteAll();
                    Stopwatch.Stop();
                    ResumeRepo.ResetTime(UserID);
                    ResumeRepo.RemoveRecord(UserID);
                    Application.Current.MainPage = new NavigationPage(new HomePage());
                }
            }
            else
            {
                RecordProgress();
                ResumeRepo.DeleteAll();
                Stopwatch.Stop();
                ResumeRepo.ResetTime(UserID);
                ResumeRepo.RemoveRecord(UserID);
                Application.Current.MainPage = new NavigationPage(new HomePage());

            }
        }
        private void UpdateDB()
        {
            WorkoutRepo.Update(EntryData, index);
            // Check the addedsets to see if there is an entry for the index key
            // if so updare the database
            if (AddedSets.ContainsKey(index))
            {
                ResumeRepo.RecordSets(index, AddedSets[index]);
            }
        }
        
        private void RecordProgress()
        {
            UserDataRepository userdatarepo = new UserDataRepository();
            WorkoutTracker[] workouts = WorkoutRepo.GetAll();
            
            int set = 0;
            int reps = 0;
            int volume = 0;
            
            foreach (WorkoutTracker workout in workouts)
            {
                if (workout.Reps != 0 && workout.Weight != 0)
                {
                    set++;
                }
                reps += workout.Reps;
                volume += workout.Reps * workout.Weight;
            }
            int totaltime = Stopwatch.Elapsed.Seconds;
            userdatarepo.LogWorkoutData(UserID,set, reps, volume,totaltime);

        }
        
        
    }
}