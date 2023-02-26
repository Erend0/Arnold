using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NEA.Models;
using SQLite;


namespace NEA.Data
{
    internal class MachineRepository
    {

        static SQLiteAsyncConnection _database;
        public MachineRepository()
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
            _database.CreateTableAsync<Machine>().Wait();
        }
        public string GetMachineName(int machineID)
        {
            var machine = _database.Table<Machine>().Where(i => i.MachineID == machineID).FirstOrDefaultAsync().Result;
            if (machine != null)
            {
                return machine.MachineName;
            }
            else
            {
                return "Error";
            }
        }
        
        public List<Machine> GetAllMachines()
        {
            return _database.Table<Machine>().ToListAsync().Result;
        }

        // This method checks if a machine exists, if it does its exerciseID is returned
        // If not the the method returns -1
        public int CheckMachineExists(string MachineName)
        {
            var machine = _database.Table<Machine>().Where(i => i.MachineName == MachineName).FirstOrDefaultAsync().Result;
            if (machine != null)
            {
                return machine.MachineID;
            }
            else
            {
                return -1;
            }
        }
        

    }
}
