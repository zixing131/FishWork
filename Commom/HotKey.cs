using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace enki.dict.Commom.windowSDK
{
    public class HotKey
    {
        IHotKey callBack = null;
        IntPtr hwnd;


        /// <summary>
        /// 触发热键时，是否激活自身窗口
        /// </summary>
        public bool IsAcitveSelfWindow { set; get; }
        public HotKey(IntPtr _hwnd,IHotKey _callBack)
        {
            hwnd = _hwnd;
            callBack = _callBack;
        }
        /// <summary>
        /// 注册热键
        /// </summary>
        /// <param name="id"></param>
        /// <param name="control"></param>
        /// <param name="key"></param>
        public void RegisterKey(int id,int key)
        {
            bool resu = WindowAPI.RegisterHotKey(hwnd, id, 1, key);
        }
        /// <summary>
        /// 注册热键
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        public void RegisterKey(int id, int key,uint fn)
        {
            bool resu = WindowAPI.RegisterHotKey(hwnd, id, fn, key);
        }

        public void UnregisterKey(int id)
        {
            WindowAPI.UnregisterHotKey(hwnd, id);
        }

        public IntPtr HotKeyHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        //热键处理过程
        {
            
            const int WM_HOTKEY = 0x0312;//如果m.Msg的值为0x0312那么表示用户按下了热键
            if (msg == WM_HOTKEY)
            {
                if (IsAcitveSelfWindow){
                    WindowAPI.SetActiveWindow(hwnd);
                    WindowAPI.SetForegroundWindow(hwnd);
                }
                callBack.OnHotKey(wParam.ToInt32());
                
            }
            return IntPtr.Zero;
        }

    }

    public interface IHotKey{
        void OnHotKey(int id);
    }
}
