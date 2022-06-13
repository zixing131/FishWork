using FishWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace NetDict.UI.UIControls
{
    /// <summary>
    /// SVGImage.xaml 的交互逻辑
    /// </summary>
    public partial class SVGImage : UserControl
    {

        /// <summary>
        /// 背景颜色
        /// </summary>
        public readonly static DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(SVGImage),
                                                          new PropertyMetadata(new PropertyChangedCallback(fillPropertyChangedCallback)));
        /// <summary>
        /// SVG数据
        /// </summary>
        public string Data {
            set {
                var obj = PathGeometry.Parse(value);
                path.Data = obj;
            }
            get{
                return "";
            }
        }

        /// <summary>
        /// Svg文件
        /// </summary>
        public string SvgFile {
            set {
                LoadSvgFile(value);
            }
            get {
                return "";
            }
        }


        /// <summary>
        /// 填充颜色
        /// </summary>
        public Brush Fill {
            set {
                SetValue(FillProperty, value);
                path.Fill = value;
            }
            get {
                return path.Fill;
            }
        }

        /// <summary>
        /// 背景颜色设置回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void fillPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SVGImage bnt = (sender as SVGImage);
            if (bnt != null)
            {
                bnt.path.Fill = e.NewValue as Brush;
            }
        }
        public SVGImage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载SVG文件
        /// </summary>
        /// <param name="svgFile"></param>
        public void LoadSvgFile(string svgFile) {
            try
            {
                var resInfo = Application.GetResourceStream(new Uri(svgFile, UriKind.RelativeOrAbsolute));
                if (resInfo == null)
                    return;
                byte[] bits = new byte[1024 * 10];
                var len = resInfo.Stream.Read(bits, 0, bits.Length);
                string xmlContent = Encoding.UTF8.GetString(bits, 0, len);
                xmlContent = xmlContent.Replace(Resource1.Svg_Replace_Content,"");
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.LoadXml(xmlContent);
                var paths = doc.GetElementsByTagName("path");

                GeometryGroup group = new GeometryGroup();
                foreach (XmlNode item in paths){
                    var data = item.Attributes["d"].Value;

                    Geometry geometry = PathGeometry.Parse(data);
                    group.Children.Add(geometry);
                }
                path.Data = group;

            }
            catch { }
          


        }
    }
}
