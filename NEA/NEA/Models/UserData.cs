using SQLite;

namespace NEA.Models
{
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
        public int TotalTime { get; set; }
        public int NumerOfWorkout { get; set; }
    }
}
