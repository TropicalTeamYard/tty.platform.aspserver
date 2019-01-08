using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tty.Configs;
using tty.Util;

namespace tty.Model
{
    public class MsgBoardUni : ISqlObject
    {
        SqlBaseProvider ISqlObject.SqlProvider => Config.MySqlProvider;
        string ISqlObject.Table => Config.MsgBoardTable;

        [SqlElement]
        [SqlSearchKey]
        public int id { get; set; }
        [SqlElement]
        public string username { get; set; } = "";
        [SqlElement]
        public string time { get; set; } = "1970-01-01 08:00:00";
        [SqlElement]
        public int istop { get; set; } = 0;
        [SqlElement]
        public int islocked { get; set; } = 0;
        [SqlElement]
        [SqlEncrypt]
        public string content { get; set; } = "";
        [SqlElement]
        [SqlEncrypt]
        public string comments { get; set; } = "";

        [SqlElement]
        public byte[] pic { get; set; }

        public List<MsgBoardComment> Comment
        {
            get
            {
                try
                {
                    return JsonConvert.DeserializeObject<List<MsgBoardComment>>(comments);
                }
                catch (Exception)
                {
                    return new List<MsgBoardComment>();
                }

            }
            set => comments = JsonConvert.SerializeObject(comments);
        }
    }

    public class MsgBoardComment
    {
        public string username;
        public string content;
    }

    public static class MsgBoard
    {
        public static ResponceModel Control(string method, string credit, int id, int subid, string content, byte[] pic)
        {
            if (method == "add")
            {

            }
            else
            {
                return ResponceModel.GetInstanceInvalid();
            }
        }

        public static ResponceModel Add(string credit,)
    }
}
