using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetDict.UI.UIControls
{
    /// <summary>
    /// UIButton.xaml 的交互逻辑
    /// </summary>
    public partial class UIButton : UserControl
    {

        /// <summary>
        /// 内容字段
        /// </summary>
        public readonly static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(UIButton),
                                                           new PropertyMetadata(new PropertyChangedCallback(transparencyPropertyChangedCallback)));

        /// <summary>
        /// 背景颜色
        /// </summary>
        public readonly static DependencyProperty BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(Brush), typeof(UIButton),
                                                          new PropertyMetadata(new PropertyChangedCallback(backColorPropertyChangedCallback)));

        /// <summary>
        /// 背景图片
        /// </summary>
        public readonly static DependencyProperty BackgroundImageProperty = DependencyProperty.Register("BackgroundImage", typeof(Brush), typeof(UIButton),
                                                          new PropertyMetadata(new PropertyChangedCallback(backImagePropertyChangedCallback)));

        /// <summary>
        /// 圆角
        /// </summary>
        public readonly static DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UIButton),
                                                          new PropertyMetadata(new PropertyChangedCallback(cornerRadiusChangedCallback)));
        /// <summary>
        /// 按钮标题
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set
            {
                lbText.Text = value;
                SetValue(TextProperty, value);
            }
        }

        /// <summary>
        /// 背景颜色
        /// </summary>
        public Brush BackgroundColor
        {
            set
            {
                border.Background = value;
                SetValue(BackgroundColorProperty, value);
            }
        }

        /// <summary>
        /// 背景图片
        /// </summary>
        public Brush BackgroundImage
        {
            set
            {
                border.Background = value;
                SetValue(BackgroundImageProperty, value);
            }
        }

        /// <summary>
        /// 设置圆角属性
        /// </summary>
        public CornerRadius CornerRadius
        {
            set
            {
                border.CornerRadius = value;
                SetValue(CornerRadiusProperty, value);
            }
        }

        /// <summary>
        /// 整数参数1
        /// </summary>
        public int Int1 { set; get; }

        /// <summary>
        /// 文本参数1
        /// </summary>
        public string Str1 { set; get; }

        private event EventHandler _click;
        /// <summary>
        /// 启用状态
        /// </summary>
        public bool EnableState {
            set {
                this.IsEnabled = value;
                if (!value)
                {
                    border.Opacity = 0.5;
                }
                else {
                    border.Opacity = 1;
                }
            }
            get {
                return this.IsEnabled;
            }
        }

        /// <summary>
        /// 设置图标
        /// </summary>
        public string Icon {
            set {
                icon.Source = new BitmapImage(new Uri(value,UriKind.RelativeOrAbsolute));
                icon.Visibility = Visibility.Visible;
                lbText.Margin = new Thickness(0, 0, 3, 0);
            }
            get {
                return "";
            }
        }

        /// <summary>
        /// 图标大小
        /// </summary>
        public int IconWidth {
            set {
                icon.Width = value;
            }
            get {
                return 0;
            }
        }
        /// <summary>
        /// 公开事件属性
        /// </summary>
        public event EventHandler OnClick
        {
            add
            {
                _click += value;
            }
            remove
            {
                _click -= value;
            }
        }
        /// <summary>
        /// Title 设置回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void transparencyPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UIButton bnt = (sender as UIButton);
            if (bnt != null)
            {
                bnt.lbText.Text = e.NewValue.ToString();
            }
        }
        /// <summary>
        /// 背景颜色设置回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void backColorPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UIButton bnt = (sender as UIButton);
            if (bnt != null)
            {
                bnt.border.Background = e.NewValue as Brush;
            }
        }
        /// <summary>
        /// 背景图片设置回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void backImagePropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UIButton bnt = (sender as UIButton);
            if (bnt != null)
            {
                bnt.border.Background = e.NewValue as ImageBrush;
            }
        }
        /// <summary>
        /// 圆角改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void cornerRadiusChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UIButton bnt = (sender as UIButton);
            if (bnt != null)
            {
                bnt.border.CornerRadius = (CornerRadius)e.NewValue;
            }
        }

        public UIButton()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.MouseDown += UIButton_MouseDown;
            this.MouseUp += UserButton_MouseUp;
        }

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UIButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Opacity = 0.5;
        }

        /// <summary>
        /// 鼠标放开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void UserButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!this.IsEnabled)
                return;
            this.Opacity = 1;
            if (_click != null)
            {
                _click(sender, e);
            }
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            this.Opacity = 0.8;
        }
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            this.Opacity = 1;
        }

        
    }
}
