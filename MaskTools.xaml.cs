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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FishWork
{
    /// <summary>
    /// MaskTools.xaml 的交互逻辑
    /// </summary>
    public partial class MaskTools : Window
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
        public MaskWindow ThumbWindow { set; get; }

        public MaskTools()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗口加载完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bntResize.MouseDown += BntResize_MouseDown;
            bntResize.MouseMove += BntResize_MouseMove;
            bntResize.MouseUp += BntResize_MouseUp;
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
                isDown = false;
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
                //Console.WriteLine(ThumbWindow.Height);
                //this.Width = ThumbWindow.Width;
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

        /// <summary>
        /// 完成调节
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntFinish_OnClick(object sender, EventArgs e)
        {
            ThumbWindow.FinishResize();
        }
    }
}
