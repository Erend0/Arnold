using NEA.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NEA.Data
{
    public class UserRepository
    {
        private readonly SQLiteConnection _database;
        public static String DbPath { get; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GymDataBase.db");


        public UserRepository()
        {
            _database = new SQLiteConnection(DbPath);
            _database.CreateTable<User>();
        }
        
        public List<User> List()
        {
            return _database.Table<User>().ToList();
        }
       
        // return usernames of all users 
        public List<User> GetUsernames()
        {
            return _database.Query<User>("SELECT [UserName] FROM [Users]");
        }
        
        // add a new user to database 
        public void AddNewUser(User user)
        {
            _database.Insert(user);
        }

        // delete all the users
        public void DeleteAllUsers()
        {
            
                _database.DeleteAll<User>();
            
        }
        // check if a user with the username and pin exists in the database
        public bool LoginUser(string username, string pin)
        {
            var user = _database.Query<User>("SELECT * FROM [Users] WHERE [UserName] = ? AND [UserPin] = ?", username, pin);
            if (user.Count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        // returns the currentley logged in user 
        public User GetLoggedInUser()
        {
            var user = _database.Query<User>("SELECT * FROM [Users] WHERE [HasLoggedIn] = 1");
            return user[0];
        }
        // returns a boolean value false if there is no user with haslogged in as 1 
        public bool CheckLoggedInUser()
        {
            var user = _database.Query<User>("SELECT * FROM [Users] WHERE [HasLoggedIn] = 1");
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
