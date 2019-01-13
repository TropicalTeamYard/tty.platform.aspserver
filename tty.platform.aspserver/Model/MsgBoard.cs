using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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
        private string picFileName => App.Current.Configuration.MsgboardCache + $"\\{id}_content";

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
        [SqlBinding("body")]
        public string content { get; set; } = "";
        /// <summary>
        /// 留言标记，0表示正常，2表示删除。
        /// </summary>
        [SqlElement]
        [SqlBinding("mark")]
        public int mark { get; set; }
        [SqlElement]
        [SqlBinding("comments")]
        [SqlBinding("body")]
        [SqlBinding("mark")]
        [JsonIgnore]
        public string updatetime { get; set; } = "1970-01-01 08:01:00";
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
        public DateTime GetUpdateTime() => DateTime.Parse(this.updatetime);

        public void ReadPic()
        {

            if (File.Exists(picFileName))
            {
                pic = File.ReadAllBytes(picFileName);
            }
        }
        public void SavePic()
        {
            if (!Directory.Exists(App.Current.Configuration.PortraitCache))
            {
                Directory.CreateDirectory(App.Current.Configuration.PortraitCache /*Config.PortraitCache*/);
            }

            File.WriteAllBytes(picFileName, pic);
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
        public static ResponceModel Control(string method, string credit, int? id, int? subid, string time, string content, string pic)
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
                        else if (method == "change")
                        {
                            if (id == null || content == null)
                            {
                                return ResponceModel.GetInstanceInvalid();
                            }
                            else
                            {
                                return Change(username, id.Value, content, pic);
                            }
                        }
                        else if (method == "delete")
                        {
                            if (id == null)
                            {
                                return ResponceModel.GetInstanceInvalid();
                            }
                            else
                            {
                                return Delete(username, id.Value);
                            }
                        }
                        else if (method == "deletecomment")
                        {
                            if (id == null || subid == null)
                            {
                                return ResponceModel.GetInstanceInvalid();
                            }
                            else
                            {
                                return DeleteCommnet(username, (int)id, (int)subid);
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

        public static ResponceModel Add(string username, string content, string pic)
        {
            if (content != "")
            {
                MsgUni msg = new MsgUni(username, content, ToolUtil.HexToByte(pic))
                {
                    time = DateTime.Now.ToString(),
                    updatetime = DateTime.Now.ToString(),
                };
                msg.Add();

                msg = SqlExtension.GetLastRecord<MsgUni>();

                msg.SavePic();
                return new ResponceModel(200, "添加成功", msg);
            }
            else
            {
                return new ResponceModel(403, "留言内容为空");
            }
        }
        public static ResponceModel Update(string time)
        {
            if (DateTime.TryParse(time, out DateTime result))
            {
                var data = from item in SqlExtension.GetLastRecords<MsgUni>(200) where (item.GetUpdateTime() > result) select item;

                // Mark: 因传输图片过于消耗流量，故特意将图片的获取分离开来。
                // 提取图片。
                foreach (var item in data)
                {
                    item.ReadPic();
                }

                return new ResponceModel(200, "获取留言成功", new
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
                    msg.updatetime = DateTime.Now.ToString();

                    msg.Update("comments");
                    //msg.Update("updatetime");

                    return new ResponceModel(200, "添加评论成功", msg);
                }
                else
                {
                    return new ResponceModel(403, "该留言不存在");
                }

            }
        }
        public static ResponceModel Change(string username, int id, string content, string pic)
        {
            if (content != "")
            {
                MsgUni msg = new MsgUni(id);
                if (msg.TryQuery())
                {
                    if (msg.mark == 2)
                    {
                        return new ResponceModel(403, "该留言已删除");
                    }
                    msg.content = content;
                    if (pic != null)
                        msg.pic = ToolUtil.HexToByte(pic);
                    msg.Update("body");
                    msg.SavePic();

                    return new ResponceModel(200, "修改留言成功", msg);
                }
                else
                {
                    return new ResponceModel(403, "该留言不存在");
                }
            }
            else
            {
                return new ResponceModel(403, "留言内容为空");
            }
        }
        public static ResponceModel Delete(string username, int id)
        {
            MsgUni msg = new MsgUni(id);
            if (msg.TryQuery())
            {
                UserInfoSql userInfo = new UserInfoSql(username);
                userInfo.TryQuery();
                bool isdelete = false;
                string remsg = "";
                if (msg.username == username)
                {
                    isdelete = true;
                    remsg = "删除留言成功";
                    //return new ResponceModel(200, "删除留言成功");
                }
                // 糟糕，该用户具有管理员管理员权限
                else if (userInfo.permission_msgboard > 0)
                {
                    // TODO 通知原用户已被删除。
                    UserInfoSql userInfoowner = new UserInfoSql(msg.username);
                    userInfoowner.TryQuery();

                    // 这个要看谁是爸爸。
                    if (userInfoowner.permission_msgboard < userInfo.permission_msgboard)
                    {
                        isdelete = true;
                        remsg = "删除留言成功";

                        //return new ResponceModel(200,"")
                    }
                    else
                    {
                        isdelete = false;
                        remsg = "用户权限不够";
                    }
                }

                if (isdelete)
                {
                    if (msg.mark == 0)
                    {
                        msg.mark = 2;
                        // Mark: 这同时也会更新时间。
                        msg.Update("mark");
                    }

                    return new ResponceModel(200, remsg);
                }
                else
                {
                    return new ResponceModel(403, remsg);
                }
            }
            else
            {
                return new ResponceModel(403, "该留言不存在");
            }
        }
        public static ResponceModel DeleteCommnet(string username, int id, int subid)
        {
            MsgUni msg = new MsgUni(id);
            if (msg.TryQuery())
            {
                UserInfoSql userInfo = new UserInfoSql(username);
                userInfo.TryQuery();

                // Mark: 找到该评论，并删除。
                var comments = from item in msg.comments where item.id == subid select item;

                if (comments == null || comments.Count() == 0)
                {
                    return new ResponceModel(403, "该评论不存在");
                }
                else
                {
                    // Mark: 获取该评论
                    var comment = comments.First();

                    bool isdelete = false;
                    string remsg = "";
                    if (comment.username == username)
                    {
                        isdelete = true;
                        remsg = "删除评论成功";
                        //return new ResponceModel(200, "删除留言成功");
                    }
                    // 糟糕，该用户具有管理员管理员权限
                    else if (userInfo.permission_msgboard > 0)
                    {
                        // TODO 通知原用户已被删除。
                        UserInfoSql userInfoowner = new UserInfoSql(comment.username);
                        userInfoowner.TryQuery();

                        // 这个要看谁是爸爸。评论发布者和尝试删除者进行PK。
                        if (userInfoowner.permission_msgboard < userInfo.permission_msgboard)
                        {
                            isdelete = true;
                            remsg = "删除评论成功";

                            //return new ResponceModel(200,"")
                        }
                        else
                        {
                            isdelete = false;
                            remsg = "用户权限不够";
                        }
                    }

                    if (isdelete)
                    {
                        var newcomments = msg.comments.Except(comments);
                        msg.comments = newcomments.ToArray();
                        msg.updatetime = DateTime.Now.ToString();

                        msg.Update("comments");

                        return new ResponceModel(200, "删除评论成功", msg);
                    }
                    else
                    {
                        return new ResponceModel(403, remsg);
                    }
                }
            }
            else
            {
                return new ResponceModel(403, "该留言不存在");
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
