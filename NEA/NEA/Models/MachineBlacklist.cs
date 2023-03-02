using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEA.Models
{
    // This class is used to store the machines blacklisted by the user
    [SQLite.Table("MachineBlacklist")]
    public class MachineBlacklist
    {
        [ForeignKey("User")]
        public int UserID { get; set; }
        [ForeignKey("Machine")]

        public int MachineID { get; set; }
    }
}
