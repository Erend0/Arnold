using NEA.Data;
using NEA.Models; 

using System; 
using System.Collections.Generic;
using System.Linq;

namespace NEA.Tasks
{
    public class Workout
    {
        public int UserID { get; set; }
        private int UserDays { get; set; }
        private string UserAim { get; set; }
        private int UserTime { get; set; }  // Time is stored in seconds in the database
        
        private readonly string[] Chest = { "Upper", "Lower", "Inner" };
        private readonly string[] Triceps = { "Longhead", "LateralHead", "MedialHead" };
        private readonly string[] Back = { "Lats", "MiddleBack", "LowerBack" };
        private readonly string[] Biceps = { "LongHead", "ShortHead" };
        private readonly string[] Shoulders = { "Anteriordelts", "Medialdelts", "Posteriordelts" };
        private readonly string[] Legs = { "Quadriceps", "Hamstrings", "Glutes", "Calves"};
        private readonly string[] Cardio = {"Cardio" };
      
        private readonly MuscleRepository _MuscleRepo = new MuscleRepository();
        private readonly MuscleTargetedRepository _MuscleTargetedRepo = new MuscleTargetedRepository();
        private readonly ExerciseRepository _ExerciseRepo = new ExerciseRepository();
        private readonly UserRepository _UserRepo = new UserRepository();
        private readonly UserDataRepository _UserDataRepo = new UserDataRepository();
        private readonly BlacklistRepository _BlacklistRepo = new BlacklistRepository();
        private readonly ScheduleRepository _ScheduleRepo = new ScheduleRepository(); 


        private int ExerciseFound { get; set; }
        private int TotalTimeTaken { get; set;}
        private int NumberOfMuscles { get; set; }
      
        public List<List<int>> GeneratedWorkout  = new List<List<int>>();
       
        public int CurrentMuscleID { get; set; }
   
       
        public Workout(string togenerate)
        {
            GetUserData();
            GenerateWorkout(togenerate);
            UpdateDB(togenerate);
        }
        
        private void GetUserData()
        {
            // This method uses the database repositories to get and set the user data
            UserID = _UserRepo.GetLoggedInUser().UserID;
            string[] userdata = _UserDataRepo.GetUserData(UserID);
             UserAim = userdata[0];
             UserTime = Convert.ToInt32(userdata[1]);
             UserDays = Convert.ToInt32(userdata[2]);
        }
        
        private void GenerateWorkout(string togenerate)
        {
            if (togenerate == "all")
            { 
                // A Queue of all the days to generate with the muscles they contain are created 
                Queue<string[][]> split = ReturnSplitQueue();
                //All of the days are individally filled by passing the days into the fillsplit method
                foreach(string[][] day in split)
                {
                    FillSplit(day);
                }
            }
            // If only one day is being generated the days is also filled with its muscled and populated with exercises
            else
            {
                if (togenerate == "Back,Biceps")
                {
                    FillSplit(new string[][] { Back, Biceps });

                }
                else if (togenerate == "Chest,Triceps")
                {
                    FillSplit(new string[][] { Chest, Triceps });
                }
                else if (togenerate == "Shoulders")
                {
                    FillSplit(new string[][] { Shoulders });
                }
                else if (togenerate == "Legs")
                {
                    FillSplit(new string[][] { Legs });
                }
                else if (togenerate == "Cardio")
                {
                    FillSplit(new string[][] { Cardio });

                }
                else if (togenerate == "Chest,Tricep,Legs")
                {
                    FillSplit(new string[][] { Chest, Triceps, Legs });
                }
                else if (togenerate == "Back,Biceps,Shoulders")
                {
                    FillSplit(new string[][] { Chest, Triceps, Legs });
                }
                else if (togenerate == "Biceps,Legs,Chest")
                {
                    FillSplit(new string[][] { Biceps, Legs, Chest });
                }
            }
        }
        private Queue<string[][]> ReturnSplitQueue()
        {
            Queue<string[][]> split = new Queue<string[][]>();
            //Jagged arrays are used to store the muscle groups that will be worked on each day
            //The outer array will be filled with string arrays containg all the muscle names
            if (UserDays == 3) 
            {
                string[][]day1 = {Chest,Triceps, Legs};
                string[][] day2 = {Back,Biceps,Shoulders};
                string[][] day3 = { Biceps,Legs,Chest};
                split.Enqueue(day1);
                split.Enqueue(day2);
                split.Enqueue(day3);
                NumberOfMuscles = 3;
                return split;           
            }
            else
            {
                string[][] day1 = { Chest, Triceps };
                string[][] day2 = { Back, Biceps };
                string[][] day3 = { Shoulders, Shoulders };
                string[][] day4 = { Legs, Legs};
                split.Enqueue(day1);
                split.Enqueue(day2);
                split.Enqueue(day3);
                split.Enqueue(day4);
                NumberOfMuscles = 2;
                if (UserDays == 5)
                {
                    string[][] day5 = { Cardio };
                    split.Enqueue(day5);
                    return split;
                }
                else
                {
                    return split;
                }
             }  
        }
        
        private void FillSplit(string[][] day)
        {
            int timeforeachfocus = UserTime / day.Length;
            List<int> exercises = new List<int>();
            for (int i = 0; i < day.Length; i++)
            {
                // Note that totaltimetaken is also in the format of seconds, same as the UserTime attribute
                TotalTimeTaken = 0;
                int index = 0;
                while (TotalTimeTaken < timeforeachfocus)
                {
                    bool hasblacklisted = true;
                    bool hasduplicate = true;
                    // This flag is used to check if the while loop has been runing for too long 
                    // This can happen if the database doesn't have an exercise to target the minor muscle
                    // In this case next minor muscle will be attempted to be filled 
                    int flag = 0;
                    while ( (hasblacklisted || hasduplicate )&& flag<10)
                    {
                        ExerciseFound = FindExerciseforMinorMuscle(day[i][index]);
                        if (ExerciseFound != -1)
                        {
                            hasblacklisted = CheckBlackLists(ExerciseFound);
                            hasduplicate = CheckDuplicates(exercises, ExerciseFound);
                        }
                        flag++;
                    }
                    if(ExerciseFound != 1)
                    {
                        UpdateTimeTaken(ExerciseFound);
                    }
                    
                    // if index is over the size reset is to zero, if not increment
                    if (index == day[i].Length - 1)
                    {
                        index = 0;
                    }
                    else
                    {
                        index++;
                    }
                    if(ExerciseFound != -1)
                    {

                        exercises.Add(ExerciseFound);
                    }

                }
            }
            GeneratedWorkout.Add(exercises);
        }
        private int FindExerciseforMinorMuscle(string minormuscle)
        {
            //The MuscleID for the parameter minormuscle is found
            CurrentMuscleID = _MuscleRepo.FindMuscleID(minormuscle);
            //The MuscleID is used to find a random Exercise that targets that muscle
            int exercise = _MuscleTargetedRepo.GetExerciseID(CurrentMuscleID);
            return exercise;  
        }
        private bool CheckBlackLists(int exerciseID)
        {
            
            // The blacklist tables for muscle, exercise and machine are checkted to find if the user has blacklisted details relating to the found exercise
            int machineID = _ExerciseRepo.GetMachineID(exerciseID);
            bool muscleblacklisted = _BlacklistRepo.CheckMuscleBlacklist(UserID, CurrentMuscleID);
            bool exerciseblacklisted = _BlacklistRepo.CheckExerciseBlacklist(UserID, exerciseID);
            bool machineblacklisted = _BlacklistRepo.CheckMachineBlacklist(UserID, machineID);
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
            int[] setsandreps = _ExerciseRepo.GetExerciseData(exerciseID);
            // Note that the rest times are in seconds 
            // It is determined by the Aim of the user
            int resttime = 60;
            if (UserAim == "Muscle Strength")
            {
                resttime = 90;
            }
            else if(UserAim == "Endurance")
            {
                resttime = 45;
            }
            if (setsandreps[0] != -1)
            {
                // the formula below assumed that the time for each rep is 5 seconds 
                int timeforexercise = setsandreps[0] * (setsandreps[1] * 5 + resttime);
                TotalTimeTaken += timeforexercise;
            }
        }      
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
        
        private int x = 0;
        public void UpdateDB (string dayname)
        {
            string[] DayNames = new string[4];
            if(dayname == "all")
            {
                if (UserDays == 3)
                {
                    DayNames[0] = ("Chest,Tricep,Legs");
                    DayNames[1] = ("Back,Biceps,Shoulders");
                    DayNames[2] = ("Biceps,Legs,Chest");
                }
                if (UserDays == 4 || UserDays == 5)
                {
                    DayNames[0] = ("Chest,Triceps");
                    DayNames[1] = ("Back,Biceps");
                    DayNames[2] = ("Shoulders");
                    DayNames[3] = ("Legs");
                }
                if (UserDays == 5)
                {
                    DayNames[4] = ("Cardio");
                }

            }
            else
            {
                DayNames[0] = dayname;
            }
            
            foreach (List<int> day in GeneratedWorkout)
            {
                foreach (int exercise in day)
                {
                    // An new record in the schedule table is created for each exercise in the generated day 
                    Schedule schedule = new Schedule
                    {
                        UserID = UserID,
                        ExerciseID = exercise,
                        DayName = DayNames[x],
                        Sets = _ExerciseRepo.GetSetsandReps(_ExerciseRepo.GetExercise(exercise).ExerciseName)[0],
                        Reps = _ExerciseRepo.GetSetsandReps(_ExerciseRepo.GetExercise(exercise).ExerciseName)[1],
                        Type = 0,
                    };
                    _ScheduleRepo.CreateSchedule(schedule);
                }
                x++;
            }


        }


    }   
}
