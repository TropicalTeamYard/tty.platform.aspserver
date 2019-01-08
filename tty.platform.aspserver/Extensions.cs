using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using tty.Model;
using tty.Util;

namespace tty
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



    }
}
