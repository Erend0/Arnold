using System;
using SQLite;
using System.IO;
using NEA.Models;

namespace NEA.Data
{
    internal class ScheduleRepository
    {
        private readonly SQLiteConnection _database;
        public static String DbPath { get; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GymDataBase.db");

        public ScheduleRepository()
        {
            _database = new SQLiteConnection(DbPath);
            _database.CreateTable<Schedule>();
   
        }

        // create a new entry to the database
        public void CreateSchedule(Schedule schedule)
        {
            _database.Insert(schedule);
        }

        /// removes all the data in the table which belongs to user with userID
        public void DeleteSchedule(int userID)
        {
            _database.Delete<Schedule>(userID);
        }
    }
}
