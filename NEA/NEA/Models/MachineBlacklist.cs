using SQLite;
namespace NEA.Models
{
    // This class is used to store the machines blacklisted by the user
    [Table("MachineBlacklist")]
    public class MachineBlacklist
    {
        [Indexed(Name = "CompositeKey", Order = 1, Unique = true)]
        public int UserID { get; set; }

        [Indexed(Name = "CompositeKey", Order = 2, Unique = true)]
        public int MachineID { get; set; }
    }
    // https://stackoverflow.com/questions/42724830/creating-and-using-a-table-with-a-composite-primary-key-in-sqlite-net-pcl
    // The lines contaning composite key are from the link above
}
