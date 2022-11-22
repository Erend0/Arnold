using NEA.Models;
using SQLite;
using System;
using System.IO;
using System.Reflection;

namespace NEA.Data
{
    internal class BlacklistRepository
    {
        static SQLiteAsyncConnection _database;
        public BlacklistRepository()
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
            _database.CreateTableAsync<ExerciseBlacklist>().Wait();
            _database.CreateTableAsync<MachineBlacklist>().Wait();
            _database.CreateTableAsync<MuscleBlacklist>().Wait();

        }
        // Checks for a record with the userid UserID and muscle id MuscleID in the muscle blacklist table called
        public bool CheckMuscleBlacklist(int UserID, int MuscleID)
        {
            var checkmyuscleblacklist = _database.Table<MuscleBlacklist>().Where(i => i.UserID == UserID && i.MuscleID == MuscleID).FirstOrDefaultAsync().Result;
            if (checkmyuscleblacklist != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // checks for a record with the userid UserID and exercise id ExerciseID in the exercise blacklist table
        public bool CheckExerciseBlacklist(int UserID, int ExerciseID)
        {
            var checkmyexerciseblacklist = _database.Table<ExerciseBlacklist>().Where(i => i.UserID == UserID && i.ExerciseID == ExerciseID).FirstOrDefaultAsync().Result;
            if (checkmyexerciseblacklist != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // checks for a record with the userid UserID and machine id MachineID in the machine blacklist table
        public bool CheckMachineBlacklist(int UserID, int MachineID)
        {
            var checkmymachineblacklist = _database.Table<MachineBlacklist>().Where(i => i.UserID == UserID && i.MachineID == MachineID).FirstOrDefaultAsync().Result;
            if (checkmymachineblacklist != null)
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
