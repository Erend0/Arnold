using System;
using System.IO;
using NEA.Models;
using SQLite;
namespace NEA.Data
{
    public class MuscleTargetedRepository
    {
        private readonly SQLiteConnection _database;
        public static String DbPath { get; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GymDataBase.db");
        public MuscleTargetedRepository()
        {
            _database = new SQLiteConnection(DbPath);
            _database.CreateTable<MuscleTargeted>();
        }

        
        // This function finds an exerise with the muscleid and exerciseid and returns the exercise ID
        public int GetExerciseID(int muscleid)
        {
            // Finds the exercise of a random exercise with the muscleid muscleid, if no exercises found -1 is returned
            var exercises = _database.Table<MuscleTargeted>().Where(y => y.MinorMuscleID == muscleid).ToArray();
            if (exercises.Length == 0)
            {
                return -1;
            }
            else
            {
                Random rnd = new Random();
                int index = rnd.Next(0, exercises.Length);
                return exercises[index].ExerciseID;
            }
        }
    }
}
