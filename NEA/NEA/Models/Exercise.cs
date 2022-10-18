using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace NEA.Models
{
    public class Exercise
    {
        [PrimaryKey, AutoIncrement]
        public int ExerciseID { get; set; }
        public string ExerciseName { get; set; }

        public int MachineID { get; set; }
        public int sets { get; set; }
        public int reps { get; set; }
    }
}
