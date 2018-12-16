using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wejh.Model
{
    public class UserResult
    {
        public UserResult(string username, int usertype, string credit,string devicename)
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
}
