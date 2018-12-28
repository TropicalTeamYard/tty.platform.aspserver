
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
    internal static class Config
    {
        internal static IConfiguration Configuration { get; set; }
        internal static string Conn => Configuration.GetConnectionString("wejhplatform");
        internal static MySqlProvider MySqlProvider => new MySqlProvider(new MySqlConnection(Conn));

        internal static string UserCreditTable => "usercredit";
        internal static string CourseTable => "course";
        internal static string UserInfoTable => "userinfo";

        internal static TermTimeUni[] GetTimeConfig()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\userconfig.json";
            JObject jObject = (JObject)JsonConvert.DeserializeObject(File.ReadAllText(path));
            TermTimeUni[] terms = jObject["TermTime"].ToObject<TermTimeUni[]>();
            return terms;
        }
    }
}

