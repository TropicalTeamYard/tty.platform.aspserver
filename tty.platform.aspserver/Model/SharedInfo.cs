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
            UserCreditSql user = new UserCreditSql(query);
            if (user.TryQuery())
            {
                string portrait = null;
                
                UserInfoSql userInfo = new UserInfoSql(query);
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
                };

                return new ResponceModel(200, "获取信息成功", result);
            }
            else
            {
                return new ResponceModel(403, "不存在该用户");
            }
        }
    }
}
