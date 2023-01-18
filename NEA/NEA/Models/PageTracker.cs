using SQLite;
namespace NEA.Models
{
    [Table("pagetracker")]
    public class PageTracker
    {
        // may implement or not, as null data can be check in userdata
        int Details { get; set; }
        int WorkoutQuitEarly { get; set; }
    }
}
