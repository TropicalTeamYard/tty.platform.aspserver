using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
