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
    public static class EduFunc
    {
        /// <summary>
        /// 获取正方课表 注释:typeof(<see cref="ResponceModel.data"/>)=<see cref="List{T}"/>where T:<see cref="CourseSql"/>
        /// </summary>
        /// <returns></returns>
        public static ResponceModel GetZfCourse(string username, string password, int year = 2017, int term = 3)
        {
            try
            {
                string param = $"username={username}&password={password}&year={year}&term={term}";

                string result = HttpUtil.get(APIKey.ZjCourse, param);
                //string result = "{'status':'success','msg':[]}";

                JObject jObject = (JObject)JsonConvert.DeserializeObject(result);
                //正确返回
                if (jObject["status"].ToString() == "success")
                {
                    ZfEduCourseData[] data = jObject["msg"].ToObject<ZfEduCourseData[]>();

                    //通过扩展方法Map将data转化为sqlData.
                    IEnumerable<CourseSql> sqlData = data.Map((m) => new CourseSql(m, year, term));

                    return new ResponceModel(200, "获取正方课表成功", sqlData);
                }
                else
                {
                    string msg = jObject["msg"].ToString();
                    return new ResponceModel(403, "用户名或密码错误");
                }
            }
            catch (Exception ex)
            {
                return ResponceModel.GetInstanceError(ex);
            }
        }
    }
}
