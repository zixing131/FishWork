using enki.dict.Commom.windowSDK;
using FishWork.Commom;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace FishWork.ComponentSupport
{
    /// <summary>
    /// 透明度支持类
    /// </summary>
    public class OpacitySupport
    {
        DeskTopComponenet componenet = null;

        /// <summary>
        /// 当前选中项
        /// </summary>
        TextBlock curSelected = null;
        public OpacitySupport(DeskTopComponenet _componenet) {
            this.componenet = _componenet;

            
            LoadOpacity();
            foreach (TextBlock item in componenet.stackOptions.Children){
                item.MouseUp += Item_MouseUp;
            }
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        public void LoadOpacity() {
            var config = componenet.GetConfig();
            UpdateText(componenet.stackOptions.Children[0] as TextBlock, config.Opacity.Level1);
            UpdateText(componenet.stackOptions.Children[1] as TextBlock, config.Opacity.Level2);
            UpdateText(componenet.stackOptions.Children[2] as TextBlock, config.Opacity.Level3);
        }

        /// <summary>
        /// 项目点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Item_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e){
            if (curSelected != null) {
                curSelected.Foreground = Brushes.White;
            }
            TextBlock block = sender as TextBlock;
            var value = int.Parse(block.Tag.ToString());
            UpdateOpacity(value);
            curSelected = block;
            curSelected.Foreground = ColorUtils.GetColor("#97FF36");
        }

        /// <summary>
        /// 更新透明度
        /// </summary>
        /// <param name="value"></param>
        public void UpdateOpacity(int value) {
            var targetHwnd = componenet.findWindowSupport.GetTargetHwnd();
            if (targetHwnd == IntPtr.Zero)
                return;
            WindowAPI.SetWindowOpacity(targetHwnd, value);
        }

        /// <summary>
        /// 更新文本
        /// </summary>
        /// <param name="textBlock"></param>
        /// <param name="value"></param>
        void UpdateText(TextBlock textBlock,int value) {
            textBlock.Text = value + "%";
            textBlock.Tag = value;
        }

        /// <summary>
        /// 应用当前透明度
        /// </summary>
        public void ApplyCurrentOpacity() {
            if (curSelected == null)
                return;
            Item_MouseUp(curSelected, null);
        }

        /// <summary>
        /// 切换透明度
        /// </summary>
        /// <param name="level"></param>
        public void ChangeOpacityLevel(int level) {
            Item_MouseUp(componenet.stackOptions.Children[level], null);
        }
    }
}
