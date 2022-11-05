using SQLite;

namespace NEA.Models
{
    [Table("Muscle")]
    internal class Muscle
    {
        [PrimaryKey, AutoIncrement]
        public int MuscleID { get; set; }
        public string MajorMuscle { get; set; }
        public string MinorMuscle { get; set; }
        
    }
}
