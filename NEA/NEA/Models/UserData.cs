using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEA.Models
{
    // This class is used to store user's workout data
    [SQLite.Table("UserData")]
    public class UserData
    {
        [PrimaryKey]
        [ForeignKey("User")]
        public int UserID { get; set; }
        public int TimeAvailable { get; set; }
        public int DaysAvailable { get; set; }
        public string Aim { get; set; }
        public int TotalSets { get; set; }
        public int TotalRep { get; set; }
        public int Volume { get; set; } // sets * reps * weight
        public int TotalTime { get; set; } // Stored in seconds
        public int NumberOfWorkout { get; set; }
    }
}

