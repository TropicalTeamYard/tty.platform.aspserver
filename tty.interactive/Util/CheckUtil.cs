using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tty.interactive.Util
{
    public static class CheckUtil
    {
        public static bool Username(string username)
        {
            if (username.Length >= 5)
            {
                if (long.TryParse(username, out long re))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
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
