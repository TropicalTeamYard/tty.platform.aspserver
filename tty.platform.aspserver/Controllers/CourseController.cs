using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tty.Configs;
using tty.Model;
using tty.Util;

namespace tty.Controllers
{
    [Produces("application/json")]
    [Route("api/course")]
    public class CourseController : Controller
    {
        [HttpGet]
        public JsonResult Get(string credit,int year,int term)
        {
#if DEBUG
            return Post(credit,year,term);
#else
            return ResponceModel.GetInstanceBaned();
#endif
        }

        public JsonResult Post(string credit,int year,int term)
        {
            if (credit == null)
            {
                return ResponceModel.GetInstanceInvalid();
            }
            else
            {
                return Course.GetCourse(credit,year,term);
            }
        }

    }
}