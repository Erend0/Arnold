using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NEA.Tasks;
using NEA.Data;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;

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
            Workout myworkout = new Workout("all");
            List<List<int>> testvar = myworkout.generatedworkout;
            // creates console output showing all the contents of testvar
            foreach (var item in testvar)
            {
                foreach (var item2 in item)
                {
                    var muscletargetedrepo = new MuscleTargetedRepository();
                    var exerciserepo = new ExerciseRepository();
                    string exercisename = exerciserepo.GetExerciseName(item2);
                    int muscleid = muscletargetedrepo.GetMuscleID(item2);
                    var muscle = new MuscleRepository();
                    string[] muscleName = muscle.GetMuscleName(muscleid);
                    Console.WriteLine(item2 + " " + exercisename + " " + muscleName[0] + " " + muscleName[1]);

                }
            }


        }

        private void Add_Pressed(object sender, EventArgs e)
        {
            


        }
    }
}