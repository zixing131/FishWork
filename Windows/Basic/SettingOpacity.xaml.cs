using FishWork.Commom.IOC;
using FishWork.Interfaces;
using FishWork.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FishWork.Windows.Basic
{
    /// <summary>
    /// SettingOpacity.xaml 的交互逻辑
    /// </summary>
    public partial class SettingOpacity : UserControl,IOKCancel
    {

        [Injection]
        private AppConfig appConfig { set; get; }

        /// <summary>
        /// 浮窗对象
        /// </summary>
        /// 
        [Injection]
        private DeskTopComponenet deskTopComponenet { set; get; }
        public SettingOpacity()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e){
            lb1.Text = string.Format("透明度1 - {0}%",appConfig.Opacity.Level1);
            slider1.Value = appConfig.Opacity.Level1;

            lb2.Text = string.Format("透明度2 - {0}%", appConfig.Opacity.Level2);
            slider2.Value = appConfig.Opacity.Level2;

            lb3.Text = string.Format("透明度3 - {0}%", appConfig.Opacity.Level3);
            slider3.Value = appConfig.Opacity.Level3;
        }


        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (appConfig == null)
                return;
            appConfig.Opacity.Level1 = (int)e.NewValue;
            lb1.Text = string.Format("透明度1 - {0}%", appConfig.Opacity.Level1);
            slider1.Value = appConfig.Opacity.Level1;
        }

        private void slider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (appConfig == null)
                return;
            appConfig.Opacity.Level2 = (int)e.NewValue;
            lb2.Text = string.Format("透明度2 - {0}%", appConfig.Opacity.Level2);
            slider2.Value = appConfig.Opacity.Level2;
        }

        private void slider3_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (appConfig == null)
                return;
            appConfig.Opacity.Level3 = (int)e.NewValue;
            lb3.Text = string.Format("透明度3 - {0}%", appConfig.Opacity.Level3);
            slider3.Value = appConfig.Opacity.Level3;
        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        public void OnConfrim()
        {
            deskTopComponenet.opacitySupport.LoadOpacity();
            deskTopComponenet.SaveConfig();
        }
    }
}
