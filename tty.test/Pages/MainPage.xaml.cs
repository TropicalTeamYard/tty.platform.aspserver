using Microsoft.Win32;
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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            OnPortraitChanged();
            //FrameUser.NavigateTo(typeof(UserPage));
            App.Current.InterAct.UserData.PropertyChanged += (a, b) => 
            {
                if (b.PropertyName == "Portrait")
                {
                    OnPortraitChanged();
                }
            };
        }

        #region 注销
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            popExit.IsOpen = true;
        }

        private void ButtonExitOK_Click(object sender, RoutedEventArgs e)
        {
            App.Current.InterAct.ExitLogin();
        }
        #endregion
        #region 修改昵称
        private void TbkChangeNickname_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CheckUtil.Nickname(tbkChangeNickname.Text))
            {
                btnChangeNickname.IsEnabled = true;
            }
            else
            {
                btnChangeNickname.IsEnabled = false;
            }
        }
        private void BtnChangeNickname_Click(object sender, RoutedEventArgs e)
        {
            if (App.Current.InterAct.ChangeNickname(tbkChangeNickname.Text))
            {
                
            }

            tbkChangeNickname.Text = "";
        }
        #endregion
        #region 修改密码
        private void pwb_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (CheckUtil.Password(pwb1.Password) && CheckUtil.Password(pwb2.Password))
            {
                btnChangePw.IsEnabled = true;
            }
            else
            {
                btnChangePw.IsEnabled = false;
            }
        }

        private void BtnChangePw_Click(object sender, RoutedEventArgs e)
        {
            btnChangePw.IsEnabled = false;
            App.Current.InterAct.ChangePw(pwb1.Password, pwb2.Password);
            btnChangePw.IsEnabled = true;

            pwb1.Password = "";
            pwb2.Password = "";
        }
        #endregion
        #region 用户头像
        private void OnPortraitChanged()
        {
            BitmapImage bitmap = new BitmapImage();
            if (App.Current.InterAct.UserData.Portrait == null)
            {
                bitmap = new BitmapImage(new Uri("pack://application:,,,/Assets/unset.jpg"));
            }
            else
            {
                bitmap = App.Current.InterAct.UserData.Portrait;
            }

            imgPortrait.Source = bitmap;
        }

        #endregion
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = ".jpg|*.jpg|.png|*.png|.jpeg|*.jpeg";
            if (dialog.ShowDialog() == false) return;
            string _fileName = dialog.FileName;
            //初始化图片
            BitmapImage tempImage = new BitmapImage();
            tempImage.BeginInit();
            tempImage.UriSource = new Uri(_fileName, UriKind.RelativeOrAbsolute);
            tempImage.EndInit();

            App.Current.InterAct.ChangePortrait(tempImage);
        }

        private void GetMsg_Click(object sender, RoutedEventArgs e)
        {
            App.Current.InterAct.SendP();
        }
    }
}
