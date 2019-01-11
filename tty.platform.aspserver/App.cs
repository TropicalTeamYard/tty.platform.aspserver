using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tty
{
    public class App
    {
        public App(IConfiguration configuration)
        {
            Configuration = new Configuration(configuration);

            Current = this;
        }

        public static App Current { get; private set; }
        public Configuration Configuration { get; }


    }
}
