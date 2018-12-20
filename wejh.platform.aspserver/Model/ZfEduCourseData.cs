using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wejh.Model
{
    public class ZfEduCourseData
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
}
