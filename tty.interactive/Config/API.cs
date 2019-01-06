using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tty.interactive.Config
{

    public sealed class API
    {
        private Dictionary<string, string> apiDomain = new Dictionary<string, string>()
        {
            { "test", "http://10.128.18.163/"},
            { "debug", "http:localhost:64208/"}
        };

        private string domain => apiDomain["test"];

        private Dictionary<APIKey, string> apiMap = new Dictionary<APIKey, string>()
        {
            { APIKey.User,"api/user"},
            { APIKey.Time,"api/time"},
            { APIKey.GetInfo,"api/getinfo"},
            { APIKey.Course,"api/course"},
        };

        public string this[APIKey key]
        {
            get => domain + apiMap[key];
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
