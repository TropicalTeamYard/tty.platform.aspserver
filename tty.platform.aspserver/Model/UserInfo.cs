﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tty.Configs;
using tty.Util;

namespace tty.Model
{

    public class UserInfoSql : ISqlObject
    {
       
        public UserInfoSql(string username, string pwbind_lib = "", string pwbind_card = "", string pwbind_ycedu = "", string pwbind_zfedu = "", string jhpid = "", string pwbind_jh = "")
        {
            this.username = username;
            this.pwbind_lib = pwbind_lib;
            this.pwbind_card = pwbind_card;
            this.pwbind_ycedu = pwbind_ycedu;
            this.pwbind_zfedu = pwbind_zfedu;
            this.jhpid = jhpid;
            this.pwbind_jh = pwbind_jh;
        }

        public UserInfoSql()
        {
        }

        [SqlElement]
        [SqlSearchKey]
        public string username { get; set; }
        [SqlElement]
        [SqlBinding("jh")]
        [SqlEncrypt]
        public string jhpid { get; set; } = "";
        [SqlElement]
        [SqlBinding("jh")]
        [SqlEncrypt]
        public string pwbind_jh { get; set; } = "";
        [SqlElement]
        [SqlBinding("jh")]
        public int state_jh { get; set; } = 0;
        [SqlElement]
        [SqlEncrypt]
        public string pwbind_lib { get; set; } = "";
        [SqlElement]
        public int state_lib { get; set; } = 0;
        [SqlElement]
        [SqlEncrypt]
        public string pwbind_card { get; set; } = "";
        [SqlElement]
        public int state_card { get; set; } = 0;
        [SqlElement]
        [SqlEncrypt]
        public string pwbind_ycedu { get; set; } = "";
        [SqlElement]
        public int state_ycedu { get; set; } = 0;
        [SqlElement]
        [SqlBinding("zfedu")]
        [SqlEncrypt]
        public string pwbind_zfedu { get; set; } = "";
        [SqlElement]
        [SqlBinding("zfedu")]
        public int state_zfedu { get; set; } = 0;
        //---------------这些是用户的基础信息---------------

        #region 图片太大，已经隐藏
        //[SqlElement]
        //[SqlBinding("portrait")]
        public byte[] portrait { get; set; }

        private string portriaitFileName => App.Current.Configuration.PortraitCache + $"\\{username}_128_128.jpg";
        #endregion
        [SqlElement]
        [SqlEncrypt]
        [SqlBinding("email")]
        public string email { get; set; } = "";
        [SqlElement]
        [SqlEncrypt]
        [SqlBinding("phone")]
        public string phone { get; set; } = "";
        //-------------------------------------------------

        //----------------这些是用户的扩展信息---------------
        /// <summary>
        /// msg权限,0为普通用户,1为管理员用户,2为超级管理员用户。
        /// </summary>
        [SqlElement]
        public int permission_msgboard { get; set; } = 0;
        /// <summary>
        /// 课表服务,'LOCAL':本地服务(自定义),'ZJUT':浙江工业大学课表服务.
        /// </summary>
        [SqlElement]
        public string courseserver { get; set; } = "NONE";
        //-------------------------------------------------
        [SqlElement]
        [SqlBinding("linkedcourse")]
        public string linkedcourse { get; set; } = "";
        

        public List<string> Linkedcourse
        {
            get => ToolUtil.SplitString('|', linkedcourse);
            set => linkedcourse = ToolUtil.JoinString('|', value);
        }

        SqlBaseProvider ISqlObject.SqlProvider => App.Current.Configuration.MySqlProvider; // Config.MySqlProvider;
        string ISqlObject.Table => App.Current.Configuration.TableMap[TableKey.UserInfo]; //Config.UserInfoTable;

        public void UpdateJh() => this.Update("jh");
        public void UpdateZfEdu() => this.Update("zfedu");
        public void UpdateLinkedCourse() => this.Update("linkedcourse");

        public void ReadPortrait()
        {
           
            if (File.Exists(portriaitFileName))
            {
                portrait = File.ReadAllBytes(portriaitFileName);
            }
        }
        public void SavePortrait()
        {
            if (!Directory.Exists(App.Current.Configuration .PortraitCache))
            {
                Directory.CreateDirectory(App.Current.Configuration.PortraitCache /*Config.PortraitCache*/);
            }

            File.WriteAllBytes(portriaitFileName, portrait);
        }
    }

    public class PwBindInfo
    {
        public PwBindInfo()
        {
        }
        public PwBindInfo(string bindname, string password, int state)
        {
            this.bindname = bindname;
            this.password = password;
            this.state = state;
        }
        public PwBindInfo(string bindname, string pid, string password, int state)
        {
            this.bindname = bindname;
            this.pid = pid;
            this.password = password;
            this.state = state;
        }

        public string bindname { get; set; }
        public string pid { get; set; }
        [JsonIgnore]
        public string password { get; set; }
        /// <summary>
        /// 0表示未绑定，1表示虽然已绑定，但绑定失效，2表示绑定成功。
        /// </summary>
        public int state { get; set; }
    }

    /// <summary>
    /// 处理与用户信息相关的信息
    /// Link::<see cref="Controllers.BindController"/>和<see cref="Controllers.GetInfoController"/>
    /// </summary>
    public static class UserInfo
    {
        /// <summary>
        /// 绑定控制，目前支持的为jh和zfedu. Source:<see cref="Controllers.BindController"/>
        /// </summary>
        /// <param name="credit"></param>
        /// <param name="bindname"></param>
        /// <param name="password"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        internal static ResponceModel BindControl(string credit, string bindname, string password, string pid)
        {
            try
            {
                if (credit == null || bindname == null || password == null)
                {
                    return ResponceModel.GetInstanceInvalid();
                }
                else
                {
                    //说明用户绑定成功。
                    if (UserCredit.CheckUser(credit, out string username))
                    {
                        if (bindname == "jh")
                        {
                            if (pid == null)
                            {
                                return ResponceModel.GetInstanceInvalid();
                            }
                            else
                            {
                                return BindJh(username, pid, password);
                            }
                        }
                        else if (bindname == "zfedu")
                        {
                            //依赖项为精弘账号
                            if (GetBindInfo(username, "jh").state != 0)
                            {
                                return BindZfEdu(username, password);
                            }
                            else
                            {
                                return new ResponceModel(403, "你还没有绑定精弘账号");
                            }
                        }
                        else if (bindname == "ycedu")
                        {
                            //TODO 原创绑定正在开发
                            return new ResponceModel(500, "原创绑定正在开发");
                        }
                        else if (bindname == "lib")
                        {
                            //TODO 图书馆绑定正在开发
                            return new ResponceModel(500, "图书馆绑定正在开发");
                        }
                        else if (bindname == "card")
                        {
                            //TODO 校园卡绑定正在开发
                            return new ResponceModel(500, "校园卡绑定正在开发");
                        }
                        else
                        {
                            return new ResponceModel(402, "绑定类型错误");
                        }
                    }
                    else
                    {
                        return new ResponceModel(403, "自动登录已失效，请重新登录");
                    }
                }
            }
            catch (Exception ex)
            {
                return ResponceModel.GetInstanceError(ex);
            }

        }
        /// <summary>
        /// 获取用户信息控制
        /// </summary>
        /// <param name="credit"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static ResponceModel GetInfoControl(string credit, string type)
        {
            if (credit == null || type == null)
            {
                return ResponceModel.GetInstanceInvalid();
            }
            else
            {
                if (UserCredit.CheckUser(credit, out string username))
                {
                    //获取用户的绑定信息。
                    if (type == "bind")
                    {
                        return GetBindInfo(username);
                    }
                    else if (type == "base")
                    {
                        return GetBaseInfo(username);
                    }
                    else
                    {
                        return new ResponceModel(403, "不存在该类型信息");
                    }
                }
                else
                {
                    return new ResponceModel(403, "自动登录已失效，请重新登录");
                }
            }
        }
        internal static ResponceModel SetInfoControl(string credit, string email, string portrait, string phone)
        {
            if (credit == null)
            {
                return ResponceModel.GetInstanceInvalid();
            }
            else
            {
                if (UserCredit.CheckUser(credit, out string username))
                {
                    int e_email = 0;
                    int e_portrait = 0;
                    int e_phone = 0;

                    if (email != null && email != "")
                    {
                        UserInfoSql userinfo = new UserInfoSql(username);

                        //TODO 发送邮箱验证
                        //说明曾经绑定过此邮箱。
                        if (CheckUtil.Email(email) && SqlExtension.TryQuery("email", email, out List<UserInfoSql> data))
                        {
                            userinfo.email = email;
                            userinfo.Update("email");
                            e_email = 2;
                        }
                        else
                        {
                            e_email = 1;
                        }
                    }
                    else if (portrait != null && portrait.Length > 0)
                    {
                        try
                        {
                            UserInfoSql userInfo = new UserInfoSql(username);
                            userInfo.portrait = ToolUtil.HexToByte(portrait) ;
                            userInfo.SavePortrait();

                            e_portrait = 2;
                        }
                        catch (Exception)
                        {
                            e_portrait = 1;
                        }
                    }
                    else if (phone != null && phone != "")
                    {
                    }

                    return new ResponceModel(200, "修改信息成功", new
                    {
                        e_email,
                        e_portrait,
                        e_phone
                    });
                }
                else
                {
                    return new ResponceModel(403, "自动登录已失效，请重新登录");
                }
            }
        }

        private static ResponceModel GetBindInfo(string username)
        {
            return new ResponceModel(200, "获取成功",
                new string[] { "jh", "lib", "zfedu", "ycedu", "card" }.Map((m) => GetBindInfo(username, m))
                );
        }
        internal static bool TryGetUserInfo(string username, bool isshared, out dynamic data)
        {
            UserInfoSql userInfo = new UserInfoSql(username);
            UserCreditSql user = new UserCreditSql(username);

            if (user.TryQuery())
            {
                userInfo.TryQuery();
                userInfo.ReadPortrait();


                string portrait = null;

                if (userInfo.portrait == null)
                {
                    portrait = App.Current.Configuration.DefaultPortrait;// Config.defaultportrait;
                }
                else
                {
                    portrait = ToolUtil.BytesToHex(userInfo.portrait);
                }

                if (!isshared)
                {
                    data = new //匿名类型
                    {
                        username = userInfo.username,
                        nickname = user.nickname,
                        usertype = user.usertype,
                        portrait,
                        email = userInfo.email,
                        phone = userInfo.phone,
                        userInfo.permission_msgboard
                    };
                }
                else
                {
                    data = new //匿名类型
                    {
                        username = userInfo.username,
                        nickname = user.nickname,
                        usertype = user.usertype,
                        portrait,
                    };
                }

                return true;
            }
            else
            {
                data = null;
                return false;
            }
        }

        /// <summary>
        /// 获取绑定信息
        /// </summary>
        /// <param name="username"></param>
        /// <param name="bindname"></param>
        /// <returns></returns>
        internal static PwBindInfo GetBindInfo(string username, string bindname)
        {
            //SOLVED BUG
            PwBindInfo pwBindInfo = new PwBindInfo() { bindname = bindname, pid = null, password = "", state = 0 };
            UserInfoSql userInfo = new UserInfoSql(username);
            if (userInfo.TryQuery())
            {
                if (bindname == "jh")
                {
                    pwBindInfo.pid = "";
                    if (userInfo.jhpid != null && userInfo.jhpid != "" &&
                        userInfo.pwbind_jh != null && userInfo.jhpid != "")
                    {
                        pwBindInfo.pid = userInfo.jhpid;
                        pwBindInfo.password = userInfo.pwbind_jh;
                        pwBindInfo.state = userInfo.state_jh;
                    }
                }
                else if (bindname == "lib")
                {
                    if (userInfo.pwbind_lib != null && userInfo.pwbind_lib != "")
                    {
                        pwBindInfo.password = userInfo.pwbind_lib;
                        pwBindInfo.state = userInfo.state_lib;
                    }
                }
                else if (bindname == "zfedu")
                {
                    if (userInfo.pwbind_zfedu != null && userInfo.pwbind_zfedu != "")
                    {
                        pwBindInfo.password = userInfo.pwbind_zfedu;
                        pwBindInfo.state = userInfo.state_zfedu;
                    }
                }
                else if (bindname == "ycedu")
                {
                    if (userInfo.pwbind_ycedu != null && userInfo.pwbind_ycedu != "")
                    {
                        pwBindInfo.password = userInfo.pwbind_ycedu;
                        pwBindInfo.state = userInfo.state_ycedu;
                    }
                }
                else if (bindname == "card")
                {
                    if (userInfo.pwbind_card != null && userInfo.pwbind_card != "")
                    {
                        pwBindInfo.password = userInfo.pwbind_card;
                        pwBindInfo.state = userInfo.state_card;
                    }
                }
            }
            return pwBindInfo;

        }
        private static ResponceModel GetBaseInfo(string username)
        {
            TryGetUserInfo(username, false, out dynamic data);
            return new ResponceModel(200, "获取成功",data);
        }

        /// <summary>
        /// 绑定精弘，需要依赖<see cref="JhUser"/>和<see cref="JhUserData"/>.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        internal static ResponceModel BindJh(string username, string pid, string password)
        {
            if (password == "" || pid == "")
            {
                return new ResponceModel(403, "学号或密码为空");
            }
            else
            {
                var result = JhUser.CheckUser(pid, password);
                var data = (JhUserData)result.data;
                UserInfoSql userInfo = new UserInfoSql(username);
                if (result.code == 200)
                {
                    userInfo.jhpid = pid;
                    userInfo.pwbind_jh = password;
                    userInfo.state_jh = 2;
                    userInfo.email = data.email;
                }
                //TODO 绑定失败时候现在暂时不做任何处理。
                if (userInfo.Exists())
                {
                    userInfo.UpdateJh();
                }
                else
                {
                    userInfo.Add();
                }
                return result;
            }
        }
        /// <summary>
        /// 绑定正方教务，需要依赖<see cref="Course"/>.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static ResponceModel BindZfEdu(string username, string password)
        {
            if (password == "")
            {
                return new ResponceModel(403, "密码为空");
            }
            else
            {
                UserInfoSql userInfo = new UserInfoSql(username);
                //MARK 说明数据库中一定有该条记录。
                if (userInfo.TryQuery())
                {
                    //需要验证精弘账号是否已经失效。
                    if (JhUser.CheckUser(userInfo.jhpid, userInfo.pwbind_jh).code == 200)
                    {
                        if (Course.GetZfCourse(userInfo.jhpid, password).code == 200)
                        {
                            userInfo.pwbind_zfedu = password;
                            userInfo.state_zfedu = 2;
                            userInfo.UpdateZfEdu();
                            return new ResponceModel(200, "绑定正方成功");
                        }
                        else
                        {
                            return new ResponceModel(403, "绑定正方失败");
                        }
                    }
                    else
                    {
                        userInfo.state_jh = 1;
                        userInfo.UpdateJh();
                        return new ResponceModel(403, "请重新绑定精弘账号");
                    }
                }
                else
                {
                    // MARK we could expect this couldn't be raised.
                    return new ResponceModel(403, "请重新绑定精弘账号");
                }
            }
        }
    }
}
