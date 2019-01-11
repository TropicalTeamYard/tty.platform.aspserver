using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace tty.interactive.Util
{
    public static class ToolUtil
    {
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

        public static byte[] BitmapImageToBytes(BitmapImage bitmap)
        {
            byte[] ByteArray = null;
            try
            {
                Stream stream = bitmap.StreamSource;
                if (stream != null && stream.Length > 0)
                {
                    stream.Position = 0;
                    using (BinaryReader br = new BinaryReader(stream))
                    {
                        ByteArray = br.ReadBytes((int)stream.Length);
                    }
                }
                else if (bitmap.UriSource != null)
                {
                    ByteArray = File.ReadAllBytes(bitmap.UriSource.LocalPath);
                }
            }
            catch
            {

                return null;
            }
            return ByteArray;
        }

        public static BitmapImage BytesToBitmapImage(byte[] byteArray)
        {
            BitmapImage bmp = null;
            try
            {
                bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = new MemoryStream(byteArray);
                bmp.EndInit();
            }
            catch
            {
                bmp = null;
            }
            return bmp;
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
