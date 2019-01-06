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

        public /*async*/ void Login(string username, string password)
        {
            //await Task.Run(() =>
            //{
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
            catch (Exception)
            {
                MessageInvoked?.Invoke(this, new MessageEventArgs("login", "登录操作失败"));
            }
            //}
            //);
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
