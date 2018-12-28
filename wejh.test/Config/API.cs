using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wejh.test.Config
{
    public static class API
    {
        private static Dictionary<string, string> apiDomain = new Dictionary<string, string>()
        {
            { "test", "http://10.128.18.163/"},
            { "debug", "http:localhost:64208/"}
        };

        private static string domain => apiDomain["test"];

        private static Dictionary<APIKey, string> apiMap = new Dictionary<APIKey, string>()
        {
            { APIKey.User,"api/user"},
            { APIKey.Time,"api/time"},
            { APIKey.GetInfo,"api/getinfo"},
            { APIKey.Course,"api/course"},
        };

        public static string GetAPI(APIKey key)
        {
            return domain + apiMap[key];
        }
    }

    public enum APIKey
    {
        User,
        Time,
        GetInfo,
        Course
    }
}
