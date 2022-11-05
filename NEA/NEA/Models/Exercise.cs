using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace NEA.Models
{
    internal class Exercise
    {

        [Table("Exercise")]
        public class User
        {
            [AutoIncrement, PrimaryKey]

            public int ExerciseID { get; set; }
            public string ExerciseName { get; set; }
            public int MachineID { get; set; }
            public int Sets { get; set; }
            public int Reps { get; set; }
            

        }
    }
}
