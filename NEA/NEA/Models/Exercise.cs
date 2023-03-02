using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEA.Models      
{
    // This class is used to store details about the exercises
    [SQLite.Table("Exercise")]
    public class Exercise
    {
            [AutoIncrement, PrimaryKey]
            public int ExerciseID { get; set; }
            public string ExerciseName { get; set; }
            [ForeignKey("Machine")]
            public int MachineID { get; set; }
            public int Sets { get; set; }
            public int Reps { get; set; }
    }
}
