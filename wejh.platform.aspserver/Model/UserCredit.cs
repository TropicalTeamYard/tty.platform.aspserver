using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using wejh.Configs;
using wejh.Util;

namespace wejh.Model
{
    public class UserCreditModel
    {
        public UserCreditModel()
        {
        }

        public UserCreditModel(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public string username { get; set; }
        public string password { get; set; }
    }
    public class UserCreditSql : UserCreditModel, IMySqlQueryable
    {
        public UserCreditSql()
        {
        }
        public UserCreditSql(string username, int usertype, string password)
        {
            this.username = username;
            this.usertype = usertype;
            this.password = password;
        }
        public UserCreditSql(JhUserData user, string password) : this(user.pid, int.Parse( user.type), password)
        {
        }
        private UserCreditSql(DataRow row) => ((IMySqlQueryable)this).Set(row);
        
        public int id { get; set; }
        public int usertype { get; set; }
        public string mobile_name { get; set; }
        public string mobile_credit { get; set; }
        public string pc_name { get; set; }
        public string pc_credit { get; set; }

        void IMySqlQueryable.Set(DataRow row)
        {
            id = (int)row[nameof(id)];
            username = (string)row[nameof(username)];
            usertype = (int)row[nameof(usertype)];
            password = (string)row[nameof(password)];
            mobile_name = (string)row[nameof(mobile_name)];
            mobile_credit = (string)row[nameof(mobile_credit)];
            pc_name = (string)row[nameof(pc_name)];
            pc_credit = (string)row[nameof(pc_credit)];
        }
        string IMySqlQueryable.GetAddcommand()
        {

            return $"insert into {Config.UserCreditTable}(username,usertype,password,mobile_name,mobile_credit,pc_name,pc_credit) values('{username}',{usertype},'{password}','{mobile_name}','{mobile_credit}','{pc_name}','{pc_credit}')";
        }
        string IMySqlQueryable.GetQuerycommand()
        {
            return $"select * from {Config.UserCreditTable} where username like '{username}'";
        }

        /// <summary>
        /// 更新<see cref="UserCreditModel.password"/>和<see cref="usertype"/>的信息.
        /// </summary>
        /// <returns></returns>
        public void UpdateUser()
        {
            var cmd = $"update {Config.UserCreditTable} set password='{password}',usertype={usertype} where username='{username}'";
            MySqlUtil.Execute(cmd);
        }
        /// <summary>
        /// 更新<see cref="mobile_name"/>和<see cref="mobile_credit"/>的信息。
        /// </summary>
        /// <returns></returns>
        public void UpdateMobile()
        {
            var cmd = $"update {Config.UserCreditTable} set mobile_name='{mobile_name}',mobile_credit='{mobile_credit}' where username like '{username}'";
            MySqlUtil.Execute(cmd);
        }
        public void UpdatePc()
        {
            var cmd = $"update {Config.UserCreditTable} set pc_name='{pc_name}',pc_credit='{pc_credit}' where username like '{username}'";
            MySqlUtil.Execute(cmd);
        }
        public bool TryQuery(string credit, out string devicetype)
        {
            var cmdmobile = $"select * from {Config.UserCreditTable} where mobile_credit like '{credit}'"; 
            var cmdpc = $"select * from {Config.UserCreditTable} where pc_credit like '{credit}'";
            if (MySqlUtil.TryQuery(cmdmobile,out DataTable table1))
            {
                devicetype = "mobile";
                ((IMySqlQueryable)this).Set(table1.Rows[0]);
                return true;
            }
            else if (MySqlUtil.TryQuery(cmdpc,out DataTable table2))
            {
                devicetype = "pc";
                ((IMySqlQueryable)this).Set(table2.Rows[0]);
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

            return new UserCreditResult(username, usertype, mobile_credit, mobile_name);
        }
        public UserCreditResult ToUserResultPc()
        {
            return new UserCreditResult(username, usertype, pc_credit, pc_name);
        }
    }
    public class UserCreditResult
    {
        public UserCreditResult(string username, int usertype, string credit, string devicename)
        {
            this.username = username;
            this.usertype = usertype;
            this.credit = credit;
            this.devicename = devicename;
        }

        public string username { get; set; }
        public int usertype { get; set; }
        public string credit { get; set; }
        public string devicename { get; set; }
    }

    public static class UserCredit
    {
        public static ResponceModel Login(string username, string password, string devicetype, string devicename)
        {
            try
            {
                if (devicetype == "pc" || devicetype == "mobile")
                {
                    if (username != "" && password != "")
                    {
                        //向精弘用户中心服务器验证登录
                        var result = JhUser.CheckUser(username, password);
                        //验证成功
                        if (result.code == 200)
                        {
                            UserCreditSql userSql = new UserCreditSql((JhUserData)result.data, password);
                            if (devicetype == "mobile")
                            {
                                userSql.mobile_name = devicename;
                                userSql.mobile_credit = ToolUtil.GetNewToken();

                                if (userSql.Exists())
                                {
                                    userSql.UpdateUser();
                                    userSql.UpdateMobile();
                                }
                                else
                                {
                                    userSql.Add();
                                }

                                return new ResponceModel(result.code, result.msg, userSql.ToUserResultMobile());
                            }
                            else
                            {
                                userSql.pc_name = devicename;
                                userSql.pc_credit = ToolUtil.GetNewToken();

                                if (userSql.Exists())
                                {
                                    userSql.UpdateUser() ;
                                    userSql.UpdatePc() ;
                                }
                                else
                                {
                                    userSql.Add();
                                }

                                return new ResponceModel(result.code, result.msg, userSql.ToUserResultPc());
                            }
                        }
                        else
                        {
                            return new ResponceModel(result.code, result.msg);
                        }
                    }
                    else
                    {
                        return new ResponceModel(403, "用户名或密码不能为空。");
                    }
                }
                else
                {
                    return new ResponceModel(403, "设备类型不符合。");
                }
            }
            catch (Exception ex)
            {
                return ResponceModel.GetInstanceError(ex);
            }
        }

        public static ResponceModel AutoLogin(string credit)
        {
            if (credit == null)
            {
                return ResponceModel.GetInstanceInvalid();
            }
            else
            {
                try
                {
                    UserCreditSql userSql = new UserCreditSql();
                    if (userSql.TryQuery(credit,out string devicetype))
                    {
                        var result = JhUser.CheckUser(userSql.username, userSql.password);
                        if (result.code == 200)
                        {
                            //移动端登录
                            if (devicetype == "mobile")
                            {
                                userSql.mobile_credit = ToolUtil.GetNewToken();
                                //更新数据库。
                                userSql.UpdateMobile();
                                return new ResponceModel(200, "自动登录成功。", userSql.ToUserResultMobile());
                            }
                            else
                            {
                                userSql.pc_credit = ToolUtil.GetNewToken();
                                //更新数据库。
                                userSql.UpdatePc();
                                return new ResponceModel(200, "自动登录成功。", userSql.ToUserResultPc());
                            }
                        }
                        else
                        {
                            return new ResponceModel(403, "自动登录已失效，请重新绑定账号");
                        }
                    }
                    else
                    {
                        return new ResponceModel(403, "自动登录失败。");
                    }
                }
                catch (Exception ex)
                {
                    return ResponceModel.GetInstanceError(ex);
                }
            }
        }



        /// <summary>
        /// 本地检查用户凭证是否生效。
        /// </summary>
        /// <param name="credit"></param>
        /// <returns></returns>
        public static bool CheckUser(string credit, out string username)
        {
            if (credit == null)
            {
                username = null;
                return false;
            }
            else
            {
                UserCreditSql userSql = new UserCreditSql();
                if (userSql.TryQuery(credit,out string devicetype))
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
