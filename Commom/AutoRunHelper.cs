using IWshRuntimeLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace enki.dict.Commom
{
    /// <summary>
    /// 自动运行
    /// </summary>
    public class AutoRunHelper
    {
        /// <summary>
        /// 判断本程序是否自动运行
        /// </summary>
        /// <returns></returns>
        public static bool IsAutoRun()
        {
            string startup = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            return System.IO.File.Exists(startup + "\\WorkTools.lnk");

        }


        /// <summary>
        /// 设置或取消程序开机运行
        /// </summary>
        /// <param name="isAutoRun"></param>
        /// <returns></returns>
        public static bool UpdateRunStatus(bool isAutoRun)
        {
            if (isAutoRun)
            {
                try
                {
                     string startup = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                     string shortcutPath = startup + "\\WorkTools.lnk";
                     WshShell shell = new WshShell();
                     IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);//创建快捷方式对象
                     shortcut.TargetPath = Process.GetCurrentProcess().MainModule.FileName;//指定目标路径
                     shortcut.WorkingDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);//设置起始位置
                     shortcut.WindowStyle = 1;//设置运行方式，默认为常规窗口
                     shortcut.Description = "WorkTools快捷方式";//设置备注
                     shortcut.IconLocation = Process.GetCurrentProcess().MainModule.FileName;//设置图标路径
                     shortcut.Save();//保存快捷方式
                     return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                string startup = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                string shortcutPath = startup + "\\WorkTools.lnk";
                if (System.IO.File.Exists(shortcutPath))
                {
                    System.IO.File.Delete(shortcutPath);
                }
                return true;
            }
        }
    }
}
