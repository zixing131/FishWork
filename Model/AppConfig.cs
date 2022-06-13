using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FishWork.Model
{
    public class AppConfig
    {
        /// <summary>
        /// 透明度设置
        /// </summary>
        public OpacitySet Opacity { set; get; }

        /// <summary>
        /// 应用当前窗口
        /// </summary>
        public FastKey ApplyCurrentWindow { set; get; }

        /// <summary>
        /// 应用透明度1
        /// </summary>
        public FastKey ApplyLevel1 { set; get; }

        public FastKey ApplyLevel2 { set; get; }

        public FastKey ApplyLevel3 { set; get; }


        /// <summary>
        /// 切换遮罩开关
        /// </summary>
        public FastKey SwitchMask { set; get; }


        /// <summary>
        /// 切换小窗显示状态
        /// </summary>
        public FastKey SwitchThumbVisible { set; get; }

        
        /// <summary>
        /// 目标窗口显示或隐藏
        /// </summary>
        public FastKey SwitchTargetVisible { set; get; }

        /// <summary>
        /// 显示窗口以及遮罩
        /// </summary>
        public FastKey SwitchWindowWithMask { set; get; }

        /// <summary>
        /// 显示遮罩调节工具
        /// </summary>
        public FastKey ShowMaskTools { set; get; }

        /// <summary>
        /// 遮罩设置
        /// </summary>
        public MaskSetting MaskConfig { set; get; }


        /// <summary>
        /// 图像风格
        /// 0 = 正常
        /// 1 = 黑白
        /// 2 = 二值化
        /// </summary>
        public int ImageStyle { set; get; }
        public AppConfig() {
            Opacity = new OpacitySet();
            ApplyCurrentWindow = new FastKey() { IsEnbale=true, SystemKey="",Key="F1" };
            ApplyLevel1 = new FastKey() { IsEnbale = true, SystemKey = "Alt", Key = "D1" };
            ApplyLevel2 = new FastKey() { IsEnbale = true, SystemKey = "Alt", Key = "D2" };
            ApplyLevel3 = new FastKey() { IsEnbale = true, SystemKey = "Alt", Key = "D3" };

            SwitchMask = new FastKey() { IsEnbale = true, SystemKey = "Alt", Key = "W" };
            SwitchThumbVisible = new FastKey() { IsEnbale=true,SystemKey="Alt",Key="R" };

            MaskConfig = new MaskSetting() { Width=300,Height=150, Opacity=1, Radius=0, Vague = 0 };
            SwitchTargetVisible = new FastKey() { IsEnbale=true,SystemKey="Alt",Key="G" };
            SwitchWindowWithMask = new FastKey() {IsEnbale =true,SystemKey="Alt",Key="Q" };
            ShowMaskTools = new FastKey() { IsEnbale=true,SystemKey="Alt",Key="T"};
        }
    }


    /// <summary>
    /// 透明度设置
    /// </summary>
    public class OpacitySet {
        public int Level1 { set; get; }
        public int Level2 { set; get; }
        public int Level3 { set; get; }

        /// <summary>
        /// 构造函数默认值
        /// </summary>
        public OpacitySet() {
            this.Level1 = 100;
            this.Level2 = 50;
            this.Level3 = 10;
        }
    }

    /// <summary>
    /// 快捷键设置
    /// </summary>
    public class FastKey { 
        
        /// <summary>
        /// 系统键
        /// </summary>
        public string SystemKey { set; get; }

        /// <summary>
        /// 按键
        /// </summary>
        public string Key { set; get; }
        
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnbale { set; get; }
    }


    /// <summary>
    /// 遮罩设置
    /// </summary>
    public class MaskSetting { 
        
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { set; get; }

        public int Height { set; get; }

        public int Radius { set; get; }

        /// <summary>
        /// 透明度
        /// </summary>
        public double Opacity { set; get; }
    
        /// <summary>
        /// 模糊度
        /// </summary>
        public double Vague { set; get; }
    }
}
