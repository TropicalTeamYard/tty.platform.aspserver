using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wejh.Configs;
using wejh.Function;
using wejh.Model;
using wejh.Util;

namespace wejh.platform.aspserver.Controllers
{
    /// <summary>
    /// 登录的API逻辑
    /// </summary>
    [Produces("application/json")]
    [Route("api/login")]
    public class LoginController : Controller
    {
        [HttpGet]
        public JsonResult Get(string username, string password, string devicetype, string devicename)
        {
#if DEBUG
            return Post(username, password, devicetype, devicename);
#else
            return new JsonResult( ResponceModel.GetInstanceBaned());
#endif

        }

        [HttpPost]
        public JsonResult Post(string username, string password, string devicetype, string devicename)
        {
            //api访问失败
            if (username == null || password == null || devicetype == null || devicename == null)
            {
                return ResponceModel.GetInstanceInvalid();
            }
            else
            {
                try
                {
                    if (devicetype == "pc" || devicetype == "mobile")
                    {
                        if (username != "" && password != "")
                        {
                            //向精弘用户中心服务器验证登录
                            var result = JhUserFunc.CheckJhUser(username, password);
                            //验证成功
                            if (result.code == 200)
                            {
                                UserSql userSql = new UserSql((JhUserAPIData)result.data, password);
                                if (devicetype == "mobile")
                                {
                                    userSql.mobile_name = devicename;
                                    userSql.mobile_credit = StringUtil.GetNewToken();

                                    if (MySqlUtil.Exists(userSql))
                                    {
                                        MySqlUtil.Execute(userSql.GetUpdatecommandUser());
                                        MySqlUtil.Execute(userSql.GetUpdatecommandMobile());
                                    }
                                    else
                                    {
                                        MySqlUtil.Add(userSql);
                                    }

                                    return new ResponceModel(result.code, result.msg, userSql.ToUserResultMobile());
                                }
                                else
                                {
                                    userSql.pc_name = devicename;
                                    userSql.pc_credit = StringUtil.GetNewToken();

                                    if (MySqlUtil.Exists(userSql))
                                    {
                                        MySqlUtil.Execute(userSql.GetUpdatecommandUser());
                                        MySqlUtil.Execute(userSql.GetUpdateCommandPc());
                                    }
                                    else
                                    {
                                        MySqlUtil.Add(userSql);
                                    }

                                    return new ResponceModel(result.code, result.msg, userSql.ToUserResultPc());
                                }
                            }
                            else
                            {
                                return new ResponceModel(result.code, result.msg);
                            }
                        }
                        else
                        {
                            return new ResponceModel(403, "用户名或密码不能为空。");
                        }
                    }
                    else
                    {
                        return new ResponceModel(403, "设备类型不符合。");
                    }
                }
                catch (Exception ex)
                {
                    return ResponceModel.GetInstanceError(ex);
                }
            }
        }
    }
}