using System;
using System.Collections.Generic;
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
            InitializeComponent();
        }

        private string[] _args;

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.Screen screen = Screen.PrimaryScreen;
                System.Drawing.Rectangle rct = screen.Bounds;
#if DEBUG
                //this._args=new string[]{@"F:\100.png"};
#endif
                if (this._args != null && this._args.Length > 0)
                {
                    BitmapImage image = new BitmapImage(new Uri(this._args[0], UriKind.Absolute));
                    this.ImgMain.Source = image;
                    this.Width = image.PixelWidth > rct.Width ? rct.Width : image.PixelWidth;
                    this.Height = image.PixelHeight > rct.Height ? rct.Height : image.PixelHeight;
                    this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    this.Topmost = true;
                }
            }
            catch
            {
                MessageBox.Show("请重试,出错了.", "呵呵", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
