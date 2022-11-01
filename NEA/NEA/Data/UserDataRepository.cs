using System;
using SQLite;
using System.IO;
using NEA.Models;
using System.Security.Claims;

namespace NEA.Data
    
{
    public class UserDataRepository
    {

        // create an existing database userdata to the existing database connection
        

        
        private readonly SQLiteConnection _database;
        public static String DbPath { get; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GymDataBase.db");

        public UserDataRepository()
        {
            _database = new SQLiteConnection(DbPath);
            _database.CreateTable<UserData>();

        }
        // inserts new userdata into the table with userid time days and aim as fields
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
        // gets the aim, timeavailable and days available from the userdata table and stores as an array where the userID of the user is equal to the userID of the user in the table
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
