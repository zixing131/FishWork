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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FishWork
{
    /// <summary>
    /// ClipRectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ClipRectWindow : Window
    {
        static ClipRectWindow _this = null;

        /// <summary>
        /// 回调
        /// </summary>
        Action<Rect?> callBack = null;

        bool isDown = false;
        Point dwPoint = new Point();

        /// <summary>
        /// 选择的矩形
        /// </summary>
        Rect? rect = null;
        private ClipRectWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns></returns>
        public static ClipRectWindow GetInstance() {
            if (_this==null) {
                _this = new ClipRectWindow();
            }
            return _this;
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="targetHwnd"></param>
        public void ShowWindow(IntPtr targetHwnd,Action<Rect?> _callBack) {

            this.callBack = _callBack;
            WindowAPI.RECT rect = new WindowAPI.RECT();
            WindowAPI.GetWindowRect(targetHwnd, ref rect);
            this.Left = rect.Left;
            this.Top = rect.Top;
            this.Width = rect.Right - rect.Left;
            this.Height = rect.Bottom - rect.Top;

            border.Width = 0;
            border.Height = 0;
            this.Show();
        }

        /// <summary>
        /// 窗口加载完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e){
            cvs.PreviewMouseLeftButtonDown += Cvs_MouseDown;
            cvs.PreviewMouseLeftButtonUp += Cvs_MouseUp;
            cvs.MouseRightButtonDown += Cvs_MouseRightButtonDown;
            cvs.MouseMove += Cvs_MouseMove;
        }

        /// <summary>
        /// 鼠标右键按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cvs_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cvs_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDown)
                return;

            if (rect == null) {
                rect = new Rect();
            }

            var point = e.GetPosition(cvs);
            double diffX = point.X - dwPoint.X;
            double diffY = point.Y - dwPoint.Y;

            if (diffX <= 0) {
                diffX = 0;
            }
            if (diffY <= 0) {
                diffY = 0;
            }

            border.Width = diffX;
            border.Height = diffY;

        }

        /// <summary>
        /// 鼠标放开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cvs_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDown = false;
            cvs.ReleaseMouseCapture();
            if (callBack != null) {
                rect = new Rect((double)border.GetValue(Canvas.LeftProperty), (double)border.GetValue(Canvas.TopProperty),border.Width,border.Height);
                if (rect.Value.Width < 50 || rect.Value.Height < 20) {
                    rect = null;
                }
                callBack(rect);
            }
            this.Hide();
        }

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cvs_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isDown = true;
            dwPoint = e.GetPosition(cvs);
            cvs.CaptureMouse();
            border.SetValue(Canvas.LeftProperty, dwPoint.X-1);
            border.SetValue(Canvas.TopProperty, dwPoint.Y-2);
            
        }
    }
}
