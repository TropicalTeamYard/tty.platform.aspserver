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
            //FrameUser.NavigateTo(typeof(UserPage));
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
    }
}
