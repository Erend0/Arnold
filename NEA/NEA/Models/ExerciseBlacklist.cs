using SQLite;
namespace NEA.Models
{
    // This class is used to store the exercises that the user has blacklisted
    [Table("ExerciseBlacklist")]
    public class ExerciseBlacklist
    {  
        public int UserID { get; set; }
        public int ExerciseID { get; set; }
    }
    
}
