using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tty.Configs;
using tty.Util;

namespace tty.Model
{
    public class ChatUni : ISqlObject
    {
        public ChatUni()
        {
        }

        SqlBaseProvider ISqlObject.SqlProvider => Config.MySqlProvider;
        string ISqlObject.Table => Config.ChatTable;

        [SqlElement]
        [SqlSearchKey]
        public int id { get; set; }
        [SqlElement]
        public string from { get; set; }
        [SqlElement]
        public string to { get; set; }
        [SqlElement]
        public string date { get; set; }
        [SqlElement]
        [SqlEncrypt]
        public string content { get; set; }

    }

    /// <summary>
    /// 处理与聊天系统有关的请求
    /// </summary>
    public static class Chat
    {


    }
}
