using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wejh.Model
{
    public class ResponceModel
    {
        public ResponceModel(int code, string msg, object data = null)
        {
            this.code = code;
            this.msg = msg;
            this.data = data;
        }
        public int code { get; set; }
        public string msg { get; set; }
        public object data { get; set; }
    }
}
