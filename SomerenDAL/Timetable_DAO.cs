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
    public class Timetable_DAO : Base
    {
        public Timetable_DAO() : base()
        {

        }
        // Returns all timetables into a list from database.
        public List<Timetable> Db_Get_AllTimetables()
        {
            string query = "SELECT timetable_id,Timetable.activity_id,start_time,end_time FROM timetable inner join Activity on Activity.activity_id = timetable.activity_id;";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }
        //get the timetables for that time period
        public List<Timetable> GetTimetableForDates(DateTime startDate, DateTime endDate)
        {
            List<Timetable> tempTables = Db_Get_AllTimetables();
            List<Timetable> timetables = new List<Timetable>();
            foreach (Timetable timetable in tempTables)
            {
                if (timetable.activity.activity_Date.Date >= startDate.Date)
                {
                    if (timetable.activity.activity_Date.Date <= endDate.Date)
                        timetables.Add(timetable);
                }

            }               
            return timetables;
        }
        private List<Timetable> ReadTables(DataTable dataTable)
        {
            List<Timetable> timetables = new List<Timetable>();

            try
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    Activity_DAO activity_dao = new Activity_DAO();
            
                    Timetable timetable = new Timetable()
                    {
                        
                        timetable_id = (int)dr["timetable_id"],
                        activity = activity_dao.GetById((int)dr["activity_id"]),
                        startTime = (DateTime)dr["start_time"],
                        endTime = (DateTime)dr["end_time"],
                       
                        
                    };
                    timetable.nrOfSupervisors = timetable.activity.nrOfSupervisors;

                    timetables.Add(timetable);
                }
                return timetables;
            }
            catch
            {
                ErrorDAO error = new ErrorDAO("Application couldn't read the timetable table");
            }
            return null;
        }

    }
}
