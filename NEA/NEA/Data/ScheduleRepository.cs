using System;
using SQLite;
using System.IO;
using NEA.Models;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

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

        public List<Schedule> GetSchedule(int userID, string dayname, int type)
        {
            return _database.Table<Schedule>().Where(x => x.UserID == userID && x.DayName == dayname && x.Type == type).ToListAsync().Result;
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
        public string[] GetDays(int userID, int type)
        {
            // select the dayname given the userID and type, and store all the daynames in an array if its not already in the array
            List<Schedule> schedule = _database.Table<Schedule>().Where(x => x.UserID == userID && x.Type == type).ToListAsync().Result;
            string[] daynames = new string[20];
            foreach(Schedule record in schedule)
            {
                if (!daynames.Contains(record.DayName))
                {
                    daynames[Array.IndexOf(daynames, null)] = record.DayName;
                }
            }
            return daynames;
        }
        public bool CheckDayName(int userID, string dayname)
        {
            List<Schedule> schedule = _database.Table<Schedule>().Where(x => x.UserID == userID && x.DayName == dayname).ToListAsync().Result;
            if (schedule.Count > 0)
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
