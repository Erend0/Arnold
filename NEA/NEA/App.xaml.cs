using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NEA
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            // create a new navigation page
            MainPage = new NavigationPage(new HomePage());

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
