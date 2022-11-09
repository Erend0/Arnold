using System;
using System.IO;
using NEA.Models;
using SQLite;
namespace NEA.Data
{
    internal class MuscleRepository
    {

        private readonly SQLiteConnection _database;
        public static String DbPath { get; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GymDataBase.db");

        public MuscleRepository()
        {
            _database = new SQLiteConnection(DbPath);
            _database.CreateTable<Muscle>();
            _database.CreateTable<MuscleTargeted>();
        }


        // a function which returns the muscle id from the muscle minor input 
        public int GetMuscleID(string muscleMinor)
        {
            var muscle = _database.Table<Muscle>().Where(x => x.MinorMuscle == muscleMinor).FirstOrDefault();
            return muscle.MuscleID;
        }

        // returns all the exercise ids from the muscletargeted table where he minormuscleid is equal to int minomuscleid  
        public int[] GetExercises(int minormuscleid)
        {
            var exercises = _database.Table<MuscleTargeted>().Where(y => y.MinorMuscleID == minormuscleid).ToArray();
            int[] exerciseids = new int[exercises.Length];
            for (int i = 0; i < exercises.Length; i++)
            {
                exerciseids[i] = exercises[i].ExerciseID;
            }
            return exerciseids;
        }

    }
}
