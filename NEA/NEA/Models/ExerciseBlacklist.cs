using SQLite;
namespace NEA.Models
{
    // This class is used to store the exercises that the user has blacklisted
    [Table("ExerciseBlacklist")]
    public class ExerciseBlacklist
    {   // The UserID and ExerciseID are turned into a composite key below
        [Indexed(Name = "CompositeKey", Order = 1, Unique = true)]
        public int UserID { get; set; }
        [Indexed(Name = "CompositeKey", Order = 2, Unique = true)]
        public int ExerciseID { get; set; }
    }
    
}
