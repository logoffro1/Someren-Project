using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Collections.ObjectModel;
using SomerenModel;
using System.Configuration;

namespace SomerenDAL
{
    public class Lecturer_DAO : Base
    {
        public Lecturer_DAO():base()
        {
        
        }
        // Returns all lecturers into a list from database.
        public List<Teacher> Db_Get_AllTeachers()
        {
            string query = "SELECT teacher_id, name, room_id FROM Teacher;";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        private List<Teacher> ReadTables(DataTable dataTable)
        {
            List<Teacher> teachers = new List<Teacher>();

            foreach (DataRow dr in dataTable.Rows)
            {
                Teacher teacher = new Teacher()
                {
                    Number = (int)dr["teacher_id"],
                    Name = (dr["name"].ToString()),
                    RoomID = (int)(dr["room_id"])

                };
                teachers.Add(teacher);
            }
            return teachers;
        }

    }
}
