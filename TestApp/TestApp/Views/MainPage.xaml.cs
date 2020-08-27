using System;
using System.Globalization;
using Xamarin.Forms;

namespace TestApp.Views
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();            
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            this.npkWrapper.IsVisible = false;
            this.minlblWrapper.IsVisible = true;
            this.circleWrapper.IsVisible = true;
        }
    }

  
}
