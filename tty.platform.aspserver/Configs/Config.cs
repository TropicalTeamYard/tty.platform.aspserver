using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using tty.Model;
using tty.Util;

namespace tty.Configs
{
    internal static class Config
    {
        internal static IConfiguration Configuration { get; set; }
        internal static string Conn => Configuration.GetConnectionString("ttyplatform");
        internal static MySqlProvider MySqlProvider => new MySqlProvider(new MySqlConnection(Conn));

        internal static string UserCreditTable => "usercredit";
        internal static string CourseTable => "course";
        internal static string UserInfoTable => "userinfo";
        internal static string MsgBoardTable => "msgboard";
        internal static string ChatTable => "chattable";
        internal static string EncryptKey => "LAVERALSTARDANDMDS7024200345IEVS";
        internal static AesAddin Aes => new AesAddin(EncryptKey);

        internal static string PortraitCache => AppDomain.CurrentDomain.BaseDirectory + @"\portrait";
        #region 默认用户头像
        internal static string defaultportrait = "default::unset";
        #endregion


        internal static TermTimeUni[] GetTimeConfig()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\userconfig.json";
            JObject jObject = (JObject)JsonConvert.DeserializeObject(File.ReadAllText(path));
            TermTimeUni[] terms = jObject["TermTime"].ToObject<TermTimeUni[]>();
            return terms;
        }
    }
}

