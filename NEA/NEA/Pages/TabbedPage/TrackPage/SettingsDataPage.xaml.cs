using NEA.Data;
using NEA.Models;
using NEA.Models.ListViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace NEA.Pages.TabbedPage.TrackPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsDataPage : ContentPage
    {
        public int UserID { get; set; }
        // The blacklist type variable is set when a user selects a blacklist type in the ChangeBlacklist method
        // This variable is used to determine which blacklist type the user is currently viewing, in order to update the right DB table
        public string BlacklistType { get; set; }
        public SettingsDataPage(int action)
        {
            InitializeComponent();
            var userepo = new UserRepository();
            UserID = userepo.GetLoggedInUser().UserID;
            // Depending on the parameter, different functions will populate the page with different UI items to fullfill their purpose
            switch (action)
            {
                case 1:
                    ChangeUserData();
                    break;
                case 2:
                    ChangePin();
                    break;
                case 3:
                    ChangeBlacklist();
                    break;
                case 4:
                    ChangeWorkoutData();
                    break;
                    


            }
        }
        // This method allows the user to change their aim, time available and day available 
        // Note that the user is given the choice to manually regenerate their workout 
        // Rather than generating it automatically
        private void ChangeUserData()
        {
            Picker aimpicker = new Picker
            {
                Title = "Select an aim",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                SelectedIndex = 1

            };
            aimpicker.Items.Add("Muscle Mass");
            aimpicker.Items.Add("Endurance");
            aimpicker.Items.Add("Muscle Strength");
            

            Picker daypicker = new Picker
            {
                Title = "Select the number of days you can workout",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                SelectedIndex = 1
            };
            daypicker.Items.Add("3");
            daypicker.Items.Add("4");
            daypicker.Items.Add("5");

            // time selector which goes up in increments of two
            // the initial and minimum value is 30 and maximum value is 90
            Label steppertext = new Label
            {
                Text = "30",
                TextColor = Color.White,
            };
               
            Stepper timestepper = new Stepper
            {
               
                Minimum = 30,
                Maximum = 90,
                Increment = 5,
                Value = 30,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            // stepper value changed
            timestepper.ValueChanged += (sender, e) =>
            {
                steppertext.Text = string.Format(Convert.ToString(e.NewValue)+" minutes");
            };

            Button submit = new Button
            {
                Text = "Submit",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            submit.Clicked += (sender, e) => Submit_Clicked(sender, e, aimpicker.Items[aimpicker.SelectedIndex], Convert.ToInt32(timestepper.Value), Convert.ToInt32(daypicker.Items[daypicker.SelectedIndex]));



            root.Children.Add(aimpicker);
            root.Children.Add(daypicker);
            root.Children.Add(timestepper);
            root.Children.Add(steppertext);
            root.Children.Add(submit);
        }

        // The userdata table is updated with the new data, and the workout is regenerated with the new data
        private void Submit_Clicked(object sender, EventArgs e, string aim, int time,int days)
        {
            UserDataRepository userDataRepo = new UserDataRepository();
            userDataRepo.UpdateUserData(UserID,time*60,days,aim);
            DisplayAlert("Success",
                "Your data has been updated, Please regenerate your auto generated workout to see the effect",
                "OK") ;
            Navigation.PopAsync();

        }
        private void ChangePin()
        {
            // creates a numberic password entry 
            Entry pinentry = new Entry
            {
                Placeholder = "Enter your new pin",
                IsPassword = true,
                Keyboard = Keyboard.Numeric,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            // creates a button to submit the new pin
            Button submit = new Button
            {
                Text = "Submit",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            // if button is clicked and pin is not null db is updated
            submit.Clicked += (sender, e) => SubmitPin_Clicked(sender, e, pinentry.Text);
            root.Children.Add(pinentry);
            root.Children.Add(submit);
        }
        private void SubmitPin_Clicked(object sender, EventArgs e, string pin)
        {
            if (pin != null)
            {
                UserRepository userRepo = new UserRepository();
                userRepo.ChangeUserPin(Convert.ToInt32(pin));
                DisplayAlert("Success",
                    "Your pin has been updated",
                    "OK");
                Navigation.PopAsync();
            }
            // if pin is not 4 digits error
            else if (pin.Length != 4)
            {
                DisplayAlert("Error",
                    "Please enter a 4 digit pin",
                    "OK");
            }
            else
            {
                DisplayAlert("Error",
                    "Please enter a pin",
                    "OK");
            }
        }
        private void ChangeBlacklist()
        {
            // Provides buttons for user to select which db they want to edit
            Label header = new Label
            {
                Text = "Pick a blacklist to access",
                TextColor = Color.White,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };
            Button muscle = new Button
            {
                Text = "Muscle Blacklist",
            };

            Button exercise = new Button
            {
                Text = "Exercise Blacklist"
            };
            Button machine = new Button
            {
                Text = "Machine Blacklist"
            };
            
            // Sets the handler for the clicked of each button to the function below 
            // The third paramter determines which db tables will be accessed
            muscle.Clicked += (sender, e) => BlacklistButton_Clicked(sender, e, "Muscle");
            exercise.Clicked += (sender, e) => BlacklistButton_Clicked(sender, e, "Exercise");
            machine.Clicked += (sender, e) => BlacklistButton_Clicked(sender, e, "Machine");

            root.Children.Add(header);
            root.Children.Add(muscle);
            root.Children.Add(exercise);
            root.Children.Add(machine);

            // deletes all blaclists button
            Button deleteall = new Button
            {
                Text = "Reset all blacklists"
            };
            deleteall.Clicked += (sender, e) => DeleteAllBlacklists_Clicked(sender, e);
            root.Children.Add(deleteall);

        }

        private void BlacklistButton_Clicked(object sender, EventArgs e, string blacklistType)
        {
            root.Children.Clear();
            BlacklistType = blacklistType;

            switch (blacklistType)
            {
                case "Muscle":
                    // The listview is populated with all the muscles
                    // The checkboxes for the muscles are checked if they are in the blacklist table
                    
                    MuscleRepository muscleRepo = new MuscleRepository();
                    List<Muscle> muscles = muscleRepo.GetAllMuscleNames();

                    BlacklistRepository muscleBlacklistRepo = new BlacklistRepository();
                    
                    List<string> blacklistedMuscles = muscleBlacklistRepo.GetBlacklistedMuscles(UserID);
                    ObservableCollection<Blacklist> muscleListViewItems = new ObservableCollection<Blacklist>();
                    foreach (Muscle m in muscles)
                    {
                        muscleListViewItems.Add(new Blacklist
                        {
                            Name = m.MinorMuscle,
                            IsChecked = blacklistedMuscles.Contains(m.MinorMuscle),
                        });
                    }
                    BlacklistView.ItemsSource = muscleListViewItems;
                    break;

                case "Exercise":
                    // The listview is populated with all the exercises
                    // The checkboxes for the exercises are checked if they are in the blacklist table
                    
                    ExerciseRepository exerciseRepo = new ExerciseRepository();
                    List<Exercise> exercises = exerciseRepo.GetAllExercises();

                    BlacklistRepository exerciseBlacklistRepo = new BlacklistRepository();
                    List<string> blacklistedExercises = exerciseBlacklistRepo.GetBlacklistedExercises(UserID);

                    ObservableCollection<Blacklist> exerciseListViewItems = new ObservableCollection<Blacklist>();
                    foreach (Exercise ex in exercises)
                    {
                        exerciseListViewItems.Add(new Blacklist
                        {
                            Name = ex.ExerciseName,
                            IsChecked = blacklistedExercises.Contains(ex.ExerciseName)
                        });
                    }
                    BlacklistView.ItemsSource = exerciseListViewItems;
                    break;

                case "Machine":
                    // The listview is populated with all the machines
                    // The checkboxes for the machines are checked if they are in the blacklist table
                    
                    MachineRepository machineRepo = new MachineRepository();
                    List<Machine> machines = machineRepo.GetAllMachines();

                    BlacklistRepository machineBlacklistRepo = new BlacklistRepository();
                    List<string> blacklistedMachines = machineBlacklistRepo.GetBlacklistedMachines(UserID);

                    ObservableCollection<Blacklist> machineListViewItems = new ObservableCollection<Blacklist>();
                    foreach (Machine m in machines)
                    {
                        machineListViewItems.Add(new Blacklist
                        {
                            Name = m.MachineName,
                            IsChecked = blacklistedMachines.Contains(m.MachineName)
                        });
                    }
                    BlacklistView.ItemsSource = machineListViewItems;
                    break;
            }
        }


        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            // This is the handler for the checkboxes in the listview
            // The name of the muscle/exercise/machine is obtained from the binding context
            // And using the name necessary database tables are updated to add/remove the muscle/exercise/machine from the blacklist
            
            var label = ((CheckBox)sender).BindingContext as Blacklist;
            var cb = (CheckBox)sender;
            BlacklistRepository blacklistrepo = new BlacklistRepository();

            switch (BlacklistType)
            {
                case "Muscle":
                    if (cb.IsChecked)
                    {
                        blacklistrepo.AddMuscleToBlacklist(UserID, label.Name);
                    }
                    else
                    {
                        blacklistrepo.RemoveMuscleFromBlacklist(UserID, label.Name);
                    }
                    break;

                case "Exercise":
                    if (cb.IsChecked)
                    {
                        blacklistrepo.AddExerciseToBlacklist(UserID, label.Name);
                    }
                    else
                    {
                        blacklistrepo.RemoveExerciseFromBlacklist(UserID, label.Name);
                    }
                    break;

                case "Machine":
                    if (cb.IsChecked)
                    {
                        blacklistrepo.AddMachineToBlacklist(UserID, label.Name);
                    }
                    else
                    {
                        blacklistrepo.RemoveMachineFromBlacklist(UserID, label.Name);
                    }
                    break;
            }
        }



        private void DeleteAllBlacklists_Clicked(object sender, EventArgs e)
        {
            BlacklistRepository blacklistrepo = new BlacklistRepository();
            blacklistrepo.DeleteAllBlacklists(UserID);
            DisplayAlert("Success",
                "All blacklists have been reset",
                "OK");
            Navigation.PopAsync();
        }
            
        // This method allows the changing of userdata related to workouts manually
        private void ChangeWorkoutData()
        {
            mainroot.Children.Clear();


            UserDataRepository userdatarepo = new UserDataRepository();
            int[] workoutdata = userdatarepo.GetWorkoutData(UserID);

            // Create necessary UI elements
            Label setsLabel = new Label { Text = "Sets: " + workoutdata[0].ToString() };
            Label repsLabel = new Label { Text = "Reps: " + workoutdata[1].ToString() };
            Label volumeLabel = new Label { Text = "Volume: " + workoutdata[2].ToString() };
            Label timeLabel = new Label { Text = "Time: " + workoutdata[3].ToString() };
            Label workoutsLabel = new Label { Text = "Number of Workouts: " + workoutdata[4].ToString() };

            Stepper setsStepper = new Stepper { Minimum = 0, Maximum = 100000, Increment = 1, Value = workoutdata[0] };
            Stepper repsStepper = new Stepper { Minimum = 0, Maximum = 100000, Increment = 1, Value = workoutdata[1] };
            Stepper volumeStepper = new Stepper { Minimum = 0, Maximum = 10000000000, Increment = 10, Value = workoutdata[2] };
            Stepper timeStepper = new Stepper { Minimum = 0, Maximum = 3600000000000, Increment = 10, Value = workoutdata[3] };
            Stepper workoutsStepper = new Stepper { Minimum = 0, Maximum = 1000000, Increment = 1, Value = workoutdata[4] };

            Button saveButton = new Button { Text = "Save" };
            Button cancelButton = new Button { Text = "Cancel" };

            // Create a stack layout to hold the UI elements
            StackLayout root = new StackLayout { Padding = new Thickness(20) };


            // Add the UI elements to the stack layout
            mainroot.Children.Add(setsLabel);
            mainroot.Children.Add(setsStepper);
            mainroot.Children.Add(repsLabel);
            mainroot.Children.Add(repsStepper);
            mainroot.Children.Add(volumeLabel);
            mainroot.Children.Add(volumeStepper);
            mainroot.Children.Add(timeLabel);
            mainroot.Children.Add(timeStepper);
            mainroot.Children.Add(workoutsLabel);
            mainroot.Children.Add(workoutsStepper);
            mainroot.Children.Add(saveButton);


            // Handle stepper value changes to update labels
            setsStepper.ValueChanged += (sender, e) =>
            {
                setsLabel.Text = "Sets: " + setsStepper.Value.ToString();
            };

            repsStepper.ValueChanged += (sender, e) =>
            {
                repsLabel.Text = "Reps: " + repsStepper.Value.ToString();
            };

            volumeStepper.ValueChanged += (sender, e) =>
            {
                volumeLabel.Text = "Volume: " + volumeStepper.Value.ToString()+"kg";
            };

            timeStepper.ValueChanged += (sender, e) =>
            {
                timeLabel.Text = "Time: " + timeStepper.Value.ToString() +" seconds";
            };

            workoutsStepper.ValueChanged += (sender, e) =>
            {
                workoutsLabel.Text = "Number of Workouts: " + workoutsStepper.Value.ToString();
            };

            saveButton.Clicked += (sender, e) =>
            {
                // Update the workout data in the database
                userdatarepo.LogWorkoutData(UserID, (int)setsStepper.Value - workoutdata[0], (int)repsStepper.Value - workoutdata[1], (int)volumeStepper.Value - workoutdata[2], (int)timeStepper.Value - workoutdata[3]);
                userdatarepo.ChangeNumberofWorkouts(UserID, (int)workoutsStepper.Value);
                Application.Current.MainPage = new NavigationPage(new HomePage());
            };

        }



    }
}