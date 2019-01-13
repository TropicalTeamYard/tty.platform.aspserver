using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using tty.Configs;
using tty.Util;

namespace tty.Model
{
    public enum UserType
    {
        /// <summary>
        /// 普通用户
        /// </summary>
        COMMON = 2,
        /// <summary>
        /// 测试账户
        /// </summary>
        TEST = 2048,
        /// <summary>
        /// 用于和微精弘唯一绑定的账户
        /// </summary>
        WEJH = 1024,
    }
    public abstract class UserCreditModel
    {
        public UserCreditModel()
        {
        }

        public UserCreditModel(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        [SqlElement]
        [SqlSearchKey]
        public string username { get; set; }
        [SqlElement]
        [SqlBinding("nickname")]
        [SqlEncrypt]
        public string nickname { get; set; }
        [SqlElement]
        [SqlBinding("password")]
        [SqlEncrypt]
        public string password { get; set; }
    }
    public class UserCreditSql : UserCreditModel, ISqlObject
    {
        public UserCreditSql()
        {
        }
        public UserCreditSql(string username)
        {
            this.username = username;
        }
        public UserCreditSql(string username, string nickname, string password, UserType userType)
        {
            this.username = username;
            this.nickname = nickname;
            this.password = password;
            UserType = userType;
        }

        [SqlElement]
        [SqlBinding("web")]
        [SqlEncrypt]
        public string web_credit { get; set; } = "";
        [SqlElement]
        [SqlBinding("mobile")]
        [SqlEncrypt]
        public string mobile_credit { get; set; } = "";
        [SqlElement]
        [SqlBinding("pc")]
        [SqlEncrypt]
        public string pc_credit { get; set; } = "";
        [SqlElement]
        [SqlBinding("usertype")]
        public string usertype { get; set; } = "COMMON";


        public UserType UserType
        {
            get => Enum.Parse<UserType>(usertype);
            set => usertype = value.ToString();
        }
        SqlBaseProvider ISqlObject.SqlProvider => App.Current.Configuration.MySqlProvider; 
        string ISqlObject.Table => App.Current.Configuration.TableMap[TableKey.UserCredit]; 

        public void UpdateUserType() => this.Update("usertype");
        public void UpdateNickName() => this.Update("nickname");
        public void UpdatePassword() => this.Update("password");
        public void UpdateWeb() => this.Update("web");
        public void UpdateMobile() => this.Update("mobile");
        public void UpdatePc() => this.Update("pc");
        public bool TryQuery(string credit, out string devicetype)
        {
            //SOLVED BUG 查询名出错，导致程序无法检查凭证是否有效。
            if (SqlExtension.TryQuery<UserCreditSql>("mobile_credit", credit, out var result))
            {
                devicetype = "mobile";
                this.SetValue(result[0]);
                return true;
            }
            else if (SqlExtension.TryQuery<UserCreditSql>("pc_credit", credit, out var result2))
            {
                devicetype = "pc";
                //SOLVED BUG 使用result导致程序无法返回正确结果。
                this.SetValue(result2[0]);
                return true;
            }
            else
            {
                devicetype = null;
                return false;
            }
        }

        public UserCreditResult ToUserResultMobile()
        {
            return new UserCreditResult(username, nickname, mobile_credit, usertype);
        }
        public UserCreditResult ToUserResultPc()
        {
            return new UserCreditResult(username, nickname, pc_credit, usertype);
        }
    }
    public class UserCreditResult
    {
        public UserCreditResult(string username, string nickname, string credit, string usertype)
        {
            this.username = username;
            this.nickname = nickname;
            this.credit = credit;
            this.usertype = usertype;
        }

        public string username { get; set; }
        public string nickname { get; set; }
        public string credit { get; set; }
        public string usertype { get; set; }
    }

    /// <summary>
    /// 处理与用户凭证相关的信息，包括[登录]、[自动登录]、[注册]、[改密码]、[改昵称]
    /// Link::<see cref="Controllers.UserController"/>
    /// </summary>
    public static class UserCredit
    {
        internal static ResponceModel Control(string method, string username = "", string password = "", string nickname = "", string devicetype = "", string newpassword = "", string credit = "")
        {
            try
            {
                if (method == "register")
                {
                    if (username == null || password == null || nickname == null)
                    {
                        return ResponceModel.GetInstanceInvalid();
                    }
                    else
                    {
                        return Register(username, password, nickname);
                    }
                }
                else if (method == "register2")
                {
                    if (password == null || nickname == null)
                    {
                        return ResponceModel.GetInstanceInvalid();
                    }
                    else
                    {
                        return Register2(password, nickname);
                    }
                }
                //else if (method == "wejhlogin")
                //{
                //    if (username == null || password == null || devicetype == null)
                //    {
                //        return ResponceModel.GetInstanceInvalid();
                //    }
                //    else
                //    {
                //        return WejhLogin(username, password, devicetype);
                //    }
                //}
                else if (method == "login")
                {
                    if (username == null || password == null || devicetype == null)
                    {
                        return ResponceModel.GetInstanceInvalid();
                    }
                    else
                    {
                        return Login(username, password, devicetype);
                    }
                }
                else if (method == "autologin")
                {
                    if (credit == null)
                    {
                        return ResponceModel.GetInstanceInvalid();
                    }
                    else
                    {
                        return AutoLogin(credit);
                    }
                }
                else if (method == "changepw")
                {
                    if (username == null || password == null || newpassword == null)
                    {
                        return ResponceModel.GetInstanceInvalid();
                    }
                    else
                    {
                        return ChangePw(username, password, newpassword);
                    }
                }
                else if (method == "changenickname")
                {
                    if (credit == null || nickname == null)
                    {
                        return ResponceModel.GetInstanceInvalid();
                    }
                    else
                    {
                        return ChangeNickName(credit, nickname);
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
        private static ResponceModel Register(string username, string password, string nickname)
        {
            //检查输入，此部分需要在本地先进行检查。
            if (username == "" || password == "" || nickname == "")
            {
                return new ResponceModel(403, "用户名，密码，或者昵称为空");
            }
            else if (!CheckUtil.Username(username, UserType.COMMON))
            {
                return new ResponceModel(403, "用户名不符合命名规则，用户名应不包含英文特殊字符，长度在2~10位，且不能为纯数字。");
            }
            else if (!CheckUtil.Password(password))
            {
                return new ResponceModel(403, "密码太长或太短。");
            }
            else if (!CheckUtil.Nickname(nickname))
            {
                return new ResponceModel(403, "昵称不符合命名规则，昵称长度应该在2~15位。");
            }
            UserCreditSql user = new UserCreditSql(username, nickname, password, UserType.COMMON);
            //-----此处说明该username已经创建-----
            if (user.TryQuery())
            {
                return new ResponceModel(403, "该账号已存在。");
            }
            else
            {
                user.Add();
                return new ResponceModel(200, "注册账户成功");
            }

        }
        private static ResponceModel Register2(string password, string nickname)
        {
            if (password == "" || nickname == "")
            {
                return new ResponceModel(403, "密码/昵称为空");
            }
            else if (!CheckUtil.Password(password))
            {
                return new ResponceModel(403, "密码需要经过加密");
            }
            else if (!CheckUtil.Nickname(nickname))
            {
                return new ResponceModel(403, "昵称太长或太短");
            }
            UserCreditSql user = new UserCreditSql("10086", nickname, password, UserType.COMMON);
            UserCreditSql userlast = SqlExtension.GetLastRecord<UserCreditSql>();

            #region 按用户名自增顺序生成id.
            long id = 10086;
            if (userlast != null)
            {
                if (long.TryParse(userlast.username, out long id2))
                {
                    id = id2 + 1;
                }

                user.username = id.ToString();
                if (user.Exists())
                {
                    id--;
                    while (true)
                    {
                        id++;
                        userlast.username = id.ToString();
                        if (!SqlExtension.Exists(userlast))
                        {
                            break;
                        }
                    }
                }
            }

            user.username = id.ToString();
            user.Add(); 
            #endregion

            return new ResponceModel(200, "注册账户成功", new
            {
                user.username,
                user.nickname,
                user.usertype
            });
        }

        /// <summary>
        /// 为了兼容微精弘添加的快速绑定方法。
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="nickname"></param>
        /// <returns></returns>
        [Obsolete]
        private static ResponceModel WejhLogin(string username, string password, string devicetype)
        {
            if (username != "" && password != "")
            {
                UserCreditSql user = new UserCreditSql(username);
                //成功登录
                var result = JhUser.CheckUser(username, password);
                var data = (JhUserData)result.data;
                if (result.code == 200)
                {
                    //登录成功，自动更新密码信息。
                    user.password = password;
                    user.UserType = UserType.WEJH;
                    user.nickname = "wejh";
                    if (user.Exists())
                    {
                        user.UpdatePassword();
                        user.UpdateUserType();
                    }
                    else
                    {
                        user.Add();
                    }
                    //UserInfoSql userInfo = new UserInfoSql(username);
                    //MARK 从BindJH实现绑定。
                    UserInfo.BindJh(username, username, password);
                }
                return Login(username, password, devicetype);
            }
            else
            {
                return new ResponceModel(403, "用户名、密码或昵称为空");
            }

        }
        private static ResponceModel Login(string username, string password, string devicetype)
        {
            if (devicetype == "mobile" || devicetype == "pc")
            {
                if (username == "" || password == "")
                {
                    return new ResponceModel(403, "用户名或密码为空");
                }
                else
                {
                    //SOLVED BUG 曾导致无法依据键查询导致故障。
                    UserCreditSql user = new UserCreditSql(username);
                    if (user.TryQuery())
                    {
                        if (user.password == password)
                        {
                            if (devicetype == "mobile")
                            {
                                user.mobile_credit = ToolUtil.GetNewToken();
                                user.UpdateMobile();
                                return new ResponceModel(200, "登录成功", user.ToUserResultMobile());
                            }
                            else if (devicetype == "pc")
                            {
                                user.pc_credit = ToolUtil.GetNewToken();
                                user.UpdatePc();
                                return new ResponceModel(200, "登录成功", user.ToUserResultPc());
                            }
                            else
                            {
                                // Mark: This branch could never be raised.
                                // TODO 暂不支持网页端的操作。
                                return null;
                            }
                        }
                        else
                        {
                            return new ResponceModel(403, "用户密码错误");
                        }
                    }
                    else
                    {
                        return new ResponceModel(403, "该用户不存在");
                    }
                }
            }
            else
            {
                return new ResponceModel(403, "设备类型不符合");
            }
        }
        private static ResponceModel ChangePw(string username, string password, string newpassword)
        {
            if (username == "" || password == "" || newpassword == "")
            {
                return new ResponceModel(403, "用户名，旧密码或新密码为空");
            }
            else
            {
                UserCreditSql user = new UserCreditSql(username);
                if (user.TryQuery())
                {
                    if (user.UserType != UserType.WEJH)
                    {
                        if (user.password == password)
                        {
                            user.password = newpassword;
                            //-----修改密码后，所有自动登录方式都会失效-----
                            user.mobile_credit = "";
                            user.pc_credit = "";
                            //user.web_credit = "";
                            user.UpdatePassword();
                            user.UpdateMobile();
                            user.UpdatePc();
                            return new ResponceModel(200, "修改密码成功，请重新登录");
                        }
                        else
                        {
                            return new ResponceModel(403, "密码错误");
                        }
                    }
                    else
                    {
                        return new ResponceModel(403, "微精弘用户不能修改密码");
                    }
                }
                else
                {
                    return new ResponceModel(403, "该用户不存在");
                }
            }
        }
        private static ResponceModel ChangeNickName(string credit, string nickname)
        {
            if (credit == "" || nickname == "")
            {
                return new ResponceModel(403, "用户凭证或昵称为空");
            }
            //说明该用户存在。
            if (CheckUser(credit, out string username))
            {
                UserCreditSql user = new UserCreditSql(username);
                user.nickname = nickname;
                user.UpdateNickName();
                return new ResponceModel(200, "修改昵称成功");
            }
            else
            {
                return new ResponceModel(403, "自动登录已失效，请重新登录");
            }
        }
        private static ResponceModel AutoLogin(string credit)
        {
            if (credit == "")
            {
                return new ResponceModel(403, "用户凭证为空");
            }
            else
            {
                UserCreditSql user = new UserCreditSql();
                if (user.TryQuery(credit, out string devicetype))
                {
                    if (devicetype == "mobile")
                    {
                        user.mobile_credit = ToolUtil.GetNewToken();
                        user.UpdateMobile();

                        return new ResponceModel(200, "自动登录成功", user.ToUserResultMobile());
                    }
                    else
                    {
                        user.pc_credit = ToolUtil.GetNewToken();
                        user.UpdatePc();

                        return new ResponceModel(200, "自动登录成功", user.ToUserResultPc());
                    }
                }
                else
                {
                    // SOLVED BUG
                    return new ResponceModel(403, "自动登录已失效，请重新登录");
                }
            }
        }

        /// <summary>
        /// 本地检查用户凭证是否生效。
        /// </summary>
        /// <param name="credit"></param>
        /// <returns></returns>
        internal static bool CheckUser(string credit, out string username)
        {
            if (credit == null || credit == "")
            {
                username = null;
                return false;
            }
            else
            {
                //SOVLED BUG
                UserCreditSql userSql = new UserCreditSql();
                if (userSql.TryQuery(credit, out string devicetype))
                {
                    username = userSql.username;
                    return true;
                }
                else
                {
                    username = null;
                    return false;
                }

            }
        }
    }
}
