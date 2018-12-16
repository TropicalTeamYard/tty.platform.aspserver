using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wejh.Configs;
using wejh.Model;
using wejh.Util;

namespace wejh.platform.aspserver.Controllers
{
    /// <summary>
    /// 自动登录的API逻辑
    /// </summary>
    [Produces("application/json")]
    [Route("api/autoLogin")]
    public class AutoLoginController : Controller
    {
        [HttpGet]
        public JsonResult Get(string credit)
        {
            if (Config.IsTest)
            {
                return Post(credit);
            }
            else
            {
                return new JsonResult(new ResponceModel(405, "Get方法已禁止"));
            }
        }

        [HttpPost]
        public JsonResult Post(string credit)
        {
            if (credit == null)
            {
                return new JsonResult(new ResponceModel(400, "无效的访问"));
            }
            else
            {
                DataSet data = null;
                int flag = -1;
                if (SqlUtil.TryQuery(UserSql.GetQuerycommandMobileCredit(credit), out var dataSet1))
                {
                    data = dataSet1;
                    flag = 0;
                }
                if (SqlUtil.TryQuery(UserSql.GetQuerycommandPcCredit(credit), out var dataSet2))
                {
                    data = dataSet2;
                    flag = 1;
                }

                if (flag == -1)
                {
                    return new JsonResult(new ResponceModel(403, "自动登录失败。"));
                }
                else
                {
                    //验证账户。
                    UserSql userSql = UserSql.FromDataRow(data.Tables[0].Rows[0]);

                    var result = Function.JhUserFunc.CheckJhUser(userSql.username, userSql.password);

                    if (result.code == 200)
                    {
                        //移动端登录
                        if (flag == 0)
                        {
                            userSql.mobile_credit = StringUtil.GetNewToken();
                            //更新数据库。
                            SqlUtil.Execute(userSql.GetUpdatecommandMobile());

                            return new JsonResult(new ResponceModel(200, "自动登录成功。", userSql.ToUserResultMobile()));
                        }
                        else
                        {
                            userSql.pc_credit = StringUtil.GetNewToken();
                            //更新数据库。
                            SqlUtil.Execute(userSql.GetUpdateCommandPc());

                            return new JsonResult(new ResponceModel(200, "自动登录成功。", userSql.ToUserResultPc()));
                        }
                    }
                    else
                    {
                        return new JsonResult(new ResponceModel(403, "自动登录已失效，请重新绑定账号"));
                    }
                } 
            }
        }
    }
}