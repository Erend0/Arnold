using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NEA.Data;
using NEA.Tasks;

namespace NEA
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AutoPage : ContentPage
    {
        public AutoPage()
        {
            // pass the days to generate into workout
            Workout myworkout = new Workout("fill");
            
            InitializeComponent();
          


        }
    }
}