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
using System.Windows.Shapes;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;
using TextBox = System.Windows.Controls.TextBox;

namespace peachview
{
    /// <summary>
    /// ConfigWIndow.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigWIndow : Window
    {
        public ConfigWIndow()
        {
            InitializeComponent();
            LoadConfig();
            System.Windows.Forms.Screen screen = Screen.PrimaryScreen;
            System.Drawing.Rectangle rct = screen.Bounds;
            this.Width = rct.Width;
            this.Height = rct.Height;
        }

        private void ConfigWIndow_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) this.Close();
        }

        private bool _isLoadConfig = false;
        private void LoadConfig()
        {
            _isLoadConfig = true;
            var color = Configer.ReadByKey("bgcolor");
            if (string.IsNullOrEmpty(color))
            {
                color = "128,128,128";
            }
            string[] rgbs = color.Split(',');
            var alpha = Configer.ReadByKey("bgalpha");
            if (string.IsNullOrEmpty(alpha))
            {
                alpha = 60.ToString();
            }
            string allowDrag = Configer.ReadByKey("allowdrag");
            try
            {
                this.CheckBoxMultiScreen.IsChecked = !string.IsNullOrEmpty(allowDrag) && Convert.ToBoolean(allowDrag);
                this.TextBoxPa.Text = alpha;
                this.LabelColor.Background =
                    new SolidColorBrush(Color.FromRgb(byte.Parse(rgbs[0]), byte.Parse(rgbs[1]), byte.Parse(rgbs[2])));
                this.Background =
                    new SolidColorBrush(Color.FromArgb(Convert.ToByte(255 * byte.Parse(alpha) / 100.0), byte.Parse(rgbs[0]),
                        byte.Parse(rgbs[1]), byte.Parse(rgbs[2])));
            }
            catch
            {
                MessageBox.Show("配置文件出错了.", "呵呵", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _isLoadConfig = false;
            }
        }

        private void LabelColor_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
            var result = colorDialog.ShowDialog(null);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                var color = colorDialog.Color;
                var alpha = Convert.ToByte(color.A * int.Parse(this.TextBoxPa.Text) / 100.0);
                var backBrush = new SolidColorBrush(Color.FromArgb(alpha, color.R, color.G, color.B));
                this.LabelColor.Background = backBrush;
                this.Background = backBrush;
                Configer.SaveConf("bgcolor", color.R + "," + color.G + "," + color.B);
            }
        }

        private void TextBoxPa_OnKeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;
            //屏蔽非法按键
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Decimal)
            {
                if (txt.Text.Contains(".") && e.Key == Key.Decimal)
                {
                    e.Handled = true;
                    return;
                }
                e.Handled = false;
            }
            else if (((e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.OemPeriod) && e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
            {
                if (txt.Text.Contains(".") && e.Key == Key.OemPeriod)
                {
                    e.Handled = true;
                    return;
                }
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private string _lastInputText = "0";
        private void TextBoxPa_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //屏蔽中文输入和非法字符粘贴输入
            TextBox textBox = sender as TextBox;
            byte num;
            if (byte.TryParse(textBox.Text, out num))
            {
                if (num > 100) num = 100;
                _lastInputText = num.ToString();
            }
            else
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = "0";
                    num = 0;
                }
                else
                {
                    textBox.Text = _lastInputText;
                    num = byte.Parse(_lastInputText);
                }
            }
            if (_isLoadConfig) return;
            var color = Configer.ReadByKey("bgcolor");
            if (string.IsNullOrEmpty(color))
            {
                color = "128,128,128";
            }
            string[] rgps = color.Split(',');
            try
            {
                this.Background =
                    new SolidColorBrush(Color.FromArgb(Convert.ToByte(255 * num / 100.0), byte.Parse(rgps[0]), byte.Parse(rgps[1]), byte.Parse(rgps[2])));
                Configer.SaveConf("bgalpha", num.ToString());
            }
            catch
            {
                MessageBox.Show("配置文件出错了.", "呵呵", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CheckBoxMultiScreen_OnChecked(object sender, RoutedEventArgs e)
        {
            Configer.SaveConf("allowdrag", this.CheckBoxMultiScreen.IsChecked.ToString());
        }
    }
}
