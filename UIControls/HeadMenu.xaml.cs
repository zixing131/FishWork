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

namespace FishWork.UIControls
{
    /// <summary>
    /// HeadMenu.xaml 的交互逻辑
    /// </summary>
    public partial class HeadMenu : UserControl
    {
        /// <summary>
        /// 数据
        /// </summary>
        public string Data { set; get; }

        bool selected = false;

        /// <summary>
        /// 内容
        /// </summary>
        public string Text {
            set {
                lbText.Text = value;
            }
            get {
                return lbText.Text;
            }
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Selected {
            set {
                this.selected = value;
                UpdateStatus();
            }
            get {
                return this.selected;
            }
        }
        public HeadMenu()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 更新状态
        /// </summary>
        void UpdateStatus() {
            if (selected)
            {
                lbText.FontSize = 20;
                //lbText.FontWeight = FontWeights.Bold;
                lbText.Foreground = Brushes.Black;
            }
            else {
                lbText.FontSize = 15;
                //lbText.FontWeight = FontWeights.Normal;
                lbText.Foreground = ColorUtils.GetColor(ColorType.Color666);
            }
        }

    }
}
