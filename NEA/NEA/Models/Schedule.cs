using SQLite;
namespace NEA.Models
{
    [Table("Schedule")]
    public class Schedule
    {
        public int UserID { get; set; } 
        public string DayName { get; set; }
        public int ExerciseID{ get; set; }
    }
}
