using SQLite;

namespace NEA.Models.OtherModels
{
    public class WorkoutTracker
    {
        public int Index { get; set; }
        public int Set { get; set; }
        public int Reps { get; set; }
        public int Weight { get; set; }
    }
}
