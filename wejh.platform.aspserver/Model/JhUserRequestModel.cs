using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wejh.Model
{
    public class JhUserRequestModel
    {
        public JhUserRequestModel()
        {
        }

        public JhUserRequestModel(string app, string action)
        {
            this.app = app;
            this.action = action;
        }
        public JhUserRequestModel(string app, string action, string passport, string password)
        {
            this.app = app;
            this.action = action;
            this.passport = passport;
            this.password = password;
        }
        public JhUserRequestModel(UserModel user, string app, string action):this(app,action,user.username,user.password)
        {

        }

        public string app { get; set; }
        public string action { get; set; }
        public string passport { get; set; }
        public string password { get; set; }

        public string ToParameters()
        {
            return string.Format("app={0}&action={1}&passport={2}&password={3}",app,action,passport,password);
        }
    }
}
