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

namespace FishWork.Windows.FastKey
{
    /// <summary>
    /// FastKeySetting.xaml 的交互逻辑
    /// </summary>
    public partial class FastKeySetting : UserControl,IOKCancel
    {

        /// <summary>
        /// 浮窗
        /// </summary>
        [Injection]
        private DeskTopComponenet deskTopComponenet { set; get; }

        /// <summary>
        /// 配置文件
        /// </summary>
        [Injection]
        private AppConfig appConfig { set; get; }
        public FastKeySetting()
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
            apply_current_window.BindModel(appConfig.ApplyCurrentWindow);
            apply_level1.BindModel(appConfig.ApplyLevel1);
            apply_level2.BindModel(appConfig.ApplyLevel2);
            apply_level3.BindModel(appConfig.ApplyLevel3);
            switch_mask.BindModel(appConfig.SwitchMask);
            switch_targetvisible.BindModel(appConfig.SwitchTargetVisible);
            switch_thumb.BindModel(appConfig.SwitchThumbVisible);
            switch_targetvisiblemask.BindModel(appConfig.SwitchWindowWithMask);
            switch_showmaskresize.BindModel(appConfig.ShowMaskTools);
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void OnConfrim()
        {
            apply_current_window.UpdateModel();
            apply_level1.UpdateModel();
            apply_level2.UpdateModel();
            apply_level3.UpdateModel();
            switch_mask.UpdateModel();
            switch_thumb.UpdateModel();
            switch_targetvisible.UpdateModel();
            switch_targetvisiblemask.UpdateModel();
            switch_showmaskresize.UpdateModel();
            deskTopComponenet.SaveConfig();
        }
    }
}
