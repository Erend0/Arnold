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
                ChangePin();
            }
            if (action == 2)
            {
            }
            if (action == 3)
            {
                ChangeBlacklist();
            }
        }
        private void ChangePin()
        {
            
        }
        private void ChangeData()
        {
            
        }
        private void ChangeBlacklist()
        {
            
        }
    }
}