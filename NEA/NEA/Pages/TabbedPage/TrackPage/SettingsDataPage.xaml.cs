using NEA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NEA.Pages.TabbedPage.TrackPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsDataPage : ContentPage
    {
        public SettingsDataPage(int action)
        {
            InitializeComponent();
            if(action == 1)
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
        private void ChangeUserData()
        {
            Picker aimpicker = new Picker
            {
                Title = "Select an aim",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            aimpicker.Items.Add("Muscle Mass");
            aimpicker.Items.Add("Endurance");
            aimpicker.Items.Add("Muscle Strength");

            Picker daypicker = new Picker
            {
                Title = "Select the number of days you can workout",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            daypicker.Items.Add("3");
            daypicker.Items.Add("4");
            daypicker.Items.Add("5");

            // time selector which goes up in increments of two
            // the initial and minimum value is 30 and maximum value is 90
            Stepper timestepper = new Stepper
            {
                Minimum = 30,
                Maximum = 90,
                Increment = 5,
                Value = 30,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Button submit = new Button
            {
                Text = "Submit",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            submit.Clicked += Submit_Clicked;

            // All of the items are added to the stacklayout
            StackLayout layout = new StackLayout
            {
                Children = { aimpicker, daypicker, timestepper, submit }
            };
        }

        // The userdata table is updated with the new data, and the workout is regenerated with the new data
        private void Submit_Clicked(object sender, EventArgs e)
        {
            /////////////////////////////////////////
            /// /////////////////////////////////////////
            ///  /////////////////////////////////////////
            ///   /////////////////////////////////////////
            UserDataRepository userdatarepo = new UserDataRepository();
            UserRepository userrepo = new UserRepository();
            ///userdatarepo.UpdateUserData(userID, aimpicker.SelectedIndex, daypicker.SelectedIndex, timestepper.Value);

        }
        private void ChangePin()
        {
            
        }
        private void ChangeBlacklist()
        {
            
        }
        private void ChangeWorkoutData()
        {
            
        }
        private void ClearGrid()
        {
            
        }
        
    }
}