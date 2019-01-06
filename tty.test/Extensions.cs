using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace tty.test
{
    public static class Extensions
    {
        public static void NavigateTo(this Frame obj, Type pageType)
        {
            Page page = (Page)Activator.CreateInstance(pageType);
            obj.Content = page;
        }
    }
}
