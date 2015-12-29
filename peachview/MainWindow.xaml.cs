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
        }

        private string[] _args;
        private string[] _imgList;
        private int _index = 0;

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
#if DEBUG
                //this._args=new string[]{@"F:\100.png"};
#endif
                if (this._args != null && this._args.Length > 0)
                {
                    string parent = System.IO.Path.GetDirectoryName(this._args[0]);
                    _imgList = (from f in Directory.GetFiles(parent)
                                where f.EndsWith(".jpg") || f.EndsWith(".jpeg") || f.EndsWith(".png") || f.EndsWith(".bmp")
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
            this.ImgMain.Source = image;
            System.Windows.Forms.Screen screen = Screen.PrimaryScreen;
            System.Drawing.Rectangle rct = screen.Bounds;
            this.Width = image.PixelWidth > rct.Width ? rct.Width : image.PixelWidth;
            this.Height = image.PixelHeight > rct.Height ? rct.Height : image.PixelHeight;
            this.Top = (rct.Height - this.Height) / 2;
            this.Left = (rct.Width - this.Width) / 2;
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
            this.DragMove();
        }
    }
}
