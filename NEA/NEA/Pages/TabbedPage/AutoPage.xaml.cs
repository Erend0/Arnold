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
        private void Button_Clicked(object sender, EventArgs e)
        {
            Workout workout = new Workout("all");
            List<List<int>> testvar = workout.generatedworkout;

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
                    if (muscleName[0] != "error")
                    {
                        Console.WriteLine(item2 + " " + exercisename + " " + muscleName[0] + " " + muscleName[1]);
                    }
                }
            }


        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            var shedulerepo = new ScheduleRepository();
            shedulerepo.DeleteAll();
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            // log out user
            var userrepo = new UserRepository();
            userrepo.LogoutUser();
            

        }


        // add a child elem

        // add a child element to the stacklayout exercise container


    }
}