using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wejh.Configs;
using wejh.Model;
using wejh.Util;

namespace wejh.Function
{
    public static class JhUserFunc
    {
        public static ResponceModel CheckJhUser(string username,string password)
        {
            var request = JhUserRequestModel.getInstanceLogin(new UserModel(username, password));
            string result = HttpUtil.get(API.GetAPI(APIKey.JhUser), request.ToParameters());

            JObject jObject = (JObject)JsonConvert.DeserializeObject(result);

            if (jObject["state"].ToString() == "success")
            {
                JhUserModel jhUser = JsonConvert.DeserializeObject<JhUserModel>(jObject["data"].ToString());

                return new ResponceModel(200, "登录成功", jhUser);
            }
            else
            {
                string info = jObject["info"].ToString();
                if (info.Contains("密码不正确"))
                {
                    return new ResponceModel(403, "密码错误。", null);
                }
                else
                {
                    return new ResponceModel(403, "未注册账户。", null);
                }
            }
        }
    }
}
