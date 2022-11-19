using NEA.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
namespace NEA.Data
{
    public class UserRepository
    {
        // A connection to the database file is estabilished
        private readonly SQLiteConnection _database;
        public static String DbPath { get; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GymDataBase.db");


        public UserRepository() 
        { 
            _database = new SQLiteConnection(DbPath);
            // read from the user table in the database file
            _database.CreateTable<User>();




            //_database.CreateTable<User>();
        }
        
        // This function is used to return all the user names in the format of a string
        public List<User> GetUsernames()
        {
            return _database.Query<User>("SELECT [UserName] FROM [User]");
        }
        
        // This function is used to insert new users to the database
        public void AddNewUser(User user)
        {
            _database.Insert(user);
        }
        
        // Deletes all the users in the table
        public void DeleteAllUsers()
        {
                _database.DeleteAll<User>();
        }
        
        // Check if a user with the username and pin exists in the database (used in the login page)
        public bool LoginUser(string username, string pin)
        {
            var user = _database.Query<User>("SELECT * FROM [User] WHERE [UserName] = ? AND [UserPin] = ?", username, pin);
            if (user.Count == 1)
            {
                // Set the has logged in for the user to 1
                _database.Query<User>("UPDATE [User] SET [HasLoggedIn] = 1 WHERE [UserName] = ? AND [UserPin] = ?", username, pin);
                return true;
            }
            else
            {
                return false;
            }
        }
        
        // Returns the currentley logged in user 
        public User GetLoggedInUser()
        {
            var user = _database.Query<User>("SELECT * FROM [User] WHERE [HasLoggedIn] = 1");
            return user[0];
        }
        // This function returns a boolean value false if there is no user with haslogged in as 1 
        // Meaning the program will have to use the login page
        // Instead of directley launcing the HomePage
        public bool CheckLoggedInUser()
        {
            var user = _database.Query<User>("SELECT * FROM [User] WHERE [HasLoggedIn] = 1");
            if (user.Count == 1)
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
