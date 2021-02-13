using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenModel
{
  public  class Activity
    {
        public int activity_id { get; set; } // the activity's primary key
        public string activity_name { get; set; }
        public int nrOfStudents { get; set; } // number of students currently in that activity
        public int nrOfSupervisors { get; set; } // number of supervisers for that activity
     public DateTime activity_Date { get; set; } // the date when the activity takes place
        public string activityDateString { get; set; } //the date when the activity takes place but in a string format

        public Activity() //default
        {

        }
        public Activity(string activity_name,string activityDateString)
        {
            this.activity_name = activity_name;
            this.activityDateString = activityDateString;
        }
    }

}
