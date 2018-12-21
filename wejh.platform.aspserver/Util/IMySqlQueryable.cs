using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace wejh.Util
{
    public interface IMySqlQueryable
    {
        void Set(DataRow row);
        string GetAddcommand();
        string GetQuerycommand();
    }
}
