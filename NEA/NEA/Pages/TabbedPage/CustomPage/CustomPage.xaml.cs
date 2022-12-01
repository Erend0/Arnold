
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEA.Data;
using NEA.Pages.TabbedPage.CustomPage;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NEA
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomPage : ContentPage
    {
        

        public CustomPage()
        {
            
            InitializeComponent();
        }
        private void Routine_Pressed(object sender, EventArgs e)
        {
            bool created = false;
            App.Current.MainPage = new RoutineCreatePage();
            if (created)
            {
                App.Current.MainPage = new NavigationPage(new HomePage());
            }

        }
    }
}