using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FishWork.Model
{
    /// <summary>
    /// 配置
    /// </summary>
    public class ApplicationEntity{

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { set; get; }

        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { set; get; }
    
        /// <summary>
        /// 透明度
        /// </summary>
        public int Alpha { set; get; }

        /// <summary>
        /// 窗口句柄
        /// </summary>
        public IntPtr Hwnd { set; get; }

        /// <summary>
        /// 是否启用遮罩
        /// </summary>
        public bool IsMask { set; get; }
    }
}
