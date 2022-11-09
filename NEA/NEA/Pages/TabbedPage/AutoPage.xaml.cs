using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NEA.Data;
using NEA.Tasks;
using System;

namespace NEA
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AutoPage : ContentPage
    {
        public AutoPage()
        {
           
            InitializeComponent();
            // pass the days to generate into workout
            Workout myworkout = new Workout("all");


        }

    }
}