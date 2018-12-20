using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wejh.Configs;
using wejh.Util;

namespace wejh.Model
{
    public class CourseSql:CourseModel,ISqlQueryable
    {
        public CourseSql()
        {
        }

        public CourseSql(ZfEduCourseData data, int year, int term)
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

            courseid = ToolUtil.MD5Encrypt32( $"{year}*{term}*{name}*{location}*{weekrange}*{dayofweek}*{timerange}");
        }

        public int id { get; set; }
        //课程的唯一标识符，主要用于查询。
        public string courseid { get; set; }
        public int year { get; set; }
        /// <summary>
        /// 3表示上学期，12表示下学期，16表示短学期。
        /// </summary>
        public int term { get; set; }

        string ISqlQueryable.GetAddcommand()
        {
            return $"insert into {Config.CourseTable} (id,courseid,year,term,name,college,type,teacher,campus,location,weekrange,dayofweek,timerange,classscore,classhour) values ({id},'{courseid}',{year},{term},'{name}','{college}','{type}','{teacher}','{campus}','{location}','{weekrange}','{dayofweek}','{timerange}',{classscore},{classhour})";
        }
        string ISqlQueryable.GetQuerycommand()
        {
            return $"select * from {Config.CourseTable} where courseid like '{courseid}'";
        }
    }
}
