using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace TestApp.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";
            CurrentItem = "3";
            CountdownCommand = new DelegateCommand(CountdownCommandExcute);
        }

        /// <summary>
        /// 当前值
        /// </summary>
        private string _currentItem;
        public string CurrentItem
        {
            get { return _currentItem; }
            set { SetProperty(ref _currentItem, value); }
        }

        /// <summary>
        /// 选定时的倒计时值
        /// </summary>
        private int _times;
        public int Times
        {
            get { return _times; }
            set { SetProperty(ref _times, value); }
        }

        public DelegateCommand CountdownCommand { get; private set; }


        void CountdownCommandExcute()
        {
            Times = int.Parse(CurrentItem) * 60;//获取总秒数
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
             {
                 if (Times <= 0)
                 {
                     return false;
                 }
                 else
                 {
                     Times--;                
                     return true;
                 }
             });
        }
    }

    /// <summary>
    /// 将times的值转化为分钟,秒的形式
    /// </summary>
    public class SecondsToMinConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int nowSeconds = (int)value;
            if (nowSeconds<60)
            {
                return $"{nowSeconds}秒";
            }
            else
            {
                return $"{nowSeconds/60}分钟";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }

    public class TimeToProgressConveter : IValueConverter
    {
        double allSeconds = -1;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int nowSeconds = (int)value;//获取当前秒
            if (allSeconds == -1 && nowSeconds != 0)
            {
                allSeconds = nowSeconds;
                return 0;
            }
            double progress = (allSeconds - nowSeconds) / allSeconds;
            return progress;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}
