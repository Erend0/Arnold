using SQLite;
namespace NEA.Models
{
    // This class is used to store which muscles are targeted during an exercise
    [Table("MuscleTargeted")]
    public class MuscleTargeted
    {
        [PrimaryKey, AutoIncrement]
        [Indexed(Name = "CompositeKey", Order = 1, Unique = true)]
        public int ExerciseID { get; set; }

        [Indexed(Name = "CompositeKey", Order = 2, Unique = true)]
        public int MinorMuscleID { get; set; }
    }
}
