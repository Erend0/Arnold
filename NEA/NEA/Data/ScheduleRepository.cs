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

        /// removes all the data in the table which belongs to user with userID
        public void DeleteSchedule(int userID)
        {
            _database.DeleteAsync<Schedule>(userID);
        }
        public void DeleteAll()
        {
            _database.DeleteAllAsync<Schedule>();
        }

        
        public List<Schedule> GetSchedule(int userID)
        {
            var schedule = _database.Table<Schedule>().Where(i => i.UserID == userID).ToListAsync().Result;
            return schedule;
        }
    }
}
