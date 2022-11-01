using NEA.Data;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace NEA.Tasks
{
    public class CreateWorkout
    {
        public string[] splitdata()
        {
            var Muscles = new Dictionary<string, string[]>()
            {
                {"Chest",new string[]{"upper","lower","inner"} },
                {"Triceps",new string[]{"longhead","lateralhead","medialhead"} },
                {"Back",new string[]{"lats","middleback","lowerback"} },
                {"Biceps",new string[]{"long","short"} },
                {"Shoulders",new string[]{"anteriordelts","medialdelts","posteriordelts"} },
                {"Legs", new string[] {"Quadriceps","Hamstrings","Glutes","Calves","Abductors"} },
            };
            string[] placeholder = new string[10];
            return placeholder;
        }

        public void generateworkout(string daytofill)
        {
            var userrepo = new UserRepository();
            int currentuserid = Convert.ToInt32(userrepo.GetLoggedInUser());
            // uses the getuserdata method to get the aim, time and days available from the userdata table
            var userdatarepo = new UserDataRepository();
            string[] userdata = userdatarepo.GetUserData(currentuserid);

            string useraim = userdata[0];
            int usertime = Convert.ToInt32(userdata[1]);
            int userdays = Convert.ToInt32(userdata[2]);
            //string[] splitdays = fillsplit(userdays);
            if (daytofill == "all")
            {
                for (int i = 0; i < userdays; i++)
                {
                   // fillsplit(splitdays[i]);

                }

            }
            else
            {
                fillsplit(daytofill);

            }


        }

        private void fillsplit(string dayname)
        {
            // determines if there are multiple muscle groups in the split 
            int numberofmuscles = 0;
            string mystr = "";
            for (int i = 0; i < dayname.Length; i++)
            {
                mystr = dayname[i].ToString();
                if (mystr == ",") {
                    numberofmuscles = numberofmuscles + 1;
                }
                // if there is only one muscle group in the split
                if (numberofmuscles == 1)
                {
                    Console.Write("fill");

                }
                else if (numberofmuscles == 2)
                {
                    Console.Write("fill");

                }
                else
                {
                    Console.Write("fill");

                }
            }


            public string[] createsplits(int days)
            {
                string[] chest = { "upper", "lower", "inner" };
                string[] triceps = { "longhead", "lateralhead", "medialhead" };
                string[] back = { "lats", "middleback", "lowerback" };
                string[] biceps = { "long", "short" };
                string[] shoulders = { "anteriordelts", "medialdelts", "posteriordelts" };
                string[] legs = { "Quadriceps", "Hamstrings", "Glutes", "Calves", "Abductors" };

                if (days == 3)
                {
                    string[] splitdays = { "Chest,Triceps,Legs", "Back,Biceps,Shoulders", "Bicep,Chest,Legs" };
                    return splitdays;
                }
                else if (days == 4)
                {
                    string[] splitdays = { "Back,Biceps", "Shoulders", "Chest,triceps", "Legs" };
                    return splitdays;
                }
                else
                {
                    string[] splitdays = { "Back,Biceps", "Shoulders", "Chest,triceps", "Legs", "Cardio" };
                    return splitdays;
                }
                
            }
        }
    }
}
