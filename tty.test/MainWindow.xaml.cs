using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace tty.test
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //App.Cache = new Data.DataCache();
            //App.Window = this;
            
            //App.Cache.Load();
            //Console.WriteLine(App.Cache.user.state);

            //App.Cache.OnStateChanged += (a, b) => OnStateChanged();

            //OnStateChanged();
            
        }
        public void SetMessage(string msg)
        {
            TextBlockMessage.Text = msg;
        }

        private void OnStateChanged()
        {
            //if (IsLoaded)
            //{
            //    if (App.Cache.user.state == 0)
            //    {
            //        FrameContent.NavigateTo(typeof(Pages.StartPage));
            //    }
            //    else 
            //    {
            //        FrameContent.NavigateTo(typeof(Pages.MainPage));
            //    }
            //    TextBlockUsername.Text = App.Cache.user.username;
            //    TextBlockNickname.Text = App.Cache.user.nickname;
            //    if (App.Cache.user.state == 0)
            //    {
            //        TextBlockState.Text = "未登录";
            //    }
            //    else if (App.Cache.user.state == 1)
            //    {
            //        TextBlockState.Text = "账户异常";
            //    }
            //    else 
            //    {
            //        TextBlockState.Text = "已登录";
            //    }
            //}

        }
    }
}
