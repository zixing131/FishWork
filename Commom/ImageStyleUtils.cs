using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FishWork.Commom
{
    public class ImageStyleUtils
    {
        /// <summary>
        /// 转换到灰度图像
        /// </summary>
        /// <param name="bitmapImage"></param>
        /// <returns></returns>
        public static ImageSource ConvertToGray(BitmapImage bitmapImage) {
            FormatConvertedBitmap newFormatedBitmapSource = new FormatConvertedBitmap();
            newFormatedBitmapSource.BeginInit();
            newFormatedBitmapSource.Source = bitmapImage;
            newFormatedBitmapSource.DestinationFormat = PixelFormats.Gray8;
            newFormatedBitmapSource.EndInit();
            return newFormatedBitmapSource;
        }


 

    }
}
