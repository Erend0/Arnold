using System;
using System.IO;
using NEA.Models;
using SQLite;
namespace NEA.Data
{
    public class MuscleRepository
    {

        private readonly SQLiteConnection _database;
        public static String DbPath { get; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GymDataBase.db");

        public MuscleRepository()
        { 
            _database = new SQLiteConnection(DbPath);
            _database.CreateTable<Muscle>();
            
        }


        // finds the muscle id given the major and minor muscles and returns as an integer, function name is FindMuscleID
        public int FindMuscleID(string majorMuscle, string minorMuscle)
        {
            // insert into muscle major and minor muscle
            // Find the muscle id of the muscle with the major and minor muscle names
            var muscle = _database.Table<Muscle>().Where(x => x.MajorMuscle == majorMuscle && x.MinorMuscle == minorMuscle).ToArray();
            if (muscle.Length == 0)
            {  
                return -1;
                
            }
            else
            {
                return muscle[0].MuscleID;
            }
        }
    }
}
