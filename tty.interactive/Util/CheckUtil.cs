using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tty.interactive.Util
{
    public static class CheckUtil
    {
        public static bool Username(string username, out string msg)
        {
            if (long.TryParse(username, out long re))
            {
                msg = "你使用的是精弘账号";
                return true;
            }
            else if (username.Length >= 2 && username.Length <= 10)
            {
                string except = @"!@#$%^&*()_+-=,./\[]{}:;'<>?`~";
                bool flag = true;
                foreach (var item in except)
                {
                    if (username.Contains(item))
                    {
                        flag = false;
                        break;
                    }
                }

                if (flag == false)
                {
                    msg = "包含非法字符(英文特殊字符)";
                }
                else
                {
                    msg = "";
                }
                return flag;
            }
            else if(username.Length < 2)
            {
                msg = "账号过短";
                return false;
            }
            else
            {
                msg = "账号过长";
                return false;
            }
        }
        public static bool Password(string password)
        {
            if (password.Length >= 6 && password.Length <= 20)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool Nickname(string nickname)
        {
            if (nickname.Length >= 2 && nickname.Length <= 15)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
