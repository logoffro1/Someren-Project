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
    public class Student_DAO : Base
    {
        
      public Student_DAO():base ()
        {
          
        }
        //Returns all students into a list from database.
        public List<Student> Db_Get_All_Students()
        {
            string query = "SELECT student_id, name, [date_of_birth],[room_id] FROM [Student];";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));

        }

        //get the student with the provided id
        public Student GetById(int id)
        {
            OpenConnection();
            SqlCommand cmd = new SqlCommand("SELECT student_id,name,room_id,date_of_birth FROM Student WHERE student_id = @student_id", conn);
            cmd.Parameters.AddWithValue("@student_id", id);
            SqlDataReader reader = cmd.ExecuteReader();
            Student student = null;
            if (reader.Read())
            {
                student=ReadStudent(reader);
            }
            reader.Close();
            conn.Close();
            return student;
        }

        //get the student with the provided name
        public Student GetByName(string name)
        {
            OpenConnection();
            SqlCommand cmd = new SqlCommand("SELECT student_id, date_of_birth,name,room_id FROM Student WHERE [name] = @student_name", conn);
            cmd.Parameters.AddWithValue("@student_name", name);
            SqlDataReader reader = cmd.ExecuteReader();
            Student student = null;

            if (reader.Read())
            {
                student = ReadStudent(reader);
            }
            reader.Close();
            conn.Close();
            return student;
        }
        private Student ReadStudent(SqlDataReader reader)
        {
            Student student = new Student()
            {
                Number = (int)reader["student_id"],
                BirthDate = (DateTime)reader["date_of_birth"],
                Name = (string)reader["name"],
                RoomID = (int)reader["room_id"]
            };
            return student;

        }

        private List<Student> ReadTables(DataTable dataTable)
        {
            List<Student> students = new List<Student>();

            foreach (DataRow dr in dataTable.Rows)
            {
                Student student = new Student()
                {
                    Number = (int)dr["student_id"],
                    Name = (String)(dr["name"].ToString()),
                    BirthDate =(DateTime)(dr["date_of_birth"]),
                    RoomID = (int)(dr["Room_id"])
                };
                students.Add(student);
            }
            return students;
        }

    }
}
