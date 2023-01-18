using SQLite;
namespace NEA.Models
{
    [Table("ExerciseTracker")]
    public class ExerciseTracker
    {
        int ExerciseID { get; set; }
        int SetNumber { get; set; }
        int Reps { get; set; }
        int Weight { get; set; }

    }
}
