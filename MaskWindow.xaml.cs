using enki.dict.Commom.windowSDK;
using FishWork.Commom;
using FishWork.Commom.IOC;
using FishWork.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PiP_Tool.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// MaskWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MaskWindow : Window
    {
        static MaskWindow _this = null;

        /// <summary>
        /// 目标窗口
        /// </summary>
        IntPtr hwnd = IntPtr.Zero;

        /// <summary>
        /// 遮罩窗口句柄
        /// </summary>
        IntPtr maskHwnd = IntPtr.Zero;

        /// <summary>
        /// 后台线程
        /// </summary>
        Thread thread = null;

        int clipLeft = 0;
        int clipTop = 0;
        int clipWidth = 0;
        int clipHeight = 0;

        [Injection]
        private AppConfig appConfig { set; get; }

        /// <summary>
        /// 桌面组件
        /// </summary>
        [Injection]
        private DeskTopComponenet deskTopComponenet { set; get; }
        /// <summary>
        /// 缩略图句柄
        /// </summary>
        IntPtr thumbHandle = IntPtr.Zero;

        /// <summary>
        /// 目标区域
        /// </summary>
        WindowAPI.RECT targetRect = new WindowAPI.RECT();

        /// <summary>
        /// 上次更新区域时间
        /// </summary>
        DateTime lastUpdateRect = DateTime.MinValue;

        /// <summary>
        /// 工具组件
        /// </summary>
        MaskTools maskTools = null;


        Thread threadWatchWindow = null;


        /// <summary>
        /// 是否已显示遮罩
        /// </summary>
        public bool IsShowMask { set; get; }

        int curAlpha = -1;

        /// <summary>
        /// 最大透明度
        /// </summary>
        int maxAlpha = 100;
        private MaskWindow()
        {
            InitializeComponent();
            maskTools = new MaskTools();
            maskTools.ThumbWindow = this;

            threadWatchWindow = new Thread(new ThreadStart(WatchWindowThread));
            threadWatchWindow.IsBackground = true;
            threadWatchWindow.Start();
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns></returns>
        public static MaskWindow GetInstance() {
            if (_this == null) {
                _this = new MaskWindow();
                _this.Width = SystemParameters.PrimaryScreenWidth;
                _this.Height = SystemParameters.PrimaryScreenHeight;
                _this.Left = 0;
                _this.Top = 0;
             
                AutoIoc.GetInstance().InjectionAttribute(_this);


            }
            return _this;
        }

        /// <summary>
        /// 监控窗口线程
        /// </summary>
        void WatchWindowThread() {
            while (true) {
                if (!IsShowMask || hwnd==IntPtr.Zero) {
                    Thread.Sleep(500);
                    continue;
                }
                var foreHwnd = WindowAPI.GetForegroundWindow();
                if (foreHwnd != hwnd)
                {
                    //已切换到其他窗口
                    if (curAlpha != 10) {
                        curAlpha = 10;
                        WindowAPI.SetWindowOpacity1(maskHwnd, curAlpha);
                    }
                }
                else {
                    //当前窗口
                    if (curAlpha != maxAlpha) {
                        curAlpha = maxAlpha;
                        WindowAPI.SetWindowOpacity1(maskHwnd, curAlpha);
                    }
                }
                //Console.WriteLine("当前监控窗口="+hwnd+"===当前前台窗口="+foreHwnd);
                Thread.Sleep(300);
                
            }
        }

        

        /// <summary>
        /// 监听窗口
        /// </summary>
        /// <param name="_hwnd"></param>
        public void WatchWindow(IntPtr _hwnd) {
            this.hwnd = _hwnd;

            UpdateClipRect();
            //if (thread == null) {
            //    thread = new Thread(new ThreadStart(ThreadProcess));
            //    thread.IsBackground = true;
            //    thread.Start();
            //}

            IntPtr handle = new WindowInteropHelper(this).Handle;
            maskHwnd = handle;
            if (NativeMethods.DwmRegisterThumbnail(handle, hwnd, out thumbHandle) == 0)
            {
                var dest = new NativeStructs.Rect(0, 0, (int)this.Width, (int)this.Height);
                var tarRect = new NativeStructs.Rect(clipLeft, clipTop, clipLeft + clipWidth, clipTop + clipHeight);
                var props = new NativeStructs.DwmThumbnailProperties
                {
                    fVisible = true,
                    dwFlags = (int)(DWM_TNP.DWM_TNP_VISIBLE | DWM_TNP.DWM_TNP_RECTDESTINATION | DWM_TNP.DWM_TNP_OPACITY | DWM_TNP.DWM_TNP_RECTSOURCE),
                    opacity = 255,
                    rcDestination = dest,
                    rcSource = tarRect
                };

                NativeMethods.DwmUpdateThumbnailProperties(thumbHandle, ref props);


                System.Drawing.Point point = new System.Drawing.Point();
                WindowAPI.GetCursorPos(ref point);
                var bLeft = point.X - (this.Width / 2);
                var bTop = point.Y - (this.Height / 2);
                this.Left = bLeft;
                this.Top = bTop;

            }

            
        }


        /// <summary>
        /// 更新裁切区域
        /// </summary>
        void UpdateClipRect() {
            clipLeft = (int)this.Left; //(int)(double)border.GetValue(Canvas.LeftProperty);
            clipTop = (int)this.Top;//(int)(double)border.GetValue(Canvas.TopProperty);
            clipWidth = (int)this.Width;//(int)border.Width;
            clipHeight = (int)this.Height;//(int)border.Height;
        }
        /// <summary>
        /// 后台线程处理
        /// </summary>
        void ThreadProcess() {
            while (true) {
                if (hwnd == IntPtr.Zero) {
                    Thread.Sleep(1000);
                    continue;
                }
                var bimap = ImageClipHelper.GetWindow(hwnd);
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(clipLeft,clipTop,clipWidth,clipHeight);

                bimap = ImageClipHelper.ClipBitmap(bimap, rect);
                //bimap.Save("1.png");
                this.Dispatcher.BeginInvoke(new Action(delegate () {
                    UpdateImage(bimap);
                }));
                Thread.Sleep(30);
            }
        }
        /// <summary>
        /// 更新图片
        /// </summary>
        /// <param name="bitmap"></param>
        void UpdateImage(System.Drawing.Bitmap bitmap) {
            WindowAPI.RECT rect = new WindowAPI.RECT();
            WindowAPI.GetWindowRect(hwnd,ref rect);
            UpdateClipRect();
            this.Width = rect.Right - rect.Left;
            this.Height = rect.Bottom - rect.Top;
            if (rect.Left > 0) {
                this.Left = rect.Left;
            }
            if (rect.Top > 0) {
                this.Top = rect.Top;
            }
            var image = BitmapToBitmapImage(bitmap);
            if (appConfig.ImageStyle == 0) {
                
                ImageBrush back = new ImageBrush();
                back.Stretch = Stretch.UniformToFill;
                back.ImageSource = image;
                border.Background = back;
            }
            else  if (appConfig.ImageStyle == 1) {
                //图像灰度处理
                var source = ImageStyleUtils.ConvertToGray(image);
                ImageBrush back = new ImageBrush();
                back.Stretch = Stretch.UniformToFill;
                back.ImageSource = source;
                border.Background = back;
            }
           

            UpdateMousePosition();
        }

        /// <summary>
        /// 更新鼠标位置
        /// </summary>
        void UpdateMousePosition() {
            System.Drawing.Point point = new System.Drawing.Point();
            WindowAPI.GetCursorPos(ref point);

            clipLeft = point.X - (int)this.Left-(int)(border.Width/2);
            clipTop = point.Y - (int)this.Top- (int)(border.Height / 2);

            var bLeft = (point.X - (int)this.Left) - (border.Width / 2);
            var bTop = (point.Y - (int)this.Top) - (border.Height / 2);
            border.SetValue(Canvas.LeftProperty,(double)bLeft);
            border.SetValue(Canvas.TopProperty, (double)bTop);
        }


        private BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            BitmapImage bitmapImage = new BitmapImage();
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }
            return bitmapImage;
        }

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(this);
            WindowAPI.MouseEvent(hwnd, (int)point.X, (int)point.Y, 0, 0);
        }

        /// <summary>
        /// 鼠标放开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(this);
            WindowAPI.MouseEvent(hwnd, (int)point.X, (int)point.Y, 0, 1);
        }

        /// <summary>
        /// 窗口加载完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadConfig();
            //设置穿透
            IntPtr thishwnd = new WindowInteropHelper(this).Handle;
            uint extendedStyle = WindowAPI.GetWindowLong(thishwnd, WindowAPI.GWL_EXSTYLE);
            WindowAPI.SetWindowLong(thishwnd, WindowAPI.GWL_EXSTYLE, extendedStyle | WindowAPI.WS_EX_TRANSPARENT);

            var appEvent = Gma.System.MouseKeyHook.Hook.GlobalEvents();
            appEvent.MouseMoveExt += AppEvent_MouseMoveExt;
            //appEvent.MouseWheelExt += AppEvent_MouseWheel;

            
        }

        /// <summary>
        /// 鼠标位置移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppEvent_MouseMoveExt(object sender, Gma.System.MouseKeyHook.MouseEventExtArgs e)
        {
            if (maskTools.Visibility == Visibility.Visible)
                return;

            var second = DateTime.Now.Subtract(lastUpdateRect).TotalSeconds;
            if (second > 3) {
                WindowAPI.RECT rect = new WindowAPI.RECT();
                WindowAPI.GetWindowRect(this.hwnd, ref targetRect);
            }
           

            clipLeft = e.X -  (int)(this.Width / 2) - targetRect.Left;
            clipTop = e.Y -  (int)(this.Height / 2) - targetRect.Top;

            var bLeft = e.X - (this.Width/2);
            var bTop = e.Y - (this.Height/2);
            this.Left = bLeft;
            this.Top = bTop;

            UpdateMask();

        }

        /// <summary>
        /// 显示遮罩调节工具
        /// </summary>
        public void ShowMaskTools() {
            maskTools.Left = this.Left;
            maskTools.Top = this.Top + this.Height - maskTools.Height;
            maskTools.Width = this.Width;
            maskTools.Show();

        }

        void UpdateMask() {
            var dest = new NativeStructs.Rect(0, 0, (int)this.Width, (int)this.Height);
            var tarRect = new NativeStructs.Rect(clipLeft, clipTop, clipLeft + clipWidth, clipTop + clipHeight);
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

        public IntPtr HotKeyHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        //热键处理过程
        {
            Console.WriteLine(msg + "," + wParam + "," + lParam);
            return IntPtr.Zero;
        }

        private void AppEvent_MouseWheel(object sender, Gma.System.MouseKeyHook.MouseEventExtArgs e)
        {
            if (this.Visibility != Visibility.Visible)
                return;
            var shiftStatus = Keyboard.GetKeyStates(Key.LeftShift);
            var ctrlStatus = Keyboard.GetKeyStates(Key.LeftCtrl);
            var altStatus = Keyboard.GetKeyStates(Key.LeftAlt);
            var winStatus = Keyboard.GetKeyStates(Key.F1);
            #region 宽度调整
            if ((shiftStatus & KeyStates.Down) > 0)
            {
                if (e.Delta > 0)
                {
                    if (this.border.Width - 50 > 50)
                    {
                        this.border.Width -= 50;
                    }

                        ;
                }
                else
                {
                    this.border.Width += 50;
                }
                e.Handled = true;
            }
            #endregion
            #region 高度调整
            #region 圆角调整
            if ((ctrlStatus & KeyStates.Down) > 0)
            {
                if (e.Delta > 0)
                {
                    if (this.border.Height - 30 > 30)
                    {
                        this.border.Height -= 30;
                    }

                        ;
                }
                else
                {
                    this.border.Height += 30;
                }
                e.Handled = true;
            }
            #endregion
            if ((altStatus & KeyStates.Down) > 0)
            {
                if (e.Delta > 0)
                {
                    if (this.border.CornerRadius.TopLeft - 10 >= 0) {
                        this.border.CornerRadius = new CornerRadius(this.border.CornerRadius.TopLeft - 10);
                    }
                }
                else
                {
                    this.border.CornerRadius = new CornerRadius(this.border.CornerRadius.TopLeft + 10);

                }
                e.Handled = true;
            }
            #endregion

            if ((winStatus & KeyStates.Down) > 0)
            {
                if (e.Delta > 0)
                {
                    if (this.Opacity > 0.3) {
                        this.Opacity -= 0.1;
                    }
                }
                else
                {
                    if (this.Opacity < 1) {
                        this.Opacity += 0.1;
                    }

                }
                e.Handled = true;
            }

            SaveConfig();
        }

        /// <summary>
        /// 保存遮罩配置
        /// </summary>
        void SaveConfig() {

        }

        /// <summary>
        /// 加载配置
        /// </summary>
        public void LoadConfig() {

            this.Width = appConfig.MaskConfig.Width;
            this.Height = appConfig.MaskConfig.Height;
            //border.CornerRadius = new CornerRadius(appConfig.MaskConfig.Radius);
            //this.Opacity = appConfig.MaskConfig.Opacity;
            //blur.Radius = appConfig.MaskConfig.Vague;

            double alpha = appConfig.MaskConfig.Opacity * 100;
            maxAlpha = (int)alpha;
            IntPtr handle = new WindowInteropHelper(this).Handle;
            WindowAPI.SetWindowOpacity1(handle, maxAlpha);
            curAlpha = maxAlpha;
        }

        /// <summary>
        /// 窗口大小改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //maskTools.Width = this.Width;
            //maskTools.Top = this.Top + this.Height - maskTools.Height;


            //clipLeft = (int)this.Left - (int)(this.Width / 2) - targetRect.Left;
            //clipTop = (int)this.Top - (int)(this.Height / 2) - targetRect.Top;
            clipWidth = (int)this.Width;
            clipHeight = (int)this.Height;


            UpdateMask();

        }

        /// <summary>
        /// 完成调节
        /// </summary>
        public void FinishResize() {
            appConfig.MaskConfig.Width = (int)this.Width;
            appConfig.MaskConfig.Height = (int)this.Height;
            maskTools.Hide();
            deskTopComponenet.SaveConfig();
        }
    }
}
