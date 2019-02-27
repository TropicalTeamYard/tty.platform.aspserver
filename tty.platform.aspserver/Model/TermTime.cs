using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tty.Configs;

namespace tty.Model
{
    public class TermTimeConfig
    {
        public string provider { get; set; }
        public TermTimeUni[] config { get; set; }
    }
    /// <summary>
    /// 储存了学期时间的相关信息，当然，它也能够返回寒暑假的时间。
    /// </summary>
    public class TermTimeUni
    {
        public TermTimeUni()
        {
        }

        public TermTimeUni(string term, /*string format,*/ string begin, string end)
        {
            this.term = term;
            //this.format = format;
            this.begin = begin;
            this.end = end;
        }

        public string term { get; set; }
        //public string format { get; set; }
        public string begin { get; set; }
        public string end { get; set; }

        //public DateTime Begin => DateTime.Parse(begin);
        //public DateTime End => DateTime.Parse(end);

        //public int dayofweek { get; set; }
        //public int week { get; set; }
        //public int weeklasting { get; set; }
    }

    public static class TermTime
    {
        public static TermTimeUni Get(string provider)
        {
            TermTimeConfig[] terms = App.Current.Configuration.GetTimeConfig();

            //var x = (from item in terms orderby item.begin select item).ToArray();

            //DateTime date = DateTime.Now.Date;
            //int index = -1;
            //for (int i = 0; i < x.Length; i++)
            //{
            //    //在学期里面
            //    if (date < DateTime.Parse(x[i].begin))
            //    {
            //        if (i == 0)
            //        {
            //            return null;
            //        }
            //        else
            //        {
            //            index = i - 1;
            //        }
            //    }
            //}
            //if (index == -1)
            //{
            //    return null;
            //}

            //TermTimeUni fo = x[index];

            //fo.weeklasting = (((DateTime.Parse(fo.end) - DateTime.Parse(fo.begin)).Days + 1) / 7);
            //fo.week = (date - DateTime.Parse(fo.begin)).Days / 7 + 1;
            //fo.dayofweek = (int)date.DayOfWeek;
            //return fo;

            return null;
        }

        public static ResponceModel GetResponce(string provider)
        {
            try
            {
                return new ResponceModel(200, "获取时间成功", Get(provider));
            }
            catch (Exception ex)
            {
                return ResponceModel.GetInstanceError(ex);
            }
        }
    }
}
