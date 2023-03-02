using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEA.Models
{
    // This class is used to store the muscles blacklisted by the user
    [SQLite.Table("MuscleBlacklist")]
    public class MuscleBlacklist
    {
        [ForeignKey("User")]
        public int UserID { get; set; }
        [ForeignKey("Muscle")]
        public int MuscleID { get; set; }
        
    }
}
