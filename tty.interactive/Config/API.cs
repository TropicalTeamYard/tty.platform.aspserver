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
            { "test", "http://10.128.75.24/"},
            { "debug", "http:localhost:64208/"},
            { "product","http://39.108.120.239/" }

        };

        private string domain => apiDomain["product"];

        private Dictionary<APIKey, string> apiMap = new Dictionary<APIKey, string>()
        {
            { APIKey.User,"api/user"},
            { APIKey.Time,"api/time"},
            { APIKey.GetInfo,"api/getinfo"},
            { APIKey.Course,"api/course"},
            { APIKey.SetInfo,"api/setinfo" },
            { APIKey.MsgBoard,"api/msgboard"}
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
        Course,
        SetInfo,
        MsgBoard
    }
}
