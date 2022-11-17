using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NEA.Tasks;


namespace NEA
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AutoPage : ContentPage
    {
        public AutoPage()
        {
           
            InitializeComponent();
           

        }

        private void Button2_Pressed(object sender, System.EventArgs e)
        {
           // pass the days to generate into workout
           Workout myworkout = new Workout("Biceps");
           int[][] testvar = myworkout.generatedworkout;
           foreach (int[] item in testvar)
           {
               // create a display alert with teh item variablle 
              DisplayAlert("test", item.ToString(), "ok");
           }


        }
    }
}