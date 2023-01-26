using SQLite;

namespace NEA.Models.OtherModels
{
    [Table("ResumePage")]
    public class ResumePage
    {
        [PrimaryKey]
        public int UserID { get; set; }
        public int DataGrab { get; set; }
        public string DayName { get; set; }
        public int Type { get; set; }
        public int Time { get; set; }
    }
}
