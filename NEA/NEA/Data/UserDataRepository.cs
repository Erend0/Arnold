using System;
using SQLite;
using System.IO;
using NEA.Models;
using System.Reflection;

namespace NEA.Data  
{
    public class UserDataRepository
    {

        // A connection to the database file is estabilished
        static SQLiteAsyncConnection _database;
        public UserDataRepository()
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
            _database.CreateTableAsync<UserData>().Wait();

        }

        // This function is used to insert the new user data into the database called insertuserdata which inserts userid time days and aim
        public void InsertUserData(int userID, int time, int days, string aim)
        {
            _database.InsertAsync(new UserData { UserID = userID, TimeAvailable = time, DaysAvailable = days, Aim = aim });
        }


        // This function gets the aim, timeavailable and days available from the userdata table and stores as an array where the userID of the user is equal to the userID of the user in the table
        public string[] GetUserData(int userID)
        {
            var user = _database.Table<UserData>().Where(i => i.UserID == userID).FirstOrDefaultAsync().Result;
            string[] userdata = new string[3];
            userdata[0] = user.Aim;
            userdata[1] = user.TimeAvailable.ToString();
            userdata[2] = user.DaysAvailable.ToString();
            return userdata;
        }
    }
}
