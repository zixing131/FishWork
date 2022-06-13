using enki.dict.Commom.windowSDK;
using FishWork.Commom;
using NetDict.UI.UIControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace FishWork.ComponentSupport
{
    /// <summary>
    /// 寻找窗口支持类
    /// </summary>
    public class FindWindowSupport
    {
        /// <summary>
        /// 指针
        /// </summary>
        Cursor curPoint = null;

        DeskTopComponenet componenet = null;

        SVGImage ic_point = null;

        bool isDownFind = false;

        /// <summary>
        /// 目标窗口
        /// </summary>
        IntPtr targetHwnd = IntPtr.Zero;

        /// <summary>
        /// 自身窗口
        /// </summary>
        IntPtr selfHwnd = IntPtr.Zero;
        public FindWindowSupport(DeskTopComponenet _componenet) {
            this.componenet = _componenet;


            ic_point = componenet.ic_point_window;
            ic_point.PreviewMouseLeftButtonDown += Ic_point_window_PreviewMouseLeftButtonDown;
            ic_point.PreviewMouseLeftButtonUp += Ic_point_window_PreviewMouseLeftButtonUp;
            ic_point.MouseRightButtonDown += Ic_point_MouseRightButtonDown;
            ic_point.PreviewMouseRightButtonUp += Ic_point_PreviewMouseRightButtonUp;
            ic_point.PreviewMouseMove += Ic_point_window_PreviewMouseMove;


            System.IO.MemoryStream memory = new System.IO.MemoryStream(Resource1.curPoint);
            curPoint = new Cursor(memory);

            selfHwnd = new WindowInteropHelper(Window.GetWindow(componenet)).Handle;

        }

        private void Ic_point_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// 显示与隐藏窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ic_point_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            componenet.SwitchTargetVisible();
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ic_point_window_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!isDownFind)
                return;

            var hwnd = WindowAPI.GetMouseWindowHandle(); 
            if (hwnd == IntPtr.Zero || hwnd== selfHwnd)
                return;
           
            targetHwnd = hwnd;


            WindowAPI.RECT rect = new WindowAPI.RECT();
            WindowAPI.GetWindowRect(hwnd, ref rect);

            var borderShow = BorderShowWindow.GetInstance();
            borderShow.Left = rect.Left;
            borderShow.Top = rect.Top;
            borderShow.Width = rect.Right - rect.Left;
            borderShow.Height = rect.Bottom - rect.Top;
            borderShow.Show();

            
        }

        /// <summary>
        /// 鼠标左键放开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ic_point_window_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isDownFind = false;
            ic_point.Opacity = 1;
            ic_point.ReleaseMouseCapture();
            ic_point.Cursor = Cursors.Arrow;
            BorderShowWindow.GetInstance().Hide();

            if (targetHwnd == IntPtr.Zero)
                return;

            string windowTitle = "当前目标 - " + WindowAPI.GetWindowText(targetHwnd);
            ic_point.Fill = ColorUtils.GetColor("#7BFF00");
            ic_point.ToolTip = windowTitle;
        }

        /// <summary>
        /// 鼠标左键按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ic_point_window_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e){
            e.Handled = true;
            isDownFind = true;
            ic_point.Cursor = curPoint;
            ic_point.CaptureMouse();
            ic_point.Opacity = 0.1;
        }

        /// <summary>
        /// 获取目标窗口
        /// </summary>
        /// <returns></returns>
        public IntPtr GetTargetHwnd() {
            return targetHwnd;
        }

        /// <summary>
        /// 获取当前焦点窗口
        /// </summary>
        public string FindCurrentActivityWindow() {
            var hwnd = WindowAPI.GetForegroundWindow();
            if (hwnd == selfHwnd)
                return "";
            targetHwnd = hwnd;
            string windowTitle = "当前目标 - " + WindowAPI.GetWindowText(targetHwnd);
            ic_point.Fill = ColorUtils.GetColor("#7BFF00");
            ic_point.ToolTip = windowTitle;
           
            return windowTitle;
        }
    }
}
