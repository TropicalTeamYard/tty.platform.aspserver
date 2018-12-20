using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wejh.Configs
{
    public class API
    {
        public static Dictionary<APIKey, string> apiMap = new Dictionary<APIKey, string>() {
            { APIKey.JhUser,"http://user.jh.zjut.edu.cn/api.php"},
            { APIKey.ZjCourse,"http://api.jh.zjut.edu.cn/student/classZf.php" }
        };

        public static string GetAPI(APIKey key) => apiMap[key];
    }

    public enum APIKey
    {
        JhUser,
        ZjCourse,


    }

    
}
