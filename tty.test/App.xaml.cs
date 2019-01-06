using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using tty.interactive;
using tty.interactive.Data;

namespace tty.test
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static new App Current => (App)Application.Current;

        public App()
        {
            InterAct = new InterAct();
            InterAct.Load();
            this.Exit += App_Exit;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            InterAct.Save();
        }

        public InterAct InterAct { get; } 
        public MainWindow Window { get; set; }
    }
}
