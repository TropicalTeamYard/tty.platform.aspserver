using Microsoft.AspNetCore.Mvc;
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
        private static bool TryGetUserInfo(string username, out dynamic data)
        {
            UserCreditSql user = new UserCreditSql(username);
            if (user.TryQuery())
            {
                string portrait = null;

                UserInfoSql userInfo = new UserInfoSql(username);
                if (userInfo.TryQuery())
                {
                    portrait = Convert.ToBase64String(userInfo.portrait);
                }
                else
                {
                    portrait = Config.defaultportrait;
                }

                var result = new
                {
                    username = user.username,
                    user.nickname,
                    user.usertype,
                    portrait = portrait,
                    premission_msgboard = userInfo.permission_msgboard
                };

                data = result;
                return true;
            }
            else
            {
                data = null;
                return false;
            }
        }
        internal static ResponceModel GetUserInfo(string query)
        {
            if (TryGetUserInfo(query,out dynamic data))
            {
                return new ResponceModel(200, "获取信息成功", data);
            }
            else
            {
                return new ResponceModel(403, "不存在该用户");
            }
        }
        internal static ResponceModel GetUserMD5(string query)
        {
            if (TryGetUserInfo(query, out dynamic data))
            {
                string jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                return new ResponceModel(200, "获取信息成功", ToolUtil.MD5Encrypt32(jsonstring) );
            }
            else
            {
                return new ResponceModel(403, "不存在该用户");
            }
        }
    }
}
