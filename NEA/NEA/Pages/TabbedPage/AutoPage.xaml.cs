using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NEA.Tasks;
using NEA.Data;
using System.Collections.Generic;
using System;
namespace NEA
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AutoPage : ContentPage
    {
        public AutoPage()
        {
           
            InitializeComponent();
            
        }

        private void Button2_Pressed(object sender, System.EventArgs e)
        {
           // pass the days to generate into workout
           Workout myworkout = new Workout("Biceps");
           List<List<int>> testvar = myworkout.generatedworkout;
            // creates console output showing all the contents of testvar
            foreach (var item in testvar)
            {
                foreach (var item2 in item)
                {
                    Console.WriteLine(item2);
                }
            }


        }
    }
}