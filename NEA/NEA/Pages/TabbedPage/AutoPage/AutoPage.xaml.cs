using NEA.Data;
using System;
using NEA.Pages.TabbedPage;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NEA.Tasks;
using NEA.Models.ListViewModels;

namespace NEA
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AutoPage : ContentPage
    {
        protected ObservableCollection<Day> Days { get; set; }
        ScheduleRepository _ScheduleRepo = new ScheduleRepository();
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
            string[] daynames = _ScheduleRepo.GetDays(UserID, 0);
            foreach (string day in daynames)
            {
                if (day != null)
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
            _ScheduleRepo.DeleteSchedule(UserID);
            DisplayAlert("Success", "All days have been regenerated", "Ok");
            Workout workout = new Workout("all");
        }
    }
}