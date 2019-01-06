using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tty.interactive.Config;
using tty.interactive.Model;
using tty.interactive.Util;

namespace tty.interactive
{
    /// <summary>
    /// <see cref="tty.interactive"/>的交互类。
    /// </summary>
    public class InterAct
    {
        public InterAct()
        {
        }

        public event EventHandler<MessageEventArgs> MessageInvoked;
        public API API { get; } = new API();
        public Data.UserData UserData { get; private set; } = new Data.UserData();
        private readonly string path = AppDomain.CurrentDomain.BaseDirectory;

        public bool Register(string username, string nickname, string password,out string msg)
        {
            try
            {
                var postdata = $"method=register&username={username}&nickname={nickname}&password={password}";
                var result = JsonConvert.DeserializeObject<ResponceModel>(
                    HttpUtil.post(API[APIKey.User], postdata)
                    );

                msg = result.msg;
                MessageInvoked?.Invoke(this, new MessageEventArgs("register", result.msg));
                if (result.code == 200)
                {
                    return true;
                    
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageInvoked?.Invoke(this, new MessageEventArgs("register", $"注册失败 {ex.Message}"));
                msg = "";
                return false;
            }
        }
        public void Login(string username, string password)
        {
            try
            {
                var postdata = $"method=login&username={username}&password={password}&devicetype=pc";
                var result = JsonConvert.DeserializeObject<ResponceModel<UserCredit>>(
                    HttpUtil.post(API[APIKey.User], postdata)
                    );
                //var data = result.data;

                if (result.code == 200)
                {
                    UserData.username = result.data.username;
                    UserData.nickname = result.data.nickname;
                    UserData.credit = result.data.credit;
                    UserData.userstate = Data.UserState.Success;
                }
                MessageInvoked?.Invoke(this, new MessageEventArgs("login", result.msg));
            }
            catch (Exception ex)
            {
                MessageInvoked?.Invoke(this, new MessageEventArgs("login", $"登录操作失败 {ex.Message}"));
            }
        }
        public void AutoLogin()
        {
            try
            {
                if (UserData.credit != null)
                {
                    var postdata = $"method=autologin&credit={UserData.credit}";
                    var result = JsonConvert.DeserializeObject<ResponceModel<UserCredit>>(
                        HttpUtil.post(API[APIKey.User], postdata)
                        );

                    if (result.code == 200)
                    {
                        UserData.username = result.data.username;
                        UserData.nickname = result.data.nickname;
                        UserData.credit = result.data.credit;
                        UserData.userstate = Data.UserState.Success;
                    }
                    else
                    {
                        UserData.userstate = Data.UserState.Waring;
                    }

                    MessageInvoked?.Invoke(this, new MessageEventArgs("autologin", result.msg));
                }
                else
                {
                    MessageInvoked?.Invoke(this, new MessageEventArgs("autologin", "用户凭证不存在"));
                }
            }
            catch (Exception ex)
            {
                MessageInvoked?.Invoke(this, new MessageEventArgs("autologin", $"自动登录失败 {ex.Message}"));
            }

        }
        public void ExitLogin()
        {
            UserData.username = "";
            UserData.credit = null;
            UserData.nickname = "";
            UserData.userstate =  Data.UserState.NoLogin;
        }
        public bool ChangeNickname(string nickname)
        {
            try
            {
                var postdata = $"method=changenickname&credit={UserData.credit}&nickname={nickname}";
                var result = JsonConvert.DeserializeObject<ResponceModel>(
                    HttpUtil.post(API[APIKey.User], postdata)
                    );

                if (result.code == 200)
                {
                    UserData.nickname = nickname;
                }
                else
                {
                    UserData.userstate = Data.UserState.Waring;
                }

                MessageInvoked?.Invoke(this, new MessageEventArgs("changenickname", result.msg));
                return false;
            }
            catch (Exception ex)
            {
                MessageInvoked?.Invoke(this, new MessageEventArgs("changenickname", $"修改昵称失败 {ex.Message}"));
                return false;
            }
        }

        public void Load()
        {

            try
            {

                File.ReadAllText(path + @"\user.json");
                UserData = JsonConvert.DeserializeObject<Data.UserData>(File.ReadAllText(path + @"\user.json"));
            }
            catch (Exception)
            {

            }
        }
        public void Save()
        {
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception)
            {
            }

            string data = JsonConvert.SerializeObject(UserData);
            File.WriteAllText(path + @"\user.json", data);

        }
    }

    public class MessageEventArgs : EventArgs
    {
        public MessageEventArgs(string action, string message)
        {
            Action = action;
            Message = message;
        }

        public string Action { get; }
        public string Message { get; }
    }
}
