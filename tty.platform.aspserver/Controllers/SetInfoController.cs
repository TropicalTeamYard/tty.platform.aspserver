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
    [Route("api/setinfo")]
    public class SetInfoController : Controller
    {
        [HttpGet]
        public JsonResult Get(string credit, string email, string portrait, string phone)
        {
#if DEBUG
            return Post(credit,email,portrait,phone);
#else
            return ResponceModel.GetInstanceBaned();
#endif
        }

        [HttpPost]
        public JsonResult Post(string credit,string email,string portrait,string phone)
        {
            return UserInfo.SetInfoControl(credit,email,portrait,phone);
        }
    }
}