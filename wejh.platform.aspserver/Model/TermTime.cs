using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wejh.Model
{
    /// <summary>
    /// 储存了学期时间的相关信息，当然，它也能够返回寒暑假的时间。
    /// </summary>
    public class TermTime
    {
        public TermTime()
        {
        }

        public TermTime(int year, int term, string begin, string end)
        {
            this.year = year;
            this.term = term;
            this.begin = begin;
            this.end = end;
        }

        public int year { get; set; }
        /// <summary>
        /// 为了方便，我们规定：3-上学期，12-下学期，16-短学期，1-寒假，9-暑假。
        /// </summary>
        public int term { get; set; }
        public string begin { get; set; }
        public string end { get; set; }

        //public DateTime Begin => DateTime.Parse(begin);
        //public DateTime End => DateTime.Parse(end);

        public int dayofweek { get; set; }
        public int week { get; set; }
        public int weeklasting { get; set; }
    }
}
