using System;
using System.IO;
using System.Reflection;
using NEA.Models;
using SQLite;
namespace NEA.Data
{
    public class MuscleRepository
    {

        static SQLiteAsyncConnection _database;
        public MuscleRepository()
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
            _database.CreateTableAsync<Muscle>().Wait();
        }


        // finds the muscle id given the major and minor muscles and returns as an integer, function name is FindMuscleID, returns -1 if not found
        public int FindMuscleID(string majorMuscle, string minorMuscle)
        {
            var muscle = _database.Table<Muscle>().Where(i => i.MajorMuscle == majorMuscle && i.MinorMuscle == minorMuscle).FirstOrDefaultAsync().Result;
            return muscle.MuscleID;
            
        }
    }
}
