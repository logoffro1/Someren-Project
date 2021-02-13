using SomerenDAL;
using SomerenModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenLogic
{
   public class Lecturer_Service
    {
        Lecturer_DAO lecturer_db = new Lecturer_DAO();

        public List<Teacher> GetLecturers()
        {
            List<Teacher> teacherlist = new List<Teacher>();
            try
            {
                teacherlist= lecturer_db.Db_Get_AllTeachers();
                return teacherlist;
            }
            catch
            {
                string exp = "SomerenApp couldn't get the lecturer list";
                ErrorDAO error = new ErrorDAO(exp);
                return null;
            }

        }
    }
}
