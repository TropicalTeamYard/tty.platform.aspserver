using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tty
{
    public class Token
    {
        private static Util.AesAddin aesAddin = new Util.AesAddin("LAVERALSTARDANDMDSFEA9200345IEVS");
        public static Token Decrypt(string token)
        {
            return JsonConvert.DeserializeObject<Token>(aesAddin.Decrypt(Encoding.UTF8.GetString(Util.ToolUtil.HexToByte(token))));
            //var arr = aesAddin.Decrypt(Encoding.UTF8.GetString( Util.ToolUtil.HexToByte(token))).Split(' ');
            //return new Token() {
            //    Username=arr[0],
            //    Password = arr[1],
            //    Time = DateTime.Parse(arr[2]),
            //    DeviceType = arr[3],
            //    OpenId = int.Parse(arr[4])
            //};
        }

        public override string ToString()
        {
            return Util.ToolUtil.BytesToHex(Encoding.UTF8.GetBytes(aesAddin.Encrypt(JsonConvert.SerializeObject(this))));
            
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime Time { get; set; }
        public string DeviceType { get; set; }
        public int OpenId { get; set; }
    }
}
