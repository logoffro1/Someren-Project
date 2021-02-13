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
 public class Supervisor_DAO : Base
    {
        public Supervisor_DAO() : base()
        {

        }
        // Returns all supervisors into a list from database.
        public List<Supervisor> Db_Get_AllSupervisors()
        {
            string query = "SELECT Supervisor.supervisor_id,Teacher.teacher_id,teacher.[name],Activity.activity_id,Activity.activity_name FROM Supervisor INNER JOIN Teacher on teacher.teacher_id = Supervisor.teacher_id INNER JOIN Activity on Activity.activity_id = Supervisor.activity_id;";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }
        //edit an existing supervisor
        public void UpdateSupervisor(Supervisor supervisor, int activityId)
            { 
            OpenConnection();
            SqlCommand cmd = new SqlCommand("UPDATE Supervisor set activity_id = @activity_id WHERE supervisor_id = @supervisor_id ", conn);
            cmd.Parameters.AddWithValue("@activity_id", activityId);
            cmd.Parameters.AddWithValue("@supervisor_id", supervisor.supervisor_id);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Close();
            conn.Close();
        }
     //return the supervisor with the given id
        public Supervisor GetById(int id)
        {
            OpenConnection();
            SqlCommand cmd = new SqlCommand("SELECT Supervisor.supervisor_id,Supervisor.teacher_id,Supervisor.activity_id , teacher.[name] FROM Supervisor inner join teacher on supervisor.teacher_id = teacher.teacher_id WHERE supervisor_id = @Id; ", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataReader reader = cmd.ExecuteReader();
            Supervisor supervisor = null;
            if (reader.Read())
            {
                supervisor = ReadSupervisor(reader);
            }
            reader.Close();
            conn.Close();
            return supervisor;
        }
        //get all the activities that are supervised by that teacher
        public List<Activity> GetTeacherActivities(int teacherId)
        {
            List<Activity> activities = new List<Activity>();
            OpenConnection();
            SqlCommand cmd = new SqlCommand("SELECT Activity.activity_id,activity_name,activity_date FROM Activity inner join Supervisor on Activity.activity_id= Supervisor.activity_id WHERE Supervisor.teacher_id=@teacher_id; ", conn);
            cmd.Parameters.AddWithValue("@teacher_id", teacherId);
         
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                activities.Add(ReadActivity(reader));
            }
            reader.Close();
            conn.Close();

            return activities;
        }
        private Activity ReadActivity(SqlDataReader reader)
        {
            Activity activity = new Activity()
            {
                activity_id = (int)reader["activity_id"],
                activity_name = reader["activity_name"].ToString(),
                activity_Date = (DateTime)reader["activity_date"],
                
            };


            return activity;

        }
        //add a new supervisor
        public void AddSupervisor(Supervisor supervisor)
        {
            OpenConnection();
            SqlCommand cmd = new SqlCommand("INSERT INTO Supervisor  VALUES(@teacher_id,@activity_id); ", conn);
            cmd.Parameters.AddWithValue("@teacher_id", supervisor.Number);
            cmd.Parameters.AddWithValue("@activity_id", supervisor.activity_id);

            SqlDataReader reader = cmd.ExecuteReader();
            reader.Close();
            conn.Close();
        }
        //remove a supervisor
        public void RemoveSupervisor(Supervisor supervisor)
        {
            OpenConnection();
            SqlCommand cmd = new SqlCommand("DELETE FROM Supervisor WHERE supervisor_id = @Id; ", conn);
            cmd.Parameters.AddWithValue("@Id", supervisor.supervisor_id);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Close();
            conn.Close();
        }
        private Supervisor ReadSupervisor(SqlDataReader reader)
        {
            Supervisor supervisor = new Supervisor()
            {
                supervisor_id = (int)reader["supervisor_id"],
                activity_id = (int)reader["activity_id"],
                Name = reader["name"].ToString(),
                Number = (int)reader["teacher_id"]
            };

            return supervisor;

        }
        private List<Supervisor> ReadTables(DataTable dataTable)
        {
            List<Supervisor> supervisors = new List<Supervisor>();

            foreach (DataRow dr in dataTable.Rows)
            {
                Supervisor supervisor = new Supervisor()
                {
                    Number = (int)dr["teacher_id"],
                    Name = dr["name"].ToString(),
                    activity_id = (int)(dr["activity_id"]),
                    supervisor_id = (int)(dr["supervisor_id"])


                };
                supervisors.Add(supervisor);
            }
            return supervisors;
        }
    }
}
