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
using tty.interactive.Util;

namespace tty.test.Pages
{
    /// <summary>
    /// StartPage.xaml 的交互逻辑
    /// </summary>
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            btn2.IsEnabled = false;

            if ((string)btn2.Content == "登录")
            {
                App.Current.InterAct.Login(tbx1.Text, PasswordBox1.Password);
            }
            else
            {
                if (App.Current.InterAct.Register(tbxuser.Text, tbxnick.Text, pwb2.Password,out string msg)) 
                {
                    tbxuser.Text = "";
                    tbxnick.Text = "";
                    pwb2.Password = "";
                    pwb3.Password = "";
                }

                tbkMsg.Text = msg;
            }
            btn2.IsEnabled = true;
        }

        private void CheckLoginInput()
        {
            var message = "";
            bool isvalid = false;
            if (tbx1.Text != "")
            {
                isvalid = CheckUtil.Username(tbx1.Text, out string msg);
                message = msg;
            }
            else
            {
                btn2.IsEnabled = false;
                message = "";
            }
            if (PasswordBox1.Password == "")
            {
                isvalid = false;
            }
            else if (!CheckUtil.Password(PasswordBox1.Password))
            {
                message += "\n密码太长或太短";
                isvalid = false;
            }

            btn2.IsEnabled = isvalid;
            tbkMsg.Text = message;
        }
        private void CheckRegisterInput()
        {
            var message = "";
            var isvalid = true;

            if (tbxuser.Text == "" || tbxnick.Text == "" || pwb2.Password == "" || pwb3.Password == "")
            {
                tbkMsg.Text = "";
                btn2.IsEnabled = false;
            }
            else
            {
                if (long.TryParse(tbxuser.Text, out long result))
                {
                    isvalid = false;
                    message += "注册的用户名不能为纯数字";
                }
                else if (!CheckUtil.Username(tbxuser.Text, out string msg))
                {
                    isvalid = false;
                    message += msg + "\n";
                }

                if (!CheckUtil.Nickname(tbxnick.Text))
                {
                    isvalid = false;
                    message += "昵称太长或太短\n";
                }

                if (pwb2.Password != pwb3.Password)
                {
                    isvalid = false;
                    message += "两次密码不相等";
                }
                else
                {
                    if (!CheckUtil.Password(pwb2.Password))
                    {
                        isvalid = false;
                        message += "密码太长或太短";
                    }
                }


                tbkMsg.Text = message;
                btn2.IsEnabled = isvalid;
            }

        }
        
        /// <summary>
        /// 账号文本框文本发生更改。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckLoginInput();
        }

        private void PasswordBox1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckLoginInput();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((string)btn2.Content == "登录")
            {
                btn2.Content = "注册";
                btn1.Content = "to 登录";
                gridRegister.Visibility = Visibility.Visible;
                gridLogin.Visibility = Visibility.Collapsed;
                CheckRegisterInput();
            }
            else
            {
                btn2.Content = "登录";
                btn1.Content = "to 注册";
                gridRegister.Visibility = Visibility.Collapsed;
                gridLogin.Visibility = Visibility.Visible;
                CheckLoginInput();
            }
        }

        private void Tbxuser_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckRegisterInput();
        }

        private void Pwb2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckRegisterInput();
        }
    }
}
