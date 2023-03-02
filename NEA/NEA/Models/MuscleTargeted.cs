using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEA.Models
{
    // This class is used to store which muscles are targeted during an exercise
    [SQLite.Table("MuscleTargeted")]
    public class MuscleTargeted
    {
        [PrimaryKey, AutoIncrement]

        [ForeignKey("Exercise")]
        
        public int ExerciseID { get; set; }

        [ForeignKey("Muscle")]
        public int MuscleID { get; set; }
    }
}
