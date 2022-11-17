using SQLite;
namespace NEA.Models
{
    // This class is used to store the muscles blacklisted by the user
    [Table("MachineBlacklist")]
    public class MuscleBlacklist
    {
        [Indexed(Name = "CompositeKey", Order = 1, Unique = true)]
        public int UserID { get; set; }

        [Indexed(Name = "CompositeKey", Order = 2, Unique = true)]
        public int MuscleID { get; set; }
    }
}
