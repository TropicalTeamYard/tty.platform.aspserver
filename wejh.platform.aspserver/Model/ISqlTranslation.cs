using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wejh.Model
{
    public interface ISqlQueryable
    {
        string GetAddcommand();
        string GetQuerycommand();
    }
}
