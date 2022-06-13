using FishWork.Commom;
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

namespace FishWork.Windows.About
{
    /// <summary>
    /// AboutApp.xaml 的交互逻辑
    /// </summary>
    public partial class AboutApp : UserControl
    {
        public AboutApp()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            lbVersion.Text = "软件版本号 " + SelfInfo.GetAppVersion();
        }
    }
}
