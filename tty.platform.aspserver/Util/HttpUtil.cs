using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using tty.Configs;

namespace tty.Util
{
    /// <summary>
    /// <see cref="HttpUtil"/>用于和http服务器进行交互。
    /// </summary>
    public static class HttpUtil
    {
        private static void SetHeaderValue(this HttpWebRequest request, string name, string value)
        {
            var header = request.Headers;
            var property = typeof(WebHeaderCollection).GetProperty("InnerCollection", BindingFlags.Instance | BindingFlags.NonPublic);
            if (property != null)
            {
                var collection = property.GetValue(header, null) as NameValueCollection;
                collection[name] = value;
            }
        }

        private static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                         source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }

        private static string get(string url, string paramters, string token = "")
        {
            string urlName = url + "?" + paramters;
            //ToolUtil realUrl = new ToolUtil(urlName);

            var request = (HttpWebRequest)WebRequest.Create(urlName);

            request.Method = "GET";

            if (token != null && token != "")
            {
                request.SetHeaderValue("Authorization", "Bearer " + token);
            }


            var responseString = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();

            return Unicode2String(responseString);
        }

        public static string get(APIKey key, string paramters, string token = "") => get(API.GetAPI(key), paramters, token);

        public static string post(string url, string postdata, string token = "")
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            //var _postData = postdata;
            var _postData = System.Web.HttpUtility.UrlEncode(postdata, Encoding.GetEncoding("UTF-8"));

            var data = Encoding.ASCII.GetBytes(_postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            if (token != null && token != "")
            {
                request.SetHeaderValue("Authorization", "Bearer " + token);
            }

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            return Unicode2String(new StreamReader(response.GetResponseStream()).ReadToEnd());
        }
    }

}
