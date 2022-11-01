﻿using NEA.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;


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
        
        public List<User> GetLoggedInUser()
        {
            return _database.Query<User>("SELECT * FROM [Users] WHERE [HasLoggedIn] = 1");
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

        // returns the userID for the current logged in user 
        public int GetLoggedInUserId()
        {
            var user = _database.Query<User>("SELECT * FROM [Users] WHERE [HasLoggedIn] = 1");
            return user[0].UserID;
        }


       
       



    }
}
