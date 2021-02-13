using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenModel
{
  public  class Timetable
    {
        public int timetable_id { get; set; }
        public Activity activity { get; set; }
        public int nrOfSupervisors { get; set; }

        public DateTime startTime { get; set; }

        public DateTime endTime { get; set; }

    }
}
