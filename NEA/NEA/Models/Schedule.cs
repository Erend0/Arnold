using SQLite;
namespace NEA.Models
{
    
    
    [Table("Schedule")]
    public class Schedule
    {
        
        public int UserID { get; set; } 
        public string Dayname { get; set; }
        // The database contains the exercisename and not the exerciseid 
        // This is done so that this model for the table can be used both for the auto and custom generated workouts 
        // User generated workouts may contain exercises not in the database meaning no exerciseid can be found
        public string ExerciseName{ get; set; }
        public int Type { get; set; }
        public int MuscleID { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; } 
       
    }
}
