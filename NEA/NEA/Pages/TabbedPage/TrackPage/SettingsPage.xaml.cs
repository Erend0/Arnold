﻿using NEA.Pages.TabbedPage.TrackPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NEA.Pages.TabbedPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void UserDataChange_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsDataPage(1));

        }

        private void PinChange_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsDataPage(2));

        }

        private void Blacklist_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsDataPage(3));

        }
        private void WorkoutChange_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsDataPage(4));

        }
    }
}