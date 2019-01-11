using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using tty.interactive.Util;

namespace tty.interactive.Data
{
    public class UserData:INotifyPropertyChanged
    {
        public UserData()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _username = "";
        private string _nickname = "";
        private UserState _userstate = UserState.NoLogin;
        private BitmapImage _portrait = null;
        private string _email = "";
        private string _phone = "";

        /// <summary>
        /// 用户名(可绑定)
        /// </summary>
        public string username
        {
            get => _username;
            set

            {
                _username = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(username)));
            }
        }
        /// <summary>
        /// 昵称(可绑定)
        /// </summary>
        public string nickname
        {
            get => _nickname;
            set
            {
                _nickname = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(nickname)));
            }
        }
        /// <summary>
        /// 用户凭证
        /// </summary>
        public string credit { get; set; }
        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 用户手机号码
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 用户状态(可绑定)
        /// </summary>
        public UserState userstate
        {
            get => _userstate;
            set
            {
                _userstate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(userstate)));
            }
        }
        public byte[] portrait
        {
            get => ToolUtil.BitmapImageToBytes(_portrait);
            set => Portrait = ToolUtil.BytesToBitmapImage(value);
        }

        [JsonIgnore]
        public BitmapImage Portrait
        {
            get => _portrait;
            set
            {
                _portrait = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Portrait)));
            }
        }
    }
}
