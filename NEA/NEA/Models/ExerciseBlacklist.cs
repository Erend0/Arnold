using SQLite;

namespace NEA.Models
{
    [Table("ExerciseBlacklist")]
    internal class ExerciseBlacklist
    {

        [Indexed(Name = "CompositeKey", Order = 1, Unique = true)]
        public int UserID { get; set; }

        [Indexed(Name = "CompositeKey", Order = 2, Unique = true)]
        public int ExerciseID { get; set; }


    }
    
}
