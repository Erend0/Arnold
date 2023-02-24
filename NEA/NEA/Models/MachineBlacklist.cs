using SQLite;
namespace NEA.Models
{
    // This class is used to store the machines blacklisted by the user
    [Table("MachineBlacklist")]
    public class MachineBlacklist
    {
        public int UserID { get; set; }
        
        public int MachineID { get; set; }
    }
}
