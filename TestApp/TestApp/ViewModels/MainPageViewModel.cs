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
            //CurrentItem = "10";
            CountdownCommand = new DelegateCommand(CountdownCommandExcute);
            _modelState = ModelState.Picker;//默认为数字选择状态
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
        /// 当前值
        /// </summary>
        private ModelState _modelState;
        public ModelState ModelState
        {
            get { return _modelState; }
            set { SetProperty(ref _modelState, value); }
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


        /// <summary>
        /// 倒计时命令,根据状态判断
        /// </summary>
        void CountdownCommandExcute()
        {
            if (ModelState == ModelState.Picker)
            {
                Times = int.Parse(CurrentItem) * 60;//确认Times
                ModelState = ModelState.CountDown;//处于计时状态
                CountDown();//开始计时
            }
            else if (ModelState == ModelState.CountDown) //处于计时状态,则取消计时,关闭
            {
                //强制转为数字选择状态
                ModelState = ModelState.Picker;
            }
            else //处于正常结束状态
            {
                //正常倒计时结束后,转为数字选择状态
                ModelState = ModelState.Picker;
            }

        }

        /// <summary>
        /// 倒计时方法
        /// </summary>
        void CountDown()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (ModelState != ModelState.CountDown || Times <= 0)
                {
                    return false;
                }
                Times--;
                if (Times == 0)
                {
                    ModelState = ModelState.CountDownOver;//自然结束状态
                    return false;
                }
                return true;
            });
        }
    }

    /// <summary>
    /// Viewmodel的状态
    /// </summary>
    public enum ModelState
    {
        /// <summary>
        /// 数字选择状态
        /// </summary>
        Picker,
        /// <summary>
        /// 倒计时状态
        /// </summary>
        CountDown,
        /// <summary>
        /// 倒计时正常结束状态
        /// </summary>
        CountDownOver,
    }


    /// <summary>
    /// 将状态转化为 IsVisible的true,false
    /// </summary>
    public class StateToIsVisibleConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter">!!转换参数,传入字符串 npk->数字选择器,cp->进度条 lbl->分钟文字  </param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ModelState state = (ModelState)value;
            string wrapper = parameter == null ? null : parameter.ToString();
            switch (state)
            {
                case ModelState.Picker:
                    if (wrapper == "npk")
                    {
                        return true;
                    }
                    else if (wrapper == "cp" || wrapper == "lbl")
                    {
                        return false;
                    }
                    else
                    {
                        return false; //为null返回false
                    }
                case ModelState.CountDown:
                    if (wrapper == "npk")
                    {
                        return false;
                    }
                    else if (wrapper == "cp" || wrapper == "lbl")
                    {
                        return true;
                    }
                    else
                    {
                        return false; //为null返回false
                    }
                case ModelState.CountDownOver: //
                    if (wrapper == "npk")
                    {
                        return false;
                    }
                    else if (wrapper == "cp" || wrapper == "lbl")
                    {
                        return true;
                    }
                    else
                    {
                        return false; //为null返回false
                    }
                default:
                    return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ModelState.Picker;
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
            if (nowSeconds < 60)
            {
                return $"{nowSeconds}秒";
            }
            else
            {
                return $"{nowSeconds / 60}分钟";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }

    /// <summary>
    /// 已修改
    /// </summary>
    public class TimeToProgressConveter : IValueConverter
    {

        NumberPicker2 npk;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter">参数就是Npk数字选择器控件</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (npk == null)
            {
                npk = parameter as NumberPicker2;
            }
            int nowSeconds = (int)value;//获取当前秒    
            double allSeconds = 0;
            if (!string.IsNullOrWhiteSpace(npk.CurrentItem))
            {
                allSeconds = double.Parse(npk.CurrentItem) * 60;
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
