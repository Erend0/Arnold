using NEA.Data;
using NEA.Models;
using NEA.Models.ListViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace NEA.Pages.TabbedPage.TrackPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsDataPage : ContentPage
    {
        public int UserID { get; set; }
        public SettingsDataPage(int action)
        {
            InitializeComponent();
            var userepo = new UserRepository();
            UserID = userepo.GetLoggedInUser().UserID;
                


            if (action == 1)
            {
                ChangeUserData();
            }
            else if (action == 2)
            {
                ChangePin();
            }
            else if (action == 3)
            {
                ChangeBlacklist();
            }
            else if(action == 4)
            {
                ChangeWorkoutData();
            }
        }
        // This method allows the user to change their aim, time available and day available 
        // Note that the user is given the choice to manually regenerate their workout
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
            // create a numberic password entry 
            Entry pinentry = new Entry
            {
                Placeholder = "Enter your new pin",
                IsPassword = true,
                Keyboard = Keyboard.Numeric,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            // create a button to submit the new pin
            Button submit = new Button
            {
                Text = "Submit",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            // if button is clicked and pin is not null update db
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
            muscle.Clicked += (sender, e) => MuscleBlacklist_Clicked(sender, e);
            //exercise.Clicked += (sender, e) => ExerciseBlacklist_Clicked(sender, e);
            //machine.Clicked += (sender, e) => MachineBlacklist_Clicked(sender, e);
            root.Children.Add(header);
            root.Children.Add(muscle);
            root.Children.Add(exercise);
            root.Children.Add(machine);

            // delete all blaclists button
            Button deleteall = new Button
            {
                Text = "Reset all blacklists"
            };
            deleteall.Clicked += (sender, e) => DeleteAllBlacklists_Clicked(sender, e);
            root.Children.Add(deleteall);

        }

        private void MuscleBlacklist_Clicked(object sender, EventArgs e)
        {
            root.Children.Clear();

            MuscleRepository muscleRepo = new MuscleRepository();
            List<Muscle> muscles = muscleRepo.GetAllMuscleNames();

            BlacklistRepository blacklistrepo = new BlacklistRepository();
            List<string> blacklistedmuscles = blacklistrepo.GetBlacklistedMuscles(UserID);

            ObservableCollection<Blacklist> listviewitems = new ObservableCollection<Blacklist>();
            foreach(Muscle m in muscles)
            {
                
                listviewitems.Add(new Blacklist
                {
                    Name = m.MinorMuscle,
                    IsChecked = blacklistedmuscles.Contains(m.MinorMuscle)
                });

            }
            BlacklistView.ItemsSource = listviewitems;
        }
        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var label = ((CheckBox)sender).BindingContext as Blacklist;
            var cb = (CheckBox)sender;
            
            
            BlacklistRepository blacklistrepo = new BlacklistRepository();
            if (cb.IsChecked)
            {
                blacklistrepo.AddMuscleToBlacklist(UserID, label.Name);
            }
            else
            {
                blacklistrepo.RemoveMuscleFromBlacklist(UserID, label.Name);
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
            
        private void ChangeWorkoutData()
        {
        }

       
    }
}