using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NEA.Pages.TabbedPage.CustomPage;
using NEA.Data;
using NEA.Models.ListViewModels;
using NEA.Models;
using NEA.Pages.TabbedPage;

namespace NEA
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomPage : ContentPage
    {
        protected ObservableCollection<Day> Days { get; set; }
        private int UserID { get; set; }
        public CustomPage()
        {
            InitializeComponent();
            Days = new ObservableCollection<Day>();
            ListofCustomDays.ItemsSource = Days;
            var schedulerepo = new ScheduleRepository();

            var userrepo = new UserRepository();
            UserID = userrepo.GetLoggedInUser().UserID;
            string[] daynames = schedulerepo.GetDays(UserID, 1);
            foreach(string day in daynames)
            {
                if(day != null)
                {
                    Days.Add(new Day { DayName = day });
                }
            }
        }
        private void AddRoutine_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NavigationPage(new ExerciseSearchPage()));
        }

        private void ListofCustomDays_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var day = e.Item as Day;
            Navigation.PushAsync(new DayOverviewPage(UserID, day.DayName, 1));

        }
    }
}