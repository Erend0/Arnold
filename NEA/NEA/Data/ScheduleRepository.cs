using System;
using SQLite;
using System.IO;
using NEA.Models;
using System.Reflection;

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


        // Gets all the schedules given the UserID from the database
        // Makes an array of arrays to contain the fields with the same dayname 
        // Within the arrays inside the arrays store the exerciseName, sets, reps and type
        // This is done so that the data can be displayed in a listview
        
        



    }

}
