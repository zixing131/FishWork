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

namespace FishWork.Windows.FastKey
{
    /// <summary>
    /// FastKeyView.xaml 的交互逻辑
    /// </summary>
    public partial class FastKeyView : UserControl
    {
        /// <summary>
        /// 系统按键
        /// </summary>
        string systemKey = "";

        /// <summary>
        /// 按键
        /// </summary>
        string key = "";

        /// <summary>
        /// 数据
        /// </summary>
        Model.FastKey Model { set; get; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Text {
            set {
                lbText.Text = value;
            }
            get
            {
                return lbText.Text;
            }
        }
        public FastKeyView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 点击边框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void borderKey_MouseUp(object sender, MouseButtonEventArgs e)
        {
            txtInput.Focus();
        }

        private void txtInput_GotFocus(object sender, RoutedEventArgs e)
        {
            borderKey.BorderBrush = ColorUtils.GetColor("#0094FF");
            
        }

        private void txtInput_LostFocus(object sender, RoutedEventArgs e)
        {
            borderKey.BorderBrush = ColorUtils.GetColor("#CECECE");
        }

        /// <summary>
        /// 按键按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInput_PreviewKeyDown(object sender, KeyEventArgs e){

            Console.WriteLine(e.Key + "---" + e.SystemKey);
            int ikey = (int)e.Key;
            int iSystemKey = (int)e.SystemKey;
            e.Handled = true;
            if (ikey >= 34 && ikey <= 83)
            {
                //单字幕范围
                var shiftStatus = Keyboard.GetKeyStates(Key.LeftShift);
                var altStutus = Keyboard.GetKeyStates(Key.LeftAlt);
                var ctrlStatus = Keyboard.GetKeyStates(Key.LeftCtrl);
                if ((shiftStatus & KeyStates.Down) > 0)
                {
                    systemKey = "Shift";
                    key = e.Key.ToString();
                    UpdateKey();
                    return;
                }
                else if ((ctrlStatus & KeyStates.Down) > 0)
                {
                    systemKey = "Ctrl";
                    key = e.Key.ToString();
                    UpdateKey();
                    return;
                }
                else if ((altStutus & KeyStates.Down) > 0)
                {
                    systemKey = "Alt";
                    key = e.Key.ToString();
                    UpdateKey();
                    return;
                }
                return;
            }
            else if (e.Key == Key.System && iSystemKey >= 34 && iSystemKey <= 83)
            {
                key = e.SystemKey.ToString();
                var shiftStatus = Keyboard.GetKeyStates(Key.LeftShift);
                var altStutus = Keyboard.GetKeyStates(Key.LeftAlt);
                var ctrlStatus = Keyboard.GetKeyStates(Key.LeftCtrl);
                if ((shiftStatus & KeyStates.Down) > 0)
                {
                    systemKey = "Shift";
                    key = key;
                    UpdateKey();
                    return;
                }
                else if ((ctrlStatus & KeyStates.Down) > 0)
                {
                    systemKey = "Ctrl";
                    key = key;
                    UpdateKey();
                    return;
                }
                else if ((altStutus & KeyStates.Down) > 0)
                {
                    systemKey = "Alt";
                    key = key;
                    UpdateKey();
                    return;
                }
                return;
            }
            else {
                if (e.Key == Key.System || e.Key == Key.LeftShift || e.Key == Key.LeftCtrl || e.Key == Key.LeftAlt)
                    return;
                systemKey = "";
                key = e.Key.ToString();
                UpdateKey();
            }


        }


        /// <summary>
        /// 更新key
        /// </summary>
        void UpdateKey() {
            if (!string.IsNullOrEmpty(systemKey))
            {
                lbKey.Text = systemKey + " + " + key;
            }
            else {
                lbKey.Text = key;
            }
        }

        private void txtInput_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            
            
        }
        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="fastKey"></param>
        public void BindModel(Model.FastKey fastKey) {
            this.Model = fastKey;
            systemKey = fastKey.SystemKey;
            key = fastKey.Key;
            checkbox.IsChecked = fastKey.IsEnbale;
            UpdateKey();
        }

        /// <summary>
        /// 更新模型
        /// </summary>
        public void UpdateModel() {
            Model.IsEnbale = checkbox.IsChecked.GetValueOrDefault();
            Model.Key = key;
            Model.SystemKey = systemKey;
        }
    }
}
