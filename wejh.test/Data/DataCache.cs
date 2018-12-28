using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCore.Component;
using wejh.test.Config;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using wejh.test.Model;
using wejh.test.Http;
using System.Xml.Serialization;

namespace wejh.test.Data
{

    public class DataCache:USettings
    {
        public DataCache()
        {
            Folder = AppDomain.CurrentDomain.BaseDirectory;
            DisplayName = "settings";
        }

        public User user { get; set; } = new User();

        public event EventHandler OnStateChanged;

        public void Login(string username, string password)
        {
            try
            {
                var postdata = $"method=quicklogin&username={username}&password={password}&devicetype=pc";

                var result = JsonConvert.DeserializeObject<ResponceModel<UserCredit>>(
                    HttpUtil.post(API.GetAPI(APIKey.User), postdata)
                    );
                var data = result.data;

                if (result.code == 200)
                {
                    user.username = data.username;
                    user.nickname = data.nickname;
                    user.credit = data.credit;
                    user.state = 2;

                    OnStateChanged?.Invoke(this, new EventArgs());
                }
                App.Window.SetMessage(result.msg);
            }
            catch (Exception)
            {
                App.Window.SetMessage("登录操作失败");
            }
        }
        public void ChangeNickname(string nickname)
        {
            try
            {
                var postdata = $"method=changenickname&credit={user.credit}&nickname={nickname}";

                var result = JsonConvert.DeserializeObject<ResponceModel> (HttpUtil.post(API.GetAPI(APIKey.User), postdata));

                if (result.code == 200)
                {
                    user.state = 2;
                    user.nickname = nickname;
                }
                else
                {
                    user.state = 1;
                }

                OnStateChanged?.Invoke(this, new EventArgs());
                App.Window.SetMessage(result.msg);
            }
            catch (Exception)
            {
                App.Window.SetMessage("修改昵称操作失败");
            }
        }
    }

    public class User
    {
        public User()
        {
        }
        public string username { get; set; } = "";
        public string nickname { get; set; } = "";
        public string credit { get; set; } = "";
        public int state { get; set; } = 0;
    }
}
