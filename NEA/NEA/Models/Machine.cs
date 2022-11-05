using SQLite;

namespace NEA.Models
{
    [Table("Machine")]
    internal class Machine
    {
        [AutoIncrement,PrimaryKey]
        public int MachineId { get; set; }
        public string MachineName { get; set; }
        
    }
}
