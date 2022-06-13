using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FishWork.Windows.Commom
{
    /// <summary>
    /// ToastWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ToastWindow : Window
    {
        /// <summary>
        /// 单例对象
        /// </summary>
        static ToastWindow _this = null;
        private ToastWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns></returns>
        static ToastWindow GetInstance() {
            if (_this == null) {
                _this = new ToastWindow();
            }
            return _this;
        }

        /// <summary>
        /// 显示提示
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static void ShowTip(string msg) {
            var window = GetInstance();
            window.ShowNormalTip(msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tip"></param>
        void ShowNormalTip(string tip) {
            lbText.Text = tip;
            this.Left = (SystemParameters.PrimaryScreenWidth / 2) - this.Width / 2;
            this.Top = SystemParameters.PrimaryScreenHeight - 100;
            this.border.Opacity = 0;
            this.Show();
            PlayAnimation(0, 1, delegate () {
                Task.Factory.StartNew(delegate () {
                    Thread.Sleep(2000);
                    this.Dispatcher.BeginInvoke(new Action(delegate () {
                        PlayAnimation(1, 0, delegate () {
                            this.Hide();
                        });
                    }));
                    
                });
            });
        }

        void PlayAnimation(double from,double to,Action finish) {
            DoubleAnimation animation = new DoubleAnimation(from, to, TimeSpan.FromSeconds(0.3));
            animation.Completed += delegate (object sender, EventArgs e)
            {
                if (finish != null) {
                    finish();
                }
            };
           
            border.BeginAnimation(Border.OpacityProperty, animation);

        }

     
    }
}
