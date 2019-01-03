using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tty.Configs
{
    public class API
    {
        private static Dictionary<string, string> apiDomain = new Dictionary<string, string>()
        {
            { "user","http://user.jh.zjut.edu.cn"},
            { "api","http://api.jh.zjut.edu.cn/student"}
        };

        private static Dictionary<APIKey, string> apiMap = new Dictionary<APIKey, string>() {
            { APIKey.JhUser,"@user/api.php"},
            { APIKey.Zf_Score,"@api/scoresZf.php" },
            { APIKey.Zf_Course,"@api/classZf.php" },
            { APIKey.Zf_Exam,"@api/examQueryZf.php" },
            { APIKey.Zf_Freeroom,"@api/roomZf.php" },
            { APIKey.Zf_ScoreDetail,"@api/scoresDetailZf.php" },
            { APIKey.Card,"@api/cardRecords.php"},
            { APIKey.Library_Search,"@api/library_search.php" },
            { APIKey.Library_Book,"@api/library_book.php" },
            { APIKey.Library_Borrow,"@api/library_borrow.php" },
            

        };

        public static string GetAPI(APIKey key)
        {
            var url = apiMap[key];
            foreach (var item in apiDomain)
            {
               url = url.Replace($"@{item.Key}", item.Value);
            }
            return url;
        }
       
    }

    public enum APIKey
    {
        JhUser,
        Zf_Course,
        Zf_Score,
        Zf_Exam,
        Zf_Freeroom,
        Zf_ScoreDetail,
        Card,
        Library_Search,
        Library_Book,
        Library_Borrow
    } 
}
