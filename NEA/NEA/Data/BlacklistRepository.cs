﻿using NEA.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NEA.Data
{
    internal class BlacklistRepository
    {
        static SQLiteAsyncConnection _database;
        public BlacklistRepository()
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
            _database.CreateTableAsync<MuscleBlacklist>().Wait();
            _database.CreateTableAsync<ExerciseBlacklist>().Wait();
            _database.CreateTableAsync<MachineBlacklist>().Wait();

        }
        // Deletes the contents of all blacklist tables
        public void DeleteAllBlacklists(int UserID)
        {
            _database.ExecuteAsync("DELETE FROM MuscleBlacklist WHERE UserID = ?", UserID);
            _database.ExecuteAsync("DELETE FROM ExerciseBlacklist WHERE UserID = ?", UserID);
            _database.ExecuteAsync("DELETE FROM MachineBlacklist WHERE UserID = ?", UserID);
        }
        // Checks for a record with the userid UserID and muscle id MuscleID in the muscle blacklist table called
        public bool CheckMuscleBlacklist(int UserID, int MuscleID)
        {
            var checkmuscleblacklist = _database.Table<MuscleBlacklist>().Where(i => i.UserID == UserID && i.MuscleID == MuscleID).FirstOrDefaultAsync().Result;
            if (checkmuscleblacklist != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        // checks for a record with the userid UserID and exercise id ExerciseID in the exercise blacklist table
        public bool CheckExerciseBlacklist(int UserID, int ExerciseID)
        {
            var checkmyexerciseblacklist = _database.Table<ExerciseBlacklist>().Where(i => i.UserID == UserID && i.ExerciseID == ExerciseID).FirstOrDefaultAsync().Result;
            if (checkmyexerciseblacklist != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        // checks for a record with the userid UserID and machine id MachineID in the machine blacklist table
        public bool CheckMachineBlacklist(int UserID, int MachineID)
        {
            var checkmachineblacklist = _database.Table<MachineBlacklist>().Where(i => i.UserID == UserID && i.MachineID == MachineID).FirstOrDefaultAsync().Result;
            if (checkmachineblacklist != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<string> GetBlacklistedMuscles(int UserID)
        {
            List<string> blacklistedmuscles = new List<string>();
            var getblacklistedmuscles = _database.Table<MuscleBlacklist>().Where(i => i.UserID == UserID).ToListAsync().Result;
            foreach (var muscle in getblacklistedmuscles)
            {
                var getmuscle = _database.Table<Muscle>().Where(i => i.MuscleID == muscle.MuscleID).FirstOrDefaultAsync().Result;
                blacklistedmuscles.Add(getmuscle.MinorMuscle);
            }
            return blacklistedmuscles;
        }
        
        public void AddMuscleToBlacklist(int UserID, string minormuscle)
        {
            var getmuscleid = _database.Table<Muscle>().Where(i => i.MinorMuscle == minormuscle).FirstOrDefaultAsync().Result.MuscleID;
            bool exists = CheckMuscleBlacklist(UserID, getmuscleid);
            if (exists == false)
            {
                _database.InsertAsync(new MuscleBlacklist { UserID = UserID, MuscleID = getmuscleid }).Wait();
            }
        }
        public void RemoveMuscleFromBlacklist(int UserID, string minormuscle)
        {
            var getmuscleid = _database.Table<Muscle>().Where(i => i.MinorMuscle == minormuscle).FirstOrDefaultAsync().Result;
            bool exists = CheckMuscleBlacklist(UserID, getmuscleid.MuscleID);
            if (exists)
            {
                string deleteQuery = $"DELETE FROM MuscleBlacklist WHERE UserID = {UserID} AND MuscleID = {getmuscleid.MuscleID}";
                _database.ExecuteAsync(deleteQuery).Wait();
            }
        }

        public List<string> GetBlacklistedExercises(int UserID)
        {
            List<string> blacklistedexercises = new List<string>();
            var getblacklistedexercises = _database.Table<ExerciseBlacklist>().Where(i => i.UserID == UserID).ToListAsync().Result;
            foreach (var exercise in getblacklistedexercises)
            {
                var getexercise = _database.Table<Exercise>().Where(i => i.ExerciseID == exercise.ExerciseID).FirstOrDefaultAsync().Result;
                blacklistedexercises.Add(getexercise.ExerciseName);
            }
            return blacklistedexercises;
        }
        public void AddExerciseToBlacklist(int UserID, string exercisename)
        {
            var getexerciseid = _database.Table<Exercise>().Where(i => i.ExerciseName == exercisename).FirstOrDefaultAsync().Result.ExerciseID;
            bool exists = CheckExerciseBlacklist(UserID, getexerciseid);
            if (exists == false)
            {
                _database.InsertAsync(new ExerciseBlacklist { UserID = UserID, ExerciseID = getexerciseid }).Wait();
            }
        }
        public void RemoveExerciseFromBlacklist(int UserID, string exercisename)
        {
            var getexerciseid = _database.Table<Exercise>().Where(i => i.ExerciseName == exercisename).FirstOrDefaultAsync().Result;
            bool exists = CheckExerciseBlacklist(UserID, getexerciseid.ExerciseID);
            if (exists)
            {
                string deleteQuery = $"DELETE FROM ExerciseBlacklist WHERE UserID = {UserID} AND ExerciseID = {getexerciseid.ExerciseID}";
                _database.ExecuteAsync(deleteQuery).Wait();
            }
        }

        public List<string> GetBlacklistedMachines(int UserID)
        {
            List<string> blacklistedmachines = new List<string>();
            var getblacklistedmachines = _database.Table<MachineBlacklist>().Where(i => i.UserID == UserID).ToListAsync().Result;
            foreach (var machine in getblacklistedmachines)
            {
                var getmachine = _database.Table<Machine>().Where(i => i.MachineID == machine.MachineID).FirstOrDefaultAsync().Result;
                blacklistedmachines.Add(getmachine.MachineName);
            }
            return blacklistedmachines;
        }
        public void AddMachineToBlacklist(int UserID, string machinename)
        {
            var getmachineid = _database.Table<Machine>().Where(i => i.MachineName == machinename).FirstOrDefaultAsync().Result.MachineID;
            bool exists = CheckMachineBlacklist(UserID, getmachineid);
            if (exists == false)
            {
                _database.InsertAsync(new MachineBlacklist { UserID = UserID, MachineID = getmachineid }).Wait();
            }
        }
        public void RemoveMachineFromBlacklist(int UserID, string machinename)
        {
            var getmachineid = _database.Table<Machine>().Where(i => i.MachineName == machinename).FirstOrDefaultAsync().Result;
            bool exists = CheckMachineBlacklist(UserID, getmachineid.MachineID);
            if (exists)
            {
                string deleteQuery = $"DELETE FROM MachineBlacklist WHERE UserID = {UserID} AND MachineID = {getmachineid.MachineID}";
                _database.ExecuteAsync(deleteQuery).Wait();
            }
        }
        

    }
}
