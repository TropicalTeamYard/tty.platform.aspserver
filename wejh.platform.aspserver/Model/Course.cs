using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using wejh.Configs;
using wejh.Util;

namespace wejh.Model
{
    /// <summary>
    /// <see cref="Course"/>的业务模型。
    /// </summary>
    public class CourseUni : IMySqlQueryable
    {
        public CourseUni()
        {
        }
        public CourseUni(CourseDataZfedu data, int year, int term)
        {
            this.year = year;
            this.term = term;
            name = data.kcmc;
            college = data.collage;
            teacher = data.xm;
            campus = data.xqmc;
            type = data.khfsmc;
            location = data.cdmc;
            weekrange = data.zcd.Substring(0, data.zcd.Length - 1);
            dayofweek = data.xqj;
            timerange = data.jcor;
            classscore = data.classscore;
            classhour = data.classhuor;

            courseid = ToolUtil.MD5Encrypt32($"{year}*{term}*{name}*{location}*{weekrange}*{dayofweek}*{timerange}");
        }
        
        [JsonIgnore]
        public int id { get; set; }
        //课程的唯一标识符，主要用于查询。
        [JsonIgnore]
        public string courseid { get; set; }
        [JsonIgnore]
        public int year { get; set; }
        /// <summary>
        /// 3表示上学期，12表示下学期，16表示短学期。
        /// </summary>
        [JsonIgnore]
        public int term { get; set; }

        public string name { get; set; }
        public string college { get; set; }
        public string type { get; set; }
        public string teacher { get; set; }
        public string campus { get; set; }
        public string location { get; set; }
        public string weekrange { get; set; }
        public string dayofweek { get; set; }
        public string timerange { get; set; }
        public int classscore { get; set; }
        public int classhour { get; set; }

        void IMySqlQueryable.Set(DataRow row)
        {
            throw new NotImplementedException();
        }
        string IMySqlQueryable.GetAddcommand()
        {
            return $"insert into {Config.CourseTable} (id,courseid,year,term,name,college,type,teacher,campus,location,weekrange,dayofweek,timerange,classscore,classhour) values ({id},'{courseid}',{year},{term},'{name}','{college}','{type}','{teacher}','{campus}','{location}','{weekrange}','{dayofweek}','{timerange}',{classscore},{classhour})";
        }
        string IMySqlQueryable.GetQuerycommand()
        {
            return $"select * from {Config.CourseTable} where courseid like '{courseid}'";
        }
    }
    /// <summary>
    /// <see cref="CourseUni"/>的依赖数据。
    /// </summary>
    public class CourseDataZfedu
    {
        /// <summary>
        /// 上课教室
        /// </summary>
        public string cdmc { get; set; }
        /// <summary>
        /// 上课时间，指一天内第几节到第几节上课
        /// </summary>
        public string jcor { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string kcmc { get; set; }
        /// <summary>
        /// 考核方式
        /// </summary>
        public string khfsmc { get; set; }
        /// <summary>
        /// 教室姓名
        /// </summary>
        public string xm { get; set; }
        /// <summary>
        /// 星期几上课
        /// </summary>
        public string xqj { get; set; }
        /// <summary>
        /// 上课校区
        /// </summary>
        public string xqmc { get; set; }
        /// <summary>
        /// 上课周
        /// </summary>
        public string zcd { get; set; }
        /// <summary>
        /// 开课学院
        /// </summary>
        public string collage { get; set; }
        /// <summary>
        /// 学分
        /// </summary>
        public int classscore { get; set; }
        /// <summary>
        /// 学时，但是<see cref="classhuor"/>拼错了
        /// </summary>
        public int classhuor { get; set; }
    }

    /// <summary>
    /// <see cref="CourseUni"/>业务逻辑。
    /// </summary>
    public static class Course
    {
        /// <summary>
        /// 获取正方课表 注释:typeof(<see cref="ResponceModel.data"/>)=<see cref="List{T}"/>where T:<see cref="CourseUni"/>
        /// </summary>
        /// <returns></returns>
        internal static ResponceModel GetZfCourse(string username, string password, int year = 2017, int term = 3)
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
                    CourseDataZfedu[] data = jObject["msg"].ToObject<CourseDataZfedu[]>();

                    //通过扩展方法Map将data转化为sqlData.
                    IEnumerable<CourseUni> sqlData = data.Map((m) => new CourseUni(m, year, term));

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

        /// <summary>
        /// 获取课表，此方法用于与外部进行交互。
        /// </summary>
        /// <param name="credit"></param>
        /// <returns></returns>
        public static ResponceModel GetCourse(string credit)
        {

            try
            {
                if (UserCredit.CheckUser(credit, out string username))
                {
                    UserInfoSql userInfoSql = new UserInfoSql(username);
                    if (userInfoSql.TryQuery())
                    {
                        if (userInfoSql.pwbind_zfedu != null && userInfoSql.pwbind_zfedu != "")
                        {
                            TermTimeUni time = TermTime.Get();
                            var result = GetZfCourse(username, userInfoSql.pwbind_zfedu, time.year, time.term);
                            if (result.code == 200)
                            {
                                var data = (List<CourseUni>)result.data;


                                //用"|"来分割各个部分，这里我们假设courseid不会出现这个字符。
                                //var strlink = string.Join('|', .ToArray());

                                //TODO Put Data Into Database.
                                if (data.Count > 0)
                                {
                                    foreach (var item in data)
                                    {
                                        //在这里我们假设课表信息从不改变，虽然说绝大多数情况下是这样。
                                        if (!item.Exists())
                                        {
                                            item.Add();
                                        }
                                        userInfoSql.linkedcourse = data.Map((m) => m.courseid).ToList();
                                        userInfoSql.UpdateLinkedcourse();
                                    }
                                }

                                return new ResponceModel(200, "获取课表成功。", data);

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
