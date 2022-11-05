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
    }
}
