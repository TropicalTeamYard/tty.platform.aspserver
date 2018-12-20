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
    [Produces("application/json")]
    [Route("api/course")]
    public class CourseController : Controller
    {
        [HttpGet]
        public JsonResult Get(string credit)
        {
#if DEBUG
            return Post(credit);
#else
            return ResponceModel.GetInstanceBaned();
#endif
        }

        public JsonResult Post(string credit)
        {
            if (credit == null)
            {
                return ResponceModel.GetInstanceInvalid();
            }
            else
            {
                try
                {
                    if (ToolUtil.CheckUser(credit, out string username))
                    {
                        UserInfoSql userInfoSql = new UserInfoSql(username);
                        if (MySqlUtil.TryQuery(userInfoSql, out var table))
                        {
                            userInfoSql = UserInfoSql.FromDataRow(table.Rows[0]);
                            if (userInfoSql.pwbind_zfedu != null && userInfoSql.pwbind_zfedu != "")
                            {
                                TermTime time = Config.GetTermTime();
                                var result = EduFunc.GetZfCourse(username, userInfoSql.pwbind_zfedu, time.year, time.term);
                                if (result.code == 200)
                                {
                                    var data = (List<CourseSql>)result.data;
                                    
                                    var datare = data.Map((m) => (CourseModel)m);
                                    //用"|"来分割各个部分，这里我们假设courseid不会出现这个字符。
                                    //var strlink = string.Join('|', .ToArray());
                                    
                                    //TODO Put Data Into Database.
                                    if (data.Count > 0)
                                    {
                                        foreach (var item in data)
                                        {
                                            //在这里我们假设课表信息从不改变，虽然说绝大多数情况下是这样。
                                            if (!MySqlUtil.Exists(item))
                                            {
                                                MySqlUtil.Add(item);
                                            }
                                            userInfoSql.linkedcourse = data.Map((m) => m.courseid).ToList();
                                            MySqlUtil.Execute(userInfoSql.GetUpdatelinkedcoursecommand());
                                        }
                                    }

                                    return new ResponceModel(200, "获取课表成功。",datare);
                                   
                                }
                                else
                                {
                                    return new ResponceModel(403, "请重新绑定正方。");
                                }
                            }
                            else
                            {
                                return new ResponceModel(403, "请重新绑定正方。");
                            }
                        }
                        else
                        {
                            return new ResponceModel(403, "你还没有绑定正方。");
                        }
                    }
                    else
                    {
                        return new ResponceModel(403, "自动登录已失效，请重新绑定。");
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