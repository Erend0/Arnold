using SQLite;
namespace NEA.Models      
{
    // This class is used to store details about the exercises
    [Table("Exercise")]
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
