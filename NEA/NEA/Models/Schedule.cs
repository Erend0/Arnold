using SQLite;
namespace NEA.Models
{
    [Table("Schedule")]
    public class Schedule
    {
        [NotNull]
        public int UserID { get; set; }
        [NotNull]
        public string DayName { get; set; }
        [NotNull]
        public int ExerciseID{ get; set; }
        // The sets and reps are not necessary to be set and are only set for custom exercises, or TYPE = 1
        public int Sets { get; set; }
        public int Reps { get; set; }
        [NotNull]
        // For autogenerate days the type will be set to 0, for custom days to 1
        public int Type { get; set; }
    }
}
