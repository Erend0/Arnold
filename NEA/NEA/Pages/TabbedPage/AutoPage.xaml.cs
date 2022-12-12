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
            DaysList.ItemsSource = Days;


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
                DayNames[1] = ("Chest,triceps");
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
        
         private void DaysList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
         {
            // The cell is clicked
            // THe clicked cell is found
            // THe dayname of the clicked cell is used to create a new day overview page 




            
            var day = e.SelectedItem as Day;
            if (day != null)
            {
                Navigation.PushAsync(new DayOverviewPage(UserID,day.DayName));
            }
         }
      
      
    }
}