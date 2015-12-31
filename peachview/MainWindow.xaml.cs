using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;

namespace peachview
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(string[] args)
        {
            this._args = args;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            LoadConfig();
            this.Width = ScreenSize.Width;
            this.Height = ScreenSize.Height;
        }

        private Size ScreenSize
        {
            get
            {
                System.Windows.Forms.Screen screen = Screen.PrimaryScreen;
                System.Drawing.Rectangle rct = screen.Bounds;
                return new Size(rct.Width, rct.Height);
            }
        }

        private void LoadConfig()
        {
            var color = Configer.ReadByKey("bgcolor");
            if (string.IsNullOrEmpty(color))
            {
                color = "128,128,128";
            }
            string[] rgbs = color.Split(',');
            var alpha = Configer.ReadByKey("bgalpha");
            if (string.IsNullOrEmpty(alpha))
            {
                alpha = 90.ToString();
            }
            try
            {
                this.Background =
                    new SolidColorBrush(Color.FromArgb(Convert.ToByte(255 * byte.Parse(alpha) / 100.0), byte.Parse(rgbs[0]),
                        byte.Parse(rgbs[1]), byte.Parse(rgbs[2])));
            }
            catch
            {
                MessageBox.Show("配置文件出错了.", "呵呵", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string[] _args;
        private string[] _imgList;
        private int _index;
        private Size _normalSize;
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
#if DEBUG
                this._args = new string[] { @"F:\100.png" };
#endif
                if (this._args != null && this._args.Length > 0)
                {
                    string parent = System.IO.Path.GetDirectoryName(this._args[0]);
                    _imgList = (from f in Directory.GetFiles(parent)
                                where f.EndsWith(".jpg") || f.EndsWith(".jpeg") || f.EndsWith(".png") || f.EndsWith(".bmp") || f.EndsWith(".gif")
                                select f).ToArray();
                    BitmapImage image = new BitmapImage(new Uri(this._args[0], UriKind.Absolute));
                    DisplayImage(image);
                }
            }
            catch
            {
                MessageBox.Show("请重试,出错了.", "呵呵", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisplayImage(BitmapImage image)
        {
            if (image.UriSource.AbsolutePath.EndsWith(".gif"))
            {
                this.GifImage.GifSource = image.UriSource.AbsolutePath;
                this.GifImage.Visibility = Visibility.Visible;
                this.ImgMain.Visibility = Visibility.Collapsed;
                this.CanvasGif.Width = image.Width;
                this.CanvasGif.Height = image.Height;
            }
            else
            {
                this.ImgMain.Source = image;
                this.GifImage.Visibility = Visibility.Collapsed;
                this.ImgMain.Visibility = Visibility.Visible;
                this.CanvasImage.Width = image.Width;
                this.CanvasImage.Height = image.Height;
            }
            this._normalSize = new Size(image.Width, image.Height);
            this.Title = image.UriSource.AbsolutePath;
            this.ImgMain.Stretch = Stretch.None;
            this.Topmost = true;
        }

        private void MainWindow_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) this.Close();
            if (this._imgList == null || !this._imgList.Any()) return;
            if (e.Key == Key.Left)
            {
                if (this._index <= 0) return;
                BitmapImage image = new BitmapImage(new Uri(this._imgList[--_index], UriKind.Absolute));
                this.DisplayImage(image);
            }
            else if (e.Key == Key.Right)
            {
                if (this._index >= this._imgList.Length - 1) return;
                BitmapImage image = new BitmapImage(new Uri(this._imgList[++_index], UriKind.Absolute));
                this.DisplayImage(image);
            }
        }

        private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //this.DragMove();
        }

        private void ImgMain_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            double verPa = this._normalSize.Height / this._normalSize.Width;
            if (e.Delta < 0)
            {
                if (Math.Abs(e.Delta) > Convert.ToInt32(this.ImgMain.ActualHeight) ||
                    Math.Abs(e.Delta) > Convert.ToInt32(this.ImgMain.ActualWidth))
                    return;
            }
            this.ImgMain.Height = this.ImgMain.ActualHeight + e.Delta;
            this.ImgMain.Width = this.ImgMain.Height / verPa;
            this.ImgMain.Stretch = Stretch.Uniform;
        }
    }
}
