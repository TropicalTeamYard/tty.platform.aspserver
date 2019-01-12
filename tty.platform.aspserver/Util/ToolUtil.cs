using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using tty.Model;

namespace tty.Util
{
    public static class ToolUtil
    {
        public static string GetNewToken()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
        public static string MD5Encrypt32(string data)
        {
            string cl = data;
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
        public static string JoinString(char seperator, IEnumerable<object> obj)
        {
            if (obj == null || obj.Count() == 0)
            {
                return "";
            }
            else
            {
                return string.Join(seperator, obj);
            }
        }
        public static List<string> SplitString(char seperator, string obj)
        {
            if (obj == null)
            {
                return new List<string>();
            }
            else
            {
                return obj.Split(seperator).ToList();
            }
        }

        public static string BytesToHex(byte[] data)
        {
            StringBuilder ret = new StringBuilder();
            foreach (byte b in data)
            {
                //{0:X2} 大写
                ret.AppendFormat("{0:x2}", b);
            }
            var hex = ret.ToString();

            return hex;
        }

        public static byte[] HexToByte(string hex)
        {
            var inputByteArray = new byte[hex.Length / 2];
            for (var x = 0; x < inputByteArray.Length; x++)
            {
                var i = Convert.ToInt32(hex.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }

            return inputByteArray;
        }
    }

}