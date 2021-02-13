using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SomerenDAL;
namespace SomerenLogic
{
   public class Error_Service
    {
        public Error_Service(string errorMsg)
        {

            ErrorDAO error = new ErrorDAO(errorMsg);
        }
      

    }
}
