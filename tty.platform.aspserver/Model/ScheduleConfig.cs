using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tty.Model
{
    public class ScheduleConfig
    {
        public string provider { get; set; }
        public Schedule[] config { get; set; }
    }

    public class Schedule
    {
        public Period[] periods { get; set; }
        public ScheduleLayer[] layers { get; set; }
    }

    public class ScheduleLayer
    {
        public string weekrange { get; set; }
        public string[] content { get; set; }
    }


}
