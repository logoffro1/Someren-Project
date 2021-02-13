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
    public class Student_Service
    {
        Student_DAO student_db = new Student_DAO();

        public List<Student> GetStudents()
        {
            List<Student> student = new List<Student>();
            try
            {
                 student = student_db.Db_Get_All_Students();
                 return student;
            }
            catch
            {
                string exp = "SomerenApp couldn't get the student list";
                ErrorDAO error = new ErrorDAO(exp);
                return null; 
               
            }
        }
        public Student GetByName(string name)
        {
            return student_db.GetByName(name);
        }
    }
}
