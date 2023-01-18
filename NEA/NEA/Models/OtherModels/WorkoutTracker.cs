using SQLite;

namespace NEA.Models.OtherModels
{
    [Table("WorkoutTracker")]
    public class WorkoutTracker
    {
        [PrimaryKey]
        int ExerciseID { get; set; }
        int Set { get; set; }
        int Reps { get; set; }
        int Weight { get; set; }
        
    }
}
