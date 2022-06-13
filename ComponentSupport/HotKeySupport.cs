using enki.dict.Commom.windowSDK;
using FishWork.Windows.Commom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace FishWork.ComponentSupport
{
    /// <summary>
    /// 热键支持类
    /// </summary>
    public class HotKeySupport: IHotKey
    {
        DeskTopComponenet deskTopComponenet = null;

        HotKey hotKey = null;
        public HotKeySupport(DeskTopComponenet _deskTopComponenet) {
            this.deskTopComponenet = _deskTopComponenet;

            RegisterKey();
        }

        

        /// <summary>
        /// 注册热键
        /// </summary>
        void RegisterKey() {
            var config = deskTopComponenet.GetConfig();
            IntPtr handle = new WindowInteropHelper(deskTopComponenet).Handle;
            hotKey = new HotKey(handle, this);
            System.Windows.Interop.HwndSource source = System.Windows.Interop.HwndSource.FromHwnd(handle);
            source.AddHook(hotKey.HotKeyHook);

            RegisterOneKey(config.ApplyCurrentWindow);
            RegisterOneKey(config.ApplyLevel1);
            RegisterOneKey(config.ApplyLevel2);
            RegisterOneKey(config.ApplyLevel3);
            RegisterOneKey(config.SwitchMask);
            RegisterOneKey(config.SwitchThumbVisible);
            RegisterOneKey(config.SwitchTargetVisible);
            RegisterOneKey(config.SwitchWindowWithMask);
            RegisterOneKey(config.ShowMaskTools);
        }

        /// <summary>
        /// 注册热键
        /// </summary>
        /// <param name="fastKey"></param>
        void RegisterOneKey(Model.FastKey fastKey) {
            if (!fastKey.IsEnbale)
                return;

            if (!string.IsNullOrEmpty(fastKey.SystemKey))
            {
                if (fastKey.SystemKey == "Alt")
                {
                    hotKey.RegisterKey(fastKey.GetHashCode(), ConvertToIKey(fastKey.Key));
                }
                else if (fastKey.SystemKey == "Shift")
                {
                    hotKey.RegisterKey(fastKey.GetHashCode(), ConvertToIKey(fastKey.Key), 4);
                }
                else if (fastKey.SystemKey == "Ctrl")
                {
                    hotKey.RegisterKey(fastKey.GetHashCode(), ConvertToIKey(fastKey.Key), 2);
                }
            }
            else {
                hotKey.RegisterKey(fastKey.GetHashCode(), ConvertToIKey(fastKey.Key), 0);
            }
        }

        /// <summary>
        /// 转换按键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        int ConvertToIKey(string key) {
            if (key.Length == 1)
            {
                int ikey = key[0];
                return ikey;
            }
            else if (key.Length == 2 && key.StartsWith("F"))
            {
                int inum = int.Parse(key.Replace("F", ""));
                return 111 + inum;
            }
            else if (key.Length == 2 && key.StartsWith("D"))
            {
                int inum = int.Parse(key.Replace("D", ""));
                return 48 + inum;
            }
            else if (key.StartsWith("NumPad")) {
                int inum = int.Parse(key.Replace("NumPad", ""));
                return 96 + inum;
            }

            return 0;

        }


        /// <summary>
        /// 热键响应
        /// </summary>
        /// <param name="id"></param>
        public void OnHotKey(int id)
        {
            var config = deskTopComponenet.GetConfig();
            if (id == config.ApplyCurrentWindow.GetHashCode())
            {
                //寻找当前窗口
                string title = deskTopComponenet.findWindowSupport.FindCurrentActivityWindow();
                if (string.IsNullOrEmpty(title))
                    return;
                deskTopComponenet.opacitySupport.ApplyCurrentOpacity();
                ToastWindow.ShowTip(title);
            }
            else if (id == config.ApplyLevel1.GetHashCode())
            {
                //切换透明度1
                deskTopComponenet.opacitySupport.ChangeOpacityLevel(0);
            }
            else if (id == config.ApplyLevel2.GetHashCode())
            {
                //切换透明度1
                deskTopComponenet.opacitySupport.ChangeOpacityLevel(1);
            }
            else if (id == config.ApplyLevel3.GetHashCode())
            {
                //切换透明度1
                deskTopComponenet.opacitySupport.ChangeOpacityLevel(2);
            }
            else if (id == config.SwitchMask.GetHashCode())
            {
                //开启关闭遮罩
                deskTopComponenet.SwitchMaskView();
            }
            else if (id == config.SwitchThumbVisible.GetHashCode())
            {
                //切换小窗显示状态
                foreach (var item in Application.Current.Windows)
                {
                    if (item.GetType().Equals(typeof(ThumbWindow)))
                    {
                        ThumbWindow thumbWindow = item as ThumbWindow;
                        if (thumbWindow.Visibility == Visibility.Visible)
                        {
                            thumbWindow.Hide();
                        }
                        else
                        {
                            thumbWindow.Show();
                        }
                    }
                }
            }
            else if (id == config.SwitchTargetVisible.GetHashCode())
            {
                //快速隐藏显示目标窗口
                deskTopComponenet.SwitchTargetVisible();
            }
            else if (id == config.SwitchWindowWithMask.GetHashCode())
            {
                //显示隐藏遮罩以及窗口
                deskTopComponenet.SwitchWindowAndMaskVisible();
            }
            else if (id == config.ShowMaskTools.GetHashCode()) {
                //显示遮罩调节工具
                deskTopComponenet.ShowMaskTools();
            }

        }
    }
}
