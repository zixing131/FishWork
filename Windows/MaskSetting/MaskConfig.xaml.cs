using FishWork.Commom;
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

namespace FishWork.Windows.MaskSetting
{
    /// <summary>
    /// MaskConfig.xaml 的交互逻辑
    /// </summary>
    public partial class MaskConfig : UserControl
    {
        /// <summary>
        /// 桌面组件
        /// </summary>
        [Injection]
        private DeskTopComponenet deskTopComponenet { set; get; }

        [Injection]
        private AppConfig appconfig { set; get; }

        public MaskConfig()
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
            slider_width.Value = appconfig.MaskConfig.Width;
            slider_height.Value = appconfig.MaskConfig.Height;
            slider_radius.Value = appconfig.MaskConfig.Radius;
            slider_opacity.Value = appconfig.MaskConfig.Opacity*100;
            slider_mohu.Value = appconfig.MaskConfig.Vague;

            slider_width.ValueChanged += Slider_width_ValueChanged;
            slider_height.ValueChanged += Slider_height_ValueChanged;
            slider_radius.ValueChanged += Slider_radius_ValueChanged;
            slider_opacity.ValueChanged += Slider_opacity_ValueChanged;
            slider_mohu.ValueChanged += Slider_mohu_ValueChanged;

           
            comboImageStyle.SelectedIndex = appconfig.ImageStyle;
            comboImageStyle.SelectionChanged += ComboImageStyle_SelectionChanged;
            UpdateBorder();
            UpdateImageStyle();
        }

        /// <summary>
        /// 图像风格切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboImageStyle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (appconfig == null)
                return;
            appconfig.ImageStyle = comboImageStyle.SelectedIndex;
            UpdateImageStyle();
        }

        /// <summary>
        /// 模糊度改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Slider_mohu_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (appconfig == null)
                return;
            appconfig.MaskConfig.Vague =e.NewValue;
            UpdateBorder();
        }

        /// <summary>
        /// 透明度改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Slider_opacity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (appconfig == null)
                return;
            appconfig.MaskConfig.Opacity = (e.NewValue/100);
            UpdateBorder();
        }
        /// <summary>
        /// 原角度改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Slider_radius_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (appconfig == null)
                return;
            appconfig.MaskConfig.Radius = (int)e.NewValue;
            UpdateBorder();
        }

        /// <summary>
        /// 高度改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Slider_height_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (appconfig == null)
                return;
            appconfig.MaskConfig.Height = (int)e.NewValue;
            UpdateBorder();
        }

        /// <summary>
        /// 宽度改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Slider_width_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (appconfig == null)
                return;
            appconfig.MaskConfig.Width = (int)e.NewValue;
            UpdateBorder();
        }

        /// <summary>
        /// 更新边框
        /// </summary>
        void UpdateBorder() {
            border.Width = appconfig.MaskConfig.Width;
            border.Height = appconfig.MaskConfig.Height;
            border.CornerRadius = new CornerRadius(appconfig.MaskConfig.Radius);
            border.Opacity = appconfig.MaskConfig.Opacity;
            blur.Radius = appconfig.MaskConfig.Vague;

            lbWidth.Text = string.Format("遮罩宽度 - {0}",appconfig.MaskConfig.Width);
            lbHeight.Text = string.Format("遮罩高度 - {0}", appconfig.MaskConfig.Height);
            lbRadius.Text = string.Format("圆角度 - {0}", appconfig.MaskConfig.Radius);
            lbOpacity.Text = string.Format("透明度 - {0}", (appconfig.MaskConfig.Opacity*100).ToString("F2"));
            lbMhu.Text = string.Format("模糊度 - {0}", ((appconfig.MaskConfig.Vague /30) * 100).ToString("F2"));

            
        }


        /// <summary>
        /// 更新图像风格
        /// </summary>
        void UpdateImageStyle() {
            BitmapImage bitmapImage = new BitmapImage(new Uri("pack://application:,,,/Images/simple.png",UriKind.RelativeOrAbsolute));

            switch (appconfig.ImageStyle) {
                case 0://正常
                    borderImage.ImageSource = bitmapImage;
                    break;
                case 1://灰度
                    borderImage.ImageSource = ImageStyleUtils.ConvertToGray(bitmapImage);
                    break;
            }

        }
    }
}
