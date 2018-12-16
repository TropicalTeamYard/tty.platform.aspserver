using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wejh.Configs
{
    public static class API
    {
        public static Dictionary<APIKey, string> apiMap = new Dictionary<APIKey, string>() {
            { APIKey.JhUser,"http://user.jh.zjut.edu.cn/api.php"}
        };

        public static string GetAPI(APIKey key) => apiMap[key];
    }

    public enum APIKey
    {
        JhUser,
    }
}
