using enki.dict.Commom.windowSDK;
using FishWork.Commom;
using FishWork.Model;
using Newtonsoft.Json;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FishWork.Pages.Process
{
    /// <summary>
    /// ProcessPage.xaml 的交互逻辑
    /// </summary>
    public partial class ProcessPage : Page
    {

        Cursor curPoint = null;

        bool isDownFind = false;

        IntPtr thisHwnd = IntPtr.Zero;
        IntPtr findHwnd = IntPtr.Zero;
        /// <summary>
        /// 目标句柄
        /// </summary>
        IntPtr targetHwnd = IntPtr.Zero;

        /// <summary>
        /// 当前程序
        /// </summary>
        ApplicationEntity curApp = null;

        /// <summary>
        /// 当前配置列表
        /// </summary>
        List<ApplicationEntity> listEntity = new List<ApplicationEntity>();

        /// <summary>
        /// 遮罩窗口
        /// </summary>
        MaskWindow maskWindow = null;

        ApplicationView curView = null;
        public ProcessPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 拖拽鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            isDownFind = true;
            borderFind.Cursor = curPoint;
            borderFind.CaptureMouse();
            borderFind.Opacity=0.1;
        }

        /// <summary>
        /// 拖拽鼠标放开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void border_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDownFind = false;
            borderFind.Opacity = 1;
            borderFind.ReleaseMouseCapture();
            borderFind.Cursor = Cursors.Arrow;
            BorderShowWindow.GetInstance().Hide();
            //var hwnd = GetMouseWindowHandle();
            //if (hwnd == IntPtr.Zero)
            //{
            //    ic_point.Fill = Brushes.Red;
            //    return;
            //}
            //mHwnd = hwnd;
            //ic_point.Fill = Brushes.CadetBlue;
            //Console.WriteLine(hwnd.ToInt32());
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void borderFind_PreviewMouseMove(object sender, MouseEventArgs e){
            if (!isDownFind)
                return;
            //System.Drawing.Point point = new System.Drawing.Point();
            //WindowAPI.GetCursorPos(ref point);
            var hwnd = WindowAPI.GetMouseWindowHandle(); //WindowAPI.WindowFromPoint(point);
            if (hwnd == IntPtr.Zero)
                return;
            if (hwnd == thisHwnd || hwnd== findHwnd)
                return;

            targetHwnd = hwnd;
            //var parent = WindowAPI.GetParent(hwnd);
            //if (parent != IntPtr.Zero){
            //    targetHwnd = parent;
            //}

            lbHwnd.Text = targetHwnd.ToInt32().ToString();

            WindowAPI.RECT rect = new WindowAPI.RECT();
            WindowAPI.GetWindowRect(hwnd, ref rect);

            var borderShow = BorderShowWindow.GetInstance();
            borderShow.Left = rect.Left;
            borderShow.Top = rect.Top;
            borderShow.Width = rect.Right - rect.Left;
            borderShow.Height = rect.Bottom - rect.Top;
            borderShow.Show();

            int alpha = WindowAPI.GetWindowOpacity(targetHwnd);
            alphaSilder.Value = alpha;

            try
            {
                Console.WriteLine(targetHwnd.ToInt32());

                string windowTitle = WindowAPI.GetWindowText(targetHwnd);
                lbHwndTitle.Text = windowTitle;
                StringBuilder buffer = new StringBuilder(255);
                WindowAPI.GetClassName(targetHwnd, buffer, 255);
                curApp = new ApplicationEntity();
                curApp.Alpha = alpha;
                curApp.ClassName = buffer.ToString();
                curApp.Title = windowTitle;
            }
            catch(Exception ec) { }
         

            //Console.WriteLine("alpha="+alpha);
            //Console.WriteLine("{0},{1},{2},{3}",rect.Left,rect.Top,rect.Right,rect.Bottom);
        }

        /// <summary>
        /// 加载完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            System.IO.MemoryStream memory = new System.IO.MemoryStream(Resource1.curPoint);
            curPoint = new Cursor(memory);

            thisHwnd  =  new WindowInteropHelper(Window.GetWindow(this)).Handle;
            findHwnd = new WindowInteropHelper(BorderShowWindow.GetInstance()).Handle;

            LoadConfig();
        }

        /// <summary>
        /// 透明度改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void alphaSilder_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (targetHwnd == IntPtr.Zero || curApp==null)
                return;
            byte alpha = (byte)((int)e.NewValue);
            WindowAPI.SetWindowLong(targetHwnd, -20, 524288);
            WindowAPI.SetLayeredWindowAttributes(targetHwnd, 0, alpha, 2);
         
            curApp.Alpha = (int)e.NewValue;
        }

        /// <summary>
        /// 加入配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnJoin_OnClick(object sender, EventArgs e)
        {
            if (curApp == null)
                return;
            
            listEntity.Add(curApp);
            if (listEntity.Count == 1) {
                curApp.IsMask = true;
            }
            SaveConfig();
            LoadApplication();
            UpdateApplication();
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        void SaveConfig() {
            string config = JsonConvert.SerializeObject(listEntity);
            System.IO.File.WriteAllText("config.json", config, Encoding.UTF8);
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        void LoadConfig() {
            if (!System.IO.File.Exists("config.json"))
                return;
            string config = System.IO.File.ReadAllText("config.json", Encoding.UTF8);
            listEntity = JsonConvert.DeserializeObject<List<ApplicationEntity>>(config);
            LoadApplication();
            UpdateApplication();
        }

        /// <summary>
        /// 更新应用状态
        /// </summary>
        void LoadApplication() {
            stackApps.Children.Clear();
            foreach (var item in listEntity){
                ApplicationView view = new ApplicationView();
                view.Margin = new Thickness(0, 10, 0, 0);
                view.BindModel(item);
                view.OnEvent += View_OnEvent;
                view.OnRemove += View_OnRemove;
                stackApps.Children.Add(view);
                if (item.IsMask) {
                    curView = view;
                }
            }
        }

        /// <summary>
        /// 应用事件
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="key"></param>
        private void View_OnEvent(object arg1, string key){
            switch (key) {
                case "Mask":
                    UpdateMask(arg1 as ApplicationView);
                    break;
            }
        }

        void UpdateMask(ApplicationView view) {
            if (curView != null) {
                curView.MaskStatus = false;
            }
            curView = view;
            curView.MaskStatus = true;
            SaveConfig();
        }

        /// <summary>
        /// 移除配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void View_OnRemove(object sender, EventArgs e){

            ApplicationView app = sender as ApplicationView;
            listEntity.Remove(app.Model);
            stackApps.Children.Remove(app);
            SaveConfig();
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        void UpdateApplication() {
            foreach (ApplicationView view in stackApps.Children){
                IntPtr hwnd = WindowAPI.FindWindowEx(0,0,view.Model.ClassName, 0);
                view.Model.Hwnd = hwnd;
                view.Model.Alpha = WindowAPI.GetWindowOpacity(view.Model.Hwnd);

                view.BindModel(view.Model);
            }
        }

        /// <summary>
        /// 刷新应用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ic_refresh_MouseUp(object sender, MouseButtonEventArgs e)
        {
            UpdateApplication();
        }

        /// <summary>
        /// 开启遮罩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ic_mask_MouseUp(object sender, MouseButtonEventArgs e){
            SwitchMaskView();
        }

        /// <summary>
        /// 开启关闭遮罩
        /// </summary>
        public void SwitchMaskView() {

            var maskWindow = MaskWindow.GetInstance();
            if (maskWindow.Visibility == Visibility.Visible)
            {
                maskWindow.Hide();
                ic_mask.Fill = ColorUtils.GetColor("#666666");
            }
            else {
                if (curView == null)
                {
                    MessageBox.Show("未指定遮罩应用，请先设置遮罩应用!");
                    return;
                }
                if (curView.Model.Hwnd == IntPtr.Zero)
                {
                    MessageBox.Show("遮罩应用未运行，请运行刷新状态");
                    return;
                }
                maskWindow.Show();
                maskWindow.WatchWindow(curView.Model.Hwnd);
                ic_mask.Fill = ColorUtils.GetColor("#0094FF");
            }
           
        }

        /// <summary>
        /// 快捷键1
        /// </summary>
        public void FastKey1() {
            if (curView == null)
            {
                MessageBox.Show("未指定遮罩应用，请先设置遮罩应用!");
                return;
            }
            if (curView.Model.Hwnd == IntPtr.Zero)
            {
                MessageBox.Show("遮罩应用未运行，请运行刷新状态");
                return;
            }
            curView.alphaSilder.Value = 255;
        }
        /// <summary>
        /// 快捷键2
        /// </summary>
        public void FastKey2()
        {
            if (curView == null)
            {
                MessageBox.Show("未指定遮罩应用，请先设置遮罩应用!");
                return;
            }
            if (curView.Model.Hwnd == IntPtr.Zero)
            {
                MessageBox.Show("遮罩应用未运行，请运行刷新状态");
                return;
            }
            curView.alphaSilder.Value = 127;
        }
        /// <summary>
        /// 快捷键2
        /// </summary>
        public void FastKey3()
        {
            if (curView == null)
            {
                MessageBox.Show("未指定遮罩应用，请先设置遮罩应用!");
                return;
            }
            if (curView.Model.Hwnd == IntPtr.Zero)
            {
                MessageBox.Show("遮罩应用未运行，请运行刷新状态");
                return;
            }
            curView.alphaSilder.Value = 25;
        }
    }
}
