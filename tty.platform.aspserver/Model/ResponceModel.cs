using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tty.Model
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

        public static ResponceModel GetInstanceBaned()
        {
            return new ResponceModel(405, "Get方法已禁止。");
        }
        public static ResponceModel GetInstanceInvalid()
        {
            return new ResponceModel(400, "无效的访问。");
        }
        public static ResponceModel GetInstanceError(Exception ex)
        {
            return new ResponceModel(500, ex.Message + " " + ex.StackTrace);
        }

        public static implicit operator JsonResult(ResponceModel obj)
        {
            return new JsonResult(obj);
        }
    }
}
