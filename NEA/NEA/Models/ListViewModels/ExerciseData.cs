using System.Diagnostics.CodeAnalysis;

namespace NEA.Models.ListViewModels
{
    public class ExerciseData
    {
        public string ExerciseName { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set;}
        public string MachineName { get; set; }
        public string MinorMuscle { get; set; }
        public string MajorMuscle { get; set; }
        public bool IsChecked { get; set; }
        public bool IsCustom { get; set; }
    }
}
