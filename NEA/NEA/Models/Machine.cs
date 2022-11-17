using SQLite;
namespace NEA.Models
{
    // This class is used to store the data for each machine
    [Table("Machine")]
    public class Machine
    {
        [AutoIncrement,PrimaryKey]
        public int MachineId { get; set; }
        public string MachineName { get; set; }
        
    }
}
