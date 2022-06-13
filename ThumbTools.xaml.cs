using enki.dict.Commom.windowSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FishWork
{
    /// <summary>
    /// ThumbTools.xaml 的交互逻辑
    /// </summary>
    public partial class ThumbTools : Window
    {
        /// <summary>
        /// 鼠标按下
        /// </summary>
        bool isDown = false;

        double dwWidth = 0;
        double dwHeight = 0;

        Point dwPoint = new Point();
        
        /// <summary>
        /// 主窗口
        /// </summary>
        public ThumbWindow ThumbWindow { set; get; }

        public ThumbTools()
        {
            InitializeComponent();
        }

        private void slider_opacity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ThumbWindow == null)
                return;

            IntPtr handle = new WindowInteropHelper(ThumbWindow).Handle;
            WindowAPI.SetWindowOpacity(handle, (int)e.NewValue);

        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Show();
            Console.WriteLine("Tools-进入");
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Hide();
            Console.WriteLine("Tools-离开");
        }


        #region 调节窗口大小
        /// <summary>
        /// 鼠标放开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BntResize_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                isDown= false;
                bntResize.ReleaseMouseCapture();
                this.Top = ThumbWindow.Top + ThumbWindow.Height - this.Height;
                this.Opacity = 1;
            }
        }
        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BntResize_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDown)
            {
                var point = e.GetPosition(this.Parent as Canvas);
                var diffX = point.X - dwPoint.X;
                var diffY = point.Y - dwPoint.Y;

                ThumbWindow.Width = dwWidth + diffX;
                ThumbWindow.Height = dwHeight + diffY;
                this.Width = ThumbWindow.Width;
                //

            }
        }
        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BntResize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                e.Handled = true;
                dwPoint = e.GetPosition(this);
                isDown = true;
                dwWidth = ThumbWindow.Width;
                dwHeight = ThumbWindow.Height;



                bntResize.CaptureMouse();
                this.Opacity = 0.01;
            }
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bntResize.MouseDown += BntResize_MouseDown;
            bntResize.MouseMove += BntResize_MouseMove;
            bntResize.MouseUp += BntResize_MouseUp;
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbClose_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ThumbWindow.Close();
            this.Close();
        }
    }
}
