using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tty.Configs
{
    public class TableMap
    {
        public TableMap()
        {
        }

        private static Dictionary<TableKey, string> tableMap = new Dictionary<TableKey, string>() {
            { TableKey.UserCredit,"usercredit"},
            { TableKey.UserInfo,"userinfo" },
            { TableKey.Course,"course" },
            { TableKey.MsgBoard,"msgboard" }
        };

        public string this[TableKey index] => tableMap[index];
    }

    public enum TableKey
    {
        UserCredit,
        UserInfo,
        Course,
        MsgBoard
    }
}
