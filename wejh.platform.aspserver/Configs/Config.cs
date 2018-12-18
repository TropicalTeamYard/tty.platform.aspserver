
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wejh.Configs
{
    public static class Config
    {
        public static IConfiguration Configuration { get; set; }
        public static string Conn => Configuration.GetConnectionString("wejhplatform");
        public static string UserCreditTable => "usercredit";
        public static bool IsTest => true;
    }
}
