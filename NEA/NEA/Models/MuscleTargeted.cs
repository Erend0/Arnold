using SQLite;
namespace NEA.Models
{
    // This class is used to store which muscles are targeted during an exercise
    [Table("MuscleTargeted")]
    public class MuscleTargeted
    {
        [PrimaryKey, AutoIncrement]
        
        public int ExerciseID { get; set; }

        
        public int MuscleID { get; set; }
    }
}
