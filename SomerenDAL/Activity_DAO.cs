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
    public class Activity_DAO : Base
    {
        public Activity_DAO() : base()
        {

        }
        // Returns all activities into a list from database.
        public List<Activity> Db_Get_AllActivities()
        {
            string query = "SELECT Activity.activity_id, activity_name,activity_date,count(student_id)AS [NrOfStudents],count(supervisor_id) AS [NrOfSupervisors] FROM Activity left join Supervisor on Activity.activity_id = Supervisor.activity_id left join Participate on Activity.activity_id = Participate.activity_id GROUP BY Activity.activity_id,activity_name,activity_date";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }
        //return all the activity types as a string
        public List<string> GetActivityTypes()
        {
            OpenConnection();
            List<string> activityTypes = new List<string>();
            SqlCommand cmd = new SqlCommand("SELECT activity_name FROM ActivityType", conn);  
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string temp = ReadActivityType(reader);
                //fill the list with the activity types
                activityTypes.Add(temp);

            }    
            reader.Close();
            conn.Close();
            return activityTypes;
        }

        private string ReadActivityType(SqlDataReader reader)
        {
    
           string  activityType = reader["activity_name"].ToString();

            return activityType;

        }
        //add a new activity
        public void AddActivity(Activity activity)
        {
            OpenConnection();
            //insert the values into the database

            SqlCommand cmd = new SqlCommand("INSERT INTO [Activity] VALUES(@activity_name,@activity_date); ", conn);
            cmd.Parameters.AddWithValue("@activity_name", activity.activity_name);
            cmd.Parameters.AddWithValue("@activity_date", activity.activityDateString);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Close();

         

        }
        //get the activity with the biggest ID (which is the most recent one)
        public Activity GetBiggestId()
        {

            SqlCommand cmd = new SqlCommand("SELECT Activity.activity_id,Activity.activity_name,activity_date FROM Activity WHERE Activity.activity_id = (SELECT MAX(activity_id) FROM Activity)", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            Activity activity = null;
            if (reader.Read())
            {
                activity = ReadActivity(reader);
            }
            reader.Close();
            conn.Close();
            return activity;

        }
        //add a new timetable
        public void AddTimetable(Timetable timetable)
        {
            OpenConnection();
            //insert the values into the database
            string _startTime = $"{timetable.startTime.Year}-{timetable.startTime.Month}-{timetable.startTime.Day} {timetable.startTime.Hour}:{timetable.startTime.Minute}:{timetable.startTime.Second}";
            string _endTime = $"{timetable.endTime.Year}-{timetable.endTime.Month}-{timetable.endTime.Day} {timetable.endTime.Hour}:{timetable.endTime.Minute}:{timetable.endTime.Second}";

            SqlCommand cmd = new SqlCommand("INSERT INTO Timetable values(@activity_id,@date,@startTime,@endTime);",conn);
            cmd.Parameters.AddWithValue("@activity_id", timetable.activity.activity_id);
            cmd.Parameters.AddWithValue("@date", timetable.activity.activity_Date);
            cmd.Parameters.AddWithValue("@startTime", _startTime);
            cmd.Parameters.AddWithValue("@endTime", _endTime);
            //ErrorDAO err = new ErrorDAO(timetable.startTime.ToString());
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Close();

        }
        //update an existing activity
        public void UpdateActivity(Activity activity, string activity_date)
        {
      
            OpenConnection();
            SqlCommand cmd = new SqlCommand("update [Activity] set activity_date = @activity_date where activity_id = @Id; ", conn);

            cmd.Parameters.AddWithValue("@Id", activity.activity_id);
            cmd.Parameters.AddWithValue("@activity_date", activity_date);

            SqlDataReader reader = cmd.ExecuteReader();

            reader.Close();
            conn.Close();

        }
        //add a new activity type
        public void AddActivityType(string activityType)
        {
            OpenConnection();
            //insert the values into the database
            SqlCommand cmd = new SqlCommand("INSERT INTO [ActivityType] VALUES(@activity_name); ", conn);
            cmd.Parameters.AddWithValue("@activity_name", activityType);
         
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Close();

        }
        //return an activity by the ID
        public Activity GetById(int id)
        {

            OpenConnection();
            SqlCommand cmd = new SqlCommand("SELECT activity_id,activity_name,activity_date FROM Activity WHERE activity_id = @activity_id",conn);
           // SqlCommand cmd = new SqlCommand("SELECT Activity.activity_id,activity_name,activity_date ,Count(Supervisor.supervisor_id) AS [NrOfSupervisors] FROM Activity inner join Supervisor on supervisor.activity_id = Activity.activity_id WHERE Activity.activity_id = @activity_id GROUP BY Activity.activity_id, activity_name, activity_date", conn);
            cmd.Parameters.AddWithValue("@activity_id", id);
            SqlDataReader reader = cmd.ExecuteReader();
            Activity activity = null;
            if (reader.Read())
               activity = ReadActivity(reader);

            conn.Close();
            reader.Close();

            return activity;
        }
        //get the number of supervisors for that activity
        public int GetSupervisorsNr(int activityId)
        {
            int supervisors = 0;
            OpenConnection();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS [nrOfSup] FROM Supervisor where Supervisor.activity_id = @Id; ", conn);
            cmd.Parameters.AddWithValue("@Id", activityId);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
                supervisors = ReadSupervisors(reader);

         

            conn.Close();
            reader.Close();
            return supervisors;
        }
        private int ReadSupervisors(SqlDataReader reader)
        {
            int supervisors = (int)reader["nrOfSup"];

            return supervisors;
        }
        private Activity ReadActivity(SqlDataReader reader)
        {
            Activity activity = new Activity()
            {
                activity_id = (int)reader["activity_id"],
                activity_name = reader["activity_name"].ToString(),
                activity_Date = (DateTime)reader["activity_date"],
            nrOfSupervisors = GetSupervisorsNr((int)reader["activity_id"])


        };        
            return activity;
        }
        //remove an activity from the database
        public void RemoveActivity(Activity activity)
        {
            OpenConnection();
            SqlCommand cmd = new SqlCommand("DELETE FROM Activity WHERE activity_id=@Id; ", conn);
            cmd.Parameters.AddWithValue("@Id", activity.activity_id);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Close();
            conn.Close();
        }
        private List<Activity> ReadTables(DataTable dataTable)
        {
            List<Activity> activities = new List<Activity>();

            try
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    Activity activity = new Activity()
                    {
                        activity_id = (int)dr["activity_id"],
                        activity_name = (dr["activity_name"].ToString()),
                        nrOfStudents = (int)dr["nrOfStudents"],
                        activity_Date = (DateTime)dr["activity_date"],
                        nrOfSupervisors = (int)dr["nrOfSupervisors"]
                    };
                    activities.Add(activity);
                }
                return activities;
            }
            catch
            {
                ErrorDAO error = new ErrorDAO("Application couldn't read the activity table");
            }
            return null;
        }

    }
}
