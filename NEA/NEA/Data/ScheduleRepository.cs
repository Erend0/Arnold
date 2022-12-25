using System;
using SQLite;
using System.IO;
using NEA.Models;
using System.Reflection;
using System.Collections.Generic;

namespace NEA.Data
{
    internal class ScheduleRepository
    {
        static SQLiteAsyncConnection _database; 
        public ScheduleRepository()
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
            _database.CreateTableAsync<Schedule>().Wait();
        }

        // create a new entry to the database
        public void CreateSchedule(Schedule schedule)
        {
            _database.InsertAsync(schedule);
        }

        public void ResetTable()
        {
            _database.DeleteAllAsync<Schedule>();
        }
       
        public int[] GetSchedule(int userID, string dayname)
        {
            var schedule = _database.Table<Schedule>().Where(i => i.UserID == userID && i.DayName == dayname).ToListAsync().Result;
            int[] exerciseIDS = new int[schedule.Count];
            for (int i = 0; i < schedule.Count; i++)
            {
                exerciseIDS[i] = schedule[i].ExerciseID;
            }
            return exerciseIDS;
        }

        // delete every single schedule for a given userid
        public void DeleteSchedule(int userID)
        {
            _database.Table<Schedule>().Where(i => i.UserID == userID).DeleteAsync();
        }
        
        public void DeleteDay(int userID, string DayName)
        {
            _database.Table<Schedule>().Where(i => i.UserID == userID && i.DayName == DayName).DeleteAsync();
        }


    }
}
