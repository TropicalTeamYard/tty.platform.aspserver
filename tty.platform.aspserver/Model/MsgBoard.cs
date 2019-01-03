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
    public class MsgBoardSql : ISqlObject
    {
        SqlBaseProvider ISqlObject.SqlProvider => Config.MySqlProvider;
        string ISqlObject.Table => Config.MsgBoardTable;

        [SqlElement]
        [SqlSearchKey]
        public int id { get; set; }
        [SqlElement]
        public string username { get; set; } = "";
        [SqlElement]
        public string time { get; set; } = "1970-01-01 12:00:00";
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

        public static explicit operator MsgBoardModel(MsgBoardSql obj)
        {
            UserModel user = new UserModel(obj.username);

            UserInfoSql userInfo = new UserInfoSql();
            if (userInfo.TryQuery())
            {

            }
            string portrait = userInfo.portrait;

            MsgComment[] comments = JsonConvert.DeserializeObject<MsgComment[]>(obj.comments);



            return new MsgBoardModel(obj.id, user, new MsgMeta(
                userInfo.portrait, obj.istop == 1 ? true : false,
                obj.islocked == 1 ? true : false,
                DateTime.Parse(obj.time)),
                obj.content,
                comments
                );

        }
    }

    public class MsgMeta
    {
        public MsgMeta()
        {
        }

        public MsgMeta(string portrait, bool istop, bool islocked, DateTime time)
        {
            this.portrait = portrait;
            this.istop = istop;
            this.islocked = islocked;
            Time = time;
        }

        public string portrait { get; set; }
        public bool istop { get; set; } = false;
        public bool islocked { get; set; } = false;
        public string time { get; set; }

        [JsonIgnore]
        public DateTime Time
        {
            get => DateTime.Parse(time);
            set => time = value.ToString("yyyy-mm-dd hh:mm:ss");
        }
    }

    public class MsgComment
    {
        public MsgComment()
        {
        }

        public MsgComment(int id, UserModel user, DateTime time, string content)
        {
            this.id = id;
            this.user = user;
            Time = time;
            this.content = content;
        }

        public int id { get; set; }
        public UserModel user { get; set; }
        public string time { get; set; }

        [JsonIgnore]
        public DateTime Time
        {
            get => DateTime.Parse(time);
            set => time = value.ToString("yyyy-mm-dd hh:mm:ss");
        }

        public string content { get; set; }
    }

    public class MsgBoardModel
    {
        public MsgBoardModel()
        {
        }

        public MsgBoardModel(int id, UserModel user, MsgMeta meta, string content, params MsgComment[] comments)
        {
            this.id = id;
            this.user = user;
            this.meta = meta;
            this.content = content;
            this.comments = comments.ToList();
        }

        public int id { get; set; }
        public UserModel user { get; set; }
        public MsgMeta meta { get; set; }
        public string content { get; set; }
        public List<MsgComment> comments { get; set; }
    }

    public class MsgBoard
    {
    }
}
