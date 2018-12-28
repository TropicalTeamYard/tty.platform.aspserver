using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using wejh.test.Data;

namespace wejh.test
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static DataCache Cache { get; set; }
        public static MainWindow Window { get; set; }
    }
}
