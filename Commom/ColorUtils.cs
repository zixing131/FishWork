using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace FishWork.Commom
{
    public class ColorUtils
    {

        /// <summary>
        /// 缓存颜色
        /// </summary>
        static Dictionary<string, SolidColorBrush> colorCache = new Dictionary<string, SolidColorBrush>();
        /// <summary>
        /// 解析16进制颜色
        /// </summary>
        /// <param name="hexColor"></param>
        /// <returns></returns>
        public static Brush ParseHexColor(string hexColor)
        {
            SolidColorBrush solidColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor.Replace("\"", "")));
            return solidColor;
        }

        public static Color ParseHexColorEx(string hexColor)
        {
            return (Color)ColorConverter.ConvertFromString(hexColor.Replace("\"", ""));
        }

        /// <summary>
        /// 颜色转换成16进制
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string ColorToHexString(Color color)
        {

            string a = color.A.ToString("X").PadLeft(2, '0');
            string r = color.R.ToString("X").PadLeft(2, '0');
            string g = color.G.ToString("X").PadLeft(2, '0');
            string b = color.B.ToString("X").PadLeft(2, '0');
            string hexColor = "#" + a + r + g + b;
            return hexColor;
        }

        /// <summary>
        /// 获取一个颜色
        /// </summary>
        /// <param name="hexColor"></param>
        /// <returns></returns>
        public static SolidColorBrush GetColor(string hexColor)
        {
            if (colorCache.ContainsKey(hexColor))
            {
                return colorCache[hexColor];
            }
            SolidColorBrush solidColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor.Replace("\"", "")));
            colorCache.Add(hexColor, solidColor);
            return solidColor;
        }

        /// <summary>
        /// 获取颜色
        /// </summary>
        /// <param name="colorType"></param>
        /// <returns></returns>
        public static SolidColorBrush GetColor(ColorType colorType)
        {
            switch (colorType)
            {
                case ColorType.Color666:
                    return GetColor("#666666");
                default:
                    return Brushes.Black;
            }

        }
    }

    /// <summary>
    /// 顔色類型
    /// </summary>
    public enum ColorType
    {
        /// <summary>
        /// 正确绿色
        /// </summary>
        Color666 = 1
    }
}
