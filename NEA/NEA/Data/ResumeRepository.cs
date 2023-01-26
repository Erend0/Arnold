using NEA.Models;
using SQLite;
using System.IO;
using System.Reflection;
using System;
using NEA.Models.OtherModels;

namespace NEA.Data
{
    internal class ResumeRepository
    {
        static SQLiteAsyncConnection _database;
        public ResumeRepository()
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
            _database.CreateTableAsync<ResumePage>().Wait();
            _database.CreateTableAsync<AddedSets>().Wait();
        }
        // This method is used to allow workouts to be continued in the companion page
        public void ChangeDayToResume(int UserID,string dayname)
        {
            // The db is checked to see if there is an entry for the user
            // If not one is created and the dayname is added to the table
            var check = _database.Table<ResumePage>().Where(i => i.UserID == UserID).FirstOrDefaultAsync().Result;
            if (check == null)
            {
                _database.InsertAsync(new ResumePage { UserID = UserID, DayName = dayname, DataGrab = 0 }).Wait();
            }
            else
            {
                _database.ExecuteAsync("UPDATE ResumePage SET DayName = ? WHERE UserID = ?", dayname, UserID).Wait();
            }

        }
        // This method checks if a user with the UserID has any workout to continue
        public string CheckResume(int UserID)
        {
            var check = _database.Table<ResumePage>().Where(i => i.UserID == UserID).FirstOrDefaultAsync().Result;
            if (check == null)
            {
                return null;
            }
            else
            {
                return check.DayName;
            }
        }
        public int ReturnType(int UserID)
        {
            var check = _database.Table<ResumePage>().Where(i => i.UserID == UserID).FirstOrDefaultAsync().Result;
            if (check == null)
            {
                return 0;
            }
            else
            {
                return check.Type;
            }
        }
        public void AddEntry(int UserID,string dayname ,int Type)
        {
            var check = _database.Table<ResumePage>().Where(i => i.UserID == UserID).FirstOrDefaultAsync().Result;
            if (check == null)
            {
                _database.InsertAsync(new ResumePage { UserID = UserID, DayName = dayname, DataGrab = 0, Type = Type }).Wait();
            }
            else
            {
                _database.ExecuteAsync("UPDATE ResumePage SET DayName = ? WHERE UserID = ?", dayname, UserID).Wait();
                _database.ExecuteAsync("UPDATE ResumePage SET Type = ? WHERE UserID = ?", Type, UserID).Wait();
            }
        }
        public void RemoveRecord(int UserID)
        {
            _database.ExecuteAsync("UPDATE ResumePage SET DayName = ? WHERE UserID = ?", null, UserID).Wait();
            _database.ExecuteAsync("UPDATE ResumePage SET Type = ? WHERE UserID = ?", 0, UserID).Wait();
        }

        public void RecordSets(int index, int addedsets)
        {
            // change the added sets or add them if they are not in
            var check = _database.Table<AddedSets>().Where(i => i.ExerciseIndex == index).FirstOrDefaultAsync().Result;
            if (check == null)
            {
                _database.InsertAsync(new AddedSets { ExerciseIndex = index, NumberofSets = addedsets }).Wait();
            }
            else
            {
                _database.ExecuteAsync("UPDATE AddedSets SET NumberofSets = ? WHERE ExerciseIndex = ?", addedsets, index).Wait();
            }
        }
        
        public void DeleteAll()
        {
           _database.ExecuteAsync("DELETE FROM AddedSets").Wait();
        }
        // get all the added sets with index 
        public int GetAddedSets(int index)
        {
            var check = _database.Table<AddedSets>().Where(i => i.ExerciseIndex == index).FirstOrDefaultAsync().Result;
            if (check == null)
            {
                return 0;
            }
            else
            {
                return check.NumberofSets;
            }
        }

        // update the time
        public void UpdateTime(int UserID, int time)
        {
            int timetaken = _database.Table<ResumePage>().Where(i => i.UserID == UserID).FirstOrDefaultAsync().Result.Time;
            _database.ExecuteAsync("UPDATE ResumePage SET Time = ? WHERE UserID = ?", time + timetaken, UserID).Wait();
        }
        public void ResetTime(int UserID)
        {
            _database.ExecuteAsync("UPDATE ResumePage SET Time = ? WHERE UserID = ?", 0, UserID).Wait();
        }


    }
}
