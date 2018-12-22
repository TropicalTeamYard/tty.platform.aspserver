using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using wejh.Configs;
using wejh.Util;

namespace wejh.Model
{
    public class UserInfoSql : ISqlObject
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

        [SqlElement]
        public int id { get; set; }
        [SqlElement]
        public string username { get; set; }
        [SqlElement]
        public string pwbind_lib { get; set; } = "";
        [SqlElement]
        public string pwbind_card { get; set; } = "";
        [SqlElement]
        public string pwbind_ycedu { get; set; } = "";
        [SqlElement][SqlBinding("pwbind_zfedu")]
        public string pwbind_zfedu { get; set; } = "";
        [SqlElement]
        public string email { get; set; } = "";
        [SqlElement]
        public string phone { get; set; } = "";
        [SqlElement][SqlBinding("linkedcourse")]
        public string linkedcourse { get; set; } = "";

        public List<string> Linkedcourse
        {
            get => ToolUtil.SplitString('|', linkedcourse);
            set => linkedcourse = ToolUtil.JoinString('|', value);
        }

        SqlBaseProvider ISqlObject.SqlProvider { get; } = Config.MySqlProvider;
        string ISqlObject.Table => Config.UserInfoTable;

        public void UpdatePwbind_ZfEdu() => this.Update("pwbind_zfedu");
        public void UpdateLinkedCourse() => this.Update("linkedcourse");
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
                            //SOLVED BUG pwbind_lib 曾导致绑定出错。
                            UserInfoSql userInfoSql = new UserInfoSql(username, pwbind_zfedu: password);
                            if (userInfoSql.Exists())
                            {
                                userInfoSql.UpdatePwbind_ZfEdu();
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
