using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using wejh.Model;

namespace wejh.Util
{
    public static class ToolUtil
    {
        public static string GetNewToken()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public static string MD5Encrypt32(string password)
        {
            string cl = password;
            //string pwd = "";
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in s)
            {
                stringBuilder.AppendFormat("{0:X2}", item);
            }
            

            return stringBuilder.ToString();
        }
    }
}