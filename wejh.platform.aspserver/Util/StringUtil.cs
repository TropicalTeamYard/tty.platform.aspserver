using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wejh.Util
{
    public static class StringUtil
    {
        public static string GetNewToken()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

    }
}
