using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wejh.Function;
using wejh.Model;
using wejh.Util;

namespace wejh.platform.aspserver.Controllers
{
    [Produces("application/json")]
    [Route("api/pwbind")]
    public class PwBindController : Controller
    {
        [HttpGet]
        public JsonResult Get(string credit, string bindname, string password)
        {
            return Post(credit,bindname,password);
        }

        [HttpPost]
        public JsonResult Post(string credit, string bindname, string password)
        {
            if (credit == null || bindname == null || password == null)
            {
                return new ResponceModel(400, "无效的访问。");
            }
            else
            {
                try
                {
                    if (ToolUtil.CheckUser(credit,out string username))
                    {
                        //TODO
                        if (bindname == "lib")
                        {
                            return new ResponceModel(500, "图书馆绑定正在开发中。");
                        }
                        else if (bindname == "card")
                        {
                            return new ResponceModel(500, "校园网绑定正在开放中。");
                        }
                        else if (bindname == "ycedu")
                        {
                            return new ResponceModel(500, "原创教务绑定正在开发中。");
                        }
                        else if (bindname == "zfedu")
                        {
                            if (EduFunc.GetZfCourse(username,password).code == 200)
                            {
                                UserInfoSql userInfoSql = new UserInfoSql(username, pwbind_lib: password);
                                if (MySqlUtil.Exists(userInfoSql))
                                {
                                    MySqlUtil.Execute(userInfoSql.GetUpdatepwbind_zfeducommand());
                                }
                                else
                                {
                                    MySqlUtil.Add(userInfoSql);
                                }

                                return new ResponceModel(200, "绑定正方成功。");
                            }
                            else
                            {
                                return new ResponceModel(403, "绑定正方失败。");
                            }
                        }
                        else
                        {
                            return new ResponceModel(400, "绑定类型发生错误。");
                        }
                    }
                    else
                    {
                        return new ResponceModel(403, "自动登录已失效，请重新登录。");
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