using System;
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
            int[] setsAndReps = new int[2];
            setsAndReps[0] = exercise.Sets;
            setsAndReps[1] = exercise.Reps;
            return setsAndReps;
        }

        // return the machine id given the exercise id
        public int GetMachineID(int exerciseID)
        {
            var exercise = _database.Table<Exercise>().Where(i => i.ExerciseID == exerciseID).FirstOrDefaultAsync().Result;
            return exercise.MachineID;
        }
    }
}
