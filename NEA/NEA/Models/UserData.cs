using SQLite;
namespace NEA.Models
{
    // This class is used to store user's workout data
    [Table("UserData")]
    public class UserData
    {
        [PrimaryKey]
        public int UserID { get; set; }
        public int TimeAvailable { get; set; }
        public int DaysAvailable { get; set; }
        public string Aim { get; set; }
        public int TotalSets { get; set; }
        public int TotalRep { get; set; }
        public int Volume { get; set; }
        // the TotalTime will in the format of seconds
        public int TotalTime { get; set; }
        public int NumberOfWorkout { get; set; }
    }
}
