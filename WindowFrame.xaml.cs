using FishWork.Commom.IOC;
using FishWork.Interfaces;
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
    /// WindowFrame.xaml 的交互逻辑
    /// </summary>
    public partial class WindowFrame : Window
    {

        UIElement content = null;


        private WindowFrame()
        {
            InitializeComponent();
        }

        public WindowFrame(string title,UIElement _uIElement,int width,int height) {
            InitializeComponent();

            lbTitle.Text = title;
            this.content = _uIElement;
            this.Width = width;
            this.Height = height;
           
        }

        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <param name="title"></param>
        public static bool? ShowDialog<T>(string title,int width=400,int height=480) where T:class {
            var obj = typeof(T).GetConstructor(new Type[] { }).Invoke(null);
            AutoIoc.GetInstance().InjectionAttribute(obj);
            WindowFrame windowFrame = new WindowFrame(title, obj as UIElement, width, height);
            return windowFrame.ShowDialog();
        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMiniState_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_MouseUp(object sender, MouseButtonEventArgs e){
            this.DialogResult = false;
        }

        /// <summary>
        /// 加载完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e){
            borderContent.Child = content;
            if (content is IOKCancel)
            {
                rowSave.Height = new GridLength(60);
                stackOKCancel.Visibility = Visibility.Visible;
            }
            else {
                rowSave.Height = new GridLength(0);
                stackOKCancel.Visibility = Visibility.Collapsed;
            }
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
        /// 确认按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_OnClick(object sender, EventArgs e){
            (content as IOKCancel).OnConfrim();
            this.DialogResult = true;
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_OnClick(object sender, EventArgs e){
            this.DialogResult = false;
        }
    }
}
