using SQLite;
namespace NEA.Models
{
    [Table("Schedule")]
    public class Schedule
    {
        public int UserID { get; set; } 
        public string DayName { get; set; }
        public int ExerciseID{ get; set; }
        // For autogenerate days the type will be set to 0, for custom days to 1
        public int Type { get; set; }
    }
}
