using NEA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NEA
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AutoPage : ContentPage
    {
        public AutoPage()
        {
            InitializeComponent();
            
        }

        private async void Regenerate_Clicked(object sender, EventArgs e)
        {

            
            await Navigation.PushAsync(new Regenerate());
            

        }

        
        
    }
}