using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tty.interactive.Model
{
    internal class _UserCredit
    {
        public _UserCredit(string username, string nickname, string credit)
        {
            this.username = username;
            this.nickname = nickname;
            this.credit = credit;
        }

        public string username { get; set; }
        public string nickname { get; set; }
        public string credit { get; set; }
    }
}
