﻿using NEA.Data;
using System;
using System.Collections.Generic;





namespace NEA.Tasks
{
    public class Workout
    {
        // General variables that will be used to generate the schedule
        private readonly Dictionary<string, string[]> Muscles = new Dictionary<string, string[]>()
        {
                {"Chest",new string[]{"upper","lower","inner"} },
                {"Triceps",new string[]{"longhead","lateralhead","medialhead"} },
                {"Back",new string[]{"lats","middleback","lowerback"} },
                {"Biceps",new string[]{"long","short"} },
                {"Shoulders",new string[]{"anteriordelts","medialdelts","posteriordelts"} },
                {"Legs", new string[] {"quadriceps","hamstrings","glutes","calves","abductors"} },
        };
        private int userdays { get; set; }
        private string useraim { get; set; }
        private int usertime { get; set; }
        private string[] chest = { "upper", "lower", "inner" };
        private string[] triceps = { "longhead", "lateralhead", "medialhead" };
        private string[] back = { "lats", "middleback", "lowerback" };
        private string[] biceps = { "longhead", "shorthead" };
        private string[] shoulders = { "anteriordelts", "medialdelts", "posteriordelts" };
        private string[] legs = { "quadriceps", "hamstrings", "glutes", "calves", "abductors" };
        private string[] cardio = {"cardio" };
        // the variable below is used by the fill split method to determine how many muscles there are in each day of splits
        private int numberofmuscles { get; set; }

        // The constructor
        public Workout(string togenerate)
        {
            
            GetUserData();
            GenerateWorkout(togenerate);

        }
        public void GetUserData()
        {
            // GetLoggedInUser method from the UserRepository is used to fetch the currentley logged in users id from DB
            var userrepo = new UserRepository();
            int currentuserid = userrepo.GetLoggedInUser().UserID;
            
            // Getuserdata method is used to get the aim, time and days available from the userdata table in the form of a array 
            var userdatarepo = new UserDataRepository();
            string[] userdata = userdatarepo.GetUserData(currentuserid);

            string useraim = userdata[0];
            int usertime = Convert.ToInt32(userdata[1]);
            int userdays = Convert.ToInt32(userdata[2]);


        }
        public void GenerateWorkout(string togenerate)
        {
            if (togenerate == "all")
            {
                Queue<string[][]> split = returnsplitqueue();
                // all the days are individally generated by passing the days into the fillsplit method
                foreach(string[][] day in split)
                {
                    fillsplit(day);
                }
            }
            // the element in the queue containing the queue will be found 
            else
            {
            
            }

        }
        public Queue<string[][]> returnsplitqueue()
        {
            Queue<string[][]> split = new Queue<string[][]>();
            // full body split 
            // jagged arrays are used to store the muscle groups that will be worked on each day
            if (userdays == 3) {
                string[][]day1 = {chest,triceps, legs};
                string[][] day2 = {back,biceps,shoulders};
                string[][] day3 = { biceps,legs,chest};
                split.Enqueue(day1);
                split.Enqueue(day2);
                split.Enqueue(day3);
                numberofmuscles = 3;
                return split;            }
            else
            {
                string[][] day1 = { chest, triceps };
                string[][] day2 = { back, biceps };
                string[][] day3 = { shoulders };
                string[][] day4 = { legs };
                split.Enqueue(day1);
                split.Enqueue(day2);
                split.Enqueue(day3);
                split.Enqueue(day4);
                numberofmuscles = 2;
                if (userdays == 5)
                {
                    string[][] day5 = { cardio };
                    split.Enqueue(day5);
                    return split;
                }
                else
                {
                    return split;
                }
             }  
        }
       

        
        public void fillsplit(string[][] day)
        {
            int totaltimetaken = 0;
            int timeforeachfocus = usertime / day.Length;
            // The musclefocus is in the form of, Triceps and Chest, each of these will be filled evenly
            foreach (string[] musclefocus in day)
            {
                int index = 0;
                // The musclefocus is stopped filling if time allocated to it runs out, or too little time is left ( less than or equal to 4 mins)
                while(totaltimetaken != timeforeachfocus | (timeforeachfocus-totaltimetaken)>=4)
                {
                    int exercisefound = FindExerciseforMinorMuscle(musclefocus[index]);
                    bool hasblacklisted = true;
                    while (!hasblacklisted)
                    {
                        hasblacklisted = CheckBlackLists(exercisefound); 
                    }

                   
                    UpdateTimeTaken(exercisefound);
                    UpdateDB();
                    // the index is used to repeatedly go over the minor muscles e.g. Hamstring, Glutes to fill them evenly
                    index += 1;
                    if(index == musclefocus.Length+1)
                    {
                        index = 0;
                    }
                }

            } 
        }
        public int FindExerciseforMinorMuscle(string minormuscle)
        {
            // use the GetMuscleID method with the minor muscle parameter to get the id of the minor muscle
            var musclerepo = new  MuscleRepository();
            int muscleid = musclerepo.GetMuscleID(minormuscle);
            // use the id of the muscle to find the exercises it links to 
            int[] exercises = musclerepo.GetExercises(muscleid);

            // Uses a random integer to pick a random exercise from the array of exercises
            Random random = new Random();
            int num = random.Next(0, exercises.Length);
            

            


            return exercises[num];
        }
        public bool CheckBlackLists(int exercisefound)
        {
            return true;
        }
        public void UpdateTimeTaken(int exerciseID)
        {
            
            

        }
        public void UpdateDB()
        {

        }
       
      

    }   
}
