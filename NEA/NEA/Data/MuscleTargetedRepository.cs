using System;
using System.IO;
using System.Reflection;
using NEA.Models;
using SQLite;
namespace NEA.Data
{
    public class MuscleTargetedRepository
    {
        static SQLiteAsyncConnection _database;
        public MuscleTargetedRepository()
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
            _database.CreateTableAsync<MuscleTargeted>().Wait();

        }


        // Function called getexerciseid which gets an array of all exerciseids given a muscleid 
        // And picks an index at random from the array and returns the exerciseid
        public int GetExerciseID(int muscleID)
        {
            var exercise = _database.Table<MuscleTargeted>().Where(i => i.MuscleID == muscleID).ToListAsync().Result;
            // return -1 if no exercises found, if there are found pick one at random
            if (exercise.Count == 0)
            {
                return -1;
            }
            else
            {
                Random rnd = new Random();
                int index = rnd.Next(0, exercise.Count);
                return exercise[index].ExerciseID;
            }

        }
        public int GetMuscleID(int exerciseID)
        {
            // returns the muscleID given the exerciseid, if not found returns -1
            var exercise = _database.Table<MuscleTargeted>().Where(i => i.ExerciseID == exerciseID).FirstOrDefaultAsync().Result;
            if (exercise != null)
            {
                return exercise.MuscleID;
            }
            else
            {
                return -1;
            }
        }

    }
}