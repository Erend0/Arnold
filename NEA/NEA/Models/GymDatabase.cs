using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NEA.Models
{
    public class GymDatabase
    {
        readonly SQLiteAsyncConnection database;

        public GymDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Exercise>().Wait();
        }

       
   
        
    }
}
