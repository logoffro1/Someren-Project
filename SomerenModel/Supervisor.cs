using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenModel
{
    public class Supervisor : Teacher
    {
         public int supervisor_id { get; set; } // primary key
         public int activity_id { get; set; } // the activity that the supervisor supervises
    }
}
