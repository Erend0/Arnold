using NEA.Models;
using SQLite;
using System;
using System.IO;

namespace NEA.Data
{
    internal class BlacklistRepository
    {
        private readonly SQLiteConnection _database;
        public static String DbPath { get; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GymDataBase.db");

        public BlacklistRepository()
        {
            _database = new SQLiteConnection(DbPath);
            _database.CreateTable<ExerciseBlacklist>();
            _database.CreateTable<MachineBlacklist>();
            _database.CreateTable<MuscleBlacklist>();
        }
        // Checks for a record with the userid UserID and muscle id MuscleID in the muscle blacklist table 
        public bool CheckMuscleBlacklist(int UserID, int MuscleID)
        {
            var muscleblacklist = _database.Table<MuscleBlacklist>().Where(x => x.UserID == UserID && x.MuscleID == MuscleID).FirstOrDefault();
            if (muscleblacklist != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // checks for a record with the userid UserID and exercise id ExerciseID in the exercise blacklist table
        public bool CheckExerciseBlacklist(int UserID, int ExerciseID)
        {
            var exerciseblacklist = _database.Table<ExerciseBlacklist>().Where(x => x.UserID == UserID && x.ExerciseID == ExerciseID).FirstOrDefault();
            if (exerciseblacklist != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // checks for a record with the userid UserID and machine id MachineID in the machine blacklist table
        public bool CheckMachineBlacklist(int UserID, int MachineID)
        {
            var machineblacklist = _database.Table<MachineBlacklist>().Where(x => x.UserID == UserID && x.MachineID == MachineID).FirstOrDefault();
            if (machineblacklist != null)
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
