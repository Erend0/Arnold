using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEA.Models
{
    [SQLite.Table("Schedule")]
    public class Schedule
    {
        [NotNull]
        [ForeignKey("User")]
        public int UserID { get; set; }
        [NotNull]
        public string DayName { get; set; }
        [NotNull]
        [ForeignKey("Exercise")]
        public int ExerciseID{ get; set; }
        // The sets and reps are not necessary to be set and are only set for custom exercises, or TYPE = 1
        public int Sets { get; set; }
        public int Reps { get; set; }
        public int Type { get; set; }
    }
}
