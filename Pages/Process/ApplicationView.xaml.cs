using enki.dict.Commom.windowSDK;
using FishWork.Commom;
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

namespace FishWork.Pages.Process
{
    /// <summary>
    /// ApplicationView.xaml 的交互逻辑
    /// </summary>
    public partial class ApplicationView : UserControl
    {
        public ApplicationEntity Model { set; get; }


        event EventHandler remove=null;



        public event EventHandler OnRemove {
            add {
                remove += value;
            }
            remove {
                remove -= value;
            }
        }

        event Action<object,string> evt = null;
        public event Action<object, string> OnEvent
        {
            add
            {
                evt += value;
            }
            remove
            {
                evt -= value;
            }
        }

        bool maskStatus = false;

        /// <summary>
        /// 标记状态
        /// </summary>
        public bool MaskStatus {
            set {
                this.maskStatus = value;
                
                if (value)
                {
                    ic_mark.Fill = ColorUtils.GetColor("#0094FF");
                }
                else {
                    ic_mark.Fill = ColorUtils.GetColor("#666666");
                }
                if (this.Model != null) {
                    this.Model.IsMask = value;
                }
            }
            get {
                return this.maskStatus;
            }
        }

        public ApplicationView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        public void BindModel(ApplicationEntity model) {
            this.Model = model;
            lbTitle.Text = model.Title;
            alphaSilder.Value = model.Alpha;
            this.MaskStatus = model.IsMask;
            if (model.Hwnd != IntPtr.Zero)
            {
                lbRunStatus.Text = "正在运行";
                lbRunStatus.Foreground = ColorUtils.GetColor("#1CD108");
            }
            else {
                lbRunStatus.Text = "未运行";
                lbRunStatus.Foreground = Brushes.Black;
            }
        }

        /// <summary>
        /// 透明度改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void alphaSilder_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e){
            if (Model == null)
                return;
            if (Model.Hwnd == IntPtr.Zero)
                return;
            byte alpha = (byte)((int)e.NewValue);
            WindowAPI.SetWindowLong(Model.Hwnd, -20, 524288);
            WindowAPI.SetLayeredWindowAttributes(Model.Hwnd, 0, alpha, 2);
            Model.Alpha = (int)e.NewValue;
        }

        /// <summary>
        /// 移除回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ic_remove_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (remove != null) {
                remove(this, e);
            }
        }

        /// <summary>
        /// 开启遮罩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ic_mark_MouseUp(object sender, MouseButtonEventArgs e){

            if (evt != null) {
                evt(this, "Mask");
            }
            //if (Model.Hwnd == IntPtr.Zero) {
            //    MessageBox.Show("程序未运行，请运行刷新状态");
            //    return;
            //}

            //MaskWindow.GetInstance().Show();
            //MaskWindow.GetInstance().WatchWindow(Model.Hwnd);

        }
    }
}
