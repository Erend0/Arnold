using SQLite;
namespace NEA.Models
{
    // This class is used to store the muscles blacklisted by the user
    [Table("MuscleBlacklist")]
    public class MuscleBlacklist
    {
        public int UserID { get; set; }
        public int MuscleID { get; set; }
        
    }
}
