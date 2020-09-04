using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SuperRabbit.Lib;
using Xamarin.Forms.Platform.Android;


[assembly: Xamarin.Forms.ExportRenderer(typeof(TestApp.NumberPicker2), typeof(TestApp.Droid.NumberPickerRender))]
namespace TestApp.Droid
{
    public class NumberPickerRender : ViewRenderer<NumberPicker2, WheelPicker>
    {
        public NumberPickerRender(Context context) : base(context)
        {

        }


        WheelPicker wheelPicker;

        protected override void OnElementChanged(ElementChangedEventArgs<NumberPicker2> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                // Unsubscribe
                var ele = e.OldElement;
            }
            if (e.NewElement != null)
            {
                var ele = e.NewElement;
                if (Control == null)
                {
                    wheelPicker = new WheelPicker(Context);
                    SetNativeControl(wheelPicker);

                }// Subscribe    

            }
        }


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == nameof(NumberPicker2.CurrentItem))
            {
                //拨动改变值后,防止递归
                if (wheelPicker.CurrentItem == Element.CurrentItem)
                {
                    return;
                }
                if (!string.IsNullOrWhiteSpace(Element.CurrentItem))
                {
                    wheelPicker.SmoothScrollToValue(Element.CurrentItem);
                }
            }
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();

            wheelPicker.SetWheelItemCount(3);
            wheelPicker.SetAdapter(new DaysAdapter());
            wheelPicker.SetSelectorRoundedWrapPreferred(true);
            wheelPicker.ValueChange += WheelPicker_ValueChange;
            //开始反射,设定文字色,字号
            ReflectionToSetValue();
            //设定初始值
            if (!string.IsNullOrWhiteSpace(Element.CurrentItem))
            {
                wheelPicker.ScrollToValue(Element.CurrentItem);
            }
        }

        void ReflectionToSetValue()
        {
            var _class = wheelPicker.Class;
            var des = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density;
            var sizeField = _class.GetDeclaredField("mTextSize");
            sizeField.Accessible = true;
            sizeField.Set(wheelPicker, (int)Math.Ceiling(Element.FontSize * des));
            var selecedColorField = _class.GetDeclaredField("mSelectedTextColor");
            selecedColorField.Accessible = true;
            selecedColorField.Set(wheelPicker, (int)Element.SelectedColor.ToAndroid());
            var unselecedColorField = _class.GetDeclaredField("mUnSelectedTextColor");
            unselecedColorField.Accessible = true;
            unselecedColorField.Set(wheelPicker, (int)Element.UnSelectedTextColor.ToAndroid());
        }

        /// <summary>
        /// 滚动时,currentitem值变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WheelPicker_ValueChange(object sender, ValueChangeEventArgs e)
        {
            Element.CurrentItem = e.NewVal;
        }
    }

    class DaysAdapter : Java.Lang.Object, IWheelAdapter
    {
        public int MaxIndex => 19;

        public int MinIndex => 0;

        public string TextWithMaximumLength => "30";

        public int GetPosition(string value)
        {
            return int.Parse(value) - 10;
        }

        public string GetValue(int position)
        {
            if (position == -2)
            {
                return "29";
            }
            if (position == -1)
            {
                return "30";
            }
            return (position + 10).ToString();
        }
    }
}