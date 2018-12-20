using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wejh.Model
{
    public class CourseModel
    {
        public CourseModel()
        {
        }

        public string name { get; set; }
        public string college { get; set; }
        public string type { get; set; }
        public string teacher { get; set; }
        public string campus { get; set; }
        public string location { get; set; }
        public string weekrange { get; set; }
        public string dayofweek { get; set; }
        public string timerange { get; set; }
        public int classscore { get; set; }
        public int classhour { get; set; }
    }
}
