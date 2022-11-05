using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NEA.Models;
using SQLite;


namespace NEA.Data
{
    internal class MachineRepository
    {

        private readonly SQLiteConnection _database;
        public static String DbPath { get; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GymDataBase.db");

        public MachineRepository()
        {
            _database = new SQLiteConnection(DbPath);
            _database.CreateTable<Exercise>();
        }

    }
}
