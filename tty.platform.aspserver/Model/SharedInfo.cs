using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tty.Configs;
using tty.Util;

namespace tty.Model
{
    /// <summary>
    /// 处理与共享信息有关的信息，包括用户公共信息
    /// </summary>
    public static class SharedInfo
    {
        internal static ResponceModel SharedInfoControl(string type, string query)
        {
            try
            {
                if (type == "" || query == "")
                {
                    return ResponceModel.GetInstanceInvalid();
                }
                else if (type == "user")
                {
                    return GetUserInfo(query);
                }
                else if (type == "usermd5")
                {
                    return GetUserMD5(query);
                }
                else
                {
                    return ResponceModel.GetInstanceInvalid();
                }
            }
            catch (Exception ex)
            {
                return ResponceModel.GetInstanceError(ex);
            }
        }
        internal static ResponceModel GetUserInfo(string query)
        {
            string[] users = JsonConvert.DeserializeObject<string[]>(query);

            List<dynamic> re = new List<dynamic>();

            foreach (var item in users)
            {
                if (UserInfo.TryGetUserInfo(item, true, out dynamic data))
                {
                    re.Add(data);
                }
            }

            return new ResponceModel(200, "获取信息成功", re.ToArray());
        }
        internal static ResponceModel GetUserMD5(string query)
        {
            string[] users = JsonConvert.DeserializeObject<string[]>(query);

            List<dynamic> re = new List<dynamic>();

            foreach (var item in users)
            {
                if (UserInfo.TryGetUserInfo(item, true, out dynamic data))
                {
                    string jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                    re.Add(new
                    {
                        username=item,
                        md5 = ToolUtil.MD5Encrypt32(jsonstring)
                    });
                }
            }
            return new ResponceModel(200, "获取信息成功", re.ToArray());
        }
    }
}
