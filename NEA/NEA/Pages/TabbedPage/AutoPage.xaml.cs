using NEA.Data;
using NEA.Models;
using System;
using NEA.Pages.TabbedPage;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NEA
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AutoPage : ContentPage
    {
        public ObservableCollection<Day> Days { get; set; }
        int UserID { get; set; }
        int UserDays { get; set; }
        


        public AutoPage()
        {
            InitializeComponent();
            Days = new ObservableCollection<Day>();
            ListofDays.ItemsSource = Days;
            
            var userRepo = new UserRepository();
            UserID = userRepo.GetLoggedInUser().UserID;
            var userdataRepo = new UserDataRepository();
            UserDays = Convert.ToInt32(userdataRepo.GetUserData(UserID)[2]);
            Populatecollection();
        }
        public void Populatecollection()
        {
            string[] DayNames = new string[5];
            if (UserDays == 3)
            {
                DayNames[1] = ("Chest,Tricep,Legs");
                DayNames[2] = ("Back,Biceps,Shoulders");
                DayNames[3] = ("Biceps,legs,chest");
            }
            if (UserDays == 4 || UserDays == 5)
            {
                DayNames[1] = ("Chest,Triceps");
                DayNames[2] = ("Back,Biceps");
                DayNames[3] = ("Shoulders");
                DayNames[4] = ("Legs");
            }
            if (UserDays == 5)
            {
                DayNames[5] = ("Cardio");
            }
            foreach (string day in DayNames)
            {
                if(day != null)
                {
                    Days.Add(new Day { DayName = day });
                }
            }
        }
        private void ListofDays_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var day = e.Item as Day;
            Navigation.PushAsync(new DayOverviewPage(UserID, day.DayName));
        }

        private void Regenerate_Clicked(object sender, EventArgs e)
        {

        }
    }
}