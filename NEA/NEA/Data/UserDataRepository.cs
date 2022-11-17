using System;
using SQLite;
using System.IO;
using NEA.Models;
namespace NEA.Data  
{
    public class UserDataRepository
    {

        // A connection to the database file is estabilished
        private readonly SQLiteConnection _database;
        public static String DbPath { get; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GymDataBase.db");

        public UserDataRepository()
        {
            _database = new SQLiteConnection(DbPath);
            _database.CreateTable<UserData>();
        }
        
        // This function is used to insert the new user data into the database
        public void InsertUserData(int userID, int time, int days, string aim)
        {
            _database.Insert(new UserData
            {
                UserID = userID,
                TimeAvailable = time,
                DaysAvailable = days,
                Aim = aim
            });
        }
        
        // This function gets the aim, timeavailable and days available from the userdata table and stores as an array where the userID of the user is equal to the userID of the user in the table
        public string[] GetUserData(int userID)
        {
            var userData = _database.Table<UserData>().Where(x => x.UserID == userID).FirstOrDefault();
            string[] userDataArray = new string[3];
            userDataArray[0] = userData.Aim;
            userDataArray[1] = userData.TimeAvailable.ToString();
            userDataArray[2] = userData.DaysAvailable.ToString();
            return userDataArray;
        }
        
    }
}
