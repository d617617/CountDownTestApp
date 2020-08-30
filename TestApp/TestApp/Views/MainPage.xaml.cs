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

        /// <summary>
        /// 完全mvvm,不需要
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Clicked(object sender, System.EventArgs e)
        {
            //this.npkWrapper.IsVisible = false;
            //this.minlblWrapper.IsVisible = true;
            //this.circleWrapper.IsVisible = true;
        }
    }

  
}
