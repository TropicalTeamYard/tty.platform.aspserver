using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using wejh.Configs;

namespace wejh.Model
{
    public class UserInfoSql:ISqlQueryable
    {
        public UserInfoSql(string username, string pwbind_lib = "", string pwbind_card ="", string pwbind_ycedu ="", string pwbind_zfedu="")
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

        public int id { get; set; }
        public string username { get; set; }
        public string pwbind_lib { get; set; } = "";
        public string pwbind_card { get; set; } = "";
        public string pwbind_ycedu { get; set; } = "";
        public string pwbind_zfedu { get; set; } = "";
        public string email { get; set; } = "";
        public string phone { get; set; } = "";

        public List<string> linkedcourse { get; set; }
        string ISqlQueryable.GetAddcommand()
        {
            return $"insert into {Config.UserInfoTable} (username,pwbind_lib,pwbind_card,pwbind_ycedu,pwbind_zfedu,email,phone,linkedcourse) values ('{username}','{pwbind_lib}','{pwbind_card}','{pwbind_ycedu}','{pwbind_zfedu}','{email}','{phone}','{string.Join('|',linkedcourse)}')";
        }
        string ISqlQueryable.GetQuerycommand()
        {
            return $"select * from {Config.UserInfoTable} where username like '{username}'";
        }
        public string GetUpdatepwbind_zfeducommand()
        {
            return $"update {Config.UserInfoTable} set pwbind_lib='{pwbind_zfedu}' where username like '{username}'";
        }
        public string GetUpdatelinkedcoursecommand()
        {
            return $"update {Config.UserInfoTable} set linkedcourse='{string.Join('|', linkedcourse)}' where username like '{username}'";
        }
        public static UserInfoSql FromDataRow(DataRow row)
        {
            return new UserInfoSql()
            {
                id = (int)row[nameof(id)],
                username = (string)row[nameof(username)],
                pwbind_lib = (string)row[nameof(pwbind_lib)],
                pwbind_card = (string)row[nameof(pwbind_card)],
                pwbind_ycedu = (string)row[nameof(pwbind_ycedu)],
                pwbind_zfedu = (string)row[nameof(pwbind_zfedu)],
                email = (string)row[nameof(email)],
                phone = (string)row[nameof(phone)]
            };
        }

    }
}
