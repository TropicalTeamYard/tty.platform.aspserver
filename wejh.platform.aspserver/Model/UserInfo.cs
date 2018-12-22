using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using wejh.Configs;
using wejh.Util;

namespace wejh.Model
{
    public class UserInfoSql : IMySqlQueryable
    {
        public UserInfoSql(string username, string pwbind_lib = "", string pwbind_card = "", string pwbind_ycedu = "", string pwbind_zfedu = "")
        {
            this.username = username;
            this.pwbind_lib = pwbind_lib;
            this.pwbind_card = pwbind_card;
            this.pwbind_ycedu = pwbind_ycedu;
            this.pwbind_zfedu = pwbind_zfedu;
        }

        public UserInfoSql()
        {
        }
        public UserInfoSql(DataRow row) => ((IMySqlQueryable)this).Set(row);

        public int id { get; set; }
        public string username { get; set; }
        public string pwbind_lib { get; set; } = "";
        public string pwbind_card { get; set; } = "";
        public string pwbind_ycedu { get; set; } = "";
        public string pwbind_zfedu { get; set; } = "";
        public string email { get; set; } = "";
        public string phone { get; set; } = "";

        public List<string> linkedcourse { get; set; } = new List<string>();

        void IMySqlQueryable.Set(DataRow row)
        {
            id = (int)row[nameof(id)];
            username = (string)row[nameof(username)];
            pwbind_lib = (string)row[nameof(pwbind_lib)];
            pwbind_card = (string)row[nameof(pwbind_card)];
            pwbind_ycedu = (string)row[nameof(pwbind_ycedu)];
            pwbind_zfedu = (string)row[nameof(pwbind_zfedu)];
            email = (string)row[nameof(email)];
            phone = (string)row[nameof(phone)];
        }
        string IMySqlQueryable.GetAddcommand()
        {
            return $"insert into {Config.UserInfoTable} (username,pwbind_lib,pwbind_card,pwbind_ycedu,pwbind_zfedu,email,phone,linkedcourse) values ('{username}','{pwbind_lib}','{pwbind_card}','{pwbind_ycedu}','{pwbind_zfedu}','{email}','{phone}','{ToolUtil.JoinString('|',linkedcourse)}')";
        }
        string IMySqlQueryable.GetQuerycommand()
        {
            return $"select * from {Config.UserInfoTable} where username like '{username}'";
        }

        public void UpdatePwbind_zfedu()
        {
            var cmd = $"update {Config.UserInfoTable} set pwbind_lib='{pwbind_zfedu}' where username like '{username}'";
            MySqlUtil.Execute(cmd);
        }
        public void UpdateLinkedcourse()
        {
            var cmd = $"update {Config.UserInfoTable} set linkedcourse='{ToolUtil.JoinString('|', linkedcourse)}' where username like '{username}'";
            MySqlUtil.Execute(cmd);
        }
    }

    public static class UserInfo
    {
        /// <summary>
        /// 绑定密码，这需要依赖<see cref="UserCredit"/>和<see cref="Course"/>。
        /// </summary>
        /// <param name="credit"></param>
        /// <param name="bindname"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static ResponceModel PwBind(string credit, string bindname, string password)
        {
            try
            {
                if (UserCredit.CheckUser(credit, out string username))
                {
                    //TODO
                    if (bindname == "lib")
                    {
                        return new ResponceModel(500, "图书馆绑定正在开发中。");
                    }
                    else if (bindname == "card")
                    {
                        return new ResponceModel(500, "校园网绑定正在开放中。");
                    }
                    else if (bindname == "ycedu")
                    {
                        return new ResponceModel(500, "原创教务绑定正在开发中。");
                    }
                    else if (bindname == "zfedu")
                    {
                        if (Course.GetZfCourse(username, password).code == 200)
                        {
                            UserInfoSql userInfoSql = new UserInfoSql(username, pwbind_lib: password);
                            if (userInfoSql.Exists())
                            {
                                userInfoSql.UpdatePwbind_zfedu();
                            }
                            else
                            {
                                userInfoSql.Add();
                            }

                            return new ResponceModel(200, "绑定正方成功。");
                        }
                        else
                        {
                            return new ResponceModel(403, "绑定正方失败。");
                        }
                    }
                    else
                    {
                        return new ResponceModel(400, "绑定类型发生错误。");
                    }
                }
                else
                {
                    return new ResponceModel(403, "自动登录已失效，请重新登录。");
                }
            }
            catch (Exception ex)
            {
                return ResponceModel.GetInstanceError(ex);
            }
        }

    }
}
