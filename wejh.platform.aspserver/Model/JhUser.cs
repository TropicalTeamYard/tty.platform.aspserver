using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wejh.Configs;
using wejh.Util;

namespace wejh.Model
{
    public class JhUserRequestModel
    {
        public JhUserRequestModel()
        {
        }

        public JhUserRequestModel(string app, string action)
        {
            this.app = app;
            this.action = action;
        }
        public JhUserRequestModel(string app, string action, string passport, string password)
        {
            this.app = app;
            this.action = action;
            this.passport = passport;
            this.password = password;
        }
        public JhUserRequestModel(UserCreditModel user, string app, string action):this(app,action,user.username,user.password)
        {

        }

        public string app { get; set; }
        public string action { get; set; }
        public string passport { get; set; }
        public string password { get; set; }

        public string ToParameters()
        {
            return string.Format("app={0}&action={1}&passport={2}&password={3}",app,action,passport,password);
        }
    }

    public class JhUserData
    {
        public string pid { get; set; }
        public string email { get; set; }
        public string type { get; set; }
    }

    public static class JhUser
    {
        /// <summary>
        /// 在精弘用户中心验证账号 注释:typeof(<see cref="ResponceModel.data"/>)=<see cref="JhUserData"/>
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static ResponceModel CheckUser(string username, string password)
        {
            var request = new JhUserRequestModel("passport", "login", username, password);
            string result = HttpUtil.get(APIKey.JhUser, request.ToParameters());

            JObject jObject = (JObject)JsonConvert.DeserializeObject(result);

            if (jObject["state"].ToString() == "success")
            {
                JhUserData jhUser = jObject["data"].ToObject<JhUserData>();

                return new ResponceModel(200, "登录成功", jhUser);
            }
            else
            {
                string info = jObject["info"].ToString();
                if (info.Contains("密码不正确"))
                {
                    return new ResponceModel(403, "密码错误", null);
                }
                else
                {
                    return new ResponceModel(403, "未注册账户", null);
                }
            }
        }
    }
}
