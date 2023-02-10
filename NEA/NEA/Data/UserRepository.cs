using NEA.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace NEA.Data
{
    public class UserRepository
    {
        // A connection to the database file is estabilished
        static SQLiteAsyncConnection _database;
        public UserRepository()
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
            _database.CreateTableAsync<User>().Wait();

        }
        // The user user is added to the database
        public void AddNewUser(User user)
        {
            _database.InsertAsync(user);
        }
        // Checks if a user with username and userpin has hasloggedin set as 1
        public bool LoginUser(string username, int userpin)
        {
            // given the username and userpin converted to int check if an record is present 
            // sets the has logged in value to 1 
            // and returns true if logged in 
            // else returns false
            var user = _database.Table<User>().Where(i => i.UserName == username && i.UserPin == userpin ).FirstOrDefaultAsync().Result;
            if (user != null)
            {
                user.HasLoggedIn = 1;
                _database.UpdateAsync(user);
                return true;
            }
            else
            {
                return false;
            }

        }

        // returns a list of all users 
        public List<User> GetUsernames()
        {
            var user = _database.Table<User>().ToListAsync().Result;
            return user;
        }
        // returns information about the current loggged in user
        public User GetLoggedInUser()
        {
            var user = _database.Table<User>().Where(i => i.HasLoggedIn == 1).FirstOrDefaultAsync().Result;
            return user;
        }
        // returns all the user details in the databasecalled get user details
        public List<User> GetAllUsers()
        {
            var user = _database.Table<User>().ToListAsync().Result;
            return user;
        }
 
        public bool CheckLoggedInUser()
        {
            var user = _database.Table<User>().Where(i => i.HasLoggedIn == 1).FirstOrDefaultAsync().Result;
            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // logs out user by setting hasloggedin to 0
        public void LogoutUser()
        {
            var user = _database.Table<User>().Where(i => i.HasLoggedIn == 1).FirstOrDefaultAsync().Result;
            user.HasLoggedIn = 0;
            _database.UpdateAsync(user);
        }
        // The users pin is changed
        public void ChangeUserPin(int newpin)
        {
            var user = _database.Table<User>().Where(i => i.HasLoggedIn == 1).FirstOrDefaultAsync().Result;
            user.UserPin = newpin;
            _database.UpdateAsync(user);
        }
    }
    
}

