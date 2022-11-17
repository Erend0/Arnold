using SQLite;
s
namespace NEA.Models
{
    
    [Table("Schedule")]
    public class Schedule
    {
        [PrimaryKey,AutoIncrement]
        public int RowID { get; set; }
        public int UserID { get; set; } 
        public string Dayname { get; set; }
        public string ExerciseName{ get; set; }
        public string MachineName { get; set; }
        public string MuscleMajorName { get; set; }
        public string MuscleMinorName { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; } 
    }
}
