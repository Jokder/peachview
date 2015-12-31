using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace peachview
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
#if DEBUG
            MainWindow myMainWindow = new MainWindow(e.Args);
            myMainWindow.Show();
#else
            if (e.Args.Length > 0)
            {
                MainWindow myMainWindow = new MainWindow(e.Args);
                myMainWindow.Show();
            }
            else
            {
                MessageBox.Show("没有要打开的图片.", "呵呵", MessageBoxButton.OK, MessageBoxImage.Error);
            }
#endif
        }

        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("请重试,出错了.", "呵呵", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
