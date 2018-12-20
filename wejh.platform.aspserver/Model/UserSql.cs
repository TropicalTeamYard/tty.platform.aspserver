using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using wejh.Configs;

namespace wejh.Model
{
    public class UserSql:UserModel,ISqlQueryable
    {
        public UserSql()
        {
        }
        public UserSql(string username, int usertype,string password)
        {
            this.username = username;
            this.usertype = usertype;
            this.password = password;
        }
        public UserSql(JhUserAPIData user, string password):this(user.pid,user.type,password)
        {
        }

        public int id { get; set; }
        public int usertype { get; set; }
        public string mobile_name { get; set; }
        public string mobile_credit { get; set; }
        public string pc_name { get; set;}
        public string pc_credit { get; set; }

        string ISqlQueryable.GetAddcommand()
        {

            return $"insert into {Config.UserCreditTable}(username,usertype,password,mobile_name,mobile_credit,pc_name,pc_credit) values('{username}',{usertype},'{password}','{mobile_name}','{mobile_credit}','{pc_name}','{pc_credit}')";
        }
        /// <summary>
        /// 更新<see cref="UserModel.password"/>和<see cref="usertype"/>的信息.
        /// </summary>
        /// <returns></returns>
        public string GetUpdatecommandUser()
        {
            return $"update {Config.UserCreditTable} set password='{password}',usertype={usertype} where username='{username}'";
        }
        /// <summary>
        /// 更新<see cref="mobile_name"/>和<see cref="mobile_credit"/>的信息。
        /// </summary>
        /// <returns></returns>
        public string GetUpdatecommandMobile()
        {
            return $"update {Config.UserCreditTable} set mobile_name='{mobile_name}',mobile_credit='{mobile_credit}' where username like '{username}'";
        }
        public string GetUpdateCommandPc()
        {
            return $"update {Config.UserCreditTable} set pc_name='{pc_name}',pc_credit='{pc_credit}' where username like '{username}'";
        }
        string ISqlQueryable.GetQuerycommand()
        {
            return $"select * from {Config.UserCreditTable} where username like '{username}'";
        }
        public static string GetQuerycommandMobileCredit(string credit)
        {

            return $"select * from {Config.UserCreditTable} where mobile_credit like '{credit}'";
        }
        public static string GetQuerycommandPcCredit(string credit)
        {

            return $"select * from {Config.UserCreditTable} where pc_credit like '{credit}'";
        }

        public static UserSql FromDataRow(DataRow row)
        {
            return new UserSql()
            {
                id = (int)row[nameof(id)],
                username = (string)row[nameof(username)],
                usertype = (int)row[nameof(usertype)],
                password = (string)row[nameof(password)],
                mobile_name = (string)row[nameof(mobile_name)],
                mobile_credit = (string)row[nameof(mobile_credit)],
                pc_name = (string)row[nameof(pc_name)],
                pc_credit = (string)row[nameof(pc_credit)],
            };
        }
        public static UserSql Combine(JhUserAPIData user, string password)
        {
            return new UserSql(user.pid, user.type, password);
        }
        public UserResult ToUserResultMobile()
        {

            return new UserResult(username, usertype, mobile_credit,mobile_name);
        }
        public UserResult ToUserResultPc()
        {
            return new UserResult(username, usertype, pc_credit, pc_name);
        }
    }
}
