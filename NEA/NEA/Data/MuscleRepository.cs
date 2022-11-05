using System;
using System.IO;
using NEA.Models;
using SQLite;
namespace NEA.Data
{
    internal class MuscleRepository
    {

        private readonly SQLiteConnection _database;
        public static String DbPath { get; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GymDataBase.db");

        public MuscleRepository()
        {
            _database = new SQLiteConnection(DbPath);
            _database.CreateTable<Muscle>();
            _database.CreateTable<MuscleTargeted>();
        }

    }
}
