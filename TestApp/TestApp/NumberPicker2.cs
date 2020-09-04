using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace TestApp
{
    public class NumberPicker2 : View
    {


        public Color SelectedColor { get; set; } = Color.Red;

        public Color UnSelectedTextColor { get; set; } = Color.Blue;


        public static readonly BindableProperty FontSizeProperty =
     BindableProperty.Create(nameof(FontSize), typeof(double), typeof(NumberPicker2), 30d);

        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }



        public event Action<object, EventArgs> CurrentItemChanged;

        #region CurrentItem
        /// <summary>
        /// mvvm 选中的值
        /// </summary>
        public static readonly BindableProperty CurrentItemProperty =
      BindableProperty.Create(nameof(CurrentItem), typeof(string), typeof(NumberPicker2), default(string)
          , propertyChanged: (obj, o, n) =>
          {
              (obj as NumberPicker2).CurrentItemPropertyChanged();
          });

        void CurrentItemPropertyChanged()
        {
            CurrentItemChanged?.Invoke(this, new EventArgs());
        }

        public string CurrentItem
        {
            get { return (string)GetValue(CurrentItemProperty); }
            set
            {
                SetValue(CurrentItemProperty, value);
            }
        }
        #endregion


        #region ChangeItemCommand
        public static readonly BindableProperty ChangeItemCommandProperty =
 BindableProperty.Create(nameof(ChangeItemCommand), typeof(ICommand), typeof(NumberPicker2), default(ICommand));

        public ICommand ChangeItemCommand
        {
            get { return (ICommand)GetValue(ChangeItemCommandProperty); }
            set
            {
                SetValue(ChangeItemCommandProperty, value);
            }
        }
        #endregion

        public NumberPicker2()
        {
            ChangeItemCommand = new Command<string>(ChangeItemCommandExcute);
        }

        void ChangeItemCommandExcute(string newItem)
        {
            if (newItem == CurrentItem)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(newItem))
            {
                return;
            }
            CurrentItem = newItem;
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {

            var sizeRequest = base.OnMeasure(widthConstraint, heightConstraint);
            var size = sizeRequest.Request;
            size.Height = this.FontSize * 3;
            return new SizeRequest(size);
        }
    }
}
