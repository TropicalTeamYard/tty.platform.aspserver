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
    public class MsgUni : ISqlObject
    {
        public MsgUni()
        {
        }

        public MsgUni(string username)
        {
            this.username = username;
        }

        public MsgUni(string username, string content, byte[] pic) : this(username)
        {
            this.content = content;
            this.pic = pic;
        }

        SqlBaseProvider ISqlObject.SqlProvider => Config.MySqlProvider;
        string ISqlObject.Table => Config.MsgBoardTable;

        [SqlElement(isreadonly:true)]
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

        public List<MsgComment> Comment
        {
            get
            {
                try
                {
                    return JsonConvert.DeserializeObject<List<MsgComment>>(comments);
                }
                catch (Exception)
                {
                    return new List<MsgComment>();
                }

            }
            set => comments = JsonConvert.SerializeObject(comments);
        }
    }

    public class MsgComment
    {
        public string username;
        public string content;
    }

    public static class MsgBoard
    {
        public static ResponceModel Control(string method, string credit, int id, int subid, string content, byte[] pic)
        {
            try
            {
                if (method == "add")
                {
                    if (credit == null || content == null)
                    {
                        return ResponceModel.GetInstanceInvalid();
                    }
                    else
                    {
                        return Add(credit, content, pic);
                    }
                }
                else
                {
                    return ResponceModel.GetInstanceInvalid();
                }
            }
            catch (Exception ex)
            {
                return ResponceModel.GetInstanceError(ex);
            }
        }

        public static ResponceModel Add(string credit, string content, byte[] pic)
        {
            if (credit == "")
            {
                return new ResponceModel(403, "用户凭证为空");
            }
            UserCreditSql user = new UserCreditSql();
            if (user.TryQuery(credit,out string devicetype))
            {
                if (content !="")
                {
                    MsgUni msg = new MsgUni(user.username, content, pic)
                    {
                        time = DateTime.Now.ToString()
                    };
                    msg.Add();

                    msg = SqlExtension.GetLastRecord<MsgUni>();

                    return new ResponceModel(200, "添加成功", new
                    {
                        time = DateTime.Now.ToString(),
                        msg
                    }); 
                }
                else
                {
                    return new ResponceModel(403, "留言内容为空");
                }
            }
            else
            {
                return new ResponceModel(403, "无效的凭证");
            }
        }

        private static int GetPriority(string username)
        {
            UserInfoSql userInfo = new UserInfoSql(username);
            if (userInfo.TryQuery())
            {
                return userInfo.priority_msgboard;
            }
            return 0;
        }
    }
}
