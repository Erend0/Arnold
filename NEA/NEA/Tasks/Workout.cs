﻿using NEA.Data;
using NEA.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NEA.Tasks
{
    public class Workout
    {
        // General variables that will be used to generate the schedule
        private int userdays { get; set; }
        private string useraim { get; set; }
        // Note that usertime is in seconds by default
        private int usertime { get; set; }
        private readonly string[] chest = { "Upper", "Lower", "Inner" };
        private readonly string[] triceps = { "Longhead", "LateralHead", "MedialHead" };
        private readonly string[] back = { "Lats", "MiddleBack", "LowerBack" };
        private readonly string[] biceps = { "LongHead", "ShortHead" };
        private readonly string[] shoulders = { "Anteriordelts", "Medialdelts", "Posteriordelts" };
        private readonly string[] legs = { "Quadriceps", "Hamstrings", "Glutes", "Calves"};
        private readonly string[] cardio = {"Cardio" };
        private int exercisefound { get; set; }

        // the variable below is used by the fill split method to determine how many muscles there are in each day of splits
        private int totaltimetaken { get; set;}
        private int numberofmuscles { get; set; }
        public List<List<int>> generatedworkout { get; set; } = new List<List<int>>();
        public int Currentuserid { get; set; }
        public int currentmuscleID { get; set; }
       
        public Workout(string togenerate)
        {
            GetUserData();
            GenerateWorkout(togenerate);
        }
        public void GetUserData()
        {
            var userrepo = new UserRepository();
            Currentuserid = userrepo.GetLoggedInUser().UserID;
            var userdatarepo = new UserDataRepository();
            // The userdaterepo is used to return an array of the users aim,time and days given the userID
            string[] userdata = userdatarepo.GetUserData(Currentuserid);

            useraim = userdata[0];
            usertime = Convert.ToInt32(userdata[1]);
            userdays = Convert.ToInt32(userdata[2]);
        }
        public void GenerateWorkout(string togenerate)
        {
            if (togenerate == "all")
            {
                Queue<string[][]> split = returnsplitqueue();
                // all the days are individally generated by passing the days into the fillsplit method
                foreach(string[][] day in split)
                {
                    Fillsplit(day);
                }
            }
            // the element in the queue containing the queue will be found 
            else
            {
                string[][] day = { };
                if (togenerate == "biceps")
                {
                    day.Append(biceps);
                    Fillsplit(day);
                }
                else if (togenerate == "back")
                {
                    day.Append(back);
                    Fillsplit(day);

                }
                else if (togenerate == "chest")
                {
                    day.Append(chest);
                    Fillsplit(day);
                }
                  
                else if (togenerate == "triceps") {
                    day.Append(triceps);
                    Fillsplit(day);
                }
                else if (togenerate == "legs")
                {
                    day.Append(legs);
                    Fillsplit(day);
                }
                
                
                
                
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
                string[][] day3 = { shoulders, shoulders };
                string[][] day4 = { legs, legs};
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

       

        public void Fillsplit(string[][] day)
        {
            int index = 0;
            int timeforeachfocus = usertime / day.Length;
            for (int i = 0; i < day.Length-1; i++)
            {
                // Note that totaltimetaken is also in the format of minutes, same as the usertime variable
                totaltimetaken = 0;
                List<int> exercises = new List<int>();
                while (totaltimetaken != timeforeachfocus && (timeforeachfocus - totaltimetaken) >= 240)
                {
                    bool hasblacklisted = true;
                    bool hasduplicate = true;
                    while (hasblacklisted || hasduplicate)
                    {
                        
                        exercisefound = FindExerciseforMinorMuscle(day[i][index]);
                        hasblacklisted = CheckBlackLists(exercisefound);
                        hasduplicate = CheckDuplicates(exercises, exercisefound);
                        
                    }
                    UpdateTimeTaken(exercisefound);
                    exercises.Add(exercisefound);
                  
                 

                    // if index is over the size reset is to zero, if not increment
                    if (index == day[i].Length - 1)
                    {
                        index = 0;
                    }
                    else
                    {
                        index++;
                    }

                }
                
                generatedworkout.Add(exercises);
            } 
        }
        public int FindExerciseforMinorMuscle(string minormuscle)
        {
            // use the GetMuscleID method with the minor muscle parameter to get the id of the minor muscle
            var muscletargetedrepo = new  MuscleTargetedRepository();
            var musclerepo = new MuscleRepository();
            currentmuscleID = musclerepo.FindMuscleID(minormuscle);
            Console.WriteLine("Muscleid is " + currentmuscleID);
            // use the id of the muscle to find the exercises it links to 
            int exercise = muscletargetedrepo.GetExerciseID(currentmuscleID);
            return exercise;  
        }
        public bool CheckBlackLists(int exerciseID)
        {
            var exerciserepo = new ExerciseRepository();
            int machineID = exerciserepo.GetMachineID(exerciseID);
            var blacklistrepo = new BlacklistRepository();
            bool muscleblacklisted = blacklistrepo.CheckMuscleBlacklist(Currentuserid, currentmuscleID);
            bool exerciseblacklisted = blacklistrepo.CheckExerciseBlacklist(Currentuserid, exerciseID);
            bool machineblacklisted = blacklistrepo.CheckMachineBlacklist(Currentuserid, machineID);
            if (muscleblacklisted || exerciseblacklisted || machineblacklisted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
      
        public void UpdateTimeTaken(int exerciseID)
        {
            // get the sets and reps for the exercise 
            var exerciserepo = new ExerciseRepository();
            int[] setsandreps = exerciserepo.GetExerciseData(exerciseID);
            // Note that the rest times are in seconds 
            int resttime = 60;
            if (useraim == "Muscle Strength")
            {
                resttime = 90;
            }
            else if(useraim == "Endurance")
            {
                resttime = 45;
            }
            if (setsandreps[0] != -1)
            {
                // the formula below assumed that the time for each rep is 5 seconds 
                int timeforexercise = setsandreps[0] * (setsandreps[1] * 5 + resttime);
                totaltimetaken += timeforexercise;

            }
         
        }
        //public void UpdateDB(string dayname, int exerciseID)
        //{
        //    var exerciserepo = new ExerciseRepository();
        //    int[] setsandreps = exerciserepo.GetExerciseData(exerciseID);
        //    string exercisename = exerciserepo.GetExerciseName(exerciseID);
        //    var muscletargetedrepo = new MuscleTargetedRepository();
        //    int muscleid = muscletargetedrepo.GetMuscleID(exerciseID);
        //    if (setsandreps[0] != -1)
        //    {
        //        var schedulecontent = new Schedule
        //        {
        //            UserID = Currentuserid,
        //            Dayname = dayname,
                    
        //        };
        //        var schedulerepo = new ScheduleRepository();
        //        schedulerepo.CreateSchedule(schedulecontent);
        //    }


        //}
                
        public bool CheckDuplicates(List<int> exercises, int exerciseID)
        {
            if (exercises.Contains(exerciseID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }



    }   
}
