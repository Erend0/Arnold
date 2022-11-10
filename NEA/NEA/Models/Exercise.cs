using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace NEA.
     [Table("Exercise")]
      
{
    public class Exercise
    {

       
            [AutoIncrement, PrimaryKey]

            public int ExerciseID { get; set; }
            public string ExerciseName { get; set; }
            public int MachineID { get; set; }
            public int Sets { get; set; }
            public int Reps { get; set; }
            

        
    }
}
