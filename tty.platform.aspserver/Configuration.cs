using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using tty.Configs;
using tty.Model;
using tty.Util;

namespace tty
{
    public class Configuration
    {
        public Configuration(IConfiguration configuration)
        {
            _configuration = configuration;
            MySqlProvider = new MySqlProvider(new MySqlConnection(configuration.GetConnectionString("ttyplatform")));
            AesAddin = new AesAddin("LAVERALSTARDANDMDS7024200345IEVS");
            TableMap = new TableMap();
            API = new API();
            //TermTimeUnis = GetTimeConfig();
        }

        private IConfiguration _configuration;
        private string userconfigpath = AppDomain.CurrentDomain.BaseDirectory + @"\userconfig.json";
        private TermTimeUni[] GetTimeConfig()
        {
            JObject jObject = (JObject)JsonConvert.DeserializeObject(File.ReadAllText(userconfigpath));
            TermTimeUni[] terms = jObject["TermTime"].ToObject<TermTimeUni[]>();
            return terms;
        }

        /// <summary>
        /// 提供MySql四大方法
        /// </summary>
        internal MySqlProvider MySqlProvider { get; }
        /// <summary>
        /// AES加密插件
        /// </summary>
        internal AesAddin AesAddin { get; }
        /// <summary>
        /// MySql数据库名键值对
        /// </summary>
        internal TableMap TableMap { get; }
        /// <summary>
        /// 第三方API
        /// </summary>
        internal API API { get; }
        /// <summary>
        /// 学期时间设置
        /// </summary>
        internal TermTimeUni[] TermTimeUnis { get=> GetTimeConfig(); }
        /// <summary>
        /// 默认头像
        /// </summary>
        internal readonly string DefaultPortrait = "default::unset";
        /// <summary>
        /// 用户头像缓存地址
        /// </summary>
        internal readonly string PortraitCache = AppDomain.CurrentDomain.BaseDirectory + @"\portrait";
    }
}
