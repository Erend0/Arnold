namespace NEA.Tasks
{

    
    internal class CurrentExercise
    {
        // Exercise Details
        public int ExerciseID { get; set; }
        public string ExerciseName { get; set; }
        public int MachineID { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public int MachineName { get; set; }
        // Muscle Details
        public int MuscleID { get; set; }
        public string MajorMuscle { get; set; }
        public string MinorMuscle { get; set; }
        // Machine Details
        public int MachineId { get; set; }
       
        public CurrentExercise()
        {
        }
        public void GetDetailsforBlacklist()
        {
            
            
        }
        
    }
}
