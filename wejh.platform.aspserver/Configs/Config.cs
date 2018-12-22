
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using wejh.Model;
using wejh.Util;

namespace wejh.Configs
{
    public static class Config
    {
        public static IConfiguration Configuration { get; set; }
        public static string Conn => Configuration.GetConnectionString("wejhplatform");
        public static MySqlProvider MySqlProvider => new MySqlProvider(new MySqlConnection(Conn));

        public static string UserCreditTable => "usercredit";
        public static string CourseTable => "course";
        public static string UserInfoTable => "userinfo";

        internal static TermTimeUni[] GetTimeConfig()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\userconfig.json";
            JObject jObject = (JObject)JsonConvert.DeserializeObject(File.ReadAllText(path));
            TermTimeUni[] terms = jObject["TermTime"].ToObject<TermTimeUni[]>();
            return terms;
        }
    }
}

