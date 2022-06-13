
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace enki.dict.Commom.windowSDK
{
    #region 自定义结构
    public struct POINTAPI//自定义结构 坐标型
    {
        public int x;
        public int y;
        public IntPtr last;
    }
    public struct RECT  //自定义类型 矩形
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
    public struct PGUITHREADINFO  //自定义类型  GUI
    {
        public int cbSize;
        public int flags;
        public IntPtr hwndActive;
        public IntPtr hwndFocus;
        public IntPtr hwndCapture;
        public IntPtr hwndMenuOwner;
        public IntPtr hwndMoveSize;
        public IntPtr hwndCaret;
        public RECT rcCaret;
    }

    #endregion
    public class WindowAPI
    {
        public const int FEATURE_DISABLE_NAVIGATION_SOUNDS = 21;
        public const int SET_FEATURE_ON_PROCESS = 0x00000002;

        public const uint WS_EX_LAYERED = 0x80000;
        public const int WS_EX_TRANSPARENT = 0x20;
        public const int GWL_STYLE = (-16);
        public const int GWL_EXSTYLE = (-20);
        private const int LWA_ALPHA = 0;


        [DllImport("urlmon.dll")]
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        public static extern int CoInternetSetFeatureEnabled(
            int FeatureEntry,
            [MarshalAs(UnmanagedType.U4)] int dwFlags,
            bool fEnable);

        //注册热键的api 
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint control, int vk);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        /// <summary>
        /// 窗口激活
        /// </summary>
        /// <param name="idthread"></param>
        /// <param name="pgui"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetActiveWindow(IntPtr hwnd);

        /// <summary>
        /// 窗口是否显示
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int IsWindowVisible(IntPtr hwnd);

        /// <summary>
        /// 设置窗口显示状态
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="isShow"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int ShowWindowAsync(IntPtr hwnd,bool isShow);

        /// <summary>
        /// 窗口置前台
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern int SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        public const UInt32 SWP_NOSIZE = 0x0001;
        public const UInt32 SWP_NOMOVE = 0x0002;
        public const UInt32 SWP_NOACTIVATE = 0x0010;
        public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        /// <summary>
        /// 取前台窗口
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// 连接线程输入
        /// </summary>
        /// <param name="dwthreadthis"></param>
        /// <param name="dwthreadfore"></param>
        /// <param name="fAttach"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr AttachThreadInput(IntPtr dwthreadthis, IntPtr dwthreadfore, bool fAttach);
        /// <summary>
        /// 取当前线程
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentThreadId();
        /// <summary>
        /// 取窗口进程ID
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lpdwProcess"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hwnd, ref IntPtr lpdwProcess);

        [DllImport("user32.dll")]
        public static extern int MessageBeep(uint uType);

        [DllImport("user32.dll")]
        private static extern IntPtr GetGUIThreadInfo(int idthread, out PGUITHREADINFO pgui);
        /// <summary>
        /// 取窗口区域位置
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr ClientToScreen(IntPtr hwnd, out POINTAPI point);

        /// <summary>
        /// 取焦点句柄
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetFocus();

        /// <summary>
        /// 取当前插入符位置
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr GetCaretPos(out POINTAPI point);

        [DllImport("user32")]
        public static extern IntPtr GetKeyboardLayout(IntPtr intPtr);

        public const int SE_SHUTDOWN_PRIVILEGE = 0x13;
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(int hWndParent,int hWndChildAfter,string lpszClass,int lpszWindow);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);


        [DllImport("user32.dll")]
        public static extern IntPtr GetParent(IntPtr hWndChild);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hWndChild, IntPtr hgdiobj);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWndChild, ref RECT rect);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hWnd, int width, int height);

        //[DllImport("user32.dll")]
        //public static extern bool SetLayeredWindowAttributes(IntPtr hwnd,int crKey,int alpha,int dwFlags);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx,
        int cy, uint uFlags);

        [DllImport("user32.dll ", EntryPoint = "SendMessage")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll ", EntryPoint = "PostMessage")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        /// <summary>
        /// 取当前鼠标位置
        /// </summary>
        /// <param name="lpPoint"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]
        public static extern bool GetCursorPos(ref System.Drawing.Point lpPoint);

        /// <summary>
        /// 返回包含了指定点的窗口的句柄。忽略屏蔽、隐藏以及透明窗口
        /// </summary>
        /// <param name="Point">指定的鼠标坐标</param>
        /// <returns>鼠标坐标处的窗口句柄，如果没有，返回</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(System.Drawing.Point Point);

        /// <summary>
        /// 取窗口类名
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="buff"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetClassName(IntPtr hwnd, StringBuilder buff, int len);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hwnd);

        /// <summary>
        /// 模拟按键
        /// </summary>
        /// <param name="bvk"></param>
        /// <param name="cscan"></param>
        /// <param name="dwflgs"></param>
        /// <param name="dwextr"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr keybd_event(int bvk, int cscan, int dwflgs, int dwextr);

        [DllImport("user32", EntryPoint = "SetWindowLong")]
        public static extern uint SetWindowLong(
       IntPtr hwnd,
       int nIndex,
       uint dwNewLong
       );

        [DllImport("user32", EntryPoint = "GetWindowLong")]
        public static extern uint GetWindowLong(
        IntPtr hwnd,
        int nIndex
        );

        [DllImport("user32", EntryPoint = "SetLayeredWindowAttributes")]
        public static extern int SetLayeredWindowAttributes(
        IntPtr hwnd,
        int crKey,
        int bAlpha,
        int dwFlags
        );

        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);
        public const int GWL_HWNDPARENT = -8;


        /// <summary>
        /// 闪烁窗口
        /// </summary>
        /// <param name="pwfi">窗口闪烁信息结构</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool FlashWindowEx(ref FLASHWINFO pwfi);


        /// <summary>
        /// 闪烁类型
        /// </summary>
        public enum flashType : uint
        {
            FLASHW_STOP = 0, //停止闪烁
            FALSHW_CAPTION = 1, //只闪烁标题
            FLASHW_TRAY = 2, //只闪烁任务栏
            FLASHW_ALL = 3, //标题和任务栏同时闪烁
            FLASHW_PARAM1 = 4,
            FLASHW_PARAM2 = 12,
            FLASHW_TIMER = FLASHW_TRAY | FLASHW_PARAM1, //无条件闪烁任务栏直到发送停止标志或者窗口被激活，如果未激活，停止时高亮
            FLASHW_TIMERNOFG = FLASHW_TRAY | FLASHW_PARAM2 //未激活时闪烁任务栏直到发送停止标志或者窗体被激活，停止后高亮
        }
        /// <summary>
        /// 包含系统应在指定时间内闪烁窗口次数和闪烁状态的信息
        /// </summary>
        public struct FLASHWINFO
        {
            /// <summary>
            /// 结构大小
            /// </summary>
            public uint cbSize;
            /// <summary>
            /// 要闪烁或停止的窗口句柄
            /// </summary>
            public IntPtr hwnd;
            /// <summary>
            /// 闪烁的类型
            /// </summary>
            public uint dwFlags;
            /// <summary>
            /// 闪烁窗口的次数
            /// </summary>
            public uint uCount;
            /// <summary>
            /// 窗口闪烁的频度，毫秒为单位；若该值为0，则为默认图标的闪烁频度
            /// </summary>
            public uint dwTimeout;
        }
        /// <summary>
        /// 闪烁窗口
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="type">闪烁类型</param>
        /// <returns></returns>
        public static bool FlashWindowEx(IntPtr hWnd, flashType type)
        {
            FLASHWINFO fInfo = new FLASHWINFO();
            fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
            fInfo.hwnd = hWnd;//要闪烁的窗口的句柄，该窗口可以是打开的或最小化的
            fInfo.dwFlags = (uint)type;//闪烁的类型
            fInfo.uCount = int.MaxValue;//闪烁窗口的次数
            fInfo.dwTimeout = 0; //窗口闪烁的频度，毫秒为单位；若该值为0，则为默认图标的闪烁频度
            return FlashWindowEx(ref fInfo);
        }


        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int nMaxCount);


        [DllImport("user32.dll")]
        private static extern int PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        [DllImport("gdi32.dll")]
        private static extern int DeleteObject(IntPtr hWnd);
        [DllImport("gdi32.dll")]
        private static extern int DeleteDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr dw);


        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(
          IntPtr hwnd
          );

        [DllImport("user32.dll")]
        private static extern int GetLayeredWindowAttributes(IntPtr hwnd, int pcrKey, out int pbAlpha, int pdwFlags);

        /// <summary>
        /// 窗口激活
        /// </summary>
        /// <param name="hwnd"></param>
        public static void SetWindowActive(IntPtr hwnd)
        {
            IntPtr pid = IntPtr.Zero;
            IntPtr tempID = GetWindowThreadProcessId(hwnd, ref pid);
            AttachThreadInput(GetCurrentThreadId(), tempID, true);
            SetActiveWindow(hwnd);//激活窗口
            AttachThreadInput(GetCurrentThreadId(), tempID, false);
            SetForegroundWindow(hwnd);
        }

        /// <summary>
        /// 获取窗口标题
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        public static string GetWindowText(IntPtr hwnd) {
            int length = GetWindowTextLength(hwnd);
            StringBuilder windowName = new StringBuilder(length + 1);
            GetWindowText(hwnd, windowName, windowName.Capacity);
            return windowName.ToString();
        }

        public static POINTAPI GetPoint()
        {
            IntPtr FoHandle = GetFocus();//获取焦点窗口句柄
            FoHandle = GetFocus();
            IntPtr PHwnd = GetForegroundWindow();//获取前台窗口句柄
            IntPtr CurrentThead = GetCurrentThreadId();//获取当前线程ID
            IntPtr intPtr = IntPtr.Zero;
            IntPtr ThreadProcessId = GetWindowThreadProcessId(PHwnd, ref intPtr);//获取线程ID
            IntPtr ret = AttachThreadInput(CurrentThead, ThreadProcessId, true);
            POINTAPI point = new POINTAPI();
            GetCaretPos(out point);
            ClientToScreen(PHwnd, out point);
            return point;
        }

        /// <summary>
        /// 取输入光标位置 OK
        /// </summary>
        /// <returns></returns>
        public static POINTAPI GetPointEx()
        {
            PGUITHREADINFO pgui = new PGUITHREADINFO();
            pgui.cbSize = 48;
            POINTAPI point = new POINTAPI();
            GetGUIThreadInfo(0, out pgui);
            GetGUIThreadInfo(0, out pgui);
            point.x = pgui.rcCaret.left;
            point.y = pgui.rcCaret.top;
            ClientToScreen(pgui.hwndCaret, out point);
            point.last = pgui.hwndCaret;//保存上次的窗口句柄
            return point;
        }

        private static void sleep(int value)
        {
            DateTime now = DateTime.Now;
            while (now.AddMilliseconds(value) > DateTime.Now)
            {

            }
            return;

        }
        /// <summary>
        /// 发送按键
        /// </summary>
        public static void SendKey(int fu, int key) {
            keybd_event(fu, 0, 0, 0);
            keybd_event(key, 0, 0, 0);
            keybd_event(key, 0, 2, 0);
            keybd_event(fu, 0, 2, 0);
        }

        /// <summary>
        　　/// 设置窗体具有鼠标穿透效果
        　　/// </summary>
        　　/// <param name="flag">true穿透，false不穿透</param>
        public static void SetPenetrate(IntPtr hwnd, bool flag = true)
        {
            uint style = GetWindowLong(hwnd, GWL_EXSTYLE);
            if (flag)
                SetWindowLong(hwnd, GWL_EXSTYLE, style | WS_EX_TRANSPARENT | WS_EX_LAYERED);
            else
                SetWindowLong(hwnd, GWL_EXSTYLE, style & ~(WS_EX_TRANSPARENT | WS_EX_LAYERED));
            SetLayeredWindowAttributes(hwnd, 0, 100, LWA_ALPHA);
        }

        /// <summary>
        /// 发送鼠标按下消息
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        public static void SendMouseDown(IntPtr hwnd, int left, int top) {
            int posi = left + top * 65535;
            SendMessage(hwnd, 512, new IntPtr(1), new IntPtr(posi));
        }

        public struct RECT
          {
              public int Left; // x position of upper-left corner
              public int Top; // y position of upper-left corner
              public int Right; // x position of lower-right corner
              public int Bottom; // y position of lower-right corner
          }

    public static System.Drawing.Bitmap GetWindowBitmap(IntPtr hWnd) {
              IntPtr hscrdc = GetWindowDC(hWnd);
              RECT rect = new RECT();
              GetWindowRect(hWnd, ref rect);
              IntPtr hbitmap = CreateCompatibleBitmap(hscrdc, rect.Right - rect.Left, rect.Bottom - rect.Top);
              IntPtr hmemdc = CreateCompatibleDC(hscrdc);
              SelectObject(hmemdc, hbitmap);
              PrintWindow(hWnd, hmemdc, 0);
            System.Drawing.Bitmap bmp = System.Drawing.Bitmap.FromHbitmap(hbitmap);
              DeleteDC(hscrdc);
              DeleteDC(hmemdc);
              return bmp;
        }

        public static void ClipImage(IntPtr hwnd)
        {
            //System.Drawing.Rectangle rect = new System.Drawing.Rectangle();
            //IntPtr screenHwnd = IntPtr.Zero;
            //IntPtr memHwnd = IntPtr.Zero;
            //IntPtr bitmapHwnd = IntPtr.Zero;
            //IntPtr oldBitmapHwnd = IntPtr.Zero;
            //System.Drawing.Bitmap bitmap = null;

            //GetWindowRect(hwnd,ref rect);

            //screenHwnd = GetDC(IntPtr.Zero);
            //memHwnd = CreateCompatibleDC(screenHwnd);
            //bitmapHwnd = CreateCompatibleBitmap(screenHwnd, rect.Width, rect.Height);
            //oldBitmapHwnd = SelectObject(memHwnd, bitmapHwnd);

            //if (PrintWindow(hwnd, memHwnd, 0) == 0) {
            //    SelectObject(memHwnd, oldBitmapHwnd);
            //    DeleteObject(bitmapHwnd);
            //    DeleteDC(memHwnd);
            //    ReleaseDC(IntPtr.Zero, screenHwnd);
            //    return;
            //}

            

//GetObjectA(位图句柄, 5 × 4 ＋ 2 ＋ 2, 位图)
//位图信息.BITMAPINFOHEADER.biSize ＝ 4 × 11
//GetDIBits1(内存设备上下文句柄, 位图句柄, 0, 0, 0, 位图信息, 0)
//位图像素点阵 ＝ 取空白字节集(位图信息.BITMAPINFOHEADER.biSizeImage)
//位图信息.BITMAPINFOHEADER.biCompression ＝ 0
//GetDIBits(内存设备上下文句柄, 位图句柄, 0, 位图.bmHeight, 位图像素点阵, 位图信息, 0)
//' 构造位图信息
//位图信息字节集 ＝ 取空白字节集(位图信息.BITMAPINFOHEADER.biSize)
//RtlMoveMemory_BITMAPINFO(位图信息字节集, 位图信息, 位图信息.BITMAPINFOHEADER.biSize)
//' 构造位图文件头
//位图文件头.bfType ＝ 19778
//位图文件头.bfOffBits ＝ 2 × 4 ＋ 3 × 2 ＋ 位图信息.BITMAPINFOHEADER.biSize
//位图文件头.bfSize ＝ 位图文件头.bfOffBits ＋ 位图信息.BITMAPINFOHEADER.biSizeImage
//位图文件头.bfReserved1 ＝ 0
//位图文件头.bfReserved2 ＝ 0
//位图文件头字节集 ＝ 取空白字节集(14)
//RtlMoveMemory_BITMAPFILEHEADER(位图文件头字节集, 位图文件头, 14)
//SelectObject(内存设备上下文句柄, 旧位图句柄)
//DeleteObject(位图句柄)
//DeleteDC(内存设备上下文句柄)
//ReleaseDC(0, 屏幕设备上下文句柄)
//返回(位图文件头字节集 ＋ 位图信息字节集 ＋ 位图像素点阵)

        }

        /// <summary>
        /// 获取顶级窗口
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        public static IntPtr GetParentWindow(IntPtr hwnd) {
            var parent = WindowAPI.GetParent(hwnd);
            if (parent == IntPtr.Zero)
                return hwnd;
            return GetParentWindow(parent);
        }

        /// <summary>
        /// 取鼠标窗口句柄
        /// </summary>
        /// <returns></returns>
        public static IntPtr GetMouseWindowHandle()
        {
            System.Drawing.Point point = new System.Drawing.Point();
            WindowAPI.GetCursorPos(ref point);
            var hwnd = WindowAPI.WindowFromPoint(point);
            if (hwnd == IntPtr.Zero)
                return hwnd;
            hwnd = GetParentWindow(hwnd);
            return hwnd;
        }


        /// <summary>
        /// 获取窗口透明度
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        public static int GetWindowOpacity(IntPtr hwnd) {
            var dyStyle = GetWindowLong(hwnd, -20);
            var isAlphaStyle = (dyStyle & 524288)!=0;
            int alpha = 255;
            if ( isAlphaStyle) {
                GetLayeredWindowAttributes(hwnd, 0, out alpha, 0);
            }
            return alpha;
        }

        /// <summary>
        /// 设置窗口透明度
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="alpha">1-100</param>
        public static void SetWindowOpacity(IntPtr hwnd, int alpha) {

            var rate = alpha / 100.0;
            var opacity = 255 * rate;
            WindowAPI.SetWindowLong(hwnd, -20, 524288);
            WindowAPI.SetLayeredWindowAttributes(hwnd, 0, (int)opacity, 2);
        }
        public static void SetWindowOpacity1(IntPtr hwnd, int alpha)
        {

            var rate = alpha / 100.0;
            var opacity = 255 * rate;
            //WindowAPI.SetWindowLong(hwnd, -20, 524288);
            WindowAPI.SetLayeredWindowAttributes(hwnd, 0, (int)opacity, 2);
        }


        /// <summary>
        /// 鼠标事件
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="key">
        /// 键
        /// 0=左键
        /// 2=右键
        /// 
        /// </param>
        /// <param name="control">
        /// 控制
        /// 
        /// </param>
        public static void MouseEvent(IntPtr hwnd,int x,int y,int key,int control) {
            var point = x + y * 65536;
            PostMessage(hwnd, 512, new IntPtr(2), new IntPtr(point));

            if (key == 0)
            {
                if (control == 0)
                {
                    PostMessage(hwnd, 513, new IntPtr(1), new IntPtr(point));
                }
                else if (control == 1)
                {
                    PostMessage(hwnd, 514, new IntPtr(0), new IntPtr(point));
                }
            }
            else if (key == 2) {
                if (control == 0)
                {
                    PostMessage(hwnd, 516, new IntPtr(1), new IntPtr(point));
                }
                else if (control == 1)
                {
                    PostMessage(hwnd, 517, new IntPtr(0), new IntPtr(point));
                }
            }

        }
    }
}
