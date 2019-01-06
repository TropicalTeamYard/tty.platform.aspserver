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
using tty.interactive.Data;

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
            var user = App.Current.InterAct.UserData;
            user.PropertyChanged += (a, b) =>
            {
                if (b.PropertyName == "userstate")
                {
                    OnStateChanged();
                }
            };
            App.Current.InterAct.MessageInvoked += (a, b) =>
            {
                SetMessage(b.Action + "" + b.Message);
            };

            tbkUsername.SetBinding(TextBlock.TextProperty, new Binding() { Source = user, Path = new PropertyPath("username") });
            tbkNickname.SetBinding(TextBlock.TextProperty, new Binding() { Source = user, Path = new PropertyPath("nickname") });
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            App.Current.Window = this;

            OnStateChanged();
        }
        public void SetMessage(string msg)
        {
            tbkMsg.Text += msg + "\n";
        }

        private void OnStateChanged()
        {
            if (IsLoaded)
            {
                var user = App.Current.InterAct.UserData;
                if (user.userstate == UserState.NoLogin)
                {
                    contentFrame.NavigateTo(typeof(Pages.StartPage));
                }
                else
                {
                    contentFrame.NavigateTo(typeof(Pages.MainPage));
                }
                if (user.userstate == UserState.NoLogin)
                {
                    tbkState.Text = "未登录";
                }
                else if (user.userstate == UserState.Waring)
                {
                    tbkState.Text = "账户异常";
                }
                else
                {
                    tbkState.Text = "已登录";
                }
            }

        }
    }
}
