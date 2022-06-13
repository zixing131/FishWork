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
using System.Windows.Shapes;

namespace FishWork
{
    /// <summary>
    /// BorderShowWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BorderShowWindow : Window
    {
        static BorderShowWindow _this = null;
        private BorderShowWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns></returns>
        public static BorderShowWindow GetInstance() {
            if (_this == null) {
                _this = new BorderShowWindow();
            }
            return _this;
        }

    }
}
