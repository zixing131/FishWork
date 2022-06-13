using enki.dict.Commom;
using enki.dict.Commom.http;
using enki.dict.Commom.windowSDK;
using FishWork.Commom;
using FishWork.Commom.IOC;
using FishWork.ComponentSupport;
using FishWork.Model;
using FishWork.Windows.About;
using FishWork.Windows.Basic;
using FishWork.Windows.Commom;
using FishWork.Windows.FastKey;
using FishWork.Windows.MaskSetting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// DeskTopComponenet.xaml 的交互逻辑
    /// </summary>
    public partial class DeskTopComponenet : Window
    {

        /// <summary>
        /// 定位窗口类
        /// </summary>
        public FindWindowSupport findWindowSupport = null;

        /// <summary>
        /// 透明度支持类
        /// </summary>
        public OpacitySupport opacitySupport = null;

        /// <summary>
        /// 热键支持类
        /// </summary>
        public HotKeySupport hotKeySupport = null;
        /// <summary>
        /// 配置文件
        /// </summary>
        AppConfig appConfig = null;

        public DeskTopComponenet()
        {
            InitializeComponent();
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
        /// 加载完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (System.IO.File.Exists("app.json"))
            {
                appConfig = JsonConvert.DeserializeObject<AppConfig>(System.IO.File.ReadAllText("app.json", Encoding.UTF8));
            }
            else {
                appConfig = new AppConfig();
            }

            findWindowSupport = new FindWindowSupport(this);
            opacitySupport = new OpacitySupport(this);
            hotKeySupport = new HotKeySupport(this);

            //注册当前对象到容器
            AutoIoc.GetInstance().Register(this);
            //注册配置对象
            AutoIoc.GetInstance().Register(appConfig);
             
        }
         
        /// <summary>
        /// 开启关闭遮罩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ic_mask_enable_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SwitchMaskView();
        }

        /// <summary>
        /// 开启关闭遮罩
        /// </summary>
        public void SwitchMaskView()
        {

            var maskWindow = MaskWindow.GetInstance();
            if (maskWindow.Visibility == Visibility.Visible)
            {
                maskWindow.Hide();
                maskWindow.IsShowMask = false;
                ic_mask_enable.Fill = ColorUtils.GetColor("#FFFFFF");
            }
            else
            {
                if (findWindowSupport.GetTargetHwnd() == IntPtr.Zero)
                {
                    ToastWindow.ShowTip("请先定位目标窗口!");
                    return;
                }
                maskWindow.IsShowMask = true;
                maskWindow.Show();
                maskWindow.LoadConfig();
                maskWindow.WatchWindow(findWindowSupport.GetTargetHwnd());
                ic_mask_enable.Fill = ColorUtils.GetColor("#7BFF00");
            }

        }

        /// <summary>
        /// 显示遮罩调节工具
        /// </summary>
        public void ShowMaskTools() {
            var maskWindow = MaskWindow.GetInstance();
            if (maskWindow.Visibility != Visibility.Visible){
                ToastWindow.ShowTip("未开启遮罩!");
                return;
            }
            maskWindow.ShowMaskTools();
        }

        /// <summary>
        /// 菜单点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ic_more_menu_MouseUp(object sender, MouseButtonEventArgs e){
            contextMenu.PlacementTarget = ic_more_menu;
            contextMenu.IsOpen = true;
        }


        /// <summary>
        /// 获取配置文件
        /// </summary>
        /// <returns></returns>
        public AppConfig GetConfig() {
            return appConfig;
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public void SaveConfig() {
            string content = JsonConvert.SerializeObject(appConfig);
            System.IO.File.WriteAllText("app.json",content, Encoding.UTF8);
        }

        /// <summary>
        /// 菜单点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            switch (menuItem.Name) {
                case "menu_set_alpha"://更改预置透明度
                    WindowFrame.ShowDialog<SettingOpacity>("设置预置透明度", 300, 320);
                    break;
                case "menu_fastkey"://快捷键设置
                    WindowFrame.ShowDialog<FastKeySetting>("快捷键设置", 460, 510);
                    break;
                case "menu_mask"://遮罩设置
                    WindowFrame.ShowDialog<MaskConfig>("遮罩设置",1100,750);
                    this.SaveConfig();
                    var maskWinodw = MaskWindow.GetInstance();
                    if (maskWinodw.Visibility == Visibility.Visible) {
                        maskWinodw.LoadConfig();
                    }
                    break;
                case "menu_exit"://退出软件
                    Application.Current.Shutdown();
                    break;
                case "menu_bootrun"://开启自动运行
                    if (AutoRunHelper.IsAutoRun())
                    {
                        AutoRunHelper.UpdateRunStatus(false);
                    }
                    else {
                        AutoRunHelper.UpdateRunStatus(true);
                    }
                    break;
                case "menu_about"://关于软件
                    WindowFrame.ShowDialog<AboutApp>("关于软件", 360, 360);
                    break;
            }
        }

        /// <summary>
        /// 菜单即将打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenu_Opened(object sender, RoutedEventArgs e)
        {
            ic_bootrun.Visibility = AutoRunHelper.IsAutoRun() ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// 后台小窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ic_back_view_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (findWindowSupport.GetTargetHwnd() == IntPtr.Zero) {
                ToastWindow.ShowTip("未定位目标窗口!");
                return;
            }
            ClipRectWindow.GetInstance().ShowWindow(findWindowSupport.GetTargetHwnd(), delegate (Rect? rect) {
                if (rect == null)
                    return;
                ThumbWindow thumbWindow = new ThumbWindow(findWindowSupport.GetTargetHwnd(), rect.Value);
                thumbWindow.Show();
            });

        }


        /// <summary>
        /// 切换目标窗口状态
        /// </summary>
        public void SwitchTargetVisible() {
            var hwnd = findWindowSupport.GetTargetHwnd();
            if (hwnd == IntPtr.Zero)
                return;
            var vi = WindowAPI.IsWindowVisible(hwnd);
            if (vi == 1)
            {
                WindowAPI.ShowWindowAsync(hwnd, false);
            }
            else {
                WindowAPI.ShowWindowAsync(hwnd, true);
            }
        }


        /// <summary>
        /// 切换目标窗口状态
        /// </summary>
        public void SwitchWindowAndMaskVisible() {
            var hwnd = findWindowSupport.GetTargetHwnd();
            if (hwnd == IntPtr.Zero)
                return;
            var maskWindow = MaskWindow.GetInstance();
            var vi = WindowAPI.IsWindowVisible(hwnd);
            if (vi == 1)
            {
                //隐藏窗口
                WindowAPI.ShowWindowAsync(hwnd, false);

                maskWindow.Hide();
                ic_mask_enable.Fill = ColorUtils.GetColor("#FFFFFF");

            }
            else
            {
                WindowAPI.ShowWindowAsync(hwnd, true);


                maskWindow.Show();
                maskWindow.LoadConfig();
                maskWindow.WatchWindow(findWindowSupport.GetTargetHwnd());
                ic_mask_enable.Fill = ColorUtils.GetColor("#7BFF00");
            }
        }
        
    }
}
