using NEA.Models.OtherModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace NEA.Data
{
    public class WorkoutRepository
    {
        static SQLiteAsyncConnection _database;
        public WorkoutRepository()
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
            _database.CreateTableAsync<WorkoutTracker>().Wait();

        }
        
        // given the exerciseid returns all the weight and reps for each set ordered by smallest set number first
        public WorkoutTracker[] GetLogs(int index)
        {
            var logs = _database.Table<WorkoutTracker>().Where(x => x.Index == index).OrderBy(x => x.Set).ToArrayAsync().Result;
            return logs;
        }
        public void Update(Dictionary<Tuple<int, int>, int[]> Entries,int index)
        {
            //removes the entries for exerciseid
            _database.Table<WorkoutTracker>().Where(x => x.Index == index).DeleteAsync().Wait();
            // adds all the data where the first integer of the tupple is the exercise id
            foreach (var entry in Entries)
            {
                if (entry.Key.Item1 == index)
                {
                    Console.WriteLine("Inserted");
                    _database.InsertAsync(new WorkoutTracker { Index = entry.Key.Item1, Set = entry.Key.Item2, Weight = entry.Value[1], Reps = entry.Value[0] }).Wait();
                }
            }
        }

        //Deletes all the contents of the table
        public void DeleteAll()
        {
            _database.DeleteAllAsync<WorkoutTracker>();
        }
    }
}
