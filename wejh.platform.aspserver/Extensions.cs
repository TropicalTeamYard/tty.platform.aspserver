using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using wejh.Model;
using wejh.Util;

namespace wejh
{
    public static class Extensions
    {
        /// <summary>
        /// 将原可枚举对象通过函数映射到一个新列表。
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static List<TResult> Map<TIn,TResult>(this IEnumerable<TIn> obj,Func<TIn,TResult> func)
        {
            List<TResult> result = new List<TResult>();
            foreach (var item in obj)
            {
                result.Add(func(item));
            }
            return result;
        }

        public static void Add(this IMySqlQueryable obj) => MySqlUtil.Add(obj);
        public static bool TryQuery(this IMySqlQueryable obj)
        {
            if (MySqlUtil.TryQuery(obj, out DataTable table))
            {
                var row = table.Rows[0];
                obj.Set(row);
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool Exists(this IMySqlQueryable obj) => MySqlUtil.Exists(obj);
    }
}
