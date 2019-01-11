using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tty.interactive.Model
{
    internal class ResponceModel<T>
    {
        public ResponceModel(int code, string msg, T data = default(T))
        {
            this.code = code;
            this.msg = msg;
            this.data = data;
        }
        public int code { get; set; }
        public string msg { get; set; }
        public T data { get; set; }

    }

    internal class _ResponceModel : ResponceModel<object>
    {
        public _ResponceModel(int code, string msg, object data = null) : base(code, msg, data)
        {
        }
    }
}
