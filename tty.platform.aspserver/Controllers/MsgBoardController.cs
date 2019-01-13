using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tty.Model;

namespace tty.platform.aspserver.Controllers
{
    [Produces("application/json")]
    [Route("api/msgboard")]
    public class MsgBoardController : Controller
    {
        [HttpGet]
        public JsonResult Get(string method,string credit,int? id,string time,string content,string pic)
        {

#if DEBUG
            return Post(method,credit,id,time,content,pic);
#else
            return ResponceModel.GetInstanceBaned();
#endif
        }

        public JsonResult Post(string method, string credit,int? id, string time, string content,string pic)
        {
            if (credit == null)
            {
                return ResponceModel.GetInstanceInvalid();
            }
            else
            {
                return MsgBoard.Control(method, credit,id, time, content, pic);
            }
        }
    }
}