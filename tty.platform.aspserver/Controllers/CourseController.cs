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
        public JsonResult Get(string credit)
        {
#if DEBUG
            return Post(credit);
#else
            return ResponceModel.GetInstanceBaned();
#endif
        }

        public JsonResult Post(string credit)
        {
            if (credit == null)
            {
                return ResponceModel.GetInstanceInvalid();
            }
            else
            {
                return Course.GetCourse(credit);
            }
        }

    }
}