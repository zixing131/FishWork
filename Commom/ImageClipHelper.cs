using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace FishWork.Commom
{
    public class ImageClipHelper
    {
         struct RECT
         {
             public int Left; // x position of upper-left corner
             public int Top; // y position of upper-left corner
             public int Right; // x position of lower-right corner
             public int Bottom; // y position of lower-right corner
         }
         public static Bitmap GetWindow(IntPtr hWnd)
         {
             IntPtr hscrdc = GetWindowDC(hWnd);
             RECT rect = new RECT();
             GetWindowRect(hWnd, out rect);
             IntPtr hbitmap = CreateCompatibleBitmap(hscrdc, rect.Right - rect.Left, rect.Bottom - rect.Top);
             IntPtr hmemdc = CreateCompatibleDC(hscrdc);
             SelectObject(hmemdc, hbitmap);
             PrintWindow(hWnd, hmemdc, 0);
             Bitmap bmp = Bitmap.FromHbitmap(hbitmap);
             DeleteDC(hscrdc); 
             DeleteDC(hmemdc);
             DeleteObject(hbitmap);
             return bmp;
         }


        //API声明

        [DllImport("user32.dll")]
         private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

          [DllImport("gdi32.dll")]
         public static extern IntPtr CreateDC(
                     string lpszDriver,        // driver name驱动名  
                     string lpszDevice,        // device name设备名  
                     string lpszOutput,        // not used; should be NULL  
                     IntPtr lpInitData  // optional printer data  
          );

          [DllImport("gdi32.dll")]
         public static extern int BitBlt(
          IntPtr hdcDest, // handle to destination DC目标设备的句柄  
          int nXDest,  // x-coord of destination upper-left corner目标对象的左上角的X坐标  
          int nYDest,  // y-coord of destination upper-left corner目标对象的左上角的Y坐标  
          int nWidth,  // width of destination rectangle目标对象的矩形宽度  
          int nHeight, // height of destination rectangle目标对象的矩形长度  
          IntPtr hdcSrc,  // handle to source DC源设备的句柄  
          int nXSrc,   // x-coordinate of source upper-left corner源对象的左上角的X坐标  
          int nYSrc,   // y-coordinate of source upper-left corner源对象的左上角的Y坐标  
          UInt32 dwRop  // raster operation code光栅的操作值  
          );

          [DllImport("gdi32.dll")]
         public static extern IntPtr CreateCompatibleDC(
          IntPtr hdc // handle to DC  
          );

          [DllImport("gdi32.dll")]
         public static extern IntPtr CreateCompatibleBitmap(
          IntPtr hdc,        // handle to DC  
          int nWidth,     // width of bitmap, in pixels  
          int nHeight     // height of bitmap, in pixels  
          );

          [DllImport("gdi32.dll")]
         public static extern IntPtr SelectObject(
          IntPtr hdc,          // handle to DC  
          IntPtr hgdiobj   // handle to object  
          );

          [DllImport("gdi32.dll")]
         public static extern int DeleteDC(
          IntPtr hdc          // handle to DC  
          );

        [DllImport("gdi32.dll")]
        public static extern int DeleteObject(
       IntPtr hdc          // handle to DC  
       );

        [DllImport("user32.dll")]
         public static extern bool PrintWindow(
          IntPtr hwnd,               // Window to copy,Handle to the window that will be copied.   
          IntPtr hdcBlt,             // HDC to print into,Handle to the device context.   
          UInt32 nFlags              // Optional flags,Specifies the drawing options. It can be one of the following values.   
          );

          [DllImport("user32.dll")]
         public static extern IntPtr GetWindowDC(
          IntPtr hwnd
          );


        /// <summary>
        /// 裁切图片
        /// </summary>
        /// <param name="src"></param>
        /// <param name="cropRect"></param>
        /// <returns></returns>
        public static Bitmap ClipBitmap(Bitmap src,Rectangle cropRect)
        {
            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),
                      cropRect,
                      GraphicsUnit.Pixel);
            }
            return target;
        }


    }
}
