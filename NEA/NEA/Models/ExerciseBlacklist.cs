using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEA.Models
{
    // This class is used to store the exercises that the user has blacklisted
    [SQLite.Table("ExerciseBlacklist")]
    public class ExerciseBlacklist
    {
        [ForeignKey("User")]
        public int UserID { get; set; }
        [ForeignKey("Exercise")]
        public int ExerciseID { get; set; }
    }
    
}
