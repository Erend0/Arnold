using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using NEA.Models;
using SQLite;

namespace NEA.Data
{
    internal class ExerciseRepository
    {

        static SQLiteAsyncConnection _database;
        public ExerciseRepository()
        {
            string DbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GymDatabase.db");
            Assembly assemly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
            Stream embeddedDatabaseStream = assemly.GetManifestResourceStream("NEA.GymDatabase.db");
            if (!File.Exists(DbPath))
            {
                FileStream filestreamtowrite = File.Create(DbPath);

                embeddedDatabaseStream.Seek(0, SeekOrigin.Begin);
                embeddedDatabaseStream.CopyTo(filestreamtowrite);
                filestreamtowrite.Close();
            }
            _database = new SQLiteAsyncConnection(DbPath);
            _database.CreateTableAsync<Exercise>().Wait();
        }


        // return the sets and reps for an exercise in the from of a int array, from the exercise table given the exerciseID
        public int[] GetExerciseData(int exerciseID)
        {
            var exercise = _database.Table<Exercise>().Where(i => i.ExerciseID == exerciseID).FirstOrDefaultAsync().Result;
            if (exercise != null)
            {
                int[] data = new int[2];
                data[0] = exercise.Sets;
                data[1] = exercise.Reps;
                return data;
            }
            else
            {
                int[] error = { -1 };
                return error;

            }

        }
        // return the machine id given the exercise id
        public int GetMachineID(int exerciseID)
        {

            var exercise = _database.Table<Exercise>().Where(i => i.ExerciseID == exerciseID).FirstOrDefaultAsync().Result;
            if (exercise != null)
            {
                return exercise.MachineID;
            }
            else
            {
                return -1;
            }
        }

        public Exercise GetExercise(int exerciseID)
        {
            var exercise = _database.Table<Exercise>().Where(i => i.ExerciseID == exerciseID).FirstOrDefaultAsync().Result;
            return exercise;
        }
        public List<Exercise> GetAllExercises()
        {
            var exercises = _database.Table<Exercise>().ToListAsync().Result;
            return exercises;
        }
        // makes a search of all exercises given a string exercise, case is not sensitive
        public List<Exercise> SearchExercises(string exercise)
        {
            var exercises = _database.Table<Exercise>().Where(i => i.ExerciseName.ToLower().Contains(exercise.ToLower())).ToListAsync().Result;
            return exercises;

        }
        // given the exercise name return the sets and reps
        public int[] GetSetsandReps(string exerciseName)
        {
            var exercise = _database.Table<Exercise>().Where(i => i.ExerciseName == exerciseName).FirstOrDefaultAsync().Result;
            if (exercise != null)
            {
                int[] data = new int[2];
                data[0] = exercise.Sets;
                data[1] = exercise.Reps;
                return data;
            }
            else
            {
                int[] error = { -1 };
                return error;

            }
        }
        public int GetExerciseID(string exerciseName)
        {
            var exercise = _database.Table<Exercise>().Where(i => i.ExerciseName == exerciseName).FirstOrDefaultAsync().Result;
            if (exercise != null)
            {
                return exercise.ExerciseID;
            }
            else
            {
                return -1;
            }
        }
    }
}
