using FishWork.Windows.Commom;
using PiP_Tool.Native;
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
    /// ThumbWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ThumbWindow : Window
    {
        /// <summary>
        /// 目标窗口
        /// </summary>
        IntPtr targetHwnd = IntPtr.Zero;

        /// <summary>
        /// 矩形范围
        /// </summary>
        Rect rect = Rect.Empty;

        /// <summary>
        /// 缩略图句柄
        /// </summary>
        IntPtr thumbHandle = IntPtr.Zero;

        /// <summary>
        /// 工具栏
        /// </summary>
        ThumbTools thumbTools = null;

        private ThumbWindow()
        {
            InitializeComponent();
        }

        public ThumbWindow(IntPtr _targetHwnd,Rect _rect) {
            this.targetHwnd = _targetHwnd;
            this.rect = _rect;
            InitializeComponent();
        }

        /// <summary>
        /// 加载完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            thumbTools = new ThumbTools();
            thumbTools.ThumbWindow = this;
            Init();

   
        }

        /// <summary>
        /// 初始化
        /// </summary>
        void Init() {
            this.Width = rect.Width;
            this.Height = rect.Height;

            IntPtr handle = new WindowInteropHelper(this).Handle;
            
            if (NativeMethods.DwmRegisterThumbnail(handle, targetHwnd, out thumbHandle) == 0)
            {
                var dest = new NativeStructs.Rect(0, 0, (int)this.Width, (int)this.Height);
                var tarRect = new NativeStructs.Rect((int)rect.Left, (int)rect.Top, (int)rect.Left+(int)rect.Width, (int)rect.Top+(int)rect.Height);
                var props = new NativeStructs.DwmThumbnailProperties
                {
                    fVisible = true,
                    dwFlags = (int)(DWM_TNP.DWM_TNP_VISIBLE | DWM_TNP.DWM_TNP_RECTDESTINATION | DWM_TNP.DWM_TNP_OPACITY | DWM_TNP.DWM_TNP_RECTSOURCE),
                    opacity = 255,
                    rcDestination = dest,
                    rcSource = tarRect
                };

                NativeMethods.DwmUpdateThumbnailProperties(thumbHandle, ref props);

            }
        }

        /// <summary>
        /// 鼠标进入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            thumbTools.Left = this.Left;
            thumbTools.Top = this.Top + this.Height-thumbTools.Height;
            thumbTools.Width = this.Width;

            thumbTools.Show();
        }
        /// <summary>
        /// 鼠标离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            Console.WriteLine("window-离开");
            thumbTools.Hide();
        }

        /// <summary>
        /// 鼠标左键按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            thumbTools.Hide();
            this.DragMove();

            thumbTools.Left = this.Left;
            thumbTools.Top = this.Top + this.Height - thumbTools.Height;
            thumbTools.Width = this.Width;

            thumbTools.Show();

        }

        /// <summary>
        /// 窗口大小改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var dest = new NativeStructs.Rect(0, 0, (int)this.Width, (int)this.Height);
            var tarRect = new NativeStructs.Rect((int)rect.Left, (int)rect.Top, (int)rect.Left + (int)rect.Width, (int)rect.Top + (int)rect.Height);
            var props = new NativeStructs.DwmThumbnailProperties
            {
                fVisible = true,
                dwFlags = (int)(DWM_TNP.DWM_TNP_VISIBLE | DWM_TNP.DWM_TNP_RECTDESTINATION | DWM_TNP.DWM_TNP_OPACITY | DWM_TNP.DWM_TNP_RECTSOURCE),
                opacity = 255,
                rcDestination = dest,
                rcSource = tarRect
            };

            NativeMethods.DwmUpdateThumbnailProperties(thumbHandle, ref props);
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            NativeMethods.DwmUnregisterThumbnail(thumbHandle);
        }
    }
}
