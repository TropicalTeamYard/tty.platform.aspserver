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

        public MsgUni(int id)
        {
            this.id = id;
        }

        [JsonIgnore]
        SqlBaseProvider ISqlObject.SqlProvider => App.Current.Configuration.MySqlProvider; // Config.MySqlProvider;
        [JsonIgnore]
        string ISqlObject.Table => App.Current.Configuration.TableMap[TableKey.MsgBoard];// Config.MsgBoardTable;

        [SqlElement(isreadonly: true)]
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

        /// <summary>
        /// 仅用于表示评论最后的id.
        /// </summary>
        [SqlElement]
        [SqlBinding("comments")]
        [JsonIgnore]
        public int commentlastid { get; set; } = 0;

        [SqlElement("comments")]
        [SqlEncrypt]
        [SqlBinding("comments")]
        [JsonIgnore]
        public string _comments { get; set; } = "";

        [SqlElement]
        public byte[] pic { get; set; }
        
        public MsgComment[] comments
        {
            get
            {
                try
                {
                    var re = JsonConvert.DeserializeObject<MsgComment[]>(_comments);
                    if (re == null)
                    {
                        return new MsgComment[0];
                    }
                    else
                    {
                        return re;
                    }
                }
                catch (Exception)
                {
                    return new MsgComment[0];
                }

            }
            set => _comments = JsonConvert.SerializeObject(value);
        }

        public DateTime GetUpdateTime()
        {
            DateTime time = DateTime.Parse(this.time);
            if (comments!=null)
            {
                foreach (var item in comments)
                {
                    DateTime time2 = DateTime.Parse(item.time);
                    if (time2 > time)
                    {
                        time = time2;
                    }
                }
            }
            return time;
        }

    }

    public class MsgComment
    {
        public MsgComment()
        {
        }

        public MsgComment(int id, string username, string time, string content)
        {
            this.id = id;
            this.username = username;
            this.time = time;
            this.content = content;
        }

        public int id { get; set; }
        public string username { get; set; }
        public string time { get; set; }
        public string content { get; set; }
    }

    public static class MsgBoard
    {
        public static ResponceModel Control(string method, string credit,int? id, string time, string content, byte[] pic)
        {
            try
            {
                if (credit == null)
                {
                    return ResponceModel.GetInstanceInvalid();
                }
                else if (credit == "")
                {
                    return new ResponceModel(403, "凭证为空");
                }
                else
                {
                    if (UserCredit.CheckUser(credit, out string username))
                    {
                        if (method == "add")
                        {
                            if (content == null)
                            {
                                return ResponceModel.GetInstanceInvalid();
                            }
                            else
                            {
                                return Add(username, content, pic);
                            }
                        }
                        else if (method == "update")
                        {
                            if (time == null)
                            {
                                return ResponceModel.GetInstanceInvalid();
                            }
                            else
                            {
                                return Update(time);
                            }
                        }
                        else if (method == "addcomment")
                        {
                            if (id == null || content == null)
                            {
                                return ResponceModel.GetInstanceInvalid();
                            }
                            else
                            {
                                return AddComment(username, id.Value, content);
                            }
                        }
                        else
                        {
                            return ResponceModel.GetInstanceInvalid();
                        }
                    }
                    else
                    {
                        return new ResponceModel(403, "无效的凭证");
                    }
                }
            }
            catch (Exception ex)
            {
                return ResponceModel.GetInstanceError(ex);
            }
        }

        public static ResponceModel Add(string username, string content, byte[] pic)
        {
            if (content != "")
            {
                MsgUni msg = new MsgUni(username, content, pic)
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
        public static ResponceModel Update(string time)
        {
            if (DateTime.TryParse(time,out DateTime result))
            {
                var data = from item in SqlExtension.GetLastRecords<MsgUni>(200) where (item.GetUpdateTime() > result) select item;

                return new ResponceModel(200, "获取留言成功",new
                {
                    time = DateTime.Now.ToString(),
                    content = data.ToArray()
                });
            }
            else
            {
                return new ResponceModel(403, "时间格式不正确");
            }
        }
        public static ResponceModel AddComment(string username, int id, string content)
        {
            if (content == "")
            {
                return new ResponceModel(403, "评论内容为空");
            }
            else
            {
                MsgUni msg = new MsgUni(id);
                if (msg.TryQuery())
                {
                    var comments = msg.comments.ToList();
                    comments.Add(new MsgComment(++msg.commentlastid, username, DateTime.Now.ToString(), content));
                    msg.comments = comments.ToArray();
                    msg.Update("comments");

                    return new ResponceModel(200, "添加评论成功", msg);
                }
                else
                {
                    return new ResponceModel(403, "该留言不存在");
                }

            }
        }

        private static int GetPriority(string username)
        {
            UserInfoSql userInfo = new UserInfoSql(username);
            if (userInfo.TryQuery())
            {
                return userInfo.permission_msgboard;
            }
            return 0;
        }
    }
}
