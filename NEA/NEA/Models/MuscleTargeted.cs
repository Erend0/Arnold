﻿using SQLite;

namespace NEA.Models
{
    [Table("MuscleTargeted")]
    public class MuscleTargeted
    {
        [PrimaryKey, AutoIncrement]
        [Indexed(Name = "CompositeKey", Order = 1, Unique = true)]
        public int ExerciseID { get; set; }

        [Indexed(Name = "CompositeKey", Order = 2, Unique = true)]
        public int MinorMuscleID { get; set; }
    }
}
