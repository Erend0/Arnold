using SQLite;
namespace NEA.Models
{
    // This class is used to store the data for each muscle
    [Table("Muscle")]
    public class Muscle
    {
        [PrimaryKey, AutoIncrement]
        public int MuscleID { get; set; }
        public string MajorMuscle { get; set; }
        public string MinorMuscle { get; set; }
        
    }
}
