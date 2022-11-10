using System;
using System.IO;
using NEA.Models;
using SQLite;

namespace NEA.Data
{
    internal class ExerciseRepository
    {

        private readonly SQLiteConnection _database;
        public static String DbPath { get; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GymDataBase.db");

        public ExerciseRepository()
        {
            _database = new SQLiteConnection(DbPath);
            _database.CreateTable<Exercise>();
        }


        // return the sets and reps for an exercise in the from of a int array, from the exercise table given the exerciseID
        public int[] GetExerciseData(int exerciseID)
        {
            var exercise = _database.Table<Exercise>().Where(x => x.ExerciseID == exerciseID).FirstOrDefault();
            int[] exerciseArray = new int[2];
            exerciseArray[0] = exercise.Sets;
            exerciseArray[1] = exercise.Reps;
            return exerciseArray;
        }





    }
}
