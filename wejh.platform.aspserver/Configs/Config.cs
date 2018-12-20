
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using wejh.Model;

namespace wejh.Configs
{
    public static class Config
    {
        public static IConfiguration Configuration { get; set; }
        public static string Conn => Configuration.GetConnectionString("wejhplatform");
        public static string UserCreditTable => "usercredit";
        public static string CourseTable => "course";
        public static string UserInfoTable => "userinfo";

        public static TermTime GetTermTime()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\userconfig.json";
            JObject jObject = (JObject)JsonConvert.DeserializeObject(File.ReadAllText(path));
            TermTime[] terms = jObject["TermTime"].ToObject<TermTime[]>();

            var x = (from item in terms orderby item.begin select item).ToArray();

            DateTime date = DateTime.Now.Date;
            int index = -1;
            for (int i = 0; i < x.Length; i++)
            {
                //在学期里面
                if (date < DateTime.Parse( x[i].begin))
                {
                    if (i == 0)
                    {
                        return null;
                    }
                    else
                    {
                        index = i - 1;
                    }
                }
            }
            if (index == -1)
            {
                return null;
            }

            TermTime fo = x[index];

            fo.weeklasting = (((DateTime.Parse( fo.end) - DateTime.Parse( fo.begin)).Days + 1) / 7);
            fo.week = (date - DateTime.Parse( fo.begin)).Days / 7 + 1;
            fo.dayofweek = (int)date.DayOfWeek;

            return fo;
        }
    }
}

