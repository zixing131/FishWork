using enki.dict.Commom.windowSDK;
using FishWork.Pages.Process;
using FishWork.UIControls;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FishWork
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window,IHotKey
    {

        Cursor curPoint = null;

        /// <summary>
        /// 窗口句柄
        /// </summary>
        IntPtr mHwnd = IntPtr.Zero;

        /// <summary>
        /// 当前菜单
        /// </summary>
        HeadMenu curHeadMenu = null;

        HotKey hotKey = null;
        public MainWindow()
        {
            InitializeComponent();
        }



        /// <summary>
        /// 窗口加载完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e){

            InitUI();
            //System.IO.MemoryStream memory = new System.IO.MemoryStream(Resource1.curPoint);
            ////memory.Write(Resource1.curPoint, 0, Resource1.curPoint.Length);
            //curPoint = new Cursor(memory);

            //Thread thread = new Thread(new ThreadStart(ThreadScreen));
            //thread.IsBackground = true;
            //thread.Start();
            
        }

        /// <summary>
        /// 初始化UI
        /// </summary>
        void InitUI() {
            foreach (HeadMenu item in stackTab.Children){
                item.MouseUp += Item_MouseUp;
                if (curHeadMenu == null) {
                    Item_MouseUp(item,null);
                }
            }

            //注册热键
            IntPtr handle = new WindowInteropHelper(this).Handle;
             hotKey = new HotKey(handle, this);
            System.Windows.Interop.HwndSource source = System.Windows.Interop.HwndSource.FromHwnd(handle);
            source.AddHook(hotKey.HotKeyHook);

            hotKey.RegisterKey((int)Key.V, 'V',4);

            hotKey.RegisterKey((int)Key.NumPad1, '1', 4);
            hotKey.RegisterKey((int)Key.NumPad2, '2', 4);
            hotKey.RegisterKey((int)Key.NumPad3, '3', 4);



        }

        /// <summary>
        /// 菜单点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Item_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (curHeadMenu != null) {
                curHeadMenu.Selected = false;
            }
            curHeadMenu = sender as HeadMenu;
            curHeadMenu.Selected = true;

            if (curHeadMenu.Data == "APP")
            {
                frameApp.Visibility = Visibility.Visible;
                frameSetting.Visibility = Visibility.Collapsed;
            }
            else {
                frameApp.Visibility = Visibility.Collapsed;
                frameSetting.Visibility = Visibility.Visible;
            }
           
        }

        void ThreadScreen() {
            //while (true) {
            //    if (mHwnd == IntPtr.Zero){
            //        Thread.Sleep(1000);
            //        continue;
            //    }
            //    var bitmap = ClipWindow();
            //    this.Dispatcher.BeginInvoke(new Action(delegate () {
            //        MemoryStream ms = new MemoryStream();
            //        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            //        byte[] bytes = ms.GetBuffer();  //byte[]   bytes=   ms.ToArray(); 这两句都可以
            //        ms.Close();
            //        //Convert it to BitmapImage
            //        BitmapImage image = new BitmapImage();
            //        image.BeginInit();
            //        image.StreamSource = new MemoryStream(bytes);
            //        image.EndInit();
            //        img.Source = image;
            //    }));
            //    Thread.Sleep(50);
            //}
        }

        System.Drawing.Bitmap ClipWindow() {
            System.Drawing.Bitmap bitmap = WindowAPI.GetWindowBitmap(mHwnd);
            return bitmap;
        }




        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {

            base.OnMouseDown(e);

            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
            {
                var elem = e.OriginalSource as FrameworkElement;
                if (elem != null && elem.Tag != null && elem.Tag.ToString() == "Move")
                {
                   
                    e.Handled = true;
                    this.DragMove();
          
                }
            }


        }

        /// <summary>
        /// 拖拽_对象按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ic_point_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //e.Handled = true;
            //ic_point.Cursor = curPoint;
            //ic_point.CaptureMouse();
        }

        /// <summary>
        /// 拖拽_放开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ic_point_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            //ic_point.ReleaseMouseCapture();
            //ic_point.Cursor = Cursors.Arrow;

            //var hwnd = GetMouseWindowHandle();
            //if (hwnd == IntPtr.Zero) {
            //    ic_point.Fill = Brushes.Red;
            //    return;
            //}
            //mHwnd = hwnd;
            //ic_point.Fill = Brushes.CadetBlue;
            //Console.WriteLine(hwnd.ToInt32());
        }




        /// <summary>
        /// 取鼠标窗口句柄
        /// </summary>
        /// <returns></returns>
        IntPtr GetMouseWindowHandle() {
            System.Drawing.Point point = new System.Drawing.Point();
            WindowAPI.GetCursorPos(ref point);
            var hwnd = WindowAPI.WindowFromPoint(point);
            if (hwnd == IntPtr.Zero)
                return hwnd;
            var parent = WindowAPI.GetParent(hwnd);
            if (parent != IntPtr.Zero) {
                return parent;
            }
            return hwnd;
        }

        /// <summary>
        /// 窗口整体透明度改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sliderFullWindow_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e){

            if (mHwnd == IntPtr.Zero)
                return;
            byte alpha = (byte)((int)e.NewValue);
            WindowAPI.SetWindowLong(mHwnd, -20, 524288);
            WindowAPI.SetLayeredWindowAttributes(mHwnd, 0, alpha, 2);
        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMiniState_MouseUp(object sender, MouseButtonEventArgs e){
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_MouseUp(object sender, MouseButtonEventArgs e){
            Application.Current.Shutdown();
        }

        /// <summary>
        /// 热键响应
        /// </summary>
        /// <param name="id"></param>
        public void OnHotKey(int id)
        {
            switch (id) {
                case (int)Key.V:
                    (frameApp.Content as ProcessPage).SwitchMaskView();
                    break;
                case (int)Key.NumPad1:
                    (frameApp.Content as ProcessPage).FastKey1();
                    break;
                case (int)Key.NumPad2:
                    (frameApp.Content as ProcessPage).FastKey2();
                    break;
                case (int)Key.NumPad3:
                    (frameApp.Content as ProcessPage).FastKey3();
                    break;
            }
        }
    }
}
