using SQLite;

namespace NEA.Models.OtherModels
{
    [Table("AddedSets")]
    public class AddedSets
    {
        public int ExerciseIndex { get; set; }
        public int NumberofSets{ get; set; }
    }
}
