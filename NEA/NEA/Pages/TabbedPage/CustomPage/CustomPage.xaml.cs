using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NEA.Pages.TabbedPage.CustomPage;
using NEA.Data;
using NEA.Models.ListViewModels;

namespace NEA
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomPage : ContentPage
    {
        public ObservableCollection<Day> Days { get; set; }
        public CustomPage()
        {
            InitializeComponent();
            Days = new ObservableCollection<Day>();
            ListofCustomDays.ItemsSource = Days;
            var schedulerepo = new ScheduleRepository();

            var userrepo = new UserRepository();
            int UserID = userrepo.GetLoggedInUser().UserID;
            string[] daynames = schedulerepo.GetDays(UserID, 1);
            foreach(string day in daynames)
            {
                if(day != null)
                {
                    Days.Add(new Day { DayName = day });
                }
            }
        }
        private void Routine_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NavigationPage(new ExerciseSearchPage()));
        }
    }
}